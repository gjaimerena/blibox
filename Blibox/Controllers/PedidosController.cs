using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Blibox;
using PagedList;
using ClosedXML.Excel;
using System.IO;
using System.Reflection;

namespace Blibox.Controllers
{
    public class PedidosController : Controller
    {
        private ICollection<Articulo> articulosColeccion = new HashSet<Articulo>();
        private BliboxEntities db = new BliboxEntities();

        // GET: Materiales
        public ActionResult Index(string sortOrder, string q, int page = 1, int pageSize = 10)
        {
            int estado = 0;

            List<SelectListItem> estado_pedido = new List<SelectListItem>();
            estado_pedido.Add(new SelectListItem() { Text = "Cumplido", Value = "1" });
            estado_pedido.Add(new SelectListItem() { Text = "Pendiente", Value = "2" });

            ViewBag.estado_pedido = new SelectList(estado_pedido, "Value", "Text");


            if (Request["estado_pedido"] != null && Request["estado_pedido"].ToString() != "")
            {
                Int32.TryParse(Request["estado_pedido"], out estado);
            }
            else
            {
               // ViewBag.ID_cliente = new SelectList(db.Cliente, "ID_cliente", "Razon_Social");
                List<Models.Pedidos> lista_pedidos = new List<Models.Pedidos>();
                return View(lista_pedidos.ToPagedList(page, pageSize));
            }

            string palabra_clave = (Request["palabra_clave"] == null) ? "" : Request["palabra_clave"].ToString();

            ViewBag.searchQuery = String.IsNullOrEmpty(q) ? "" : q;
            page = page > 0 ? page : 1;
            pageSize = pageSize > 0 ? pageSize : 10;
            //ViewBag.IdSortParam = sortOrder == "id" ? "id_desc" : "id";
            //ViewBag.DescripcionSort = sortOrder == "descripcion" ? "descripcion_desc" : "descripcion";
            //ViewBag.DateSortParam = sortOrder == "date" ? "date_desc" : "date";

            ViewBag.CurrentSort = sortOrder;

            var query = db.Pedido.OrderBy(m => m.ID_pedido);

            if (!String.IsNullOrEmpty(palabra_clave))
            {
                query = query.Where(s =>
                    s.ID_pedido.ToString().Contains(palabra_clave) ||
                    s.Cliente.Razon_Social.Contains(palabra_clave) ||
                    s.Observaciones.Contains(palabra_clave) ||
                    s.Precio.ToString().Contains(palabra_clave) 
                 
                    ).OrderBy(m => m.ID_pedido);
            }

            List<Models.materialNecesario> matNecesario = null;
            if (estado == 2) matNecesario = new List<Models.materialNecesario>();
            List<Models.Pedidos> pedidos = new List<Models.Pedidos>();

            foreach (Pedido item in query)
            {

                int cantidadPedida = item.cantidad_pedida ?? 0;
                int cantidadEntregada = (item.cantidad_entregada) ?? 0;
                int cantidadRestante = cantidadPedida - cantidadEntregada;

                if ((estado == 1) && (cantidadRestante == 0) || (estado == 2) && (cantidadRestante != 0))
                {
                    string componente1 = "", componente2 = "", componente3 = "";

                    
                    for (int i = 0; i < 3; i++)
                    {
                        string comp = "-";
                        if (item.Articulo.Componente.Count() >= i + 1)
                        {
                            Componente componente = item.Articulo.Componente.ElementAt(i);

                            var pesoRestante = componente.Material.Peso * cantidadRestante;
                            comp = componente.Material.Descripcion + " / ";
                            comp += componente.Marco.Ancho + "x" + componente.Marco.Largo + " / ";
                            comp += componente.Espesor + " / ";
                            comp += pesoRestante;

                            if (estado == 2)
                            {
                                
                                //si en la lista de material necesario ya existe el material y ademas es del mismo espesor sumo la cantidad requerida
                                //sino creo un nuevo item en la lista ya que es otro material nuevo necesario.
                                if (matNecesario.Exists(m => m.idmaterial == componente.Material.ID_material && m.espesor == Int32.Parse(componente.Espesor)))
                                {
                                    int index = matNecesario.FindIndex(m => m.idmaterial == componente.Material.ID_material && m.espesor == Int32.Parse(componente.Espesor));
                                    matNecesario[i].kilos = matNecesario[i].kilos + pesoRestante.Value;
                                }
                                else
                                {
                                    Models.materialNecesario newMat = new Models.materialNecesario
                                    {
                                        idmaterial = componente.ID_material.Value,
                                        material = componente.Material.Descripcion,
                                        espesor = Int32.Parse(componente.Espesor),
                                        kilos = pesoRestante.Value

                                    };

                                    matNecesario.Add(newMat);
                                }
                                
                            }

                        }
                        if (i == 0) componente1 = comp;
                        if (i == 1) componente2 = comp;
                        if (i == 2) componente3 = comp;

                    }

                    Models.Pedidos pedido = new Models.Pedidos
                    {
                        idPedido = item.ID_pedido,
                        cliente = item.Cliente.Razon_Social,
                        idArticulo = item.ID_articulo,
                        descArticulo = item.Articulo.Descripcion,
                        componente1 = componente1,
                        componente2 = componente2,
                        componente3 = componente3,
                        cantEntregada = cantidadEntregada,
                        cantPedida = cantidadPedida,
                        cantRestante = cantidadRestante
                    };

                    pedidos.Add(pedido);

                    

                }

            }


            ViewBag.matNecesarios = matNecesario;

            //uso variable de sesion para poder exportar los datos  excel
            HttpContext.Session["query"] = pedidos;
            return View(pedidos.ToPagedList(page, pageSize));
        }

