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
    
    public partial class CtaCteClientesMov
    {
        public int id { get; set; }
        public int id_ctacte { get; set; }
        public System.DateTime fecha_movimiento { get; set; }
        public Nullable<decimal> importe_movimiento { get; set; }
        public Nullable<decimal> saldo { get; set; }
        public Nullable<int> nro_comprobante { get; set; }
        public string observacion { get; set; }
        public string desc_debito { get; set; }
        public string desc_credito { get; set; }
        public string concepto { get; set; }
    
        public virtual CtaCte_Clientes CtaCte_Clientes { get; set; }
    }
}