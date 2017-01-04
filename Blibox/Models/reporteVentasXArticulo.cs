using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blibox.Models
{
    public class reporteVentasXArticulo
    {
    
            public string tipo { get; set; }
            public int nroFactura { get; set; }
            public DateTime fecha { get; set; }
            public int idArticulo { get; set; }
            public string nombreArticulo { get; set; }
            public decimal cantidad { get; set; }
            public decimal importe { get; set; }

    }
}
