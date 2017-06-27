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
    public class estadoOrcamentoesController : Controller
    {
        private SiproEntities db = new SiproEntities();

        // GET: estadoOrcamentoes
        public ActionResult Index()
        {
            return View(db.estadoOrcamentoes.ToList());
        }

        // GET: estadoOrcamentoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            estadoOrcamento estadoOrcamento = db.estadoOrcamentoes.Find(id);
            if (estadoOrcamento == null)
            {
                return HttpNotFound();
            }
            return View(estadoOrcamento);
        }

        // GET: estadoOrcamentoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: estadoOrcamentoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,tipo")] estadoOrcamento estadoOrcamento)
        {
            if (ModelState.IsValid)
            {
                db.estadoOrcamentoes.Add(estadoOrcamento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(estadoOrcamento);
        }

        // GET: estadoOrcamentoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            estadoOrcamento estadoOrcamento = db.estadoOrcamentoes.Find(id);
            if (estadoOrcamento == null)
            {
                return HttpNotFound();
            }
            return View(estadoOrcamento);
        }

        // POST: estadoOrcamentoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,tipo")] estadoOrcamento estadoOrcamento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(estadoOrcamento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(estadoOrcamento);
        }

        // GET: estadoOrcamentoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            estadoOrcamento estadoOrcamento = db.estadoOrcamentoes.Find(id);
            if (estadoOrcamento == null)
            {
                return HttpNotFound();
            }
            return View(estadoOrcamento);
        }

        // POST: estadoOrcamentoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            estadoOrcamento estadoOrcamento = db.estadoOrcamentoes.Find(id);
            db.estadoOrcamentoes.Remove(estadoOrcamento);
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
