using Blibox.Models;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blibox
{
   

        /// <summary>
        /// Summary description for Handler1
        /// </summary>
        public class FacturaHandler : IHttpHandler
        {
            #region IHttpHandler Members
            static Blibox.Encabezado_Factura factura = null;

            public bool IsReusable
            {
                // Return false in case your Managed Handler cannot be reused for another request.
                // Usually this would be false in case you have some state information preserved per request.
                get { return true; }
            }

            public void ProcessRequest(HttpContext context)
            {
                HttpResponse r = context.Response;
                //write your handler implementation here.
                int nroFactura = Convert.ToInt32(context.Request.QueryString["nroFactura"]);

                using (BliboxEntities db = new BliboxEntities())
                {
                    try
                    {
                        factura = db.Encabezado_Factura.Where(m => m.Nro_factura == nroFactura).FirstOrDefault();
                        byte[] pdf = this.GenerarPDF();

                        VisualizarPDF(r, pdf);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }

                }

            }

            private byte[] GenerarPDF()
            {
                byte[] pdf = null;
                string path;
                ReportViewer report = new ReportViewer();
                report.ProcessingMode = ProcessingMode.Remote;

                var ubicacionReporte = string.Empty;

                path = HttpContext.Current.Server.MapPath("ModeloFactura.rdlc");
                report.LocalReport.ReportPath = path;

                List<ReportParameter> res = new List<ReportParameter>();

                res.Add(new ReportParameter("nroComprobante", factura.Nro_factura.ToString()));
                //res.Add(new ReportParameter("tipoComprobante", "Remito"));

                DateTime fechaEmision = (factura.Fecha != null) ? (DateTime)factura.Fecha : DateTime.Now;
                res.Add(new ReportParameter("fechaEmision", fechaEmision.ToString("dd-MM-yyyy")));
                res.Add(new ReportParameter("nroCUIT", "30-70774439-8"));

                res.Add(new ReportParameter("subtotal", factura.Subtotal.ToString()));
                decimal? montoIva = factura.Total - factura.Subtotal;

                res.Add(new ReportParameter("montoIva", montoIva.ToString()));
                res.Add(new ReportParameter("total", factura.Total.ToString()));

                report.LocalReport.SetParameters(res);

                ReportDataSource dsFactura = new ReportDataSource("dsFacturaDetalle", GetFacturaDetalle(factura.Detalle_factura.ToList()));

                report.LocalReport.DataSources.Clear();
                report.LocalReport.DataSources.Add(dsFactura);
                pdf = report.LocalReport.Render("PDF");

                return pdf;


            }

            public List<DetalleCustom> GetFacturaDetalle(List<Detalle_factura> detalle)
            {

                List<DetalleCustom> detalles = new List<DetalleCustom>();

                foreach (var det in detalle)
                {
                    DetalleCustom detalleCustom = new DetalleCustom
                    {
                        Articulo = det.Articulo.Descripcion,
                        Cantidad = (det.Cantidad != null) ? (Decimal)det.Cantidad : 0,
                        PrecioUnitario = (det.Precio_unitario != null) ? (Decimal)det.Precio_unitario : 0,
                        PrecioTotal = (det.Precio_total != null) ? (Decimal)det.Precio_total : 0

                    };


                    detalles.Add(detalleCustom);

                }
                return detalles;
            }

            public void VisualizarPDF(HttpResponse r, byte[] pdf)
            {
                if (pdf != null)
                {
                    try
                    {
                        r.ContentType = "application/pdf";
                        r.AddHeader("content-length", pdf.Length.ToString());
                        r.BinaryWrite(pdf);
                    }
                    catch
                    {

                    }
                }
                else
                {
                    r.Redirect("~/Error.aspx");
                }
            }

            #endregion
        }
  
}