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
using Blibox.Models;
using Blibox.Logica.Facturacion;
using Blibox.Logica.Model;

namespace Blibox.Controllers
{
    public class FacturaController : Controller
    {
        private BliboxEntities db = new BliboxEntities();

        // GET: Factura
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

            var query = db.Encabezado_Factura.Include(e => e.Cliente).Include(e => e.Condicion_venta).Include(e => e.Detalle_factura).Include(e => e.Vendedor).OrderBy(m => m.Nro_factura);

            if (!String.IsNullOrEmpty(q))
            {
                query = query.Where(s =>
                    s.Nro_factura.ToString().Contains(q) ||
                    s.ID_movimiento.ToString().Contains(q) ||
                    s.Vendedor.Apellido.Contains(q) ||
                    s.Fecha.ToString().Contains(q) ||
                    s.Cliente.Razon_Social.Contains(q) ||
                    s.Cliente.Documento.ToString().Contains(q)
                    ).OrderBy(m => m.ID_cliente);
            }

            return View(query.ToPagedList(page, pageSize));
        }
        //public ActionResult Index()
        //{
        //    var encabezado_Factura = db.Encabezado_Factura.Include(e => e.Cliente).Include(e => e.Condicion_venta).Include(e => e.Detalle_factura).Include(e => e.Vendedor);
        //    return View(encabezado_Factura.ToList());
        //}

