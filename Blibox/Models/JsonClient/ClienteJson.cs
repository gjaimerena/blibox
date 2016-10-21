using System;
using System.Collections.Generic;

namespace Blibox.Controllers
{
    internal class ClienteJson
    {
        public int ID_cliente { get; set; }
        public string Razon_Social { get; set; }
        //public string Contacto { get; set; }
        //public string Domicilio { get; set; }
        //public string Localidad { get; set; }
        //public string Provincia { get; set; }
        //public Nullable<int> Codigo_Postal { get; set; }
        //public Nullable<int> Telefono { get; set; }
        //public string Email { get; set; }
        //public Nullable<int> ID_vendedor { get; set; }
        //public string Comision_vendedor { get; set; }
        public string CondicionIVA { get; set; }
        public string TipoResponsable { get; set; }
        //public Nullable<int> Saldo { get; set; }
        //public string Observaciones { get; set; }
        //public string Referidos { get; set; }
        //public Nullable<int> ID_rubro { get; set; }
        public Nullable<int> DiasFF { get; set; }
        public Nullable<int> Dias_Cheque { get; set; }
        //public string Grupo_mailing { get; set; }
        //public string Limite_credito { get; set; }
        //public Nullable<System.DateTime> Fecha_alta { get; set; }
        public Nullable<int> TipoDocumento { get; set; }
        public string Documento { get; set; }
        public List<Articulo> Articulos { get; set; }
    }
}