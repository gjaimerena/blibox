using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blibox.Models
{
    public class itemFactura
    {
        public int IdArticulo { get; set; }
        public decimal cantidad { get; set; }
        public decimal precioUnitario { get; set; }
        public decimal precioTotal { get; set; }
    }
}