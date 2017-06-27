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
    public class motivoOrcamentoesController : Controller
    {
        private SiproEntities db = new SiproEntities();

        // GET: motivoOrcamentoes
        public ActionResult Index()
        {
            return View(db.motivoOrcamentoes.ToList());
        }

        // GET: motivoOrcamentoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            motivoOrcamento motivoOrcamento = db.motivoOrcamentoes.Find(id);
            if (motivoOrcamento == null)
            {
                return HttpNotFound();
            }
            return View(motivoOrcamento);
        }

        // GET: motivoOrcamentoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: motivoOrcamentoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,descricao")] motivoOrcamento motivoOrcamento)
        {
            if (ModelState.IsValid)
            {
                db.motivoOrcamentoes.Add(motivoOrcamento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(motivoOrcamento);
        }

        // GET: motivoOrcamentoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            motivoOrcamento motivoOrcamento = db.motivoOrcamentoes.Find(id);
            if (motivoOrcamento == null)
            {
                return HttpNotFound();
            }
            return View(motivoOrcamento);
        }

        // POST: motivoOrcamentoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,descricao")] motivoOrcamento motivoOrcamento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(motivoOrcamento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(motivoOrcamento);
        }

        // GET: motivoOrcamentoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            motivoOrcamento motivoOrcamento = db.motivoOrcamentoes.Find(id);
            if (motivoOrcamento == null)
            {
                return HttpNotFound();
            }
            return View(motivoOrcamento);
        }

        // POST: motivoOrcamentoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            motivoOrcamento motivoOrcamento = db.motivoOrcamentoes.Find(id);
            db.motivoOrcamentoes.Remove(motivoOrcamento);
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
