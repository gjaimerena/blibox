using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blibox.Models
{
    public class reporteIvaVentas
    {
        public string tipo { get; set; }
        public int nroFactura { get; set; }
        public DateTime fecha { get; set; }
        public int idCliente { get; set; }
        public string razonSocial { get; set; }
        public decimal subTotal { get; set; }
        public decimal iva { get; set; }

    }
}