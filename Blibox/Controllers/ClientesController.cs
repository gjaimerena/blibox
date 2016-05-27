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
    public class ClientesController : Controller
    {
        private BliboxEntities db = new BliboxEntities();
        private List<SelectListItem> items = new List<SelectListItem>();


        // GET: Clientes
        public ActionResult Index(string sortOrder, string q, int page = 1, int pageSize = 10)
        {

            ViewBag.searchQuery = String.IsNullOrEmpty(q) ? "" : q;

            page = page > 0 ? page : 1;
            pageSize = pageSize > 0 ? pageSize : 10;

            //ViewBag.IdSortParam = sortOrder == "id" ? "id_desc" : "id";
            //ViewBag.DescripcionSort = sortOrder == "descripcion" ? "descripcion_desc" : "descripcion";
            //ViewBag.DateSortParam = sortOrder == "date" ? "date_desc" : "date";

            ViewBag.CurrentSort = sortOrder;

            var query = db.Cliente.OrderBy(m => m.ID_cliente);

            if (!String.IsNullOrEmpty(q))
            {
                query = query.Where(s =>
                    s.ID_cliente.ToString().Contains(q) ||
                    s.Razon_Social.Contains(q) ||
                    s.Observaciones.Contains(q) ||
                    s.Fecha_alta.ToString().Contains(q) ||
                    s.Telefono.ToString().Contains(q)
                    ).OrderBy(m => m.ID_cliente);
            }




            return View(query.ToPagedList(page, pageSize));
        }

        //public ActionResult Index(String searchString, String Items)
        //{
        //    //cargo el listview con los campos disponible spara buuqueda
        //    items.Add(new SelectListItem() { Text = "Todos", Value = "0", Selected = true });
        //    items.Add(new SelectListItem() { Text = "ID_Cliente", Value = "ID_Cliente" });
        //    items.Add(new SelectListItem() { Text = "Razon Social", Value = "Razon Social" });
        //    items.Add(new SelectListItem() { Text = "CUIT", Value = "CUIT" });


        //    ViewBag.Items = items;


        //    var cliente = from a in db.Cliente select a;

        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        switch (Items)
        //        {
        //            case "Razon Social":
        //                cliente = cliente.Where(a => a.Razon_Social.Contains(searchString));
        //                break;
        //            case "ID_Cliente":
        //                cliente = cliente.Where(a => a.ID_cliente.ToString().Equals(searchString));
        //                break;
        //            case "CUIT":
        //                cliente = cliente.Where(a => a.CUIT.ToString().Equals(searchString));
        //                break;
        //            default:
        //                break;

        //        }

        //    }


        //    return View(cliente.ToList());
        //    // var cliente = db.Cliente.Include(c => c.Rubro).Include(c => c.Vendedor);
        //    //return View(cliente.ToList());
        //}

        // GET: Clientes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // GET: Clientes/Create
        public ActionResult Create()
        {
            ViewBag.ID_rubro = new SelectList(db.Rubro, "ID_rubro", "Descirpcion");
            ViewBag.ID_vendedor = new SelectList(db.Vendedor, "ID_vendedor", "Nombre");
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_cliente,Razon_Social,Contacto,Domicilio,Localidad,Provincia,Codigo_Postal,Telefono,Email,ID_vendedor,Comision_vendedor,IVA,CUIT,Saldo,Observaciones,Referidos,ID_rubro,DiasFF,Dias_Cheque,Grupo_mailing,Limite_credito,Fecha_alta")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
               // cliente.ID_cliente = db.Cliente.OrderByDescending(m => m.ID_cliente).FirstOrDefault().ID_cliente + 1;
                db.Cliente.Add(cliente);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_rubro = new SelectList(db.Rubro, "ID_rubro", "Descirpcion", cliente.ID_rubro);
            ViewBag.ID_vendedor = new SelectList(db.Vendedor, "ID_vendedor", "Nombre", cliente.ID_vendedor);
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_rubro = new SelectList(db.Rubro, "ID_rubro", "Descirpcion", cliente.ID_rubro);
            ViewBag.ID_vendedor = new SelectList(db.Vendedor, "ID_vendedor", "Nombre", cliente.ID_vendedor);
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_cliente,Razon_Social,Contacto,Domicilio,Localidad,Provincia,Codigo_Postal,Telefono,Email,ID_vendedor,Comision_vendedor,IVA,CUIT,Saldo,Observaciones,Referidos,ID_rubro,DiasFF,Dias_Cheque,Grupo_mailing,Limite_credito,Fecha_alta")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cliente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_rubro = new SelectList(db.Rubro, "ID_rubro", "Descirpcion", cliente.ID_rubro);
            ViewBag.ID_vendedor = new SelectList(db.Vendedor, "ID_vendedor", "Nombre", cliente.ID_vendedor);
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cliente cliente = db.Cliente.Find(id);
            db.Cliente.Remove(cliente);
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
