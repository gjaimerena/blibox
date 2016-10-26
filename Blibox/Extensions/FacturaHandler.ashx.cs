using Blibox.Models;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            static string lblTipoCbte;

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

                string tipoCbte = context.Request.QueryString["tipoCbte"];
                
                if (tipoCbte == "1") lblTipoCbte = "Factura: ";
                if (tipoCbte == "2") lblTipoCbte = "Nota Débito: ";
                if (tipoCbte == "3") lblTipoCbte = "Nota Crédito: ";

                using (BliboxEntities db = new BliboxEntities())
                {
                    try
                    {
                        factura = db.Encabezado_Factura.Where(m => m.Nro_factura == nroFactura).FirstOrDefault();
                        byte[] pdf = this.GenerarPDF(lblTipoCbte);

                        VisualizarPDF(r, pdf);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }

                }

            }

            private byte[] GenerarPDF(string lblTipoCbte)
            {
                byte[] pdf = null;
                string path;
                ReportViewer report = new ReportViewer();
                report.ProcessingMode = ProcessingMode.Remote;

                var ubicacionReporte = string.Empty;

                path = ConfigurationManager.AppSettings["ReportFactura"];
            //HttpContext.Current.Server.MapPath("ModeloFactura.rdlc");
            report.LocalReport.ReportPath = path;


            List<ReportParameter> res = new List<ReportParameter>();

            //nro de comobante lo armo con el punto de venta y el nro comprobante dado por afip
            string nrocomprobante = factura.NroComprobante.ToString();
            //nro comprobante debe tener 8 digitos y 3 el pto venta
            while (nrocomprobante.Length < 8) nrocomprobante = "0" + nrocomprobante;
            nrocomprobante = "001-" + nrocomprobante;

            res.Add(new ReportParameter("NroComprobante", nrocomprobante));
            res.Add(new ReportParameter("lblTipoCbte", lblTipoCbte));
            //res.Add(new ReportParameter("tipoComprobante", "Remito"));

            DateTime fechaEmision = (factura.Fecha != null) ? (DateTime)factura.Fecha : DateTime.Now;
            res.Add(new ReportParameter("Fecha", fechaEmision.ToString("dd-MM-yyyy")));
            res.Add(new ReportParameter("CUIT", "30-70774439-8"));

            res.Add(new ReportParameter("Cliente_RazonSocial", factura.Cliente.Razon_Social));
            res.Add(new ReportParameter("Cliente_Domicilio", factura.Cliente.Domicilio + " - " + factura.Cliente.Localidad + " - " + factura.Cliente.Provincia));
            
            res.Add(new ReportParameter("Cliente_TipoResponsable", factura.Cliente.TipoResponsables.Descripcion));

            res.Add(new ReportParameter("Cliente_CUIT", factura.Cliente.Documento));
            res.Add(new ReportParameter("CondicionVenta", factura.Condicion_venta.Descripcion));
            res.Add(new ReportParameter("Cliente_Remito", factura.Nro_remito.ToString()));

            //footer
            res.Add(new ReportParameter("CAE", factura.CAE));

            DateTime fechaVencCAE = (factura.FechaVencimientoCAE != null) ? (DateTime)factura.FechaVencimientoCAE : DateTime.Now;
            res.Add(new ReportParameter("VencimientoCAE", fechaVencCAE.ToString("dd-MM-yyyy")));
            


            res.Add(new ReportParameter("Subtotal", factura.Subtotal.ToString()));
            decimal? montoIva = factura.Total - factura.Subtotal;

            res.Add(new ReportParameter("Cliente_IVA", factura.IVA.ToString()));
            res.Add(new ReportParameter("Total", factura.Total.ToString()));

            report.LocalReport.SetParameters(res);

            ReportDataSource dsFactura = new ReportDataSource("Factura", GetFacturaDetalle(factura.Detalle_factura.ToList()));

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