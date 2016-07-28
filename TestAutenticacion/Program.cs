using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blibox.Logica.Facturacion;
using Blibox.Logica.Model;

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
            ConsoleKeyInfo cki;

            do
            {
                Console.WriteLine(":: Menu para pruebas de AFIP facturacion electronica ::");
                Console.WriteLine("");
                Console.WriteLine("Presione una opcion: ");
                Console.WriteLine("1. Verificacar LoginTicket.");
                Console.WriteLine("2. Autorizacion de factura");
                Console.WriteLine("ESC. Salir");

                cki = Console.ReadKey();

                if (cki.Key == ConsoleKey.D1 || cki.Key == ConsoleKey.NumPad1)
                {
                    AutenticateAFIP();
                }
                if (cki.Key == ConsoleKey.D2 || cki.Key == ConsoleKey.NumPad2)
                {
                    DetalleRegistros[] detalles = new DetalleRegistros[1];

                    //factura sin errores
                    DetalleRegistros det = new DetalleRegistros();
                    det.Concepto = 1; //producto
                    det.DocTipo = 80; //cuit
                    det.DocNro = 20111111112;
                    det.CbteDesde = 3;
                    det.CbteHasta = 3;
                    det.CbteFch = "20160727";
                    det.ImpTotal = 121; //si importe total es mayor a cero debe existir IVAAlic
                    det.ImpTotConc = 0;
                    det.ImpNeto = 100;
                    det.ImpOpEx = 0;
                    det.ImpTrib = 0;
                    det.ImpIVA = 21;
                    det.MonId = "PES";
                    det.MonCotiz = 1;

                    IVA[] iva = new IVA[1];
                    iva[0] = new IVA();
                    iva[0].Id = 5; //21%
                    iva[0].BaseImp = 100;
                    iva[0].Importe = 21;

                    det.Iva = iva;

                    detalles[0] = det;


                    FE.AutorizacionFactura(30707744398,1,1,001,detalles);
                    //Console.WriteLine(FE.ReadElementsXML());
                }

                Console.Read(); //pause
                Console.Clear();
            } while (cki.Key != ConsoleKey.Escape);


        }

        public static void AutenticateAFIP()
        {
            string strUrlWsaaWsdl = DEFAULT_URLWSAAWSDL;
            string strIdServicioNegocio = DEFAULT_SERVICIO;
            string strRutaCertSigner = DEFAULT_CERTSIGNER;
            bool blnVerboseMode = DEFAULT_VERBOSE;

            LoginTicket login = new LoginTicket();
            Ticket ticket = login.ObtenerLoginTicketResponse(strIdServicioNegocio, strUrlWsaaWsdl, strRutaCertSigner, blnVerboseMode);


            Console.WriteLine("Sign: " + ticket.sign);
            Console.WriteLine("Token: " + ticket.token);
            Console.ReadLine(); //Pause
        }

        public void createSOAPXml()
        {
            
        }
    }
}
