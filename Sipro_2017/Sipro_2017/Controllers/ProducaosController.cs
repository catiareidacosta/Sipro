using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Sipro_2017;

namespace Sipro_2017.Controllers
{
    public class ProducaosController : Controller
    {
        private SiproEntities db = new SiproEntities();

        // GET: Producaos
        public ActionResult Index()
        {
            var producaos = db.Producaos.Include(p => p.Encomenda);
            return View(producaos.ToList());
        }

        // GET: Producaos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producao producao = db.Producaos.Find(id);
            if (producao == null)
            {
                return HttpNotFound();
            }
            return View(producao);
        }

        // GET: Producaos/Create
        public ActionResult Create()
        {
            ViewBag.encomenda_id = new SelectList(db.Encomendas, "id", "descricao_encomenda");
            return View();
        }

        // POST: Producaos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,qtd_produzir,qtd_produzida,tempo_total_producao,observacoes,concluido,encomenda_id")] Producao producao)
        {
            if (ModelState.IsValid)
            {
                db.Producaos.Add(producao);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.encomenda_id = new SelectList(db.Encomendas, "id", "descricao_encomenda", producao.encomenda_id);
            return View(producao);
        }

        // GET: Producaos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producao producao = db.Producaos.Find(id);
            if (producao == null)
            {
                return HttpNotFound();
            }
            ViewBag.encomenda_id = new SelectList(db.Encomendas, "id", "descricao_encomenda", producao.encomenda_id);
            return View(producao);
        }

        // POST: Producaos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,qtd_produzir,qtd_produzida,tempo_total_producao,observacoes,concluido,encomenda_id")] Producao producao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(producao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.encomenda_id = new SelectList(db.Encomendas, "id", "descricao_encomenda", producao.encomenda_id);
            return View(producao);
        }

        // GET: Producaos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producao producao = db.Producaos.Find(id);
            if (producao == null)
            {
                return HttpNotFound();
            }
            return View(producao);
        }

        // POST: Producaos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Producao producao = db.Producaos.Find(id);
            db.Producaos.Remove(producao);
            db.SaveChanges();
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
