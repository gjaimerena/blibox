using Blibox.Logica.Model;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Blibox.Logica.Facturacion
{
    public class FE
    {

        public static FECAERespuesta AutorizacionFactura(int nroRegistros, int PtoVta, int CbteTipo, DetalleRegistros[] detalles)
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
            auth.Cuit = Convert.ToInt64(ConfigurationManager.AppSettings["CUIT"]);

            // REQUEST
            /** FeCAEReq: Información del comprobante o lote de comprobantes de ingreso. Contiene los datos de FeCabReq y FeDetReq
            FeCabReq: Información de la cabecera del comprobante o lote de comprobantes de ingreso (OBLIGATORIO)
            FeDetReq: Información del detalle del comprobante o lote de comprobantes de ingreso (OBLIGATORIO)
            **/

            //Defino FeCabReq
            WSFEv1.FECAERequest feCAEReq = new WSFEv1.FECAERequest();

            //Genero la Cabecera
            feCAEReq.FeCabReq = new WSFEv1.FECAECabRequest();

            feCAEReq.FeCabReq.CantReg = nroRegistros; //nro de registeros del detalle del lote o lotes de comprobantes
            feCAEReq.FeCabReq.CbteTipo = CbteTipo; //Tipo de comprobante que se está informando.Si se informa más de un comprobante, todos deben ser del mismo tipo.
                                                   // 001 - Factura A, 002 Nota Debito, 003 Nota Credito
            feCAEReq.FeCabReq.PtoVta = PtoVta;

            feCAEReq.FeDetReq = new WSFEv1.FECAEDetRequest[nroRegistros];

            int Cbte = obtenerUltimoComprobante(auth, PtoVta, CbteTipo)+1;
            
            
            


            string fechaActual = DateTime.Now.ToString("yyyyMMdd");
            //Genero FeDetReq con tantos registro como indique en la cabecera
            for (int i = 0; i < nroRegistros; i++) {

                feCAEReq.FeDetReq[i] = new WSFEv1.FECAEDetRequest();

                feCAEReq.FeDetReq[i].Concepto = detalles[i].Concepto ;
                feCAEReq.FeDetReq[i].DocTipo = detalles[i].DocTipo; 
                feCAEReq.FeDetReq[i].DocNro = detalles[i].DocNro;
                feCAEReq.FeDetReq[i].CbteDesde = Cbte;
                feCAEReq.FeDetReq[i].CbteHasta = Cbte;
                feCAEReq.FeDetReq[i].CbteFch = fechaActual; //detalles[i].CbteFch;
                feCAEReq.FeDetReq[i].ImpTotal = detalles[i].ImpTotal;
                feCAEReq.FeDetReq[i].ImpTotConc = detalles[i].ImpTotConc;
                feCAEReq.FeDetReq[i].ImpNeto = detalles[i].ImpNeto;
                feCAEReq.FeDetReq[i].ImpOpEx = detalles[i].ImpOpEx;
                feCAEReq.FeDetReq[i].ImpIVA = detalles[i].ImpIVA;
                feCAEReq.FeDetReq[i].ImpTrib = detalles[i].ImpTrib;
                feCAEReq.FeDetReq[i].FchServDesde = detalles[i].FchServDesde;
                feCAEReq.FeDetReq[i].FchServHasta = detalles[i].FchServHasta;
                feCAEReq.FeDetReq[i].FchVtoPago = detalles[i].FchVtoPago;
                feCAEReq.FeDetReq[i].MonId = detalles[i].MonId;
                feCAEReq.FeDetReq[i].MonCotiz = detalles[i].MonCotiz;
              
                //CbtesAsoc
                int cantCbtesAsoc = ((detalles[i].cbtesAsoc == null) ? 0 : detalles[i].cbtesAsoc.Length);

                if (cantCbtesAsoc > 0)
                { 
                    feCAEReq.FeDetReq[i].CbtesAsoc = new WSFEv1.CbteAsoc[cantCbtesAsoc];
                
                    for (int j=0; j< cantCbtesAsoc; j++)
                    {
                        feCAEReq.FeDetReq[i].CbtesAsoc[j] = new WSFEv1.CbteAsoc();

                        feCAEReq.FeDetReq[i].CbtesAsoc[j].Tipo = detalles[i].cbtesAsoc[j].Tipo;
                        feCAEReq.FeDetReq[i].CbtesAsoc[j].Nro = detalles[i].cbtesAsoc[j].Nro;
                        feCAEReq.FeDetReq[i].CbtesAsoc[j].PtoVta = detalles[i].cbtesAsoc[j].PtoVta;
                    }
                }

                //Tributos
                int cantTributos = ((detalles[i].tributos == null) ? 0 : detalles[i].tributos.Length) ;

                if (cantTributos > 0)
                { 
                    feCAEReq.FeDetReq[i].Tributos = new WSFEv1.Tributo[cantTributos];

                    for (int j = 0; j < cantTributos; j++)
                    {
                        feCAEReq.FeDetReq[i].Tributos[j] = new WSFEv1.Tributo();

                        feCAEReq.FeDetReq[i].Tributos[j].Alic = detalles[i].tributos[j].Alic;
                        feCAEReq.FeDetReq[i].Tributos[j].BaseImp = detalles[i].tributos[j].BaseImp;
                        feCAEReq.FeDetReq[i].Tributos[j].Desc = detalles[i].tributos[j].Desc;
                        feCAEReq.FeDetReq[i].Tributos[j].Id = detalles[i].tributos[j].Id;
                        feCAEReq.FeDetReq[i].Tributos[j].Importe = detalles[i].tributos[j].Importe;

                    }
                }

                //IVA
                int cantIVA = ((detalles[i].Iva == null) ? 0 : detalles[i].Iva.Length);

                if (cantIVA > 0)
                { 
                    feCAEReq.FeDetReq[i].Iva = new WSFEv1.AlicIva[cantIVA];

                    for (int j = 0; j < cantIVA; j++)
                    {

                        feCAEReq.FeDetReq[i].Iva[j] = new WSFEv1.AlicIva();

                        feCAEReq.FeDetReq[i].Iva[j].BaseImp = detalles[i].Iva[j].BaseImp;
                        feCAEReq.FeDetReq[i].Iva[j].Id = detalles[i].Iva[j].Id;
                        feCAEReq.FeDetReq[i].Iva[j].Importe = detalles[i].Iva[j].Importe;
                    }
                }
                
                //Opcionales
                int cantOpcionales = ((detalles[i].opcionales == null) ? 0 : detalles[i].opcionales.Length);

                if (cantOpcionales > 0)
                { 
                    feCAEReq.FeDetReq[i].Opcionales = new WSFEv1.Opcional[cantOpcionales];

                    for (int j = 0; j < cantOpcionales; j++)
                    {
                        feCAEReq.FeDetReq[i].Opcionales[j] = new WSFEv1.Opcional();

                        feCAEReq.FeDetReq[i].Opcionales[j].Valor = detalles[i].opcionales[j].Valor;
                        feCAEReq.FeDetReq[i].Opcionales[j].Id = detalles[i].opcionales[j].Id;
                    }
                }



            }

            //Datos de la respuesta
            FECAERespuesta respuesta = new FECAERespuesta();
            List<Observacion> Observaciones = new List<Observacion>();

            //Solicita CAE
            WSFEv1.FECAEResponse response = fe.FECAESolicitar(auth, feCAEReq);


            //En la cabecera esta la respuesta de la solicitud
            FECAERespuestaCabecera cabecera = new FECAERespuestaCabecera()
            {
                Cuit = response.FeCabResp.Cuit,
                CantReg = response.FeCabResp.CantReg,
                CbteTipo = response.FeCabResp.CbteTipo,
                FchProceso = response.FeCabResp.FchProceso,
                PtoVta = response.FeCabResp.PtoVta,
                Resultado = response.FeCabResp.Resultado
            };
            respuesta.Cabecera = cabecera;

            int cantDetalles = ((response.FeCabResp == null) ? 0 : response.FeCabResp.CantReg);

            if (cantDetalles > 0)
            {

                List<FECAERespuestaDetalle> DetallesRespuesta = new List<FECAERespuestaDetalle>();

                for (int i=0 ; i < response.FeCabResp.CantReg; i++)
                {
                    FECAERespuestaDetalle det = new FECAERespuestaDetalle
                    {
                        CAE =  response.FeDetResp[i].CAE,
                        CAEFchVto = response.FeDetResp[i].CAEFchVto,
                        CbteDesde = response.FeDetResp[i].CbteDesde,
                        CbteFch = response.FeDetResp[i].CbteFch,
                        CbteHasta = response.FeDetResp[i].CbteHasta,
                        Concepto = response.FeDetResp[i].Concepto,
                        DocNro =  response.FeDetResp[i].DocNro,
                        DocTipo = response.FeDetResp[i].DocTipo,
                        Resultado = response.FeDetResp[i].Resultado,

                     };

                    int cantObservaciones = ((response.FeDetResp[i].Observaciones == null) ? 0 : response.FeDetResp[i].Observaciones.Length);

                    if (cantObservaciones > 0){

                        det.Observaciones = new List<Observacion>();

                        for (int j=0; j<cantObservaciones; j++)
                        det.Observaciones.Add(new Observacion {
                            Codigo =  response.FeDetResp[i].Observaciones[j].Code,
                            Mensaje = response.FeDetResp[i].Observaciones[j].Msg
                        });

                    }

                    DetallesRespuesta.Add(det);

                }

                respuesta.Detalles = DetallesRespuesta;
            }

            int cantErrores = ((response.Errors == null) ? 0 : response.Errors.Length);

            if (cantErrores > 0)
            {

                List<Error> ErroresRespuesta = new List<Error>();

                for (int i = 0; i < cantErrores; i++)
                {
                    Error err = new Error
                    {
                        Codigo = response.Errors[i].Code,
                        Mensaje = response.Errors[i].Msg
                    };

                    ErroresRespuesta.Add(err);

                }

                respuesta.Errores = ErroresRespuesta;
            }


            //FALTA GUARDAR LOS EVENTOS Y ERRORES
            return respuesta;
            
        }

        private static int obtenerUltimoComprobante(WSFEv1.FEAuthRequest auth, int ptoVenta, int cbteTipo)
        {
            try
            {
                WSFEv1.ServiceSoapClient fe = new WSFEv1.ServiceSoapClient();
               
                return fe.FECompUltimoAutorizado(auth, ptoVenta, cbteTipo).CbteNro;

            }
            catch (Exception) {
                return -1;
            }
        }

        public static FEEstado estadoAFIP()
        {
            FEEstado estado = new FEEstado();
            try
            {
                WSFEv1.ServiceSoapClient fe = new WSFEv1.ServiceSoapClient();

                var resp = fe.FEDummy();
                if (resp.AppServer.ToUpper().Equals("OK") && resp.AuthServer.ToUpper().Equals("OK") && resp.DbServer.ToUpper().Equals("OK"))
                {
                    estado.Estado = true;
                    estado.Mensaje = "Conexion AFIP disponible";

                    return estado;
                }
                else
                {
                    estado.Estado = false;
                    estado.Mensaje = "Conexion AFIP no disponible, intente en unos instantes";

                    return estado;
                }
                   

            }
            catch (Exception ex)
            {
                estado.Estado = false;
                estado.Mensaje = ex.Message.ToString();

                return estado;
            }

            
        }
    }
}
