using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Blibox.Models;
using Blibox.Logica;
using System.Collections.Generic;
using System.Net;

namespace Blibox.Controllers
{
    /**
     * Clase para mostrar mensajes por pantalla 
     * ej:
            HelperController.Instance.agregarMensaje("Exito", HelperController.CLASE_EXITO);
            HelperController.Instance.agregarMensaje("Error", HelperController.CLASE_ERROR);
            HelperController.Instance.agregarMensaje("Advertencia", HelperController.CLASE_ADVERTENCIA);

        Y solo mostrara el mensaje
     */
    public class HelperController : Controller
    {
        public const string CLASE_ERROR = "danger";
        public const string CLASE_ADVERTENCIA = "warning";
        public const string CLASE_EXITO = "success";
        
        private static HelperController instance;

        private List<MapaString> mensajes = new List<MapaString>();
        private HelperController() {}

        public static HelperController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HelperController();
                }
                return instance;
            }
        }
        
        /*
        public HelperController()
        {
            this.mensajes = new List<MapaString>();
        }
        public HelperController(List<MapaString> mensajes)
        {
            this.mensajes = mensajes;
        }
        */
        public List<MapaString> getMensajes()
        {
            List<MapaString> ret = new List<MapaString>(HelperController.Instance.mensajes);
            HelperController.Instance.mensajes = new List<MapaString>();
            return ret;
        }

        public void agregarMensaje(string mensaje, string clase)
        {
            //this.mensajes.Add(WebUtility.HtmlEncode("<div class='alert alert-dismissable alert-"+clase+"'>"+"<button type='button' class='close' data-dismiss='alert'>&times;</button>"+mensaje+"</div>"));

            HelperController.Instance.mensajes.Add( new MapaString(mensaje, clase));
        }
    }

    
}
namespace Blibox.Logica
{

    public class MapaString
    {
        protected string mensaje;
        protected string clase;

        public MapaString(string unMensaje, string unaClase)
        {
            this.mensaje = unMensaje;
            this.clase = unaClase;
        }
        public void setMensaje(string unMensaje)
        {
            this.mensaje = unMensaje;
        }
        public string getMensaje()
        {
            return this.mensaje;
        }
        public void setClase(string unaClase)
        {
            this.clase = unaClase;
        }
        public string getClase()
        {
            return this.clase;
        }
    }
}