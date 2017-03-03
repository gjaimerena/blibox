using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using System.Globalization;
using ClosedXML.Excel;
using System.IO;
using System.Reflection;

namespace Blibox.Controllers
{
    public class reportesController : Controller
    {
        private BliboxEntities db = new BliboxEntities();

        // GET: CtaCteClientes
        public ActionResult Index(string sortOrder, string q, int page = 1, int pageSize = 10)
        {
            // Obtengo datos de los filtros de busqueda //
            int id_cliente = 0;
            int id_vendedor = 0;
            if (Request["ID_cliente"] != null)
            {
                Int32.TryParse(Request["ID_cliente"],out id_cliente);
            }
            if (Request["ID_vendedor"] != null)
            {
                Int32.TryParse(Request["ID_vendedor"],out id_vendedor);
            }

            string fechadesde = (Request["Fecha Desde"] == null) ? "" : Request["Fecha Desde"].ToString();
            string fechahasta = (Request["Fecha Hasta"] == null) ? "" : Request["Fecha Hasta"].ToString();
            DateTime? desde = null, hasta = null;

            if (fechadesde != "") desde = DateTime.ParseExact(fechadesde, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            if (fechahasta != "") hasta = DateTime.ParseExact(fechahasta, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            //Fin de obtencion de datos de filtro de busqueda

            ViewBag.searchQuery = String.IsNullOrEmpty(q) ? "" : q;

            page = page > 0 ? page : 1;
            pageSize = pageSize > 0 ? pageSize : 10;

            //ViewBag.IdSortParam = sortOrder == "id" ? "id_desc" : "id";
            //ViewBag.DescripcionSort = sortOrder == "descripcion" ? "descripcion_desc" : "descripcion";
            //ViewBag.DateSortParam = sortOrder == "date" ? "date_desc" : "date";

            ViewBag.CurrentSort = sortOrder;

            var query = db.Encabezado_Factura.Include(c => c.Cliente);

            //armo distintas querys
            if (id_cliente != 0)
            {
                query = query.Where(f => (f.ID_cliente == id_cliente));
            }
            if (id_vendedor != 0)
            {
                query = query.Where(f => (f.ID_vendedor == id_vendedor));
            }

            if (fechadesde != "" && fechahasta == "")
            {
                query = query.Where(f => (f.Fecha >= desde)); 
            }
            if (fechadesde == "" && fechahasta != "")
            {
                query = query.Where(f => (f.Fecha <= hasta));
            }
            if ((fechadesde != "") && (fechahasta != ""))
            {
                query = query.Where(f => (f.Fecha >= desde) && (f.Fecha <= hasta));
            }

            if (query.ToList().Count == 0)
            {
                TempData["Noti"] = Notification.Show("No se encontraron resultado para la consulta", "REPORTES", type: ToastType.Success, position: Position.TopCenter);
               // HelperController.Instance.agregarMensaje("No se encuentran resultado para la consulta.", HelperController.CLASE_ADVERTENCIA);
            }


            ViewBag.Total = query.Sum(m => m.Total);
            query = query.OrderByDescending(m => m.Fecha);

            List<Models.reporteFactura> reporte = new List<Models.reporteFactura>();
            List<Encabezado_Factura> queryList = query.ToList();

            foreach (Encabezado_Factura item in queryList)
            {
                Models.reporteFactura registro = new Models.reporteFactura();
                registro.tipo = "A";
                registro.nroFactura = item.Nro_factura;
                registro.fecha = item.Fecha;
                registro.nroCliente = item.ID_cliente;
                registro.importe = item.Total;
                registro.nroRemito = item.Nro_remito;
                registro.vendedor = (item.Vendedor != null) ? item.Vendedor.Apellido: string.Empty;
                reporte.Add(registro);
            }

            HttpContext.Session["query"] = reporte;
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

        public ActionResult DetalleXArticulo(string sortOrder, string q, int page = 1, int pageSize = 10)
        {
            int id_articulo_desde = 0;
            int id_articulo_hasta = 0;
            if (Request["id_articulo_desde"] != null)
            {
                Int32.TryParse(Request["id_articulo_desde"], out id_articulo_desde);
            }
            if (Request["id_articulo_hasta"] != null)
            {
                Int32.TryParse(Request["id_articulo_hasta"], out id_articulo_hasta);
            }

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

            var query = db.Detalle_factura.Include(c => c.Articulo).Include(f => f.Encabezado_Factura);

            //armo distintas querys
            if (id_articulo_desde != 0 && id_articulo_hasta == 0)
            {
                query = query.Where(f => (f.ID_articulo >= id_articulo_desde));
            }
            if (id_articulo_desde == 0 && id_articulo_hasta != 0)
            {
                query = query.Where(f => (f.ID_articulo <= id_articulo_hasta));
            }
            if ((id_articulo_desde != 0) && (id_articulo_hasta != 0))
            {
                query = query.Where(f => (f.ID_articulo >= id_articulo_desde) && (f.ID_articulo <= id_articulo_hasta));
            }

            if (fechadesde != "" && fechahasta == "")
            {
                query = query.Where(f => (f.Encabezado_Factura.Fecha >= desde));
            }
            if (fechadesde == "" && fechahasta != "")
            {
                query = query.Where(f => (f.Encabezado_Factura.Fecha <= hasta));
            }
            if ((fechadesde != "") && (fechahasta != ""))
            {
                query = query.Where(f => (f.Encabezado_Factura.Fecha >= desde) && (f.Encabezado_Factura.Fecha <= hasta));
            }

            if (query.ToList().Count == 0)
            {
                TempData["Noti"] = Notification.Show("No se encontraron resultado para la consulta", "REPORTES", type: ToastType.Success, position: Position.TopCenter);
                //HelperController.Instance.agregarMensaje("No se encuentran resultado para la consulta.", HelperController.CLASE_ADVERTENCIA);
            }
            else
            {
                ViewBag.Total = query.Sum(m => m.Precio_total);
                query = query.OrderByDescending(m => m.Encabezado_Factura.Fecha);

                
            }

            List<Models.reporteVentasXArticulo> reporte = new List<Models.reporteVentasXArticulo>();
            List<Detalle_factura> queryList = query.ToList();

            foreach (Detalle_factura item in queryList)
            {
                Models.reporteVentasXArticulo registro = new Models.reporteVentasXArticulo();
                registro.tipo = "A";
                registro.nroFactura = item.Nro_factura;
                registro.fecha = item.Encabezado_Factura.Fecha;
                registro.idArticulo = item.ID_articulo;
                registro.nombreArticulo = item.Articulo.Descripcion;
                registro.cantidad = item.Cantidad;
                registro.importe = item.Precio_total;
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
            HttpContext.Session["queryTotalxArticulo"] = reporte;

            ViewBag.id_articulo_desde = new SelectList(db.Articulo, "ID_articulo", "ID_articulo");
            ViewBag.id_articulo_hasta = new SelectList(db.Articulo, "ID_articulo", "ID_articulo");

            return View(reporte.ToPagedList(page, pageSize));

        }

        public ActionResult IVAVentas(string sortOrder, string q, int page = 1, int pageSize = 10)
        {
           
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

            //armo distintas querys
           
            if (fechadesde != "" && fechahasta == "")
            {
                query = query.Where(f => (f.Fecha >= desde));
            }
            if (fechadesde == "" && fechahasta != "")
            {
                query = query.Where(f => (f.Fecha <= hasta));
            }
            if ((fechadesde != "") && (fechahasta != ""))
            {
                query = query.Where(f => (f.Fecha >= desde) && (f.Fecha <= hasta));
            }

            if (query.ToList().Count == 0)
            {
                TempData["Noti"] = Notification.Show("No se encontraron resultado para la consulta", "REPORTES", type: ToastType.Success, position: Position.TopCenter);
                // HelperController.Instance.agregarMensaje("No se encuentran resultado para la consulta.", HelperController.CLASE_ADVERTENCIA);
            }
            else
            {
                ViewBag.Subtotal = query.Sum(m => m.Subtotal).Value;

                decimal total = query.Sum(m => m.Total).Value;
                ViewBag.IvaVentas = total - ViewBag.Subtotal;
                query = query.OrderBy(m => m.Fecha);


            }

            List<Models.reporteIvaVentas> reporte = new List<Models.reporteIvaVentas>();
            List<Encabezado_Factura> queryList = query.ToList();

            foreach (Encabezado_Factura item in queryList)
            {
                Models.reporteIvaVentas registro = new Models.reporteIvaVentas();
                registro.tipo = "A";
                registro.nroFactura = item.Nro_factura;
                registro.fecha = item.Fecha;
                registro.idCliente = item.ID_cliente;
                registro.razonSocial = item.Cliente.Razon_Social;
                registro.subTotal = item.Subtotal.Value;
                registro.iva = item.Total.Value - item.Subtotal.Value;
                reporte.Add(registro);
            }

            HttpContext.Session["queryivaventas"] = reporte;
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

            return View(reporte.ToPagedList(page, pageSize));

        }

        public ActionResult ExportDataTotalVentasXArticulo()
        {

            DataTable dt = new DataTable();
            dt.TableName = "Reporte";
            List<Models.reporteVentasXArticulo> reporte = (List<Models.reporteVentasXArticulo>)HttpContext.Session["queryTotalxArticulo"];
            
            dt = ToDataTable(reporte);

            dt.Columns["tipo"].ColumnName = "tipo";
            dt.Columns["nroFactura"].ColumnName = "Factura Nº";
            dt.Columns["fecha"].ColumnName = "Fecha";
            dt.Columns["idArticulo"].ColumnName = "Id Articulo";
            dt.Columns["nombreArticulo"].ColumnName = "Articulo";
            dt.Columns["cantidad"].ColumnName = "Cantidad";
            dt.Columns["importe"].ColumnName = "Importe";

            dt.AcceptChanges();

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);

                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=TotalVentasXArticulo.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream, false);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            return RedirectToAction("DetalleXArticulo", "reportes");
        }

        public ActionResult ExportDataTotalVentas()
        {

            DataTable dt = new DataTable();
            dt.TableName = "Reporte";
            List<Models.reporteFactura> reporte = (List<Models.reporteFactura>)HttpContext.Session["query"];

            dt = ToDataTable(reporte);

            dt.Columns["tipo"].ColumnName = "tipo";
            dt.Columns["nroFactura"].ColumnName = "Factura Nº";
            dt.Columns["fecha"].ColumnName = "Fecha";
            dt.Columns["nroCliente"].ColumnName = "Cliente Nº";
            dt.Columns["importe"].ColumnName = "Importe";
            dt.Columns["nroRemito"].ColumnName = "Remito";
            dt.Columns["vendedor"].ColumnName = "Vendedor";


            dt.AcceptChanges();

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);

                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=TotalVentas.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream, false);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            return RedirectToAction("Index", "reportes");
        }

        public ActionResult ExportDataIVAVentas()
        {

            DataTable dt = new DataTable();
            dt.TableName = "Iva_Ventas";
            List<Models.reporteIvaVentas> reporte = (List<Models.reporteIvaVentas>)HttpContext.Session["queryivaventas"];

            dt = ToDataTable(reporte);

            dt.Columns["tipo"].ColumnName = "tipo";
            dt.Columns["nroFactura"].ColumnName = "Factura Nº";
            dt.Columns["fecha"].ColumnName = "Fecha";
            dt.Columns["idCliente"].ColumnName = "Cliente Nº";
            dt.Columns["razonSocial"].ColumnName = "Razon Social";
            dt.Columns["subTotal"].ColumnName = "SubTotal";
            dt.Columns["iva"].ColumnName = "IVA";

            dt.AcceptChanges();

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);

                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=IVAVentas.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream, false);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            return RedirectToAction("IVAVentas", "reportes");
        }

        private DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
    }
}