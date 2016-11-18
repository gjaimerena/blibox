using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blibox.Models
{
    /**
    * Clase usada para la vista de los movimientos de cliente
    */

    public class CtaCteClienteMovimientos
    {
        public int id { get; set; }
        public DateTime fecha { get; set; }
        public string concepto { get; set; }
        public decimal debito { get; set; }
        public decimal credito { get; set; }
        public decimal saldo { get; set; }

    }
}