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
    public class VendedoresController : Controller
    {
        private BliboxEntities db = new BliboxEntities();
        private List<SelectListItem> items = new List<SelectListItem>();
        // GET: Vendedores
        //public ActionResult Index(String searchString, String Items)
        //{


        //    //cargo el listview con los campos disponible spara buuqueda
        //    items.Add(new SelectListItem() { Text = "Todos", Value = "0", Selected = true });
        //    items.Add(new SelectListItem() { Text = "ID_Vendedor", Value = "ID_Vendedor" });
        //    items.Add(new SelectListItem() { Text = "Apellido", Value = "Apellido" });


        //    ViewBag.Items = items;


        //    var vendedor = from a in db.Vendedor select a;

        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        switch (Items)
        //        {
        //            case "Apellido":
        //                vendedor = vendedor.Where(a => a.Apellido.Contains(searchString));
        //                break;
        //            case "ID_Vendedor":
        //                vendedor = vendedor.Where(a => a.ID_vendedor.ToString().Equals(searchString));
        //                break;
        //            default:
        //                break;

        //        }

        //    }


        //    return View(vendedor.ToList());
        //}

        public ActionResult Index(string sortOrder, string q, int page = 1, int pageSize = 10)
        {

            ViewBag.searchQuery = String.IsNullOrEmpty(q) ? "" : q;

            page = page > 0 ? page : 1;
            pageSize = pageSize > 0 ? pageSize : 10;

            //ViewBag.IdSortParam = sortOrder == "id" ? "id_desc" : "id";
            //ViewBag.DescripcionSort = sortOrder == "descripcion" ? "descripcion_desc" : "descripcion";
            //ViewBag.DateSortParam = sortOrder == "date" ? "date_desc" : "date";

            ViewBag.CurrentSort = sortOrder;

            var query = db.Vendedor.OrderBy(m => m.ID_vendedor);

            if (!String.IsNullOrEmpty(q))
            {
                query = query.Where(s =>
                    s.ID_vendedor.ToString().Contains(q) ||
                    s.Nombre.Contains(q) ||
                    s.Apellido.Contains(q) ||
                    s.Observacion.Contains(q) ||
                    s.Telefono.ToString().Contains(q) 
                    ).OrderBy(m => m.ID_vendedor);
            }



           
            return View(query.ToPagedList(page, pageSize));
        }

        // GET: Vendedores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendedor vendedor = db.Vendedor.Find(id);
            if (vendedor == null)
            {
                return HttpNotFound();
            }
            return View(vendedor);
        }

        // GET: Vendedores/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Vendedores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_vendedor,Nombre,Apellido,Domicilio,Localidad,Provincia,Telefono,Celular,Email,Fecha_alta,Observacion,Codigo_postal")] Vendedor vendedor)
        {
            if (ModelState.IsValid)
            {
               // vendedor.ID_vendedor = db.Vendedor.OrderByDescending(m => m.ID_vendedor).FirstOrDefault().ID_vendedor + 1;
                  
                db.Vendedor.Add(vendedor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vendedor);
        }

        // GET: Vendedores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendedor vendedor = db.Vendedor.Find(id);
            if (vendedor == null)
            {
                return HttpNotFound();
            }
            return View(vendedor);
        }

        // POST: Vendedores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_vendedor,Nombre,Apellido,Domicilio,Localidad,Provincia,Telefono,Celular,Email,Fecha_alta,Observacion,Codigo_postal")] Vendedor vendedor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vendedor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vendedor);
        }

        // GET: Vendedores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendedor vendedor = db.Vendedor.Find(id);
            if (vendedor == null)
            {
                return HttpNotFound();
            }
            return View(vendedor);
        }

        // POST: Vendedores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vendedor vendedor = db.Vendedor.Find(id);
            db.Vendedor.Remove(vendedor);
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
