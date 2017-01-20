using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blibox.Models
{
    public class Pedidos
    {
        public int idPedido { get; set; }
        public string cliente { get; set; }
        public int idArticulo { get; set; }
        public string descArticulo { get; set; }
        public string componente1 { get; set; }
        public string componente2 { get; set; }
        public string componente3 { get; set; }
        public int cantPedida { get; set; }
        public int cantEntregada { get; set; }
        public int cantRestante { get; set; }
    }
}