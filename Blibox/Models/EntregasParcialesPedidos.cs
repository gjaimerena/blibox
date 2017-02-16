using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blibox.Models
{
    public class EntregasParcialesPedidos
    {

        public int ID_remito { get; set; }
        public Nullable<DateTime> Fecha { get; set; }
        public int ID_pedido { get; set; }
        public Nullable<int> Cantidad { get; set; }
        
    }
}