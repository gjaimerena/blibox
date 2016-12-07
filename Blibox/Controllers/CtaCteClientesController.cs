using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Blibox;
using PagedList;
using System.Globalization;

namespace Blibox.Controllers
{
    [AuthorizeOrRedirect(Roles = "Administrador")]
    public class CtaCteClientesController : Controller
    {
        private BliboxEntities db = new BliboxEntities();

        // GET: CtaCteClientes
        public ActionResult Index(string sortOrder, string q, int page = 1, int pageSize = 10 )
        {

            int id_cliente = Convert.ToInt32(Request["ID_cliente"]);

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

            var query = db.CtaCteClientes.Include(c => c.Cliente).Include(c => c.TipoMovCtaCte);

            if (fechadesde != "" || fechahasta != "")
            {
                query = query.Where(f => (f.fecha_movimiento >= desde) && (f.fecha_movimiento <= hasta) && (f.id_cliente==id_cliente));

                if (query.ToList().Count == 0)
                {
                    HelperController.Instance.agregarMensaje("No se encuentran resultado para la consulta.", HelperController.CLASE_ADVERTENCIA);
                }
                //else
                //{
                //    HelperController.Instance.agregarMensaje("Movimiento cargado con exito", HelperController.CLASE_EXITO);
                //}
            }

            query = query.OrderBy(m => m.id);

            List<Models.CtaCteClienteMovimientos> movs = new List<Models.CtaCteClienteMovimientos>();
            List<CtaCteClientes> queryList = query.ToList();

            foreach (CtaCteClientes item in queryList)
            {
                Models.CtaCteClienteMovimientos mov = new Models.CtaCteClienteMovimientos();
                mov.id = item.id;
                mov.fecha = item.fecha_movimiento.Value;
                mov.concepto = item.concepto;
                mov.saldo = (item.saldo.HasValue) ? item.saldo.Value : 0; 
                if (item.tipoMovimiento == 1)
                {
                    mov.debito = (item.importe.HasValue) ? item.importe.Value : 0;
                    mov.credito = 0;
                }
                else
                {
                    mov.credito = (item.importe.HasValue) ? item.importe.Value : 0;
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

            ViewBag.ID_cliente = new SelectList(db.Cliente, "ID_cliente", "Razon_Social");

           
            

            return View(movs.ToPagedList(page, pageSize));
          
        }

        // GET: CtaCteClientes/Details/5
        public ActionResult Details(int? id)
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

        // GET: CtaCteClientes/Create
        public ActionResult Create()
        {
            ViewBag.id_cliente = new SelectList(db.Cliente, "ID_cliente", "Razon_Social");
            ViewBag.tipoMovimiento = new SelectList(db.TipoMovCtaCte, "id", "descripcion");
            return View();
        }

        // POST: CtaCteClientes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CtaCteClientes ctaCteClientesMov)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    Cliente cliente = db.Cliente.Where(m => m.ID_cliente == ctaCteClientesMov.id_cliente).FirstOrDefault();
                    ctaCteClientesMov.Cliente = cliente;
                    //si no se le asigno salgo al crear cliente se inicializa en cero
                    if (ctaCteClientesMov.Cliente.Saldo == null) ctaCteClientesMov.Cliente.Saldo = new decimal();

                    //traigo el saldo del cliente para aplicar el debito o credito correspondiente
                    if (ctaCteClientesMov.saldo == null) ctaCteClientesMov.saldo = ctaCteClientesMov.Cliente.Saldo;

                    if (ctaCteClientesMov.tipoMovimiento == 1)
                    {
                        ctaCteClientesMov.saldo = ctaCteClientesMov.saldo + ctaCteClientesMov.importe;
                    }
                    else ctaCteClientesMov.saldo = ctaCteClientesMov.saldo - ctaCteClientesMov.importe;

                    //actualizo nuevo saldo del cliente
                    ctaCteClientesMov.Cliente.Saldo = ctaCteClientesMov.saldo;

                    db.CtaCteClientes.Add(ctaCteClientesMov);
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

            ViewBag.id_cliente = new SelectList(db.Cliente, "ID_cliente", "Razon_Social", ctaCteClientesMov.id_cliente);
            ViewBag.tipo_mov = new SelectList(db.TipoMovCtaCte, "id", "descripcion", ctaCteClientesMov.tipoMovimiento);
            return View(ctaCteClientesMov);
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
