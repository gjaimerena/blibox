using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blibox.Models
{
    public class AddArticulo
    {
        public Articulo articulo { get; set; }
        public Componente comp1 { get; set; }
        public Componente comp2 { get; set; }
        public Componente comp3 { get; set; }

        public AddArticulo()
        {
            Articulo articulo = new Articulo();
            Componente comp1 = new Componente();
            Componente comp2 = new Componente();
            Componente comp3 = new Componente();
        }
    }
}