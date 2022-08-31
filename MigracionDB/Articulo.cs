//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MigracionDB
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
        public Nullable<decimal> Costo { get; set; }
        public Nullable<decimal> Precio_lista { get; set; }
        public Nullable<System.DateTime> Precio_fecha { get; set; }
        public Nullable<decimal> Stock { get; set; }
        public Nullable<decimal> Stock_minimo { get; set; }
        public Nullable<int> Cant_x_bulto { get; set; }
        public string Tamaño_caja { get; set; }
        public Nullable<int> Tiraje_term_x_hora { get; set; }
        public Nullable<int> Tiraje_troquel_x_hora { get; set; }
        public string Observaciones { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Componente> Componente { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Detalle_factura> Detalle_factura { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pedido> Pedido { get; set; }
    }
}