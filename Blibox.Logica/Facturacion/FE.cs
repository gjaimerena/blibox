using Blibox.Logica.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Blibox.Logica.Facturacion
{
    public class FE
    {

        //const string DEFAULT_URLWSAAWSDL = "https://wsaa.afip.gov.ar/ws/services/LoginCms";
        //const string DEFAULT_SERVICIO = "wsfe";
        //const string DEFAULT_CERTSIGNER = "E:\\certificadosAfip\\certificate.pfx";
        //const bool DEFAULT_VERBOSE = true;

        public void AutorizacionFactura(long cuit, int nroRegistros, int PtoVta, int CbteTipo, DetalleRegistros[] detalles)
        {
            WSFEv1.ServiceSoapClient fe = new WSFEv1.ServiceSoapClient();
            WSFEv1.FEAuthRequest auth = new WSFEv1.FEAuthRequest();


            //AUTENTICACION
            /** Auth Información de la autenticación.Contiene los datos de Token, Sign y Cuit
              Token Token devuelto por el WSAA S
              Sign Sign devuelto por el WSAA S
              Cuit Cuit contribuyente(representado o Emisora) S 
            **/
            string strUrlWsaaWsdl = ConfigurationManager.AppSettings["DEFAULT_URLWSAAWSDL"];
            string strIdServicioNegocio = ConfigurationManager.AppSettings["DEFAULT_SERVICIO"]; 
            string strRutaCertSigner = ConfigurationManager.AppSettings["DEFAULT_CERTSIGNER"]; 
            bool blnVerboseMode = Convert.ToBoolean(ConfigurationManager.AppSettings["DEFAULT_VERBOSE"]); 

            LoginTicket login = new LoginTicket();
            Ticket TicketResponse = login.ObtenerLoginTicketResponse(strIdServicioNegocio, strUrlWsaaWsdl, strRutaCertSigner, blnVerboseMode);
            auth.Sign = TicketResponse.sign;
            auth.Token = TicketResponse.token; ;
            auth.Cuit = Convert.ToInt64(ConfigurationManager.AppSettings["CUIT_BLIBOX"]);

            // REQUEST
            /** FeCAEReq: Información del comprobante o lote de comprobantes de ingreso. Contiene los datos de FeCabReq y FeDetReq
            FeCabReq: Información de la cabecera del comprobante o lote de comprobantes de ingreso (OBLIGATORIO)
            FeDetReq: Información del detalle del comprobante o lote de comprobantes de ingreso (OBLIGATORIO)
            **/

            //Defino FeCabReq
            WSFEv1.FECAERequest feCAEReq = new WSFEv1.FECAERequest();
            feCAEReq.FeCabReq.CantReg = nroRegistros; //nro de registeros del detalle del lote o lotes de comprobantes
            feCAEReq.FeCabReq.CbteTipo = CbteTipo; //Tipo de comprobante que se está informando.Si se informa más de un comprobante, todos deben ser del mismo tipo.
                                                   // 001 - Factura A, 002 Nota Debito, 003 Nota Credito
            feCAEReq.FeCabReq.PtoVta = PtoVta;

            //Defino FeDetReq para cada registro
            for (int i = 0; i < nroRegistros; i++) {
                feCAEReq.FeDetReq[i].Concepto = detalles[i].Concepto ;
                feCAEReq.FeDetReq[i].DocTipo = detalles[i].DocTipo; 
                feCAEReq.FeDetReq[i].DocNro = detalles[i].DocNro;
               
                feCAEReq.FeDetReq[i].CbteDesde = detalles[i].CbteDesde;
                feCAEReq.FeDetReq[i].CbteHasta = detalles[i].CbteHasta;
                feCAEReq.FeDetReq[i].CbteFch = detalles[i].CbteFch;
                feCAEReq.FeDetReq[i].ImpTotal = detalles[i].ImpTotal;
                feCAEReq.FeDetReq[i].ImpTotConc = detalles[i].ImpTotConc;
                feCAEReq.FeDetReq[i].ImpNeto = detalles[i].ImpNeto;

                feCAEReq.FeDetReq[i].ImpOpEx = detalles[i].ImpOpEx;
                feCAEReq.FeDetReq[i].ImpIVA = detalles[i].ImpIVA;
                feCAEReq.FeDetReq[i].ImpTrib = detalles[i].ImpTrib;
                feCAEReq.FeDetReq[i].FchServDesde = detalles[i].FchServDesde;
                feCAEReq.FeDetReq[i].FchServHasta = detalles[i].FchServHasta;
                feCAEReq.FeDetReq[i].FchVtoPago = detalles[i].FchVtoPago;

                //CbtesAsoc
                int cantCbtesAsoc = detalles[i].cbtesAsoc.Length;

                for (int j=0; j< cantCbtesAsoc; j++)
                {
                    feCAEReq.FeDetReq[i].CbtesAsoc[j].Tipo = detalles[i].cbtesAsoc[j].Tipo;
                    feCAEReq.FeDetReq[i].CbtesAsoc[j].Nro = detalles[i].cbtesAsoc[j].Nro;
                    feCAEReq.FeDetReq[i].CbtesAsoc[j].PtoVta = detalles[i].cbtesAsoc[j].PtoVta;

                }

                //Tributos
                int cantTributos = detalles[i].tributos.Length;

                for (int j = 0; j < cantTributos; j++)
                {
                    feCAEReq.FeDetReq[i].Tributos[j].Alic = detalles[i].tributos[j].Alic;
                    feCAEReq.FeDetReq[i].Tributos[j].BaseImp = detalles[i].tributos[j].BaseImp;
                    feCAEReq.FeDetReq[i].Tributos[j].Desc = detalles[i].tributos[j].Desc;
                    feCAEReq.FeDetReq[i].Tributos[j].Id = detalles[i].tributos[j].Id;
                    feCAEReq.FeDetReq[i].Tributos[j].Importe = detalles[i].tributos[j].Importe;

                }

                //IVA
                int cantIVA = detalles[i].Iva.Length;

                for (int j = 0; j < cantIVA; j++)
                {
                    feCAEReq.FeDetReq[i].Iva[j].BaseImp = detalles[i].Iva[j].BaseImp;
                    feCAEReq.FeDetReq[i].Iva[j].Id = detalles[i].Iva[j].Id;
                    feCAEReq.FeDetReq[i].Iva[j].Importe = detalles[i].Iva[j].Importe;

                }

                //Opcionales
                int cantOpcionales = detalles[i].opcionales.Length;

                for (int j = 0; j < cantOpcionales; j++)
                {
                    feCAEReq.FeDetReq[i].Opcionales[j].Valor = detalles[i].opcionales[j].Valor;
                    feCAEReq.FeDetReq[i].Opcionales[j].Id = detalles[i].opcionales[j].Id;
                }

               

            }

            //Datos de la respuesta
            string CAE, FechaVtoCAE, ResultadoCbte;
            List<KeyValuePair<int, string>> Observaciones = new List<KeyValuePair<int, string>>();

            WSFEv1.FECAEResponse respuesta = fe.FECAESolicitar(auth, feCAEReq);

            //En la cabecera esta la respuesta de la solicitud
            string EstadoSolicitud = respuesta.FeCabResp.Resultado; //A=APROBADO, R=RECHAZADO, P=PARCIAL

            //Para el detalle de la factura se analiza cada registros viendo su resultado, CAE y FacheVto
            for (int k=0; k<respuesta.FeCabResp.CantReg; k++)
            {
                //Si el resultado es rechazado el campo CAE es vacio
                ResultadoCbte = respuesta.FeDetResp[k].Resultado;
                CAE = respuesta.FeDetResp[k].CAE;
                FechaVtoCAE = respuesta.FeDetResp[k].CAEFchVto;
                

                for (int m=0; m<respuesta.FeDetResp[k].Observaciones.Length; m++)
                {
                    Observaciones.Add(new KeyValuePair<int, string>(respuesta.FeDetResp[k].Observaciones[m].Code, respuesta.FeDetResp[k].Observaciones[m].Msg));
                }
                
            }
          
           
            
        }
        
    }
}
