﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BliboxEntities : DbContext
    {
        public BliboxEntities()
            : base("name=BliboxEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Articulo> Articulo { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Cliente> Cliente { get; set; }
        public virtual DbSet<Componente> Componente { get; set; }
        public virtual DbSet<Compras> Compras { get; set; }
        public virtual DbSet<Condicion_venta> Condicion_venta { get; set; }
        public virtual DbSet<CondicionIVA> CondicionIVA { get; set; }
        public virtual DbSet<Cortante> Cortante { get; set; }
        public virtual DbSet<CtaCte_Clientes> CtaCte_Clientes { get; set; }
        public virtual DbSet<CtaCte_Proveedores> CtaCte_Proveedores { get; set; }
        public virtual DbSet<CtaCteClientesMov> CtaCteClientesMov { get; set; }
        public virtual DbSet<Detalle_factura> Detalle_factura { get; set; }
        public virtual DbSet<Encabezado_Factura> Encabezado_Factura { get; set; }
        public virtual DbSet<IVA> IVA { get; set; }
        public virtual DbSet<Marco> Marco { get; set; }
        public virtual DbSet<Material> Material { get; set; }
        public virtual DbSet<Matriz> Matriz { get; set; }
        public virtual DbSet<Pedido> Pedido { get; set; }
        public virtual DbSet<Presupuesto> Presupuesto { get; set; }
        public virtual DbSet<Proveedor> Proveedor { get; set; }
        public virtual DbSet<Remito> Remito { get; set; }
        public virtual DbSet<Rubro> Rubro { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<TipoDocumento> TipoDocumento { get; set; }
        public virtual DbSet<TipoResponsables> TipoResponsables { get; set; }
        public virtual DbSet<Vendedor> Vendedor { get; set; }
    }
}
