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
    
    public partial class Cortante
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cortante()
        {
            this.Componente = new HashSet<Componente>();
        }
    
        public int ID_cortante { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string Sector { get; set; }
        public Nullable<int> Bocas { get; set; }
        public string Observaciones { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Componente> Componente { get; set; }
    }
}