using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blibox.Models
{
    public class reporteFactura
    {
        public string tipo { get; set; }
        public int nroFactura { get; set; }
        public int? nroRemito { get; set; }
        public DateTime fecha { get; set; }
        public int nroCliente { get; set; }
        public string razonSocial { get; set; }
        public string vendedor { get; set; }
        public int? cv { get; set; }
        public decimal? importe { get; set; }

    }
}