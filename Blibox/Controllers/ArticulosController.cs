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

        private ICollection<Material> materialColeccion = new HashSet<Material>();
        private ICollection<Marco> marcoColeccion = new HashSet<Marco>();
        private ICollection<Matriz> matrizcoleccion = new HashSet<Matriz>();
        private ICollection<Cortante> cortantesColeccion = new HashSet<Cortante>();

        private BliboxEntities db = new BliboxEntities();
        private List<SelectListItem> items = new List<SelectListItem>();
      

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
            getDropdownElements();
            ViewBag.Peso = "0";
            return View();
        }

        private void getDropdownElements()
        {
            List<Material> materiales = db.Material.Select(m => m).ToList();
            List<Marco> marcos = db.Marco.Select(m => m).ToList();
            materialColeccion = new HashSet<Material>();
            marcoColeccion = new HashSet<Marco>();

            for (int i = 0; i < materiales.Count; i++)
            {
                materialColeccion.Add(new Material { ID_material = materiales.ElementAt(i).ID_material, Descripcion = materiales.ElementAt(i).Descripcion });
            }

            for (int i = 0; i < marcos.Count; i++)
            {
                marcoColeccion.Add(new Marco { ID_marco = marcos.ElementAt(i).ID_marco, Descripcion = marcos.ElementAt(i).Descripcion });
            }

            List<Matriz> matrices = db.Matriz.Select(m => m).ToList();
            matrizcoleccion = new HashSet<Matriz>();
            for (int i = 0; i < matrices.Count; i++)
            {
                matrizcoleccion.Add(new Matriz { ID_matriz = matrices.ElementAt(i).ID_matriz, Codigo = matrices.ElementAt(i).Codigo });
            }

            List<Cortante> cortantes = db.Cortante.Select(m => m).ToList();
            cortantesColeccion = new HashSet<Cortante>();
            for (int i = 0; i < cortantes.Count; i++)
            {
                cortantesColeccion.Add(new Cortante { ID_cortante = cortantes.ElementAt(i).ID_cortante, Descripcion = cortantes.ElementAt(i).Descripcion });
            }

            ViewBag.Mat_Descripcion = materialColeccion;
            ViewBag.Marco = marcoColeccion;
            ViewBag.Matriz = matrizcoleccion;
            ViewBag.cortante = cortantesColeccion;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Articulo articulo)//([Bind(Include = "Descripcion,ID_cliente,Costo,IVA,Precio_lista,Precio_fecha,Stock,Fazon,Stock_minimo,Cant_x_bulto,Tamaño_caja,Tiraje_term_x_hora,Tiraje_troquel_x_hora,Observaciones")] Articulo articulo)
        {
            var errors = ModelState.Select(x => x.Value.Errors).Where(y => y.Count > 0).ToList();

            if (ModelState.IsValid)
            {
                    if (articulo.Componente == null || articulo.Componente.Count == 0)
                    {
                        ModelState.AddModelError("Detalle", "Debe agregar al menos un componente para el articulo");
                    }
                    else
                    {
                        db.Articulo.Add(articulo);
                       // return RedirectToAction("Index");
                    }
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                getDropdownElements();
                ViewBag.Peso = "0";
            }

            ViewBag.ID_cliente = new SelectList(db.Cliente, "ID_cliente", "Razon_Social", articulo.ID_cliente);
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

            getDropdownElements();
            ViewBag.Peso = "0";
            
            ViewBag.ID_cliente = new SelectList(db.Cliente, "ID_cliente", "Razon_Social", articulo.ID_cliente);
            return View(articulo);
        }

        // POST: Articulos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Articulo articulo)
        {
            
            if (ModelState.IsValid)
            {
                db.Entry(articulo).State = EntityState.Modified;
                for (int i = 0; i < articulo.Componente.Count; i++)
                {
                    db.Entry(articulo.Componente.ElementAt(i)).State = EntityState.Modified;
                        
                }
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

      
        public ActionResult eliminarComponente(int Id)
        {
            Componente comp = db.Componente.Where(m => m.ID_componente == Id).FirstOrDefault();
            db.Componente.Remove(comp);
            db.SaveChanges();
            Articulo art = db.Articulo.Select(m => m).Where(m => m.ID_articulo == comp.ID_articulo).FirstOrDefault();

            // return RedirectToAction("Edit","Articulos", art.ID_articulo.ToString()); ;
            return RedirectToAction("Edit", "Articulos", new { id = art.ID_articulo });
        }

        public ActionResult AgregarComponente(int Id)
        {
            db.Componente.Add(new Componente { ID_articulo = Id});
            db.SaveChanges();
            // return RedirectToAction("Edit","Articulos", art.ID_articulo.ToString()); ;
            return RedirectToAction("Edit", "Articulos", new { id = Id });
        }


    }
}
