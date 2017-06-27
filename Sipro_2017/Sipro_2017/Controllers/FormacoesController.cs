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
    public class FormacoesController : Controller
    {
        private SiproEntities db = new SiproEntities();

        // GET: Formacoes
        public ActionResult Index()
        {
            var formacoes = db.Formacoes.Include(f => f.tipoFormacao);
            return View(formacoes.ToList());
        }

        // GET: Formacoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Formaco formaco = db.Formacoes.Find(id);
            if (formaco == null)
            {
                return HttpNotFound();
            }
            return View(formaco);
        }

        // GET: Formacoes/Create
        public ActionResult Create()
        {
            ViewBag.tipoOnJob_id = new SelectList(db.tipoFormacaos, "id", "descricao");
            return View();
        }

        // POST: Formacoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,dataFormacao,tipoOnJob_id,nomeFormacao,numHoras")] Formaco formaco)
        {
            if (ModelState.IsValid)
            {
                db.Formacoes.Add(formaco);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.tipoOnJob_id = new SelectList(db.tipoFormacaos, "id", "descricao", formaco.tipoOnJob_id);
            return View(formaco);
        }

        // GET: Formacoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Formaco formaco = db.Formacoes.Find(id);
            if (formaco == null)
            {
                return HttpNotFound();
            }
            ViewBag.tipoOnJob_id = new SelectList(db.tipoFormacaos, "id", "descricao", formaco.tipoOnJob_id);
            return View(formaco);
        }

        // POST: Formacoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,dataFormacao,tipoOnJob_id,nomeFormacao,numHoras")] Formaco formaco)
        {
            if (ModelState.IsValid)
            {
                db.Entry(formaco).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.tipoOnJob_id = new SelectList(db.tipoFormacaos, "id", "descricao", formaco.tipoOnJob_id);
            return View(formaco);
        }

        // GET: Formacoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Formaco formaco = db.Formacoes.Find(id);
            if (formaco == null)
            {
                return HttpNotFound();
            }
            return View(formaco);
        }

        // POST: Formacoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Formaco formaco = db.Formacoes.Find(id);
            db.Formacoes.Remove(formaco);
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
