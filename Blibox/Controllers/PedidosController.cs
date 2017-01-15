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
    public class PedidosController : Controller
    {
        private ICollection<Articulo> articulosColeccion = new HashSet<Articulo>();
        private BliboxEntities db = new BliboxEntities();

        // GET: Materiales
        public ActionResult Index(string sortOrder, string q, int page = 1, int pageSize = 10)
        {

            ViewBag.searchQuery = String.IsNullOrEmpty(q) ? "" : q;
            page = page > 0 ? page : 1;
            pageSize = pageSize > 0 ? pageSize : 10;
            //ViewBag.IdSortParam = sortOrder == "id" ? "id_desc" : "id";
            //ViewBag.DescripcionSort = sortOrder == "descripcion" ? "descripcion_desc" : "descripcion";
            //ViewBag.DateSortParam = sortOrder == "date" ? "date_desc" : "date";

            ViewBag.CurrentSort = sortOrder;

            var query = db.Pedido.OrderBy(m => m.ID_pedido);

            if (!String.IsNullOrEmpty(q))
            {
                query = query.Where(s =>
                    s.ID_pedido.ToString().Contains(q) ||
                    s.Cliente.Razon_Social.Contains(q) ||
                    s.Observaciones.Contains(q) ||
                    s.Precio.ToString().Contains(q) 
                 
                    ).OrderBy(m => m.ID_pedido);
            }
            return View(query.ToPagedList(page, pageSize));
        }

        // GET: Pedidos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedido.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            getDropdownElements(pedido);
            return View(pedido);
        }

        // GET: Pedidos/Create
        public ActionResult Create()
        {
            getDropdownElements(new Pedido());
            return View();
        }

        // POST: Pedidos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "[ID_pedido,ID_cliente,ID_articulo,cantidad_pedida,cantidad_entregada,Fecha_pedido,Orden_compra,Armado,Fecha_armado,Fecha_entrega,Precio,Observaciones,Impreso,ID_vendedor,Importe_matriz,Tipo_caja,Codigo_articulo_interno_cliente")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                db.Pedido.Add(pedido);
                db.SaveChanges();
                HelperController.Instance.agregarMensaje("El pedido se cargo con exito", HelperController.CLASE_EXITO);
                return RedirectToAction("Index");
            } else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();
                    foreach(var error in errors)
                    {
                        // cut for brevity, need to add back more code from original
                        HelperController.Instance.agregarMensaje(error.ToString(), HelperController.CLASE_ERROR);

                    }
                HelperController.Instance.agregarMensaje(errors.ToString(), HelperController.CLASE_ERROR);

            }
            getDropdownElements(pedido);
            return View(pedido);
        }
        private void getDropdownElements(Pedido pedido)
        {
            List<Articulo> articulos = db.Articulo.Select(m => m).ToList();
            articulosColeccion = new HashSet<Articulo>();
            for (int i = 0; i < articulos.Count; i++)
            {
                articulosColeccion.Add(new Articulo { ID_articulo = articulos.ElementAt(i).ID_articulo, Descripcion = articulos.ElementAt(i).Descripcion });
            }

            //ViewBag.articulo = articulosColeccion;
            ViewBag.ID_cliente = new SelectList(db.Cliente, "ID_cliente", "Razon_Social", pedido.ID_cliente);

            ViewBag.ID_vendedor = new SelectList(db.Vendedor, "ID_vendedor", "nombre", pedido.ID_vendedor);
            ViewBag.ID_articulo = new SelectList(db.Articulo, "ID_articulo", "Descripcion", pedido.ID_articulo);

        }
        // GET: Pedidos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedido.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            getDropdownElements(pedido);
            return View(pedido);
        }

        // POST: Pedidos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pedido).State = EntityState.Modified;
                db.SaveChanges();
                HelperController.Instance.agregarMensaje("El pedido se edito con exito", HelperController.CLASE_EXITO);
                return RedirectToAction("Index");
            }
            getDropdownElements(pedido);
            return View(pedido);
        }

        // GET: Pedidos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido Pedido = db.Pedido.Find(id);
            if (Pedido == null)
            {
                return HttpNotFound();
            }
            getDropdownElements(Pedido);
            return View(Pedido);
        }

        // POST: Pedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pedido pedido = db.Pedido.Find(id);
            db.Pedido.Remove(pedido);
            db.SaveChanges();
            HelperController.Instance.agregarMensaje("El pedido se elimino con exito", HelperController.CLASE_EXITO);
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
