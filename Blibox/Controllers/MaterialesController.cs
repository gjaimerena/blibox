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
    [Authorize]
    public class MaterialesController : Controller
    {
        private BliboxEntities db = new BliboxEntities();

        // GET: Materiales
        public ActionResult Index(string sortOrder, string currentFilter, string q, int page = 1, int pageSize = 10)
        {

            ViewBag.searchQuery = String.IsNullOrEmpty(q) ? "" : q;
            //para paginado
            if (q == null) q = currentFilter;
            ViewBag.CurrentFilter = q;
            page = page > 0 ? page : 1;
            pageSize = pageSize > 0 ? pageSize : 10;
            //ViewBag.IdSortParam = sortOrder == "id" ? "id_desc" : "id";
            //ViewBag.DescripcionSort = sortOrder == "descripcion" ? "descripcion_desc" : "descripcion";
            //ViewBag.DateSortParam = sortOrder == "date" ? "date_desc" : "date";

            ViewBag.CurrentSort = sortOrder;

            var query = db.Material.OrderBy(m => m.ID_material);

            if (!String.IsNullOrEmpty(q))
            {
                query = query.Where(s =>
                    s.ID_material.ToString().Contains(q) ||
                    s.Descripcion.Contains(q) ||
                    s.Observaciones.Contains(q) ||
                    s.Peso.ToString().Contains(q) 
                 
                    ).OrderBy(m => m.ID_material);
            }
            return View(query.ToPagedList(page, pageSize));
        }

        // GET: Materiales/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["Noti"] = Notification.Show("Id nulo", "MATERIALES", type: ToastType.Warning, position: Position.TopCenter);
                return RedirectToAction("Index");
            }
            Material material = db.Material.Find(id);
            if (material == null)
            {
                TempData["Noti"] = Notification.Show("Id no asociado a un material", "MATERIALES", type: ToastType.Warning, position: Position.TopCenter);
                return RedirectToAction("Index");
            }
            return View(material);
        }

        // GET: Materiales/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Materiales/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_material,Descripcion,Peso,Observaciones")] Material material)
        {
            if (ModelState.IsValid)
            {
                db.Material.Add(material);
                db.SaveChanges();
                TempData["Noti"] = Notification.Show("Material generado exitosamente", "MATERIALES", type: ToastType.Success, position: Position.TopCenter);
                // HelperController.Instance.agregarMensaje("El material se cargo con exito", HelperController.CLASE_EXITO);
                return RedirectToAction("Index");
            }

            return View(material);
        }

        // GET: Materiales/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["Noti"] = Notification.Show("Id nulo", "MATERIALES", type: ToastType.Warning, position: Position.TopCenter);
                return RedirectToAction("Index");
            }
            Material material = db.Material.Find(id);
            if (material == null)
            {
                TempData["Noti"] = Notification.Show("Id no asociado a un material", "MATERIALES", type: ToastType.Warning, position: Position.TopCenter);
                return RedirectToAction("Index");
            }
            return View(material);
        }

        // POST: Materiales/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_material,Descripcion,Peso,Observaciones")] Material material)
        {
            if (ModelState.IsValid)
            {
                db.Entry(material).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Noti"] = Notification.Show("Material modificado exitosamente", "MATERIALES", type: ToastType.Success, position: Position.TopCenter);
                // HelperController.Instance.agregarMensaje("El material se edito con exito", HelperController.CLASE_EXITO);
                return RedirectToAction("Index");
            }
            return View(material);
        }

        // GET: Materiales/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Material material = db.Material.Find(id);
            if (material == null)
            {
                return HttpNotFound();
            }
            return View(material);
        }

        // POST: Materiales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Material material = db.Material.Find(id);
            db.Material.Remove(material);
            db.SaveChanges();
            TempData["Noti"] = Notification.Show("Material eliminado exitosamente", "MATERIALES", type: ToastType.Success, position: Position.TopCenter);
            //HelperController.Instance.agregarMensaje("El material se elimino con exito", HelperController.CLASE_EXITO);
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
