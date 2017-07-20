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
    public class MarcosController : Controller
    {
        private BliboxEntities db = new BliboxEntities();

        // GET: Marcos
        public ActionResult Index(string sortOrder, string currentFilter, string q, int page = 1, int pageSize = 10)
        {

            ViewBag.searchQuery = String.IsNullOrEmpty(q) ? "" : q;
            page = page > 0 ? page : 1;
            pageSize = pageSize > 0 ? pageSize : 10;
            //para paginado
            if (q == null) q = currentFilter;
            ViewBag.CurrentFilter = q;
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
                TempData["Noti"] = Notification.Show("Id nulo", "MARCOS", type: ToastType.Warning, position: Position.TopCenter);
                return RedirectToAction("Index");
            }
            Marco marco = db.Marco.Find(id);
            if (marco == null)
            {
                TempData["Noti"] = Notification.Show("Id no asociado a un marco", "MARCOS", type: ToastType.Warning, position: Position.TopCenter);
                return RedirectToAction("Index");
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
                TempData["Noti"] = Notification.Show("Marco generado exitosamente", "MARCOS", type: ToastType.Success, position: Position.TopCenter);
                //                HelperController.Instance.agregarMensaje("Se ha grabado con exito", HelperController.CLASE_EXITO);
                return RedirectToAction("Index");
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
                TempData["Noti"] = Notification.Show(errors.ElementAt(0).ElementAt(0).ErrorMessage, "ERROR", type: ToastType.Error);
            }

            return View(marco);
        }

        // GET: Marcos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["Noti"] = Notification.Show("Id nulo", "MARCOS", type: ToastType.Warning, position: Position.TopCenter);
                return RedirectToAction("Index");
            }
            Marco marco = db.Marco.Find(id);
            if (marco == null)
            {
                TempData["Noti"] = Notification.Show("Id no asociado a un marco", "MARCOS", type: ToastType.Warning, position: Position.TopCenter);
                return RedirectToAction("Index");
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
            var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();

            if (ModelState.IsValid)
            {
                db.Entry(marco).State = EntityState.Modified;
                db.SaveChanges();
                // HelperController.Instance.agregarMensaje("Se ha grabado con exito", HelperController.CLASE_EXITO);
                TempData["Noti"] = Notification.Show("Marco modificado exitosamente", "MARCOS", type: ToastType.Success, position: Position.TopCenter);
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Noti"] = Notification.Show(errors[0][0].ErrorMessage, "MARCOS", type: ToastType.Error, position: Position.TopCenter);
              //  HelperController.Instance.agregarMensaje(errors[0][0].ErrorMessage, HelperController.CLASE_ERROR);
            }
            return View(marco);
        }

        // GET: Marcos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["Noti"] = Notification.Show("ID nulo", "MARCOS", type: ToastType.Warning, position: Position.TopCenter);
                return RedirectToAction("Index");
            }
            Marco marco = db.Marco.Find(id);
            if (marco == null)
            {
                TempData["Noti"] = Notification.Show("Id no asociado a un marco", "MARCOS", type: ToastType.Warning, position: Position.TopCenter);
                return RedirectToAction("Index");
            }
            return View(marco);
        }

        // POST: Marcos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try {
                Marco marco = db.Marco.Find(id);
                db.Marco.Remove(marco);
                db.SaveChanges();
                TempData["Noti"] = Notification.Show("Marco eliminado exitosamente", "MARCOS", type: ToastType.Success, position: Position.TopCenter);
            }
            catch (Exception)
            {
                TempData["Noti"] = Notification.Show("No se puede eliminar el marco pues tiene dependencias con Articulos", "MARCOS", type: ToastType.Error, position: Position.TopCenter);
                //throw;
            }
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
