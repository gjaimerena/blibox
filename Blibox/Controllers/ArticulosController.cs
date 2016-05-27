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

namespace Blibox.Models
{
    public class ArticulosController : Controller
    {
        private BliboxEntities db = new BliboxEntities();
        private List<SelectListItem> items = new List<SelectListItem>();
        private SelectList mat, marco, matriz, cortante;

        // GET: Articulos
        public ActionResult Index(string sortOrder, string q, int page = 1, int pageSize = 10)
        {
            ViewBag.searchQuery = String.IsNullOrEmpty(q) ? "" : q;

            page = page > 0 ? page : 1;
            pageSize = pageSize > 0 ? pageSize : 10;

            ViewBag.IdSortParam = sortOrder == "id" ? "id_desc" : "id";
            ViewBag.DescripcionSort = sortOrder == "descripcion" ? "descripcion_desc" : "descripcion";
            //ViewBag.DateSortParam = sortOrder == "date" ? "date_desc" : "date";

            ViewBag.CurrentSort = sortOrder;

            var query = db.Articulo.OrderBy(m => m.ID_articulo);

            if (!String.IsNullOrEmpty(q))
            {
                query = query.Where(s => 
                    s.ID_articulo.ToString().Contains(q) || 
                    s.Descripcion.Contains(q) ||
                    s.Precio_fecha.Value.ToString().Contains(q) ||
                    s.Precio_lista.ToString().Contains(q) ||
                    s.Observaciones.Contains(q) ||
                    s.ID_cliente.ToString().Contains(q)
                    ).OrderBy(m => m.ID_articulo);
            }
   
            switch (sortOrder)
            {
                case "id":
                    query = query.OrderBy(x => x.ID_articulo);
                    break;
                case "id_desc":
                    query = query.OrderByDescending(x => x.ID_articulo);
                    break;
                case "descripcion":
                    query = query.OrderBy(x => x.Descripcion);
                    break;
                case "descripcion_desc":
                    query = query.OrderByDescending(x => x.Descripcion);
                    break;
                default:
                    break;
            }
            return View(query.ToPagedList(page, pageSize));
        }
 
        // GET: Articulos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Articulo articulo = db.Articulo.Find(id);
            if (articulo == null)
            {
                return HttpNotFound();
            }
            ViewBag.nrocomponentes = articulo.Componente.Count();

            return View(articulo);
        }

        // GET: Articulos/Create
        public ActionResult Create()
        {
            ViewBag.ID_cliente = new SelectList(db.Cliente, "ID_cliente", "Razon_Social");

            mat = new SelectList(db.Material.Select(m => m.Descripcion).ToList());
            marco = new SelectList(db.Marco.Select(m => m.Descripcion).ToList());
            matriz = new SelectList(db.Matriz.Select(m => m.Codigo));
            cortante = new SelectList(db.Cortante.Select(m => m.Codigo).ToList());

            ViewBag.Mat_Descripcion = mat;
            ViewBag.Marco = marco;
            ViewBag.Matriz = matriz;
            ViewBag.cortante = cortante;
            ViewBag.Peso = "0";
            ComboComponentes().First().Selected = true;
            ViewBag.componentes = ComboComponentes();

            return View();
        }