        // GET: Factura/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Encabezado_Factura encabezado_Factura = db.Encabezado_Factura.Find(id);
            if (encabezado_Factura == null)
            {
                return HttpNotFound();
            }
            return View(encabezado_Factura);
        }

        // GET: Factura/Create
        public ActionResult Create()
        {
            ViewBag.ID_cliente = new SelectList(db.Cliente, "ID_cliente", "Razon_Social");
            ViewBag.CondicionVenta = new SelectList(db.Condicion_venta, "ID_condicion_venta", "Descripcion");
            ViewBag.art = "";
            return View();
        }

        // POST: Factura/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection form)
        {
            if (ModelState.IsValid)
            {
                //db.Encabezado_Factura.Add(encabezado_Factura);
                //db.SaveChanges();
                //return RedirectToAction("Index");
            }

            int idCliente = Convert.ToInt32(form["ID_cliente"]);
            string documento = form["Cliente.Documento"];
            string tipoDocumento = form["Cliente.TipoResponsables.Descripcion"];
            double CondicionIVA = Convert.ToDouble(form["Cliente.CondicionIVA"].Replace('.', ','));
            string ID_condicion_venta = form["ID_condicion_venta"];
            DateTime FechaEmision = Convert.ToDateTime(form["Fecha"]);
            int Remito = Convert.ToInt32(form["Nro_remito"]);
            int OrdenCompra = Convert.ToInt32(form["OrdenCompra"]);
            int DiasFF = Convert.ToInt32(form["Cliente.DiasFF"]);
            int DiasCheque = Convert.ToInt32(form["Cliente.Dias_Cheque"]);
            Decimal Descuento = Convert.ToDecimal(form["Descuento"].Replace('.', ','));
            Double subtotal = Convert.ToDouble(form["subtotal"].Replace('.', ','));
            Double total = Convert.ToDouble(form["total"].Replace('.',','));

            for (int i = 12; i < form.Count-2; i = i + 4)
            {
                itemFactura item = new itemFactura
                {
                    IdArticulo = Convert.ToInt32(form[form.GetKey(i)]),
                    cantidad = Convert.ToDecimal(form[form.GetKey(i + 1)]),
                    precioUnitario = Convert.ToDecimal(form[form.GetKey(i + 2)]),
                    precioTotal = Convert.ToDecimal(form[form.GetKey(i + 3)])
                };

            }

            //Consdigo autoriazcion en AFIP para generar factura
            DetalleRegistros[] detalles = new DetalleRegistros[1];

            DetalleRegistros det = new DetalleRegistros();
            
            //el nro de comproabnte lo obtiene la libreria
            //Si no se le envia fecha de comprobante Afip asigna la fecha del proceso
            //det.CbteFch = FechaEmision.ToString("yyyy-MM-dd");

            det.Concepto= 1; //1=Productos
            det.DocNro = Convert.ToInt64(documento);
            det.DocTipo = 80 ; //80=CUIT
            det.ImpTotal = total;
            det.ImpNeto = subtotal;
            det.ImpTotConc = 0;
            det.ImpOpEx = 0;
            det.ImpTrib = 0;
            det.ImpIVA = subtotal * (CondicionIVA / 100);//
            det.MonId = "PES"; //peso
            det.MonCotiz = 1; //moneda argetnia es 1

            Blibox.Logica.Model.IVA[] iva = new Blibox.Logica.Model.IVA[1];
            iva[0] = new Blibox.Logica.Model.IVA();
            iva[0].Id = 5; //21%
            iva[0].BaseImp = subtotal;
            iva[0].Importe = subtotal * (CondicionIVA/100);

            det.Iva = iva;

            detalles[0] = det;

            FECAERespuesta resp = FE.AutorizacionFactura(1, 1, 001, detalles);


            // ViewBag.ID_cliente = new SelectList(db.Cliente, "ID_cliente", "Razon_Social", encabezado_Factura.ID_cliente);

            return View(form);
        }

        // GET: Factura/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Encabezado_Factura encabezado_Factura = db.Encabezado_Factura.Find(id);
            if (encabezado_Factura == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID_cliente = new SelectList(db.Cliente, "ID_cliente", "Razon_Social", encabezado_Factura.ID_cliente);
            ViewBag.ID_condicon_venta = new SelectList(db.Condicion_venta, "ID_condicion_venta", "Descripcion", encabezado_Factura.ID_condicon_venta);
            ViewBag.Nro_factura = new SelectList(db.Detalle_factura, "Nro_factura", "Nro_factura", encabezado_Factura.Nro_factura);
            ViewBag.ID_vendedor = new SelectList(db.Vendedor, "ID_vendedor", "Nombre", encabezado_Factura.ID_vendedor);
            return View(encabezado_Factura);
        }

        // POST: Factura/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Encabezado_Factura encabezado_Factura)
        {
            if (ModelState.IsValid)
            {
                db.Entry(encabezado_Factura).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_cliente = new SelectList(db.Cliente, "ID_cliente", "Razon_Social", encabezado_Factura.ID_cliente);
            ViewBag.ID_condicon_venta = new SelectList(db.Condicion_venta, "ID_condicion_venta", "Descripcion", encabezado_Factura.ID_condicon_venta);
            ViewBag.Nro_factura = new SelectList(db.Detalle_factura, "Nro_factura", "Nro_factura", encabezado_Factura.Nro_factura);
            ViewBag.ID_vendedor = new SelectList(db.Vendedor, "ID_vendedor", "Nombre", encabezado_Factura.ID_vendedor);
            return View(encabezado_Factura);
        }

        // GET: Factura/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Encabezado_Factura encabezado_Factura = db.Encabezado_Factura.Find(id);
            if (encabezado_Factura == null)
            {
                return HttpNotFound();
            }
            return View(encabezado_Factura);
        }

        // POST: Factura/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Encabezado_Factura encabezado_Factura = db.Encabezado_Factura.Find(id);
            db.Encabezado_Factura.Remove(encabezado_Factura);
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

        public ActionResult AgregarArticulo(int Id)
        {
            db.Detalle_factura.Add(new Detalle_factura { Nro_factura = Id, ID_item = 0 });
            db.SaveChanges();
            // return RedirectToAction("Edit","Articulos", art.ID_articulo.ToString()); ;
            return RedirectToAction("Edit", "Factura", new { id = Id });
        }

        public ActionResult eliminarArticulo(int Id_item, int Id_factura)
        {
            Detalle_factura comp = db.Detalle_factura.Where(m => (m.Nro_factura == Id_factura) && (m.ID_item == Id_item)).FirstOrDefault();
            db.Detalle_factura.Remove(comp);
            db.SaveChanges();
            Articulo art = db.Articulo.Select(m => m).Where(m => m.ID_articulo == comp.ID_articulo).FirstOrDefault();

            // return RedirectToAction("Edit","Articulos", art.ID_articulo.ToString()); ;
            return RedirectToAction("Edit", "Articulos", new { id = art.ID_articulo });
        }

        [HttpPost]
        public ActionResult obtenerDatosCliente(int? idCliente)
        {

            Cliente cliente = db.Cliente.Where(m=>m.ID_cliente == idCliente).FirstOrDefault();

            BliboxEntities db2 = new BliboxEntities();
            db2.Configuration.ProxyCreationEnabled = false;
            ClienteJson clientejson = new ClienteJson
            {
                Documento = cliente.Documento,

                ID_cliente = cliente.ID_cliente,
                Razon_Social = cliente.Razon_Social,
                TipoDocumento = cliente.TipoDocumento,
                TipoResponsable = db.TipoResponsables.Where(m => m.Codigo == cliente.TipoResponsable).FirstOrDefault().Descripcion,
                DiasFF = cliente.DiasFF,
                Dias_Cheque = cliente.Dias_Cheque,
                Articulos = db2.Articulo.Where(m => m.ID_cliente == idCliente).ToList(),
                CondicionIVA = cliente.CondicionIVA

            };

            ViewBag.art = clientejson.Articulos;

            return Json(clientejson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult obtenerArticulos(int? idCliente)
        {

            BliboxEntities db2 = new BliboxEntities();
            db2.Configuration.ProxyCreationEnabled = false;

            List<Articulo> articulos = db2.Articulo.Where(m => m.ID_cliente == idCliente).ToList();
            return Json(articulos, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult obtenerPrecioUnitario(int? idArticulo)
        {
            Articulo articulo = db.Articulo.Where(m => m.ID_articulo == idArticulo).FirstOrDefault();

            if (articulo!= null)
            {
                if (articulo.Precio_lista!=null) return Json(articulo.Precio_lista, JsonRequestBehavior.AllowGet);
            }
            
                return Json(0, JsonRequestBehavior.AllowGet);
        }

    }
}
