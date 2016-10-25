using System;
using System.Collections.Generic;

namespace Blibox.Reports
{
    public class Item
    {
        public  string Articulo { get; set; }
        public  Decimal Cantidad { get; set; }
        public  Decimal PrecioUnitario { get; set; }
        public  Decimal PrecioTotal { get; set; }

        public Item(string art, decimal cant, decimal precioUnit, decimal precTotal)
        {
            Articulo = art;
            Cantidad = cant;
            PrecioUnitario = precioUnit;
            PrecioTotal = PrecioTotal;
        }
    }

    public class Detalle
    {
        private List<Item> detalle;

        public Detalle()
        {
            detalle = new List<Item>();
            detalle.Add(new Item("1", 25,26,27));
            detalle.Add(new Item("2", 30,31,32));
            detalle.Add(new Item("3", 15,16,17));
        }

        public List<Item> GetProducts()
        {
            return detalle;
        }
    }
}