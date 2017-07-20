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
    public class MatricesController : Controller
    {
        private BliboxEntities db = new BliboxEntities();
        private List<SelectListItem> items = new List<SelectListItem>();

        // GET: Matrices
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

            var query = db.Matriz.OrderBy(m => m.ID_matriz);

            if (!String.IsNullOrEmpty(q))
            {
                query = query.Where(s =>
                    //s.ID_matriz.ToString().Contains(q) ||
                    s.Descripcion.Contains(q) ||
                    s.Observaciones.Contains(q) ||
                    s.Sector.ToString().Contains(q) ||
                    s.Bocas.ToString().Contains(q) ||
                    s.Codigo.ToString().Contains(q)
                    ).OrderBy(m => m.ID_matriz);
            }

            return View(query.ToPagedList(page, pageSize));
        }
        //public ActionResult Index(String searchString, String Items)
        //{
        //    //cargo el listview con los campos disponible spara buuqueda
        //    items.Add(new SelectListItem() { Text = "Todos", Value = "0", Selected = true });
        //    items.Add(new SelectListItem() { Text = "Codigo", Value = "Codigo" });
        //    items.Add(new SelectListItem() { Text = "Descripcion", Value = "Descripcion" });

        //    ViewBag.Items = items;

        //    var matrices = from a in db.Matriz
        //                   select a;
        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        switch (Items)
        //        {
        //            case "Descripcion":
        //                matrices = matrices.Where(a => a.Descripcion.Contains(searchString));
        //                break;
        //            case "Codigo":
        //                matrices = matrices.Where(a => a.Codigo.ToString().Equals(searchString));
        //                break;
        //            default:
        //                break;

        //        }

        //    }

        //    return View(matrices.ToList());

        //}

        // GET: Matrices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["Noti"] = Notification.Show("Id nulo", "MATRICES", type: ToastType.Warning, position: Position.TopCenter);
                return RedirectToAction("Index");
            }
            Matriz matriz = db.Matriz.Find(id);
            if (matriz == null)
            {
                TempData["Noti"] = Notification.Show("Id no asociado a una matriz", "MATRICES", type: ToastType.Warning, position: Position.TopCenter);
                return RedirectToAction("Index");
            }
            return View(matriz);
        }

        // GET: Matrices/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Matrices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_matriz,Codigo,Descripcion,Sector,Bocas,Observaciones")] Matriz matriz)
        {
            var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
            try
            {
                if (ModelState.IsValid)
                {
                    //  matriz.ID_matriz = db.Matriz.OrderByDescending(m => m.ID_matriz).FirstOrDefault().ID_matriz + 1;
                    db.Matriz.Add(matriz);
                    db.SaveChanges();
                    TempData["Noti"] = Notification.Show("Matriz generada exitosamente", "MATRICES", type: ToastType.Success, position: Position.TopCenter);
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Noti"] = Notification.Show(errors.ElementAtOrDefault(0).ElementAtOrDefault(0).ErrorMessage, "ERROR", type: ToastType.Error);
                }
            }
            catch (Exception)
            {

                TempData["Noti"] = Notification.Show("Error de conexion a la base de datos, consulta al Administrador", type: ToastType.Error);
            }
           

            return View(matriz);
        }

        // GET: Matrices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["Noti"] = Notification.Show("Id nulo", "MATRICES", type: ToastType.Warning, position: Position.TopCenter);
                return RedirectToAction("Index");
            }
            Matriz matriz = db.Matriz.Find(id);
            if (matriz == null)
            {
                TempData["Noti"] = Notification.Show("Id no asociado a una matriz", "MATRICES", type: ToastType.Warning, position: Position.TopCenter);
                return RedirectToAction("Index");
            }
            return View(matriz);
        }

        // POST: Matrices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_matriz,Codigo,Descripcion,Sector,Bocas,Observaciones")] Matriz matriz)
        {
            if (ModelState.IsValid)
            {
                db.Entry(matriz).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Noti"] = Notification.Show("Matriz modificada exitosamente", "MATRICES", type: ToastType.Success, position: Position.TopCenter);
                return RedirectToAction("Index");
            }
            return View(matriz);
        }

        // GET: Matrices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Matriz matriz = db.Matriz.Find(id);
            if (matriz == null)
            {
                return HttpNotFound();
            }
            return View(matriz);
        }

        // POST: Matrices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try {
                Matriz matriz = db.Matriz.Find(id);
                db.Matriz.Remove(matriz);
                db.SaveChanges();
                TempData["Noti"] = Notification.Show("Matriz eliminada exitosamente", "MATRICES", type: ToastType.Success, position: Position.TopCenter);
            }
            catch (Exception)
            {
                TempData["Noti"] = Notification.Show("No se puede eliminar la matriz pues tiene dependencias con Articulos", "MATRICES", type: ToastType.Error, position: Position.TopCenter);
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
