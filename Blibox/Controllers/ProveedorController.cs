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

namespace Blibox.Models
{
    public class ProveedorController : Controller
    {
        private BliboxEntities db = new BliboxEntities();
        private List<SelectListItem> items = new List<SelectListItem>();

        // GET: Proveedor
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

            var query = db.Proveedor.OrderBy(m => m.ID_proveedor);

            if (!String.IsNullOrEmpty(q))
            {
                query = query.Where(s =>
                    s.ID_proveedor.ToString().Contains(q) ||
                    s.Contacto.Contains(q) ||
                    s.CUIT.ToString().Contains(q) ||
                    s.Razon_social.Contains(q) ||
                    s.Telefono.ToString().Contains(q)
                    ).OrderBy(m => m.ID_proveedor);
            }




            return View(query.ToPagedList(page, pageSize));
        }
        //public ActionResult Index(String searchString, String Items)
        //{
        //    //cargo el listview con los campos disponible spara buuqueda
        //    items.Add(new SelectListItem() { Text = "Todos", Value = "0", Selected = true });
        //    items.Add(new SelectListItem() { Text = "ID Proveedor", Value = "ID Proveedor" });
        //    items.Add(new SelectListItem() { Text = "Razon Social", Value = "Razon Social" });
        //    items.Add(new SelectListItem() { Text = "CUIT", Value = "CUIT" });


        //    ViewBag.Items = items;


        //    var proveedor = from a in db.Proveedor select a;

        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        switch (Items)
        //        {
        //            case "ID Proveedor":
        //                proveedor = proveedor.Where(a => a.ID_proveedor.ToString().Equals(searchString));
        //                break;
        //            case "Razon Social":
        //                proveedor = proveedor.Where(a => a.Razon_social.Contains(searchString));
        //                break;
        //            case "CUIT":
        //                proveedor = proveedor.Where(a => a.CUIT.ToString().Equals(searchString));
        //                break;
        //            default:
        //                break;

        //        }

        //    }


        //    return View(proveedor.ToList());
        //    //return View(db.Proveedor.ToList());
        //}

        // GET: Proveedor/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proveedor proveedor = db.Proveedor.Find(id);
            if (proveedor == null)
            {
                return HttpNotFound();
            }
            return View(proveedor);
        }

        // GET: Proveedor/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Proveedor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_proveedor,Razon_social,Contacto,Domicilio,Localidad,Provincia,Telefono,Celular,Email,Fecha_alta,IVA,CUIT,Saldo,Observaciones")] Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                db.Proveedor.Add(proveedor);
                db.SaveChanges();
                TempData["Noti"] = Notification.Show("Proveedor generado exitosamente", "PROVEEDORES", type: ToastType.Success, position: Position.TopCenter);
                return RedirectToAction("Index");
            }

            return View(proveedor);
        }

        // GET: Proveedor/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["Noti"] = Notification.Show("Id nulo", "PROVEEDORES", type: ToastType.Warning, position: Position.TopCenter);
                return RedirectToAction("Index");
            }
            Proveedor proveedor = db.Proveedor.Find(id);
            if (proveedor == null)
            {
                TempData["Noti"] = Notification.Show("Id no asociado a un proveedor", "PROVEEDORES", type: ToastType.Warning, position: Position.TopCenter);
                return RedirectToAction("Index");
            }
            return View(proveedor);
        }

        // POST: Proveedor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(proveedor).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Noti"] = Notification.Show("Pedido modificado exitosamente", "PROVEEDORES", type: ToastType.Success, position: Position.TopCenter);
                return RedirectToAction("Index");
            }
            return View(proveedor);
        }

        // GET: Proveedor/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["Noti"] = Notification.Show("Id nulo", "PROVEEDORES", type: ToastType.Warning, position: Position.TopCenter);
                return RedirectToAction("Index");
            }
            Proveedor proveedor = db.Proveedor.Find(id);
            if (proveedor == null)
            {
                TempData["Noti"] = Notification.Show("Id no asociado a un proveedor", "PROVEEDORES", type: ToastType.Warning, position: Position.TopCenter);
                return RedirectToAction("Index");
            }
            return View(proveedor);
        }

        // POST: Proveedor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Proveedor proveedor = db.Proveedor.Find(id);
            db.Proveedor.Remove(proveedor);
            db.SaveChanges();
            TempData["Noti"] = Notification.Show("Pedido eliminado exitosamente", "PROVEEDORES", type: ToastType.Success, position: Position.TopCenter);
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
