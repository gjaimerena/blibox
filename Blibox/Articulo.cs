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
    
    public partial class Articulo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Articulo()
        {
            this.Componente = new HashSet<Componente>();
            this.Detalle_factura = new HashSet<Detalle_factura>();
            this.Pedido = new HashSet<Pedido>();
        }
    
        public int ID_articulo { get; set; }
        public string Descripcion { get; set; }
        public int ID_cliente { get; set; }
        public Nullable<int> Costo { get; set; }
        public string IVA { get; set; }
        public Nullable<int> Precio_lista { get; set; }
        public Nullable<System.DateTime> Precio_fecha { get; set; }
        public Nullable<int> Stock { get; set; }
        public string Fazon { get; set; }
        public Nullable<int> Stock_minimo { get; set; }
        public Nullable<int> Cant_x_bulto { get; set; }
        public Nullable<int> Tamaño_caja { get; set; }
        public Nullable<int> Tiraje_term_x_hora { get; set; }
        public Nullable<int> Tiraje_troquel_x_hora { get; set; }
        public string Observaciones { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Componente> Componente { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detalle_factura> Detalle_factura { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pedido> Pedido { get; set; }
        public virtual Cliente Cliente { get; set; }
    }
}
