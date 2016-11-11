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

namespace Blibox.Controllers
{
    public class CtaCteClientesController : Controller
    {
        private BliboxEntities db = new BliboxEntities();

        // GET: CtaCteClientes
        public ActionResult Index(string sortOrder, string q, int page = 1, int pageSize = 10 )
        {
            int id_cliente = Convert.ToInt32(Request["ID_cliente"]);

            ViewBag.searchQuery = String.IsNullOrEmpty(q) ? "" : q;

            page = page > 0 ? page : 1;
            pageSize = pageSize > 0 ? pageSize : 10;

            //ViewBag.IdSortParam = sortOrder == "id" ? "id_desc" : "id";
            //ViewBag.DescripcionSort = sortOrder == "descripcion" ? "descripcion_desc" : "descripcion";
            //ViewBag.DateSortParam = sortOrder == "date" ? "date_desc" : "date";

            ViewBag.CurrentSort = sortOrder;

            var query = db.CtaCteClientesMov.Include(c => c.Cliente);

            if (!String.IsNullOrEmpty(q))
            {
                query = query.Where(s =>
                    s.id.ToString().Contains(q) ||
                    s.concepto.Contains(q) ||
                    s.fecha_movimiento.ToString().Contains(q) ||
                    s.nro_comprobante.ToString().Contains(q) 
                   
                    ).OrderBy(m => m.id);
            }

            ViewBag.ID_cliente = new SelectList(db.Cliente, "ID_cliente", "Razon_Social");

            if  (ViewBag.verbusqueda== null)
            {
                ViewBag.verbusqueda = "visibility:visible";
                ViewBag.verdetalle = "visibility:hidden";
            }
            else
            {
                ViewBag.verbusqueda = "visibility:hidden";
                ViewBag.verdetalle = "visibility:visible";
            }
            

            return View(query.ToPagedList(page, pageSize));
          
        }

        // GET: CtaCteClientes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CtaCteClientesMov ctaCteClientesMov = db.CtaCteClientesMov.Find(id);
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
            return View();
        }

        // POST: CtaCteClientes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_cliente,fecha_movimiento,importe_movimiento,saldo,nro_comprobante,observacion,desc_debito,desc_credito,concepto")] CtaCteClientesMov ctaCteClientesMov)
        {
            if (ModelState.IsValid)
            {
                db.CtaCteClientesMov.Add(ctaCteClientesMov);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_cliente = new SelectList(db.Cliente, "ID_cliente", "Razon_Social", ctaCteClientesMov.id_cliente);
            return View(ctaCteClientesMov);
        }

        // GET: CtaCteClientes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CtaCteClientesMov ctaCteClientesMov = db.CtaCteClientesMov.Find(id);
            if (ctaCteClientesMov == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_cliente = new SelectList(db.Cliente, "ID_cliente", "Razon_Social", ctaCteClientesMov.id_cliente);
            return View(ctaCteClientesMov);
        }

        // POST: CtaCteClientes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_cliente,fecha_movimiento,importe_movimiento,saldo,nro_comprobante,observacion,desc_debito,desc_credito,concepto")] CtaCteClientesMov ctaCteClientesMov)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ctaCteClientesMov).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_cliente = new SelectList(db.Cliente, "ID_cliente", "Razon_Social", ctaCteClientesMov.id_cliente);
            return View(ctaCteClientesMov);
        }

        // GET: CtaCteClientes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CtaCteClientesMov ctaCteClientesMov = db.CtaCteClientesMov.Find(id);
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
            CtaCteClientesMov ctaCteClientesMov = db.CtaCteClientesMov.Find(id);
            db.CtaCteClientesMov.Remove(ctaCteClientesMov);
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
