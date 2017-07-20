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
using Blibox.Controllers;

namespace Blibox.Models
{
    [Authorize]
    public class ClientesController : Controller
    {
        private BliboxEntities db = new BliboxEntities();
        private List<SelectListItem> items = new List<SelectListItem>();


        // GET: Clientes
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
            ViewBag.TipoResponsables = new SelectList(db.TipoResponsables, "Codigo", "Descripcion");
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Cliente cliente)
        {
            cliente.Fecha_alta = DateTime.Now;

            if (ModelState.IsValid)
            {
                // cliente.ID_cliente = db.Cliente.OrderByDescending(m => m.ID_cliente).FirstOrDefault().ID_cliente + 1;
                cliente.TipoDocumento = 25; //valor para CUIT
               
                db.Cliente.Add(cliente);
                db.SaveChanges();
                TempData["Noti"] = Notification.Show("Cliente generado con éxito", "ALTA DE CLIENTES", type: ToastType.Success, position: Position.TopCenter);
                //HelperController.Instance.agregarMensaje("Se generó con éxito", HelperController.CLASE_EXITO);
                return RedirectToAction("Index");
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();
                TempData["Noti"] = Notification.Show(errors.ElementAt(0).ElementAt(0).ErrorMessage, "ERROR", type: ToastType.Error);
            }

           
            ViewBag.ID_rubro = new SelectList(db.Rubro, "ID_rubro", "Descirpcion", cliente.ID_rubro);
            ViewBag.ID_vendedor = new SelectList(db.Vendedor, "ID_vendedor", "Nombre", cliente.ID_vendedor);
            ViewBag.TipoResponsables = new SelectList(db.TipoResponsables, "Codigo", "Descripcion",cliente.TipoResponsable);
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["Noti"] = Notification.Show("Id de cliente nulo", "EDICION DE CLIENTES", type: ToastType.Error, position: Position.TopCenter);
                return RedirectToAction("Index");
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                TempData["Noti"] = Notification.Show("El Id no coresponde a un cliente", "EDICION DE CLIENTES", type: ToastType.Error, position: Position.TopCenter);
                return RedirectToAction("Index");
            }
            ViewBag.ID_rubro = new SelectList(db.Rubro, "ID_rubro", "Descirpcion", cliente.ID_rubro);
            ViewBag.ID_vendedor = new SelectList(db.Vendedor, "ID_vendedor", "Nombre", cliente.ID_vendedor);
            ViewBag.TipoResponsables = new SelectList(db.TipoResponsables, "Codigo", "Descripcion", cliente.TipoResponsable);
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_cliente,Razon_Social,Contacto,Domicilio,Localidad,Provincia,Codigo_Postal, Telefono,Email, ID_vendedor,Comision_vendedor, CondicionIVA, TipoResponsable, Saldo, Observaciones, Referidos, limite_credito,ID_rubro, DiasFF, Dias_Cheque, Grupo_mailing, Fecha_alta, TipoDocumento, Documento,id_ctacte")]Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cliente).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Noti"] = Notification.Show("Cliente actualizado con éxito", "EDICION DE CLIENTES", type: ToastType.Success, position: Position.TopCenter);
                //HelperController.Instance.agregarMensaje("Se actualizó con éxito", HelperController.CLASE_EXITO);
                return RedirectToAction("Index");
            }
            ViewBag.ID_rubro = new SelectList(db.Rubro, "ID_rubro", "Descirpcion", cliente.ID_rubro);
            ViewBag.ID_vendedor = new SelectList(db.Vendedor, "ID_vendedor", "Nombre", cliente.ID_vendedor);
            ViewBag.TipoResponsables = new SelectList(db.TipoResponsables, "Codigo", "Descripcion", cliente.TipoResponsable);
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData["Noti"] = Notification.Show("Id de cliente nulo", "BAJA DE CLIENTES", type: ToastType.Error, position: Position.TopCenter);
                return RedirectToAction("Index");
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                TempData["Noti"] = Notification.Show("El Id no coresponde a un cliente", "BAJA DE CLIENTES", type: ToastType.Error, position: Position.TopCenter);
                return RedirectToAction("Index");
            }
            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Cliente cliente = db.Cliente.Find(id);
                db.Cliente.Remove(cliente);
                db.SaveChanges();
                TempData["Noti"] = Notification.Show("Cliente dado de baja con éxito", "BAJA DE CLIENTES", type: ToastType.Success, position: Position.TopCenter);
                //HelperController.Instance.agregarMensaje("Se eliminó con éxito", HelperController.CLASE_EXITO);
            }
            catch (Exception)
            {
                TempData["Noti"] = Notification.Show("No se puede eliminar el Cliente pues tiene dependencias de facturacion y/o Cta Cte", "BAJA DE CLIENTES", type: ToastType.Error, position: Position.TopCenter);
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
