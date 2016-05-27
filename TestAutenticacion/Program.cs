using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blibox.Logica.Facturacion;
namespace TestAutenticacion
{
    class Program
    {

        // Valores por defecto, globales en esta clase 
        /*"https://wsaahomo.afip.gov.ar/ws/services/LoginCms?WSDL";*/
        const string DEFAULT_URLWSAAWSDL = "https://wsaa.afip.gov.ar/ws/services/LoginCms"; 
        const string DEFAULT_SERVICIO = "wsfe";
        const string DEFAULT_CERTSIGNER = "E:\\certificadosAfip\\certificate.pfx";
        const bool DEFAULT_VERBOSE = true;

        static void Main(string[] args)
        {

            string strUrlWsaaWsdl = DEFAULT_URLWSAAWSDL;
            string strIdServicioNegocio = DEFAULT_SERVICIO;
            string strRutaCertSigner = DEFAULT_CERTSIGNER;
            bool blnVerboseMode = DEFAULT_VERBOSE;

            LoginTicket login = new LoginTicket();
            string loginTicketResponse = login.ObtenerLoginTicketResponse(strIdServicioNegocio, strUrlWsaaWsdl, strRutaCertSigner, blnVerboseMode);


            Console.WriteLine(loginTicketResponse);

        }
    }
}
