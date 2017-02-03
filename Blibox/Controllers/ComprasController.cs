using PagedList;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blibox.Controllers
{

    [AuthorizeOrRedirect(Roles = "Administrador")]
    public class ComprasController : Controller
    {

        private BliboxEntities db = new BliboxEntities();

        // GET: Compras
        public ActionResult Index(string sortOrder, string q, int page = 1, int pageSize = 10)
        {
            int id_proveedor = 0;
            if (Request["ID_proveedor"] != null && Request["ID_proveedor"].ToString() != "")
            {
                Int32.TryParse(Request["ID_proveedor"], out id_proveedor);
            }
            else
            {
                ViewBag.ID_proveedor = new SelectList(db.Proveedor, "ID_proveedor", "Razon_Social");
                List<Compras> movimientos = new List<Compras>();
                return View(movimientos.ToPagedList(page, pageSize));
            }

            string fechadesde = (Request["Fecha Desde"] == null) ? "" : Request["Fecha Desde"].ToString();
            string fechahasta = (Request["Fecha Hasta"] == null) ? "" : Request["Fecha Hasta"].ToString();
            DateTime? desde = null, hasta = null;

            if (fechadesde != "") desde = DateTime.ParseExact(fechadesde, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            if (fechahasta != "") hasta = DateTime.ParseExact(fechahasta, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            ViewBag.searchQuery = String.IsNullOrEmpty(q) ? "" : q;

            page = page > 0 ? page : 1;
            pageSize = pageSize > 0 ? pageSize : 10;

            //ViewBag.IdSortParam = sortOrder == "id" ? "id_desc" : "id";
            //ViewBag.DescripcionSort = sortOrder == "descripcion" ? "descripcion_desc" : "descripcion";
            //ViewBag.DateSortParam = sortOrder == "date" ? "date_desc" : "date";

            ViewBag.CurrentSort = sortOrder;

            var query = db.Compras.Select(c=> c);

            if (id_proveedor != 0)
            {
                query = query.Where(f => (f.ID_proveedor == id_proveedor));
            }

            if (fechadesde != "" && fechahasta == "")
            {
                query = query.Where(f => (f.Fecha_compra >= desde));
            }
            if (fechadesde == "" && fechahasta != "")
            {
                query = query.Where(f => (f.Fecha_compra <= hasta));
            }
            if ((fechadesde != "") && (fechahasta != ""))
            {
                query = query.Where(f => (f.Fecha_compra >= desde) && (f.Fecha_compra <= hasta));
            }

            if (query.ToList().Count == 0)
            {
                TempData["Noti"] = Notification.Show("No se encontraron resultados para la consulta.", "COMPRAS", type: ToastType.Success, position: Position.TopCenter);
                //HelperController.Instance.agregarMensaje("No se encuentran resultado para la consulta.", HelperController.CLASE_ADVERTENCIA);
            }

            //ordeno de forma ascendiente por fecha de movimiento
            query = query.OrderBy(m => m.Fecha_compra);

            //habilitar si se desea implementar la busqueda de movimientos
            //if (!String.IsNullOrEmpty(q))
            //{
            //    query = query.Where(s =>
            //        s.id.ToString().Contains(q) ||
            //        s.concepto.Contains(q) ||
            //        s.fecha_movimiento.ToString().Contains(q) ||
            //        s.nro_comprobante.ToString().Contains(q) 

            //        ).OrderBy(m => m.id);
            //}

            ViewBag.ID_proveedor = new SelectList(db.Proveedor, "ID_proveedor", "Razon_Social");

            ViewBag.Total = query.Sum(m => m.Total);


            return View(query.ToPagedList(page, pageSize));

        }

        // GET: CtaCteClientes/Create
        public ActionResult Create()
        {
            ViewBag.ID_proveedor = new SelectList(db.Proveedor, "ID_proveedor", "Razon_social");
           
            return View();
        }

        // POST: CtaCteClientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Compras compra)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //ATENCION!!!
                    //EL SALDO DE MOV CTA CTE ESTA EN DESUSO, SOLO SE USA EL SALDO DEL CLIENTE
                    Proveedor proveedor = db.Proveedor.Where(m => m.ID_proveedor == compra.ID_proveedor).FirstOrDefault();
                    compra.Proveedor = proveedor;

                    db.Compras.Add(compra);

                    //asiento compra en cta cte de proveedores
                    CtaCteProveedores mov = new CtaCteProveedores();
                    mov.Proveedor = proveedor;
                    
                    //si no se le asigno saldo al crear proveedor se inicializa en cero
                    if (mov.Proveedor.Saldo == null) mov.Proveedor.Saldo = 0;
                    mov.tipo_movimiento = 1;
                    mov.Proveedor.Saldo = mov.Proveedor.Saldo - compra.Total;
                    mov.concepto = "Factura Compra Nº " + compra.Nro_factura;
                    mov.fecha_movimiento = compra.Fecha_compra.Value;
                    mov.id_proveedor = proveedor.ID_proveedor;
                    mov.importe = compra.Total.Value;
                    mov.observaciones = compra.Observaciones;
                    
                    db.CtaCteProveedores.Add(mov);



                    db.SaveChanges();
                    //HelperController.Instance.agregarMensaje("Movimiento generado exitosamente", HelperController.CLASE_EXITO);
                    TempData["Noti"] = Notification.Show("Movimiento generado exitosamente", "COMPRAS", type: ToastType.Success, position: Position.TopCenter);
                }
                catch (Exception ex)
                {
                    // HelperController.Instance.agregarMensaje("Error al intentar generar movimiento, intente nuevamente", HelperController.CLASE_ERROR);
                    TempData["Noti"] = Notification.Show("Error al intentar generar movimiento, intente nuevamente", "COMPRAS", type: ToastType.Error, position: Position.TopCenter);
                    throw;
                }

                return RedirectToAction("Index");
            }

            ViewBag.ID_proveedor = new SelectList(db.Proveedor, "ID_proveedor", "Razon_social", compra.ID_proveedor);
           
            return View(compra);
        }
    }
}