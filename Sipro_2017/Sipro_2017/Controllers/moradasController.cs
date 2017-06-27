using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Sipro_2017;

namespace Sipro_2017.Controllers
{
    public class MoradasController : Controller
    {

        public ActionResult Index(string sortOrder, string currentFilter, string criterioPesquisa, int? page)
        {
            ViewBag.CurrentSort = sortOrder;

            if (criterioPesquisa != null){
                page = 1;
            }else{
                criterioPesquisa = currentFilter;
            }

            ViewBag.CurrentFilter = criterioPesquisa;

            var morada = from s in db.moradas
                         select s;
            if (!String.IsNullOrEmpty(criterioPesquisa))
            {
                morada = morada.Where(s => s.nome.Contains(criterioPesquisa) || s.sector.Contains(criterioPesquisa)
                    || s.tipoMorada.tipo.Contains(criterioPesquisa) || s.morada1.Contains(criterioPesquisa));
            }

            switch (sortOrder)
            {
                case "nome_desc":
                    morada = morada.OrderByDescending(s => s.nome);
                    break;
                case "nome_asc":
                    morada = morada.OrderBy(s => s.nome);
                    break;
                case "sector_desc":
                    morada = morada.OrderByDescending(s => s.sector);
                    break;
                case "sector_asc":
                    morada = morada.OrderBy(s => s.sector);
                    break;
                case "morada1_desc":
                    morada = morada.OrderByDescending(s => s.morada1);
                    break;
                case "morada1_asc":
                    morada = morada.OrderBy(s => s.morada1);
                    break;
                case "tipo_desc":
                    morada = morada.OrderByDescending(s => s.tipo);
                    break;
                case "tipo_asc":
                    morada = morada.OrderBy(s => s.tipo);
                    break;
                default:
                    morada = morada.OrderBy(s => s.nome);
                break;
            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(morada.ToPagedList(pageNumber, pageSize));
    }

        private SiproEntities db = new SiproEntities();

         // GET: Moradas/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            morada morada = await db.moradas.FindAsync(id);
            if (morada == null)
            {
                return HttpNotFound();
            }
            return View(morada);
        }

        // GET: Moradas/Create
        public ActionResult Create()
        {
            ViewBag.tipo = new SelectList(db.tipoMoradas, "id", "tipo");
            ViewBag.id = new SelectList(db.produtoes, "id", "codcomercial");
            return View();
        }

        // POST: Moradas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "id,numero,nome,sector,morada1,tipo")] morada morada)
        {
            if (ModelState.IsValid)
            {
                //morada.id = Guid.NewGuid();
                if (morada.numero==0)
                    morada.numero = 0;
                db.moradas.Add(morada);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.tipo = new SelectList(db.tipoMoradas, "id", "tipo", morada.tipo);
            ViewBag.id = new SelectList(db.produtoes, "id", "codcomercial", morada.id);
            return View(morada);
        }

        // GET: Moradas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            morada morada = db.moradas.Find(id);
            if (morada == null)
            {
                return HttpNotFound();
            }
            ViewBag.tipo = new SelectList(db.tipoMoradas, "id", "tipo", morada.tipo);
            ViewBag.id = new SelectList(db.produtoes, "id", "codcomercial", morada.id);
            return View(morada);
        }

        // POST: Moradas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "id,numero,nome,sector,morada1,tipo")] morada morada)
        {
            if (ModelState.IsValid)
            {
                db.Entry(morada).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.tipo = new SelectList(db.tipoMoradas, "id", "tipo", morada.tipo);
            ViewBag.id = new SelectList(db.produtoes, "id", "codcomercial", morada.id);
            return View(morada);
        }

        // GET: Moradas/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            morada morada = await db.moradas.FindAsync(id);
            if (morada == null)
            {
                return HttpNotFound();
            }
            return View(morada);
        }

        // POST: Moradas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            morada morada = await db.moradas.FindAsync(id);
            db.moradas.Remove(morada);
            await db.SaveChangesAsync();
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
    }
}