        // GET: Pedidos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedido.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            getDropdownElements(pedido);
            return View(pedido);
        }

        // GET: Pedidos/Create
        public ActionResult Create()
        {
            getDropdownElements(new Pedido());
            return View();
        }

        // POST: Pedidos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "[ID_pedido,ID_cliente,ID_articulo,cantidad_pedida,cantidad_entregada,Fecha_pedido,Orden_compra,Armado,Fecha_armado,Fecha_entrega,Precio,Observaciones,Impreso,ID_vendedor,Importe_matriz,Tipo_caja,Codigo_articulo_interno_cliente")] Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                db.Pedido.Add(pedido);
                db.SaveChanges();
                HelperController.Instance.agregarMensaje("El pedido se cargo con exito", HelperController.CLASE_EXITO);
                return RedirectToAction("Index");
            } else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();
                    foreach(var error in errors)
                    {
                        // cut for brevity, need to add back more code from original
                        HelperController.Instance.agregarMensaje(error.ToString(), HelperController.CLASE_ERROR);

                    }
                HelperController.Instance.agregarMensaje(errors.ToString(), HelperController.CLASE_ERROR);

            }
            getDropdownElements(pedido);
            return View(pedido);
        }
        private void getDropdownElements(Pedido pedido)
        {
            List<Articulo> articulos = db.Articulo.Select(m => m).ToList();
            articulosColeccion = new HashSet<Articulo>();
            for (int i = 0; i < articulos.Count; i++)
            {
                articulosColeccion.Add(new Articulo { ID_articulo = articulos.ElementAt(i).ID_articulo, Descripcion = articulos.ElementAt(i).Descripcion });
            }
            List<SelectListItem> sino = new List<SelectListItem>()
            {
                new SelectListItem { Text = "Si", Value = "S", Selected = ( pedido.Armado == "S") },
                new SelectListItem { Text = "No", Value = "N", Selected = ( pedido.Armado == "N" )}

            };
            List<SelectListItem> boc= new List<SelectListItem>()
            {
                new SelectListItem { Text = "Elegir una opcion..." },
                new SelectListItem { Text = "B", Value = "B", Selected = ( pedido.Armado == "B") },
                new SelectListItem { Text = "C", Value = "C", Selected = ( pedido.Armado == "C" )}

            };

            ViewBag.armado = sino;
            ViewBag.tipoCaja = boc;
            //ViewBag.articulo = articulosColeccion;
            ViewBag.ID_cliente = new SelectList(db.Cliente, "ID_cliente", "Razon_Social", pedido.ID_cliente);

            ViewBag.ID_vendedor = new SelectList(db.Vendedor, "ID_vendedor", "nombre", pedido.ID_vendedor);
            ViewBag.ID_articulo = new SelectList(db.Articulo, "ID_articulo", "Descripcion", pedido.ID_articulo);

        }
        // GET: Pedidos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido pedido = db.Pedido.Find(id);
            if (pedido == null)
            {
                return HttpNotFound();
            }
            getDropdownElements(pedido);
            return View(pedido);
        }

        // POST: Pedidos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Pedido pedido)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pedido).State = EntityState.Modified;
                db.SaveChanges();
                HelperController.Instance.agregarMensaje("El pedido se edito con exito", HelperController.CLASE_EXITO);
                return RedirectToAction("Index");
            }
            getDropdownElements(pedido);
            return View(pedido);
        }

        // GET: Pedidos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pedido Pedido = db.Pedido.Find(id);
            if (Pedido == null)
            {
                return HttpNotFound();
            }
            getDropdownElements(Pedido);
            return View(Pedido);
        }

        // POST: Pedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pedido pedido = db.Pedido.Find(id);
            db.Pedido.Remove(pedido);
            db.SaveChanges();
            HelperController.Instance.agregarMensaje("El pedido se elimino con exito", HelperController.CLASE_EXITO);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult ExportData()
        {
            
            DataTable dt = new DataTable();
            dt.TableName = "Pedidos";
            List<Models.Pedidos> pedidos = (List<Models.Pedidos>)HttpContext.Session["query"];
            dt = ToDataTable(pedidos);

            dt.Columns["descArticulo"].ColumnName = "Articulo";
            dt.Columns["componente1"].ColumnName = "Componente 1 (Mat/Marco/Espesor/Kgs restantes)";
            dt.Columns["componente2"].ColumnName = "Componente 2 (Mat/Marco/Espesor/Kgs restantes)";
            dt.Columns["componente3"].ColumnName = "Componente 3 (Mat/Marco/Espesor/Kgs restantes)";
            dt.Columns["cantPedida"].ColumnName = "Cantidad Pedida";
            dt.Columns["cantEntregada"].ColumnName = "Cantidad Entregada";
            dt.Columns["cantRestante"].ColumnName = "Cantidad Restante";
            dt.Columns["idArticulo"].ColumnName = "Cod Articulo";
            dt.Columns["cliente"].ColumnName = "Cliente";
            dt.Columns["idPedido"].ColumnName = "Id Pedido";

            

            dt.AcceptChanges();

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= Pedidos.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream, false);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            return RedirectToAction("Index", "Pedidos");
        }

        private DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
    }
}
