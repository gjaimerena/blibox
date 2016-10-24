using System;


namespace Blibox.Models
{
    public class DetalleCustom
    {
        public  string Articulo { get; set; }
        public  Decimal Cantidad { get; set; }
        public  Decimal PrecioUnitario { get; set; }
        public  Decimal PrecioTotal { get; set; }
    }
}