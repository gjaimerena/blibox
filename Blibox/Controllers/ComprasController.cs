using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blibox.Controllers
{

    [AuthorizeOrRedirect(Roles = "Administrador")]
    public class ComprasController : Controller
    {

        private BliboxEntities db = new BliboxEntities();

        // GET: CtaCteClientes/Create
        public ActionResult Index()
        {
            ViewBag.ID_proveedor = new SelectList(db.Proveedor, "ID_proveedor", "Razon_social");
           
            return View();
        }

        // POST: CtaCteClientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Compras compra)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //ATENCION!!!
                    //EL SALDO DE MOV CTA CTE ESTA EN DESUSO, SOLO SE USA EL SALDO DEL CLIENTE
                    Proveedor proveedor = db.Proveedor.Where(m => m.ID_proveedor == compra.ID_proveedor).FirstOrDefault();
                    compra.Proveedor = proveedor;

                    db.Compras.Add(compra);

                    //asiento compra en cta cte de proveedores
                    CtaCteProveedores mov = new CtaCteProveedores();
                    mov.Proveedor = proveedor;
                    
                    //si no se le asigno saldo al crear proveedor se inicializa en cero
                    if (mov.Proveedor.Saldo == null) mov.Proveedor.Saldo = 0;
                    mov.tipo_movimiento = 1;
                    mov.Proveedor.Saldo = mov.Proveedor.Saldo - compra.Total;
                    mov.concepto = "Factura Compra Nº " + compra.Nro_factura;
                    mov.fecha_movimiento = compra.Fecha_compra.Value;
                    mov.id_proveedor = proveedor.ID_proveedor;
                    mov.importe = compra.Total.Value;
                    mov.observaciones = compra.Observaciones;
                    
                    db.CtaCteProveedores.Add(mov);



                    db.SaveChanges();
                    HelperController.Instance.agregarMensaje("Movimiento generado exitosamente", HelperController.CLASE_EXITO);
                }
                catch (Exception ex)
                {
                    HelperController.Instance.agregarMensaje("Error al intentar generar movimiento, intente nuevamente", HelperController.CLASE_ERROR);
                    throw;
                }

                return RedirectToAction("Index");
            }

            ViewBag.ID_proveedor = new SelectList(db.Proveedor, "ID_proveedor", "Razon_social", compra.ID_proveedor);
           
            return View(compra);
        }
    }
}