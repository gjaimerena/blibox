//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Blibox
{
    using System;
    using System.Collections.Generic;
    
    public partial class Componente
    {
        public int ID_componente { get; set; }
        public int ID_articulo { get; set; }
        public Nullable<int> Peso { get; set; }
        public Nullable<int> ID_material { get; set; }
        public Nullable<int> ID_marco { get; set; }
        public string Ciclos { get; set; }
        public string Bocas { get; set; }
        public string Espesor { get; set; }
        public Nullable<int> ID_matriz { get; set; }
        public Nullable<int> ID_cortante { get; set; }
        public string Color { get; set; }
    
        public virtual Articulo Articulo { get; set; }
        public virtual Cortante Cortante { get; set; }
        public virtual Marco Marco { get; set; }
        public virtual Material Material { get; set; }
        public virtual Matriz Matriz { get; set; }
    }
}