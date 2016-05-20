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
    public class MarcosController : Controller
    {
        private BliboxEntities db = new BliboxEntities();

        // GET: Marcos
        public ActionResult Index(string sortOrder, string q, int page = 1, int pageSize = 10)
        {

            ViewBag.searchQuery = String.IsNullOrEmpty(q) ? "" : q;
            page = page > 0 ? page : 1;
            pageSize = pageSize > 0 ? pageSize : 10;
            //ViewBag.IdSortParam = sortOrder == "id" ? "id_desc" : "id";
            //ViewBag.DescripcionSort = sortOrder == "descripcion" ? "descripcion_desc" : "descripcion";
            //ViewBag.DateSortParam = sortOrder == "date" ? "date_desc" : "date";

            ViewBag.CurrentSort = sortOrder;

            var query = db.Marco.OrderBy(m => m.ID_marco);

            if (!String.IsNullOrEmpty(q))
            {
                query = query.Where(s =>
                    s.ID_marco.ToString().Contains(q) ||
                    s.Largo.ToString().Contains(q) ||
                    s.Ancho.ToString().Contains(q) ||
                    s.Observaciones.ToString().Contains(q) ||
                    s.Descripcion.ToString().Contains(q)

                    ).OrderBy(m => m.ID_marco);
            }
            return View(query.ToPagedList(page, pageSize));
        }

        // GET: Marcos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Marco marco = db.Marco.Find(id);
            if (marco == null)
            {
                return HttpNotFound();
            }
            return View(marco);
        }

        // GET: Marcos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Marcos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_marco,Descripcion,Ancho,Largo,Observaciones")] Marco marco)
        {
            if (ModelState.IsValid)
            {
                db.Marco.Add(marco);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(marco);
        }

        // GET: Marcos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Marco marco = db.Marco.Find(id);
            if (marco == null)
            {
                return HttpNotFound();
            }
            return View(marco);
        }

        // POST: Marcos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_marco,Descripcion,Ancho,Largo,Observaciones")] Marco marco)
        {
            if (ModelState.IsValid)
            {
                db.Entry(marco).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(marco);
        }

        // GET: Marcos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Marco marco = db.Marco.Find(id);
            if (marco == null)
            {
                return HttpNotFound();
            }
            return View(marco);
        }

        // POST: Marcos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Marco marco = db.Marco.Find(id);
            db.Marco.Remove(marco);
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
