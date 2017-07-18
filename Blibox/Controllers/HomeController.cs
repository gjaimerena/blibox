using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blibox.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private BliboxEntities db = new BliboxEntities();

        public ActionResult Index()
        {

            List<Pedido> pedidos = db.Pedido.Select(m => m).ToList();
            int pendientes = 0;

            foreach(Pedido p in pedidos)
            {
                if (p.cantidad_pedida != p.cantidad_entregada) pendientes++;
            }

            ViewBag.pedidospendientes = pendientes;

            int clientes = db.Cliente.Count();
                    
            ViewBag.clientes = clientes;

            int articulos = db.Articulo.Count();

            ViewBag.articulos = articulos;


            return View();
        }

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
    }
}