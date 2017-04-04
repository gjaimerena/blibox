using Blibox.Models;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
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
            static string tipo;

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

                if (tipoCbte == "1")
                {
                    lblTipoCbte = "Factura: ";
                    tipo = "01";
                }
                if (tipoCbte == "2")
                {
                    lblTipoCbte = "Nota Débito: ";
                    tipo = "02";
                }

                if (tipoCbte == "3")
                {
                    lblTipoCbte = "Nota Crédito: ";
                    tipo = "03";
            }
                using (BliboxEntities db = new BliboxEntities())
                {
                    try
                    {
                        factura = db.Encabezado_Factura.Where(m => m.Nro_factura == nroFactura).FirstOrDefault();

                    string codigoBarra = GenerarDatoCodigoBarra();


                        GenerarCodigoBarraFactura(300, 56, codigoBarra);
                        byte[] pdf = this.GenerarPDF(lblTipoCbte, tipo);

                        VisualizarPDF(r, pdf);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }

                }

            }

        private string GenerarDatoCodigoBarra()
        {
            string codigo = "";

            codigo = factura.Cliente.Documento;

            tipo = factura.Tipo;
            while (tipo.Length != 2) tipo = "0" + tipo;
            codigo = codigo + tipo;

            string ptoVenta = System.Configuration.ConfigurationManager.AppSettings["PuntoVenta"];
            while (ptoVenta.Length != 4) ptoVenta = "0" + ptoVenta;
            codigo = codigo + ptoVenta;

            string CAE = factura.CAE;
            while (CAE.Length != 14) CAE = "0" + CAE;
            codigo = codigo + CAE;

            codigo = codigo + factura.FechaVencimientoCAE.Value.ToString("yyyyMMdd");

            int cv = codigoVerificador(codigo);

            codigo = codigo + cv.ToString();

            return codigo;
            //El código de barras deberá contener los siguientes datos con su correspondiente orden:
            //*Clave Unica de Identificación Tributaria (C.U.I.T.) del emisor de la factura(11 caracteres)
            //* Código de tipo de comprobante(2 caracteres)
            //* Punto de venta(4 caracteres)
            //* Código de Autorización de Impresión(C.A.I.)(14 caracteres)

            //* Fecha de vencimiento(8 caracteres)

            //* Dígito verificador(1 carácter)
        }

        private byte[] GenerarPDF(string lblTipoCbte, string tipo)
        {
            byte[] pdf = null;
            string path;
            ReportViewer report = new ReportViewer();
            report.ProcessingMode = ProcessingMode.Remote;

            var ubicacionReporte = string.Empty;

            path = ConfigurationManager.AppSettings["ReportFactura"];
            string cuitBlibox = ConfigurationManager.AppSettings["CUITBLIBOX"];
            string ordenCompra = (factura.OrdenCompra == null) ? "" : factura.OrdenCompra.ToString();
            string remito = (factura.Nro_remito == null) ? "" : factura.Nro_remito.ToString();
            report.LocalReport.ReportPath = path;


            List<ReportParameter> res = new List<ReportParameter>();

            //nro de comobante lo armo con el punto de venta y el nro comprobante dado por afip
            string nrocomprobante = factura.NroComprobante.ToString();
            //nro comprobante debe tener 8 digitos y 3 el pto venta
            while (nrocomprobante.Length < 8) nrocomprobante = "0" + nrocomprobante;
            

            res.Add(new ReportParameter("NroComprobante", nrocomprobante));
            res.Add(new ReportParameter("lblTipoCbte", lblTipoCbte));
            res.Add(new ReportParameter("lblCodigoCbte", "COD. " + tipo));
            res.Add(new ReportParameter("Cliente_OrdenCompra", ordenCompra));
            //res.Add(new ReportParameter("tipoComprobante", "Remito"));

            DateTime fechaEmision = (factura.Fecha != null) ? (DateTime)factura.Fecha : DateTime.Now;
            res.Add(new ReportParameter("Fecha", fechaEmision.ToString("dd-MM-yyyy")));
            res.Add(new ReportParameter("CUIT", cuitBlibox));

            res.Add(new ReportParameter("Cliente_RazonSocial", factura.Cliente.Razon_Social));
            res.Add(new ReportParameter("Cliente_Domicilio", factura.Cliente.Domicilio + " - " + factura.Cliente.Localidad + " - " + factura.Cliente.Provincia));
            
            res.Add(new ReportParameter("Cliente_TipoResponsable", factura.Cliente.TipoResponsables.Descripcion));

            res.Add(new ReportParameter("Cliente_CUIT", factura.Cliente.Documento));
            res.Add(new ReportParameter("CondicionVenta", factura.Condicion_venta.Descripcion));
            res.Add(new ReportParameter("Cliente_Remito", remito));

            //footer
            res.Add(new ReportParameter("CAE", factura.CAE));

            DateTime fechaVencCAE = (factura.FechaVencimientoCAE != null) ? (DateTime)factura.FechaVencimientoCAE : DateTime.Now;
            res.Add(new ReportParameter("VencimientoCAE", fechaVencCAE.ToString("dd-MM-yyyy")));
            


            res.Add(new ReportParameter("Subtotal", factura.Subtotal.ToString()));
            decimal? montoIva = factura.Total - factura.Subtotal;

            res.Add(new ReportParameter("Cliente_IVA", factura.IVA.ToString()));
            res.Add(new ReportParameter("Total", factura.Total.ToString()));

           
            report.LocalReport.EnableExternalImages = true;

            string pathCodigoBarra = ConfigurationManager.AppSettings["pathCodigoBarra"];
            string imagePath = new Uri(pathCodigoBarra).AbsoluteUri;
            res.Add(new ReportParameter("pathImage", imagePath));

            report.LocalReport.SetParameters(res);

            report.LocalReport.Refresh();
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
                        Cantidad = det.Cantidad,
                        PrecioUnitario = det.Precio_unitario,
                        PrecioTotal = det.Precio_total

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

        public void GenerarCodigoBarraFactura(int imgWidth, int imgHeight, string data)
        {

            //Read in the parameters
            string strData = data;
            int imageHeight = imgHeight;
            int imageWidth = imgWidth;
            string Forecolor = "000000";
            string Backcolor = "FFFFFF";

            //string strImageFormat = "PNG";

            BarcodeLib.TYPE type = BarcodeLib.TYPE.Interleaved2of5;


            System.Drawing.Image barcodeImage = null;
            try
            {
                BarcodeLib.Barcode b = new BarcodeLib.Barcode();
                
                b.Alignment = BarcodeLib.AlignmentPositions.CENTER;
                b.LabelFont = new Font("Arial", 9f, FontStyle.Regular);
                if (type != BarcodeLib.TYPE.UNSPECIFIED)
                {
                    b.IncludeLabel = true;

                    //===== Encoding performed here =====
                    barcodeImage = b.Encode(type, strData.Trim(), System.Drawing.ColorTranslator.FromHtml("#" + Forecolor), System.Drawing.ColorTranslator.FromHtml("#" + Backcolor), imageWidth, imageHeight);
                    //===================================

                    //===== Static Encoding performed here =====
                    //barcodeImage = BarcodeLib.Barcode.DoEncode(type, this.txtData.Text.Trim(), this.chkGenerateLabel.Checked, this.btnForeColor.BackColor, this.btnBackColor.BackColor);
                    //==========================================

                    // Response.ContentType = "image/" + strImageFormat;
                    //System.IO.MemoryStream MemStream = new System.IO.MemoryStream();
                    string pathCodigoBarra = ConfigurationManager.AppSettings["pathCodigoBarra"];
                    barcodeImage.Save(pathCodigoBarra, ImageFormat.Png);

                    //MemStream.WriteTo(Response.OutputStream);
                }//if
            }//try
            catch (Exception ex)
            {
                //TODO: find a way to return this to display the encoding error message
            }//catch
            finally
            {
                if (barcodeImage != null)
                {
                    //Clean up / Dispose...
                    barcodeImage.Dispose();
                }
            }//finally

        }

        private int codigoVerificador(string codigo)
        {

            int sumadorImpares = 0;
            int sumadorPares = 0;

            for (int i=0; i<codigo.Length; i++)
            {

                int codigoInt = 0;
                Int32.TryParse((codigo[i]).ToString(), out codigoInt);
                if ((i % 2) == 0)
                {
                    //Paso 1
                    sumadorImpares = sumadorImpares + codigoInt;
                }
                else
                {
                    //Paso 3
                    sumadorPares = sumadorPares + codigoInt;
                }
            }

            //Paso 2
            sumadorImpares = sumadorImpares * 3;
            //Paso4

            int suma = sumadorImpares + sumadorPares;
            int digitoVerificador = 0;
            //paso5
            for (int nro = 0; nro<=9 ; nro++)
            {
                if (((nro + suma) % 10) == 0)
                {
                    digitoVerificador = nro;
                    break;
                }
            }

            return digitoVerificador;

            //Se considera para efectuar el cálculo el siguiente ejemplo:
            //01234567890
            //Etapa 1: Comenzar desde la izquierda, sumar todos los caracteres ubicados en las posiciones impares.
            //0 + 2 + 4 + 6 + 8 + 0 = 20
            //Etapa 2: Multiplicar la suma obtenida en la etapa 1 por el número 3.
            //20 x 3 = 60
            //Etapa 3: Comenzar desde la izquierda, sumar todos los caracteres que están ubicados en las posiciones pares.
            //1 + 3 + 5 + 7 + 9 = 25
            //Etapa 4: Sumar los resultados obtenidos en las etapas 2 y 3.
            //60 + 25 = 85
            //Etapa 5: Buscar el menor número que sumado al resultado obtenido en la etapa 4 dé un número múltiplo de 10.Este será el valor del dígito verificador del módulo 10.
            //85 + 5 = 90
            //De esta manera se llega a que el número 5 es el dígito verificador módulo 10 para el código 01234567890
            //Siendo el resultado final:
            //012345678905


        }
        #endregion

        
    }
  
}