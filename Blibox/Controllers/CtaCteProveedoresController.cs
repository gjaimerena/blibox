using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Blibox.Controllers
{
    [AuthorizeOrRedirect(Roles = "Administrador")]
    public class CtaCteProveedoresController : Controller
    {
        private BliboxEntities db = new BliboxEntities();

        // GET: CtaCteClientes
        public ActionResult Index(string sortOrder, string q, int page = 1, int pageSize = 10)
        {
            int id_proveedor = 0;
            if (Request["ID_proveedor"] != null)
            {
                Int32.TryParse(Request["ID_proveedor"], out id_proveedor);
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

            var query = db.CtaCteProveedores.Include(p => p.Proveedor);

            if (id_proveedor != 0)
            {
                query = query.Where(f => (f.id_proveedor == id_proveedor));
            }

            if (fechadesde != "" && fechahasta == "")
            {
                query = query.Where(f => (f.fecha_movimiento >= desde));
            }
            if (fechadesde == "" && fechahasta != "")
            {
                query = query.Where(f => (f.fecha_movimiento <= hasta));
            }
            if ((fechadesde != "") && (fechahasta != ""))
            {
                query = query.Where(f => (f.fecha_movimiento >= desde) && (f.fecha_movimiento <= hasta));
            }

            if (query.ToList().Count == 0)
            {
                HelperController.Instance.agregarMensaje("No se encuentran resultado para la consulta.", HelperController.CLASE_ADVERTENCIA);
            }

            //ordeno de forma ascendiente por fecha de movimiento
            query = query.OrderBy(m => m.fecha_movimiento);

            List<Models.CtaCteProveedorMovimientos> movs = new List<Models.CtaCteProveedorMovimientos>();
            List<CtaCteProveedores> queryList = query.ToList();

            decimal saldoAcumulador = 0;

            foreach (CtaCteProveedores item in queryList)
            {
                // uso saldo para acumular los importes y obtener los distintos saldos para los movimientos
                if (item.tipo_movimiento == 1)
                {
                    saldoAcumulador = saldoAcumulador - item.importe;
                }
                else
                {
                    saldoAcumulador = saldoAcumulador + item.importe;
                }


                //creo movimientos
                Models.CtaCteProveedorMovimientos mov = new Models.CtaCteProveedorMovimientos();
                mov.id = item.id;
                mov.fecha = item.fecha_movimiento;
                mov.razonSocial = item.Proveedor.Razon_social;
                mov.concepto = item.concepto;
                mov.saldo = saldoAcumulador;
                if (item.tipo_movimiento == 1)
                {
                    mov.debito = item.importe;
                    mov.credito = 0;
                }
                else
                {
                    mov.credito = item.importe;
                    mov.debito = 0;
                }

                movs.Add(mov);
            }
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

            ViewBag.ID_proveedor = new SelectList(db.Proveedor ,"ID_proveedor", "Razon_Social");




            return View(movs.ToPagedList(page, pageSize));

        }

        // GET: CtaCteClientes/Details/5
        public ActionResult Details(int? id, decimal saldo)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CtaCteClientes ctaCteClientesMov = db.CtaCteClientes.Find(id);

            ctaCteClientesMov.saldo = saldo;
            if (ctaCteClientesMov == null)
            {
                return HttpNotFound();
            }
            return View(ctaCteClientesMov);
        }

        // GET: CtaCteClientes/Create
        public ActionResult Create()
        {
            ViewBag.id_proveedor = new SelectList(db.Proveedor, "ID_proveedor", "Razon_social");
            ViewBag.tipo_movimiento = new SelectList(db.TipoMovCtaCte, "id", "descripcion");
            return View();
        }

        // POST: CtaCteClientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CtaCteProveedores mov)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //ATENCION!!!
                    //EL SALDO DE MOV CTA CTE ESTA EN DESUSO, SOLO SE USA EL SALDO DEL CLIENTE
                    Proveedor proveedor = db.Proveedor.Where(m => m.ID_proveedor == mov.id_proveedor).FirstOrDefault();
                    mov.Proveedor = proveedor;

                    //si no se le asigno salgo al crear cliente se inicializa en cero
                    if (mov.Proveedor.Saldo == null) mov.Proveedor.Saldo = 0;

                    if (mov.tipo_movimiento == 1)
                    {
                        mov.Proveedor.Saldo = mov.Proveedor.Saldo - mov.importe;
                    }
                    else mov.Proveedor.Saldo = mov.Proveedor.Saldo + mov.importe;

                    db.CtaCteProveedores.Add(mov);
                    db.SaveChanges();
                    HelperController.Instance.agregarMensaje("Movimiento generado exitosamente", HelperController.CLASE_EXITO);
                }
                catch (Exception ex)
                {
                    HelperController.Instance.agregarMensaje("Error al intentar generar movimiento, intente nuevamente", HelperController.CLASE_ERROR);
                    throw;
                }

                return RedirectToAction("Index");
            }

            ViewBag.id_proveedor = new SelectList(db.Proveedor, "ID_proveedor", "Razon_social", mov.id_proveedor);
            ViewBag.tipo_movimiento = new SelectList(db.TipoMovCtaCte, "id", "descripcion", mov.tipo_movimiento);
            return View(mov);
        }

        // GET: CtaCteClientes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CtaCteClientes ctaCteClientesMov = db.CtaCteClientes.Find(id);
            if (ctaCteClientesMov == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_cliente = new SelectList(db.Cliente, "ID_cliente", "Razon_Social", ctaCteClientesMov.id_cliente);
            ViewBag.tipo_mov = new SelectList(db.TipoMovCtaCte, "id", "descripcion", ctaCteClientesMov.tipoMovimiento);
            return View(ctaCteClientesMov);
        }

        // POST: CtaCteClientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CtaCteClientes ctaCteClientes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ctaCteClientes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_cliente = new SelectList(db.Cliente, "ID_cliente", "Razon_Social", ctaCteClientes.id_cliente);
            return View(ctaCteClientes);
        }

        // GET: CtaCteClientes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CtaCteClientes ctaCteClientesMov = db.CtaCteClientes.Find(id);
            if (ctaCteClientesMov == null)
            {
                return HttpNotFound();
            }
            return View(ctaCteClientesMov);
        }

        // POST: CtaCteClientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CtaCteClientes ctaCteClientesMov = db.CtaCteClientes.Find(id);
            db.CtaCteClientes.Remove(ctaCteClientesMov);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}