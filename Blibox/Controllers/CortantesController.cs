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
    public class CortantesController : Controller
    {
        private BliboxEntities db = new BliboxEntities();
        private List<SelectListItem> items = new List<SelectListItem>();

        // GET: Cortantes
        // GET: Rubros
        public ActionResult Index(string sortOrder, string currentFilter, string q, int page = 1, int pageSize = 10)
        {

            ViewBag.searchQuery = String.IsNullOrEmpty(q) ? "" : q;
            //para paginado
            if (q == null) q = currentFilter;
            ViewBag.CurrentFilter = q;

            page = page > 0 ? page : 1;
            pageSize = pageSize > 0 ? pageSize : 10;
            ViewBag.CurrentSort = sortOrder;

            var query = db.Cortante.OrderBy(m => m.ID_cortante);

            if (!String.IsNullOrEmpty(q))
            {
                query = query.Where(s =>
                    s.Codigo.ToString().Contains(q) ||
                    s.Descripcion.Contains(q) ||
                     s.Sector.Contains(q) ||
                      s.Bocas.ToString().Contains(q) ||
                    s.Observaciones.ToString().Contains(q)

                    ).OrderBy(m => m.ID_cortante);
            }
            return View(query.ToPagedList(page, pageSize));
        }

        // GET: Cortantes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["Noti"] = Notification.Show("Id nulo", "CORTANTES", type: ToastType.Warning, position: Position.TopCenter);
                return RedirectToAction("Index");
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cortante cortante = db.Cortante.Find(id);
            if (cortante == null)
            {
                TempData["Noti"] = Notification.Show("Id no asociado a una cortante", "CORTANTES", type: ToastType.Warning, position: Position.TopCenter);
                return RedirectToAction("Index");
            }
            return View(cortante);
        }

        // GET: Cortantes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cortantes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_cortante,Codigo,Descripcion,Sector,Bocas,Observaciones")] Cortante cortante)
        {
            if (ModelState.IsValid)
            {
                int existe = db.Cortante.Where(m => m.Codigo == cortante.Codigo).ToList().Count;

                if (existe == 0)
                {
                    db.Cortante.Add(cortante);
                    db.SaveChanges();
                    TempData["Noti"] = Notification.Show("Cortante generada exitosamente", "CORTANTES", type: ToastType.Success, position: Position.TopCenter);
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Noti"] = Notification.Show("El codigo ya existe, por favor ingrese otro valor", "CORTANTES", type: ToastType.Warning, position: Position.TopCenter);
                    return View(cortante);
                }
                
            }
            return View(cortante);

        }

        // GET: Cortantes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["Noti"] = Notification.Show("ID nulo", "CORTANTES", type: ToastType.Warning, position: Position.TopCenter);
                return RedirectToAction("Index");
            }
            Cortante cortante = db.Cortante.Find(id);
            if (cortante == null)
            {
                TempData["Noti"] = Notification.Show("Id no asociado a una cortante", "CORTANTES", type: ToastType.Warning, position: Position.TopCenter);
                return RedirectToAction("Index");
            }
            return View(cortante);
        }

        // POST: Cortantes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_cortante,Codigo,Descripcion,Sector,Bocas,Observaciones")] Cortante cortante)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cortante).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Noti"] = Notification.Show("Cortante modificada exitosamente", "CORTANTES", type: ToastType.Success, position: Position.TopCenter);
                return RedirectToAction("Index");
            }
            return View(cortante);
        }

        // GET: Cortantes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["Noti"] = Notification.Show("ID nulo", "CORTANTES", type: ToastType.Warning, position: Position.TopCenter);
                return RedirectToAction("Index");
            }
            Cortante cortante = db.Cortante.Find(id);
            if (cortante == null)
            {
                TempData["Noti"] = Notification.Show("Id no asociado a una cortante", "CORTANTES", type: ToastType.Warning, position: Position.TopCenter);
                return RedirectToAction("Index");
            }
            return View(cortante);
        }

        // POST: Cortantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cortante cortante = db.Cortante.Find(id);
            db.Cortante.Remove(cortante);
            db.SaveChanges();
            TempData["Noti"] = Notification.Show("Cortante eliminada exitosamente", "CORTANTES", type: ToastType.Success, position: Position.TopCenter);
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