        // POST: Articulos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AddArticulo articulo)//([Bind(Include = "Descripcion,ID_cliente,Costo,IVA,Precio_lista,Precio_fecha,Stock,Fazon,Stock_minimo,Cant_x_bulto,Tamaño_caja,Tiraje_term_x_hora,Tiraje_troquel_x_hora,Observaciones")] Articulo articulo)
        {
          //  Componente comp1, comp2, comp3;
            
            
            var errors = ModelState.Select(x => x.Value.Errors)
                          .Where(y => y.Count > 0)
                          .ToList();
            if (ModelState.IsValid)
            {

                //obtengo nro de compoenente que tiene el articulo
                string nrocomp = (Request["nrocomponentes"]);

                //genero nuevo articulo
                db.Articulo.Add(articulo.articulo);

                int ID_new_Articulo = db.Articulo.OrderByDescending(m => m.ID_articulo).FirstOrDefault().ID_articulo;
                //genero componentes

                string cortante = "";
                string marco = "";
                string material = "";
                string matriz = "";

                //switch (nrocomp)
                //{
                //    case "1":
                //        cortante = Request["Cortante_codigo1"];
                //        marco = (Request["marco1"]);
                //        material = (Request["material1"]);
                //        matriz = (Request["matriz_codigo1"]);

                //        comp1 = new Componente
                //        {
                //            Bocas = Request["bocas1"],
                //            Ciclos = Request["ciclos1"],
                //            Color = Request["color1"],
                //            Espesor = Request["espesor1"],
                //            ID_articulo = ID_new_Articulo,
                //           // ID_componente = lastIDComponente + 1,
                //            ID_cortante = db.Cortante.Where(m => m.Codigo == cortante).FirstOrDefault().ID_cortante,
                //            ID_marco = db.Marco.Where(m => m.Descripcion == marco).FirstOrDefault().ID_marco,
                //            ID_material = db.Material.Where(m => m.Descripcion == material).FirstOrDefault().ID_material,
                //            ID_matriz = db.Matriz.Where(m => m.Codigo == matriz).FirstOrDefault().ID_matriz,
                //            Peso = 0
                //        };
                //        db.Componente.Add(comp1);
                //        break;
                //    case "2":
                //         cortante = Request["Cortante_codigo1"];
                //         marco = (Request["marco1"]);
                //         material = (Request["material1"]);
                //         matriz = (Request["matriz_codigo1"]);

                //        comp1 = new Componente
                //        {
                //            Bocas = Request["bocas1"],
                //            Ciclos = Request["ciclos1"],
                //            Color = Request["color1"],
                //            Espesor = Request["espesor1"],
                //            ID_articulo = ID_new_Articulo,
                //           // ID_componente = lastIDComponente + 1,
                //            ID_cortante = db.Cortante.Where(m => m.Codigo == cortante).FirstOrDefault().ID_cortante,
                //            ID_marco = db.Marco.Where(m => m.Descripcion == marco).FirstOrDefault().ID_marco,
                //            ID_material = db.Material.Where(m => m.Descripcion == material).FirstOrDefault().ID_material,
                //            ID_matriz = db.Matriz.Where(m => m.Codigo == matriz).FirstOrDefault().ID_matriz,
                //            Peso = 0
                //        };

                //        cortante = Request["Cortante_codigo2"];
                //        marco = (Request["marco2"]);
                //        material = (Request["material2"]);
                //        matriz = (Request["matriz_codigo2"]);

                //        comp2 = new Componente
                //        {
                //            Bocas = Request["bocas2"],
                //            Ciclos = Request["ciclos2"],
                //            Color = Request["color2"],
                //            Espesor = Request["espesor2"],
                //            ID_articulo = ID_new_Articulo,
                //        //    ID_componente = lastIDComponente + 2,
                //            ID_cortante = db.Cortante.Where(m => m.Codigo == cortante).FirstOrDefault().ID_cortante,
                //            ID_marco = db.Marco.Where(m => m.Descripcion == marco).FirstOrDefault().ID_marco,
                //            ID_material = db.Material.Where(m => m.Descripcion == material).FirstOrDefault().ID_material,
                //            ID_matriz = db.Matriz.Where(m => m.Codigo == matriz).FirstOrDefault().ID_matriz,
                //            Peso = 0
                //        };
                //        db.Componente.Add(comp1);
                //        db.Componente.Add(comp2);

                //        break;
                //    case "3":
                //         cortante = Request["Cortante_codigo1"];
                //         marco = (Request["marco1"]);
                //         material = (Request["material1"]);
                //         matriz = (Request["matriz_codigo1"]);

                //        comp1 = new Componente
                //        {
                //            Bocas = Request["bocas1"],
                //            Ciclos = Request["ciclos1"],
                //            Color = Request["color1"],
                //            Espesor = Request["espesor1"],
                //            ID_articulo = ID_new_Articulo,
                //     //       ID_componente = lastIDComponente + 1,
                //            ID_cortante = db.Cortante.Where(m => m.Codigo == cortante).FirstOrDefault().ID_cortante,
                //            ID_marco = db.Marco.Where(m => m.Descripcion == marco).FirstOrDefault().ID_marco,
                //            ID_material = db.Material.Where(m => m.Descripcion == material).FirstOrDefault().ID_material,
                //            ID_matriz = db.Matriz.Where(m => m.Codigo == matriz).FirstOrDefault().ID_matriz,
                //            Peso = 0
                //        };

                //        cortante = Request["Cortante_codigo2"];
                //        marco = (Request["marco2"]);
                //        material = (Request["material2"]);
                //        matriz = (Request["matriz_codigo2"]);

                //        comp2 = new Componente
                //        {
                //            Bocas = Request["bocas2"],
                //            Ciclos = Request["ciclos2"],
                //            Color = Request["color2"],
                //            Espesor = Request["espesor2"],
                //            ID_articulo = ID_new_Articulo,
                //           // ID_componente = lastIDComponente + 2,
                //            ID_cortante = db.Cortante.Where(m => m.Codigo == cortante).FirstOrDefault().ID_cortante,
                //            ID_marco = db.Marco.Where(m => m.Descripcion == marco).FirstOrDefault().ID_marco,
                //            ID_material = db.Material.Where(m => m.Descripcion == material).FirstOrDefault().ID_material,
                //            ID_matriz = db.Matriz.Where(m => m.Codigo == matriz).FirstOrDefault().ID_matriz,
                //            Peso = 0
                //        };

                //        cortante = Request["Cortante_codigo3"];
                //        marco = (Request["marco3"]);
                //        material = (Request["material3"]);
                //        matriz = (Request["matriz_codigo3"]);

                //        comp3 = new Componente
                //        {
                //            Bocas = Request["bocas3"],
                //            Ciclos = Request["ciclos3"],
                //            Color = Request["color3"],
                //            Espesor = Request["espesor3"],
                //            ID_articulo = ID_new_Articulo,
                //         //   ID_componente = lastIDComponente + 3,
                //            ID_cortante = db.Cortante.Where(m => m.Codigo == cortante).FirstOrDefault().ID_cortante,
                //            ID_marco = db.Marco.Where(m => m.Descripcion == marco).FirstOrDefault().ID_marco,
                //            ID_material = db.Material.Where(m => m.Descripcion == material).FirstOrDefault().ID_material,
                //            ID_matriz = db.Matriz.Where(m => m.Codigo == matriz).FirstOrDefault().ID_matriz,
                //            Peso = 0
                //        };
                //        db.Componente.Add(comp1);
                //        db.Componente.Add(comp2);
                //        db.Componente.Add(comp3);
                //        break;
                       
                //    default: break;
                //}

               // db.Articulo.Add(articulo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                mat = new SelectList(db.Material.Select(m => m.Descripcion).ToList());
                marco = new SelectList(db.Marco.Select(m => m.Descripcion).ToList());
                matriz = new SelectList(db.Matriz.Select(m => m.Codigo));
                cortante = new SelectList(db.Cortante.Select(m => m.Codigo).ToList());
                ViewBag.componentes = ComboComponentes();
                ViewBag.Mat_Descripcion = mat;
                ViewBag.Marco = marco;
                ViewBag.Matriz = matriz;
                ViewBag.cortante = cortante;
                ViewBag.Peso = "0";
            }

            ViewBag.ID_cliente = new SelectList(db.Cliente, "ID_cliente", "Razon_Social", articulo.articulo.ID_cliente);
            return View(articulo);
        }

