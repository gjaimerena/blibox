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
    public class CtaCte_ClientesController : Controller
    {
        private BliboxEntities db = new BliboxEntities();

        // GET: CtaCte_Clientes
        public ActionResult Index(string sortOrder, string q, int page = 1, int pageSize = 10)
        {

            ViewBag.searchQuery = String.IsNullOrEmpty(q) ? "" : q;
            page = page > 0 ? page : 1;
            pageSize = pageSize > 0 ? pageSize : 10;
            //ViewBag.IdSortParam = sortOrder == "id" ? "id_desc" : "id";
            //ViewBag.DescripcionSort = sortOrder == "descripcion" ? "descripcion_desc" : "descripcion";
            //ViewBag.DateSortParam = sortOrder == "date" ? "date_desc" : "date";

            ViewBag.CurrentSort = sortOrder;

            var query = db.CtaCte_Clientes.OrderBy(m => m.ID_movimiento);

            if (!String.IsNullOrEmpty(q))
            {
                query = query.Where(s =>
                    s.Razon_social.ToString().Contains(q) ||
                    s.ID_cliente.ToString().Contains(q) ||
                    s.Fecha_movimiento.ToString().Contains(q) ||
                    s.Concepto.ToString().Contains(q)

                    ).OrderBy(m => m.ID_movimiento);
            }
            return View(query.ToPagedList(page, pageSize));
        }
        // GET: CtaCte_Clientes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CtaCte_Clientes ctaCte_Clientes = db.CtaCte_Clientes.Find(id);
            if (ctaCte_Clientes == null)
            {
                return HttpNotFound();
            }
            return View(ctaCte_Clientes);
        }

        // GET: CtaCte_Clientes/Create
        public ActionResult Create()
        {
            ViewBag.ID_cliente = new SelectList(db.Cliente, "ID_cliente", "Razon_Social");
            return View();
        }

        // POST: CtaCte_Clientes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_movimiento,ID_cliente,Razon_social,Saldo,Fecha_movimiento,Concepto,Importe_movimiento,Observaciones")] CtaCte_Clientes ctaCte_Clientes)
        {
            ctaCte_Clientes.ID_movimiento = 1;
            if (ModelState.IsValid)
            {

                CtaCte_Clientes ctaCte = db.CtaCte_Clientes.OrderByDescending(m => m.ID_movimiento).FirstOrDefault();
                if (ctaCte != null)
                {
                    ctaCte_Clientes.ID_movimiento = ctaCte.ID_movimiento + 1;
                }
                db.CtaCte_Clientes.Add(ctaCte_Clientes);

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_cliente = new SelectList(db.Cliente, "ID_cliente", "Razon_Social", ctaCte_Clientes.ID_cliente);
            return View(ctaCte_Clientes);
        }

        // GET: CtaCte_Clientes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CtaCte_Clientes ctaCte_Clientes = db.CtaCte_Clientes.Find(id);
            if (ctaCte_Clientes == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_cliente = new SelectList(db.Cliente, "ID_cliente", "Razon_Social", ctaCte_Clientes.ID_cliente);
            return View(ctaCte_Clientes);
        }

        // POST: CtaCte_Clientes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_movimiento,ID_cliente,Razon_social,Saldo,Fecha_movimiento,Concepto,Importe_movimiento,Observaciones")] CtaCte_Clientes ctaCte_Clientes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ctaCte_Clientes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_cliente = new SelectList(db.Cliente, "ID_cliente", "Razon_Social", ctaCte_Clientes.ID_cliente);
            return View(ctaCte_Clientes);
        }

        // GET: CtaCte_Clientes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CtaCte_Clientes ctaCte_Clientes = db.CtaCte_Clientes.Find(id);
            if (ctaCte_Clientes == null)
            {
                return HttpNotFound();
            }
            return View(ctaCte_Clientes);
        }

        // POST: CtaCte_Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CtaCte_Clientes ctaCte_Clientes = db.CtaCte_Clientes.Find(id);
            db.CtaCte_Clientes.Remove(ctaCte_Clientes);
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
