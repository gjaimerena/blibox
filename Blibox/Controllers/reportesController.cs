using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using System.Globalization;

namespace Blibox.Controllers
{
    public class reportesController : Controller
    {
        private BliboxEntities db = new BliboxEntities();

        // GET: CtaCteClientes
        public ActionResult Index(string sortOrder, string q, int page = 1, int pageSize = 10)
        {

            int id_cliente = Convert.ToInt32(Request["ID_cliente"]);
            int id_vendedor = Convert.ToInt32(Request["ID_vendedor"]);

            string fechadesde = (Request["Fecha Desde"] == null) ? "" : Request["Fecha Desde"].ToString();
            string fechahasta = (Request["Fecha Hasta"] == null) ? "" : Request["Fecha Hasta"].ToString();
            DateTime? desde = null, hasta = null;

            if (fechadesde != "") desde = DateTime.ParseExact(fechadesde, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            if (fechahasta != "") hasta = DateTime.ParseExact(fechahasta, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            ViewBag.searchQuery = String.IsNullOrEmpty(q) ? "" : q;

            page = page > 0 ? page : 1;
            pageSize = pageSize > 0 ? pageSize : 10;

            //ViewBag.IdSortParam = sortOrder == "id" ? "id_desc" : "id";
            //ViewBag.DescripcionSort = sortOrder == "descripcion" ? "descripcion_desc" : "descripcion";
            //ViewBag.DateSortParam = sortOrder == "date" ? "date_desc" : "date";

            ViewBag.CurrentSort = sortOrder;

            var query = db.Encabezado_Factura.Include(c => c.Cliente);

            if (fechadesde != "" || fechahasta != "")
            {
                query = query.Where(f => (f.Fecha >= desde) && (f.Fecha <= hasta) && (f.ID_cliente == id_cliente));

                if (query.ToList().Count == 0)
                {
                    HelperController.Instance.agregarMensaje("No se encuentran resultado para la consulta.", HelperController.CLASE_ADVERTENCIA);
                    
                }
                //else
                //{
                //    HelperController.Instance.agregarMensaje("Movimiento cargado con exito", HelperController.CLASE_EXITO);
                //}
            }

            ViewBag.Total = query.Sum(m => m.Total);
            query = query.OrderByDescending(m => m.Fecha);

            List<Models.reporteFactura> reporte = new List<Models.reporteFactura>();
            List<Encabezado_Factura> queryList = query.ToList();

            foreach (Encabezado_Factura item in queryList)
            {
                Models.reporteFactura registro = new Models.reporteFactura();
                registro.nroFactura = item.Nro_factura;
                registro.fecha = item.Fecha;
                registro.nroCliente = item.ID_cliente;
                registro.importe = item.Total;
                registro.nroRemito = item.Nro_remito;
                registro.vendedor = (item.Vendedor != null) ? item.Vendedor.Apellido: string.Empty;
                reporte.Add(registro);
            }
            //habilitar si se desea implementar la busqueda de movimientos
            //if (!String.IsNullOrEmpty(q))
            //{
            //    query = query.Where(s =>
            //        s.id.ToString().Contains(q) ||
            //        s.concepto.Contains(q) ||
            //        s.fecha_movimiento.ToString().Contains(q) ||
            //        s.nro_comprobante.ToString().Contains(q) 

            //        ).OrderBy(m => m.id);
            //}

            ViewBag.ID_cliente = new SelectList(db.Cliente, "ID_cliente", "Razon_Social");
            ViewBag.ID_vendedor = new SelectList(db.Vendedor, "ID_vendedor", "Apellido");
            
           return View(reporte.ToPagedList(page, pageSize));

        }
    }
}