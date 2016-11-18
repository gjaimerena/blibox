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
    
    public partial class Cliente
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cliente()
        {
            this.Articulo = new HashSet<Articulo>();
            this.CtaCteClientes = new HashSet<CtaCteClientes>();
            this.Encabezado_Factura = new HashSet<Encabezado_Factura>();
            this.Pedido = new HashSet<Pedido>();
        }
    
        public int ID_cliente { get; set; }
        public string Razon_Social { get; set; }
        public string Contacto { get; set; }
        public string Domicilio { get; set; }
        public string Localidad { get; set; }
        public string Provincia { get; set; }
        public Nullable<int> Codigo_Postal { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public Nullable<int> ID_vendedor { get; set; }
        public string Comision_vendedor { get; set; }
        public string CondicionIVA { get; set; }
        public int TipoResponsable { get; set; }
        public Nullable<decimal> Saldo { get; set; }
        public string Observaciones { get; set; }
        public string Referidos { get; set; }
        public Nullable<decimal> limite_credito { get; set; }
        public Nullable<int> ID_rubro { get; set; }
        public Nullable<int> DiasFF { get; set; }
        public Nullable<int> Dias_Cheque { get; set; }
        public string Grupo_mailing { get; set; }
        public Nullable<System.DateTime> Fecha_alta { get; set; }
        public Nullable<int> TipoDocumento { get; set; }
        public string Documento { get; set; }
        public Nullable<int> id_ctacte { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Articulo> Articulo { get; set; }
        public virtual Rubro Rubro { get; set; }
        public virtual TipoDocumento TipoDocumento1 { get; set; }
        public virtual TipoResponsables TipoResponsables { get; set; }
        public virtual Vendedor Vendedor { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CtaCteClientes> CtaCteClientes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Encabezado_Factura> Encabezado_Factura { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pedido> Pedido { get; set; }
    }
}
