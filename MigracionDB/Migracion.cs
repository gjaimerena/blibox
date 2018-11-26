using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigracionDB
{
    public class Migracion
    {
        private BliboxEntities db = new BliboxEntities();
        private blibox_oriEntities db_ori = new blibox_oriEntities();

        public void MigrarClientes()
        {
            /*VERIFICAR ANTES QUE LA TABLATENGO CAMPO PROVINCIA MAYOR A CERO Y NO TENGAS FILAS NULAS
            */



            //obtengo lista de clientes de la base original
            List<clientes> clientes_ori = db_ori.clientes.Select(m => m).ToList();

            //int id_cliente = 0;
            foreach (clientes c in clientes_ori)
            {
                try
                {
                    //creo nuevo cliente 
                    Cliente cliente = new Cliente
                    {
                        Codigo_Postal = (c.CODPOS == null) ? "" : c.CODPOS,
                        Comision_vendedor = (c.COMISION == null) ? "" : c.COMISION.Value.ToString(),
                        ID_cliente = Convert.ToInt32(c.N_CLIENTE),
                        CondicionIVA = (c.COND_IVA == null) ? "" : c.COND_IVA.ToString(),
                        Contacto = (c.CONTACTO == null) ? "" : c.CONTACTO,
                        DiasFF = (c.DIAS_FF == null) ? 0 : Int32.Parse(c.DIAS_FF.Value.ToString()),
                        Dias_Cheque = (c.CHEQ_FF == null) ? 0 : Int32.Parse(c.CHEQ_FF.Value.ToString()),
                        Documento = (c.NRO_CUIT == null) ? "" : c.NRO_CUIT.Value.ToString(),
                        Domicilio = (c.DOMICILIO == null) ? "" : c.DOMICILIO,
                        Email = "",
                        Fecha_alta = DateTime.Now,
                        Grupo_mailing = (c.GRUPO_MAIL == null) ? "" : c.GRUPO_MAIL.Value.ToString(),
                        ID_rubro = (c.N_RUBRO == null) ? 0 : Int32.Parse(c.N_RUBRO.ToString()),
                        ID_vendedor = (c.N_VENDEDOR == null) ? 0 : Int32.Parse(c.N_VENDEDOR.ToString()),
                        limite_credito = (c.LIMITE_CRE == null) ? 0 : Decimal.Parse(c.LIMITE_CRE.Value.ToString()),
                        Localidad = (c.LOCALIDAD == null) ? "" : c.LOCALIDAD,
                        Observaciones = (c.OBSERVAC == null) ? "" : c.OBSERVAC,
                        Provincia = (c.PROVINCIA == null) ? 0 : Int32.Parse(c.PROVINCIA.Value.ToString()),
                        Razon_Social = (c.RAZON_CLI == null) ? "" : c.RAZON_CLI,
                        Referidos = (c.REFERIDO == null) ? "" : c.REFERIDO,
                        Saldo = (c.LIMITE_CRE == null) ? 0 : Decimal.Parse(c.SALDO.Value.ToString()),
                        Telefono = (c.TELEFONO == null) ? "" : c.TELEFONO,
                        TipoDocumento = 25,
                        TipoResponsable = 1,
                        

                    };

                   
                    db.Cliente.Add(cliente);

                    

                }
                catch (Exception ex)
                {
                    Console.WriteLine("ID Cliente"+ "- " + ex.Message);
                    throw;
                }

                try
                {
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
                //db.SaveChanges();
            }
        }

        public void MigrarRubros()
        {
            /*VERIFICAR ANTES QUE LA TABLATENGO CAMPO PROVINCIA MAYOR A CERO Y NO TENGAS FILAS NULAS
            */



            //obtengo lista de clientes de la base original
            List<rubroo> rubros_ori = db_ori.rubroo.Select(m => m).ToList();
            rubros_ori.Insert(0, new rubroo { RUBRO_DESC = "SIN DEFINIR", N_RUBRO = 0, OBSERVAC = "" });

            foreach (rubroo c in rubros_ori)
            {
                try
                {
                    //creo nuevo cliente 
                    Rubro rubro = new Rubro
                    {
                        Descirpcion = c.RUBRO_DESC,
                        ID_rubro = c.N_RUBRO,

                        Observaciones = ""
                        //Codigo_Postal = (c.CODPOS == null) ? "" : c.CODPOS,
                       
                    };
                  
                    db.Rubro.Add(rubro);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }

                try
                {
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
                //db.SaveChanges();
            }
        }

        public void MigrarProveedores()
        {
            /*VERIFICAR ANTES QUE LA TABLATENGO CAMPO PROVINCIA MAYOR A CERO Y NO TENGAS FILAS NULAS
            */



            //obtengo lista de clientes de la base original
            List<proveedo> proveedores_ori = db_ori.proveedo.Select(m => m).ToList();


            foreach (proveedo p in proveedores_ori)
            {
                try
                {
                    //creo nuevo cliente 
                    Proveedor p_new = new Proveedor
                    {
                        Celular = p.TELEFONO,
                        Contacto = p.CONTACTO,
                        CUIT = Convert.ToInt64(p.NRO_CUIT),
                        Email = "",
                        Fecha_alta = (p.FECHA_ALTA == null) ? DateTime.Now : p.FECHA_ALTA,
                        
                        IVA = (p.COND_IVA == null) ? "" : p.COND_IVA.Value.ToString(),
                        Domicilio = (p.DOMICILIO == null) ? "" : p.DOMICILIO,
                        Localidad = (p.LOCALIDAD == null) ? "" : p.LOCALIDAD,
                        Provincia = Convert.ToInt32(p.PROVINCIA),
                        Observaciones = (p.OBSERVAC == null) ? "" : p.OBSERVAC,
                        Razon_social = (p.RAZON_PRO == null) ? "" : p.RAZON_PRO,
                        Telefono = (p.TELEFONO == null) ? "" : p.TELEFONO,
                        Saldo = Convert.ToDecimal(p.SALDO)

                        
                    };
                    p_new.ID_proveedor = Convert.ToInt32(p.N_PROVEED);

                    db.Proveedor.Add(p_new);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }

                try
                {
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
                //db.SaveChanges();
            }
        }

        public void MigrarMarcos()
        {
            /*VERIFICAR ANTES QUE LA TABLATENGO CAMPO PROVINCIA MAYOR A CERO Y NO TENGAS FILAS NULAS
            */
            //obtengo lista de clientes de la base original
            List<marcos> marco_ori = db_ori.marcos.Select(m => m).ToList();
            marco_ori.Insert(0, new marcos { DESCR_MARC = "SIN DEFINIR", N_MARCO = 0, ANCHO = 0, LARGO = 0, OBSERVAC=""});

            foreach (marcos p in marco_ori)
            {
                try
                {
                    //creo nuevo cliente 
                    Marco p_new = new Marco
                    {
                        Ancho = (p.ANCHO == null) ? 0 : Convert.ToDecimal(p.ANCHO), 
                        Largo = (p.LARGO == null) ? 0 : Convert.ToDecimal(p.LARGO),
                        Observaciones = (p.OBSERVAC == null) ? "" : p.OBSERVAC,
                        Descripcion = (p.DESCR_MARC == null) ? "" : p.DESCR_MARC,


                    };
                    p_new.ID_marco = Convert.ToInt32(p.N_MARCO);

                    db.Marco.Add(p_new);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }

                try
                {
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
                //db.SaveChanges();
            }
        }

        public void MigrarMateriales()
        {
            /*VERIFICAR ANTES QUE LA TABLATENGO CAMPO PROVINCIA MAYOR A CERO Y NO TENGAS FILAS NULAS
            */
            //obtengo lista de clientes de la base original
            List<materialo> mat_ori = db_ori.materialo.Select(m => m).ToList();
            mat_ori.Insert(0, new materialo { DESCR_MAT = "SIN DEFINIR", N_MATERIAL = 0, OBSERVAC = 0, PESO_ESPEC = 0 });

            foreach (materialo m in mat_ori)
            {
                try
                {
                    //creo nuevo cliente 
                    Material m_new = new Material
                    {
                        Descripcion = (m.DESCR_MAT == null) ? "" : m.DESCR_MAT,
                        Observaciones = (m.OBSERVAC == null) ? "" : m.OBSERVAC.Value.ToString(),
                        Peso = (m.PESO_ESPEC == null) ? 0 : Convert.ToDecimal(m.PESO_ESPEC)
                        


                    };
                    m_new.ID_material = Convert.ToInt32(m.N_MATERIAL);

                    db.Material.Add(m_new);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }

                try
                {
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
                //db.SaveChanges();
            }
        }


        public void MigrarVendedores()
        {
            /*VERIFICAR ANTES QUE LA TABLATENGO CAMPO PROVINCIA MAYOR A CERO Y NO TENGAS FILAS NULAS
            */
            //obtengo lista de clientes de la base original
            List<vendedoro> ven_ori = db_ori.vendedoro.Select(m => m).ToList();
            ven_ori.Insert(0, new vendedoro
            {
                VEN_DESCR = "Sin Asignar",
                N_VENDEDOR = 0,
                DOMICILIO = "",
                LOCALIDAD = "",
                PROVINCIA = 0,
                CODPOS = 0,
                TELEFONO = "",
                FECHA_ALTA = DateTime.Now,
                OBSERVAC = "Vendedor sin asignar",
                
            });

            foreach (vendedoro m in ven_ori)
            {
                try
                {
                    //creo nuevo cliente 
                    Vendedor m_new = new Vendedor
                    {
                        Apellido = (m.VEN_DESCR == null) ? "" : m.VEN_DESCR,
                        Nombre = (m.VEN_DESCR == null) ? "" : m.VEN_DESCR,
                        Domicilio = (m.DOMICILIO == null) ? "" : m.DOMICILIO,
                        Localidad = (m.LOCALIDAD == null) ? "" : m.LOCALIDAD,
                        Provincia = (m.PROVINCIA == null) ? 1 : Convert.ToInt32(m.PROVINCIA),
                        Codigo_postal = (m.CODPOS == null) ? 0 : Convert.ToInt32(m.CODPOS),
                        Telefono = (m.TELEFONO == null) ? "" : m.TELEFONO,
                        Fecha_alta = (m.FECHA_ALTA == null) ? DateTime.Now : m.FECHA_ALTA,
                        Observacion = (m.OBSERVAC == null) ? "" : m.OBSERVAC,
                        Celular = "",
                        Email = ""
                    };
                    m_new.ID_vendedor = Convert.ToInt32(m.N_VENDEDOR);

                    db.Vendedor.Add(m_new);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }

                try
                {
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
                //db.SaveChanges();
            }
        }

        public void MigrarMatrices()
        {
            /*VERIFICAR ANTES QUE LA TABLATENGO CAMPO PROVINCIA MAYOR A CERO Y NO TENGAS FILAS NULAS
            */
            //obtengo lista de clientes de la base original
            List<MATRICES> ven_ori = db_ori.MATRICES.Select(m => m).ToList();
            ven_ori.Insert(0, new MATRICES { BOCAS = 0, MAT_DESCR = "SIN DEFINIR", N_MATRICES = "0", OBSERVAC = "", SECTOR = 0 });

            foreach (MATRICES m in ven_ori)
            {
                try
                {
                    //creo nuevo cliente 
                    Matriz m_new = new Matriz
                    {
                        Bocas = (m.BOCAS == null) ? 0 : Convert.ToInt32(m.BOCAS),
                        Codigo = (m.N_MATRICES == null) ? "" : m.N_MATRICES,
                        Descripcion = (m.MAT_DESCR == null) ? "" : m.MAT_DESCR,
                        Observaciones = (m.OBSERVAC == null) ? "" : m.OBSERVAC,
                        Sector = (m.SECTOR == null) ? "" : m.SECTOR.Value.ToString(),
                        


                    };
                   

                    db.Matriz.Add(m_new);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }

                try
                {
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
                //db.SaveChanges();
            }
        }

        public void MigrarCortante()
        {
            /*VERIFICAR ANTES QUE LA TABLATENGO CAMPO PROVINCIA MAYOR A CERO Y NO TENGAS FILAS NULAS
            */
            //obtengo lista de clientes de la base original
            List<cortanteo> cor_ori = db_ori.cortanteo.Select(m => m).ToList();
            cor_ori.Insert(0, new cortanteo { BOCAS = 0, CORT_DESCR = "SIN DEFINIR", N_CORTANTE = "0", OBSERVAC = "", SECTOR = 0 });

            foreach (cortanteo m in cor_ori)
            {
                try
                {
                    //creo nuevo cliente 
                    Cortante m_new = new Cortante
                    {
                        Bocas = (m.BOCAS == null) ? 0 : Convert.ToInt32(m.BOCAS),
                        Codigo = (m.N_CORTANTE == null) ? "" : m.N_CORTANTE,
                        Descripcion = (m.CORT_DESCR == null) ? "" : m.CORT_DESCR,
                        Observaciones = (m.OBSERVAC == null) ? "" : m.OBSERVAC,
                        Sector = (m.SECTOR == null) ? "" : m.SECTOR.Value.ToString(),
                    };


                    db.Cortante.Add(m_new);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }

                try
                {
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
                //db.SaveChanges();
            }
        }



        public void MigrarArticulos()

        {
            /*VERIFICAR ANTES QUE LA TABLA TENGO CAMPO PROVINCIA MAYOR A CERO Y NO TENGAS FILAS NULAS
            */
            //obtengo lista de clientes de la base original
            List<articulos> art_ori = db_ori.articulos.Select(m => m).ToList();
            art_ori = art_ori.OrderBy(m => m.n_articulo).ToList();
           
            

            foreach (articulos m in art_ori)

            {
                
                try
                {
                    
                    double cliente = (m.n_cliente.HasValue) ? m.n_cliente.Value : 0;

                    if (clienteExist(cliente))
                    {
                        Articulo m_new = new Articulo
                        {
                            ID_articulo = Convert.ToInt32(m.n_articulo),
                            Costo = (m.pre_costo == null) ? 0 : Convert.ToDecimal(m.pre_costo),
                            Descripcion = (m.art_descr == null) ? "" : m.art_descr,
                            ID_cliente = Convert.ToInt32(m.n_cliente),
                            Precio_fecha = (m.fecha_prec == null) ? DateTime.Now : parserFecha(m.fecha_prec),
                            Stock = (m.stock == null) ? 0 : Convert.ToDecimal(m.stock),
                            Stock_minimo = (m.stock_min == null) ? 0 : Convert.ToDecimal(m.stock_min),
                            Tiraje_term_x_hora = (m.tiraje_ter == null) ? 0 : Convert.ToInt32(m.tiraje_ter),
                            Tiraje_troquel_x_hora = (m.tiraje_tro == null) ? 0 : Convert.ToInt32(m.tiraje_tro),
                            Tamaño_caja = (m.tamanocaja == null) ? "" : m.tamanocaja,
                            Precio_lista = (m.pre_lista1 == null) ? 0 : Convert.ToDecimal(m.pre_lista1),
                            Cant_x_bulto = (m.cantidad == null) ? 0 : Convert.ToInt32(m.cantidad),


                        };

                        //unifico observaciones
                        m_new.Observaciones = (m.observac == null) ? "" : m.observac;
                        string observaciones2 = (m.observac1 == null) ? "" : m.observac1;
                        m_new.Observaciones = m_new.Observaciones + " " + observaciones2;

                        db.Articulo.Add(m_new);


                        //AGREGO COMPONENTES

                        Componente comp1 = new Componente
                        {
                            Bocas = (m.bocas1.HasValue) ? m.bocas1.Value.ToString() : "",
                            Ciclos = (m.ciclos1.HasValue) ? m.ciclos1.Value.ToString() : "",
                            Color = (m.color1 == null) ? "" : m.color1,
                            Espesor = (m.espesor1.HasValue) ? m.espesor1.Value.ToString() : "",
                            ID_articulo = Convert.ToInt32(m.n_articulo),
                            ID_marco = (m.marco1 == null) ? 0 : Convert.ToInt32(m.marco1),
                            ID_material = (m.material1 == null) ? 0 : Convert.ToInt32(m.material1),
                            Peso = (m.peso1 == null) ? 0 : Convert.ToDecimal(m.peso1),

                        };

                        string codigo_cortante = (m.cortante1 == null) ? "" : m.cortante1;
                        if (codigo_cortante != "") comp1.ID_cortante = Convert.ToInt32(db.Cortante.Select(r => r.Codigo == codigo_cortante).FirstOrDefault());
                        if (comp1.ID_cortante == null) comp1.ID_cortante = 0;

                        string codigo_matriz = (m.matriz1 == null) ? "" : m.matriz1;
                        if (codigo_matriz != "") comp1.ID_matriz = Convert.ToInt32(db.Matriz.Select(r => r.Codigo == codigo_matriz).FirstOrDefault());
                        if (comp1.ID_matriz == null) comp1.ID_matriz = 0;
                        db.Componente.Add(comp1);


                        //COMPOENENETE 2
                        if (m.peso2 != null && m.peso2 != 0)
                        {
                            Componente comp2 = new Componente
                            {
                                Bocas = (m.bocas2.HasValue) ? m.bocas2.Value.ToString() : "",
                                Ciclos = (m.ciclos2.HasValue) ? m.ciclos2.Value.ToString() : "",
                                Color = (m.color2 == null) ? "" : m.color2,
                                Espesor = (m.espesor2.HasValue) ? m.espesor2.Value.ToString() : "",
                                ID_articulo = Convert.ToInt32(m.n_articulo),
                                ID_marco = (m.marco2 == null) ? 0 : Convert.ToInt32(m.marco2),
                                ID_material = (m.material2 == null) ? 0 : Convert.ToInt32(m.material2),
                                Peso = (m.peso2 == null) ? 0 : Convert.ToDecimal(m.peso2),

                            };

                            string codigo_cortante2 = (m.cortante2 == null) ? "" : m.cortante2;
                            if (codigo_cortante2 != "") comp2.ID_cortante = Convert.ToInt32(db.Cortante.Select(r => r.Codigo == codigo_cortante2).FirstOrDefault());
                            if (comp2.ID_cortante == null) comp2.ID_cortante = 0;

                            string codigo_matriz2 = (m.matriz2 == null) ? "" : m.matriz2;
                            if (codigo_matriz2 != "") comp2.ID_matriz = Convert.ToInt32(db.Matriz.Select(r => r.Codigo == codigo_matriz2).FirstOrDefault());
                            if (comp2.ID_matriz == null) comp2.ID_matriz = 0;
                            db.Componente.Add(comp2);
                        }

                        //COMPOENENETE 3
                        if (m.peso3 != null && m.peso3 != 0)
                        {
                            Componente comp3 = new Componente
                            {
                                Bocas = (m.bocas3.HasValue) ? m.bocas3.Value.ToString() : "",
                                Ciclos = (m.ciclos3.HasValue) ? m.ciclos3.Value.ToString() : "",
                                Color = (m.color3 == null) ? "" : m.color3,
                                Espesor = (m.espesor3.HasValue) ? m.espesor3.Value.ToString() : "",
                                ID_articulo = Convert.ToInt32(m.n_articulo),
                                ID_marco = (m.marco3 == null) ? 0 : Convert.ToInt32(m.marco3),
                                ID_material = (m.material3 == null) ? 0 : Convert.ToInt32(m.material3),
                                Peso = (m.peso3 == null) ? 0 : Convert.ToDecimal(m.peso3),

                            };

                            string codigo_cortante3 = (m.cortante3 == null) ? "" : m.cortante3;
                            if (codigo_cortante3 != "") comp3.ID_cortante = Convert.ToInt32(db.Cortante.Select(r => r.Codigo == codigo_cortante3).FirstOrDefault());
                            if (comp3.ID_cortante == null) comp3.ID_cortante = 0;

                            string codigo_matriz3 = (m.matriz3 == null) ? "" : m.matriz3;
                            if (codigo_matriz3 != "") comp3.ID_matriz = Convert.ToInt32(db.Matriz.Select(r => r.Codigo == codigo_matriz3).FirstOrDefault());
                            if (comp3.ID_matriz == null) comp3.ID_matriz = 0;
                            db.Componente.Add(comp3);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }

                try
                {
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
                //db.SaveChanges();
            }
        }

        public DateTime parserFecha(string dateString)
        {
            DateTime fecha;
            bool ok = DateTime.TryParseExact(dateString, "dd-MMM-yy", System.Globalization.CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha);
            if (!ok) fecha = DateTime.Now;
            return fecha;
        }

        public bool clienteExist(double id_cliente)
        {
            List<double> clientes_ori = db_ori.clientes.Select(m => m.N_CLIENTE).ToList();

            if (clientes_ori.Contains(id_cliente)) return true;
            
            return false;
        }
    }
}