        // GET: Articulos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Articulo articulo = db.Articulo.Find(id);
            if (articulo == null)
            {
                return HttpNotFound();
            }

            mat = new SelectList(db.Material.Select(m => m.Descripcion).ToList());
            marco = new SelectList(db.Marco.Select(m => m.Descripcion).ToList());
            matriz = new SelectList(db.Matriz.Select(m => m.Codigo));
            cortante = new SelectList(db.Cortante.Select(m => m.Codigo).ToList());

            ViewBag.Mat_Descripcion = mat;
            ViewBag.Marco = marco;
            ViewBag.Matriz = matriz;
            ViewBag.cortante = cortante;

            ViewBag.Peso = "0";
            ComboComponentes().First().Selected = true;
            ViewBag.componentes = ComboComponentes();

            ViewBag.ID_cliente = new SelectList(db.Cliente, "ID_cliente", "Razon_Social", articulo.ID_cliente);
            return View(articulo);
        }

        // POST: Articulos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_articulo,Descripcion,ID_cliente,Costo,IVA,Precio_lista,Precio_fecha,Stock,Fazon,Stock_minimo,Cant_x_bulto,Tamaño_caja,Tiraje_term_x_hora,Tiraje_troquel_x_hora,Observaciones")] Articulo articulo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(articulo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_cliente = new SelectList(db.Cliente, "ID_cliente", "Razon_Social", articulo.ID_cliente);
            return View(articulo);
        }

        // GET: Articulos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Articulo articulo = db.Articulo.Find(id);
            if (articulo == null)
            {
                return HttpNotFound();
            }
            return View(articulo);
        }

        // POST: Articulos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Articulo articulo = db.Articulo.Find(id);
            
            var componentes = db.Componente.Where(m => m.ID_articulo == articulo.ID_articulo);
            foreach (var comp in componentes)
            {
                db.Componente.Remove(comp);
            }

            try
            {
                db.Articulo.Remove(articulo);
                db.SaveChanges();
            }
            catch (Exception e) {
                Console.WriteLine(e);
                
            }

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

        private SelectList ComboComponentes() {
            
            
            SelectListItem l0 = new SelectListItem { Text = "Ninguno", Value="Ninguno" };
            SelectListItem l1 = new SelectListItem { Text = "1", Value = "1" };
            SelectListItem l2 = new SelectListItem { Text = "2", Value = "2" };
            SelectListItem l3 = new SelectListItem { Text = "3", Value = "3" };
            l3.Selected = true;
            List<SelectListItem> newList = new List<SelectListItem>();
            newList.Add(l0);
            newList.Add(l1);
            newList.Add(l2);
            newList.Add(l3);


            //Return the list of selectlistitems as a selectlist
            return new SelectList(newList, "Value", "Text", "Ninguno");
            
        }





    }
}
