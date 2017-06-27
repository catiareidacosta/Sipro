using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Sipro_2017
{
    public class produto_newController : Controller
    {
        private SiproEntities db = new SiproEntities();

        // GET: produto_new
        public ActionResult Index()
        {
            var produto_new = db.produto_new.Include(p => p.Encomenda);
            return View(produto_new.ToList());
        }

        // GET: produto_new/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            produto_new produto_new = db.produto_new.Find(id);
            if (produto_new == null)
            {
                return HttpNotFound();
            }
            return View(produto_new);
        }

        // GET: produto_new/Create
        public ActionResult Create()
        {
            ViewBag.id_numeroEncomenda = new SelectList(db.Encomendas, "id", "descricao_encomenda");
            return View();
        }

        // POST: produto_new/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,codcomercial,descricao,qtd,id_numeroEncomenda,ref,NrUnidades,lote,validade,refProduto,numero_guia")] produto_new produto_new)
        {
            if (ModelState.IsValid)
            {
                db.produto_new.Add(produto_new);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_numeroEncomenda = new SelectList(db.Encomendas, "id", "descricao_encomenda", produto_new.id_numeroEncomenda);
            return View(produto_new);
        }

        // GET: produto_new/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            produto_new produto_new = db.produto_new.Find(id);
            if (produto_new == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_numeroEncomenda = new SelectList(db.Encomendas, "id", "descricao_encomenda", produto_new.id_numeroEncomenda);
            return View(produto_new);
        }

        // POST: produto_new/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,codcomercial,descricao,qtd,id_numeroEncomenda,ref,NrUnidades,lote,validade,refProduto,numero_guia")] produto_new produto_new)
        {
            if (ModelState.IsValid)
            {
                db.Entry(produto_new).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_numeroEncomenda = new SelectList(db.Encomendas, "id", "descricao_encomenda", produto_new.id_numeroEncomenda);
            return View(produto_new);
        }

        // GET: produto_new/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            produto_new produto_new = db.produto_new.Find(id);
            if (produto_new == null)
            {
                return HttpNotFound();
            }
            return View(produto_new);
        }

        // POST: produto_new/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            produto_new produto_new = db.produto_new.Find(id);
            db.produto_new.Remove(produto_new);
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
