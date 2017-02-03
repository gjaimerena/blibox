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
    public class RubrosController : Controller
    {
        private BliboxEntities db = new BliboxEntities();
        private List<SelectListItem> items = new List<SelectListItem>();

        // GET: Rubros
        public ActionResult Index(string sortOrder, string q, int page = 1, int pageSize = 10)
        {

            ViewBag.searchQuery = String.IsNullOrEmpty(q) ? "" : q;
            page = page > 0 ? page : 1;
            pageSize = pageSize > 0 ? pageSize : 10;
            ViewBag.CurrentSort = sortOrder;

            var query = db.Rubro.OrderBy(m => m.ID_rubro);

            if (!String.IsNullOrEmpty(q))
            {
                query = query.Where(s =>
                    s.ID_rubro.ToString().Contains(q) ||
                    s.Descirpcion.Contains(q) ||
                    s.Observaciones.ToString().Contains(q) 
                    
                    ).OrderBy(m => m.ID_rubro);
            }
            return View(query.ToPagedList(page, pageSize));
        }

        // GET: Rubros/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["Noti"] = Notification.Show("Id nulo", "RUBROS", type: ToastType.Warning, position: Position.TopCenter);
                return RedirectToAction("Index");
            }
            Rubro rubro = db.Rubro.Find(id);
            if (rubro == null)
            {
                TempData["Noti"] = Notification.Show("Id no asociado a un rubro", "RUBROS", type: ToastType.Warning, position: Position.TopCenter);
                return RedirectToAction("Index");
            }
            return View(rubro);
        }

        // GET: Rubros/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Rubros/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_rubro,Descirpcion,Observaciones")] Rubro rubro)
        {
            if (ModelState.IsValid)
            {
                db.Rubro.Add(rubro);
                db.SaveChanges();
                TempData["Noti"] = Notification.Show("Rubro generado exitosamente", "RUBROS", type: ToastType.Success, position: Position.TopCenter);
                return RedirectToAction("Index");
            }

            return View(rubro);
        }

        // GET: Rubros/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["Noti"] = Notification.Show("Id nulo", "RUBROS", type: ToastType.Warning, position: Position.TopCenter);
                return RedirectToAction("Index");
            }
            Rubro rubro = db.Rubro.Find(id);
            if (rubro == null)
            {
                TempData["Noti"] = Notification.Show("Id no asociado a un rubro", "RUBROS", type: ToastType.Warning, position: Position.TopCenter);
                return RedirectToAction("Index");
            }
            return View(rubro);
        }

        // POST: Rubros/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_rubro,Descirpcion,Observaciones")] Rubro rubro)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rubro).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Noti"] = Notification.Show("Rubro modificado exitosamente", "RUBROS", type: ToastType.Success, position: Position.TopCenter);
                return RedirectToAction("Index");
            }
            return View(rubro);
        }

        // GET: Rubros/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rubro rubro = db.Rubro.Find(id);
            if (rubro == null)
            {
                return HttpNotFound();
            }
            return View(rubro);
        }

        // POST: Rubros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Rubro rubro = db.Rubro.Find(id);
            db.Rubro.Remove(rubro);
            db.SaveChanges();
            TempData["Noti"] = Notification.Show("Rubro eliminado exitosamente", "RUBROS", type: ToastType.Success, position: Position.TopCenter);
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
