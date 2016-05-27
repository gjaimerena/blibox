using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Blibox;

namespace Blibox.Controllers
{
    public class Articulos2Controller : Controller
    {
        private BliboxEntities db = new BliboxEntities();

        // GET: Articulos2
        public ActionResult Index()
        {
            var articulo = db.Articulo.Include(a => a.Cliente);
            return View(articulo.ToList());
        }

        // GET: Articulos2/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Articulo articulo = db.Articulo.Find(id);
            if (articulo == null)
            {
                return HttpNotFound();
            }
            return View(articulo);
        }

        // GET: Articulos2/Create
        public ActionResult Create()
        {
            ViewBag.ID_cliente = new SelectList(db.Cliente, "ID_cliente", "Razon_Social");
            return View();
        }

        // POST: Articulos2/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Articulo articulo)//([Bind(Include = "ID_articulo,Descripcion,ID_cliente,Costo,IVA,Precio_lista,Precio_fecha,Stock,Fazon,Stock_minimo,Cant_x_bulto,Tamaño_caja,Tiraje_term_x_hora,Tiraje_troquel_x_hora,Observaciones")] Articulo articulo)
        {
            if (ModelState.IsValid)
            {
                db.Articulo.Add(articulo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_cliente = new SelectList(db.Cliente, "ID_cliente", "Razon_Social", articulo.ID_cliente);
            return View(articulo);
        }

        // GET: Articulos2/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Articulo articulo = db.Articulo.Find(id);
            if (articulo == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_cliente = new SelectList(db.Cliente, "ID_cliente", "Razon_Social", articulo.ID_cliente);
            return View(articulo);
        }

        // POST: Articulos2/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_articulo,Descripcion,ID_cliente,Costo,IVA,Precio_lista,Precio_fecha,Stock,Fazon,Stock_minimo,Cant_x_bulto,Tamaño_caja,Tiraje_term_x_hora,Tiraje_troquel_x_hora,Observaciones")] Articulo articulo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(articulo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_cliente = new SelectList(db.Cliente, "ID_cliente", "Razon_Social", articulo.ID_cliente);
            return View(articulo);
        }

        // GET: Articulos2/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Articulo articulo = db.Articulo.Find(id);
            if (articulo == null)
            {
                return HttpNotFound();
            }
            return View(articulo);
        }

        // POST: Articulos2/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Articulo articulo = db.Articulo.Find(id);
            db.Articulo.Remove(articulo);
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
