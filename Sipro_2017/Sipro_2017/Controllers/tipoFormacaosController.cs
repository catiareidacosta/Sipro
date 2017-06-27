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
    public class tipoFormacaosController : Controller
    {
        private SiproEntities db = new SiproEntities();

        // GET: tipoFormacaos
        public ActionResult Index()
        {
            return View(db.tipoFormacaos.ToList());
        }

        // GET: tipoFormacaos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tipoFormacao tipoFormacao = db.tipoFormacaos.Find(id);
            if (tipoFormacao == null)
            {
                return HttpNotFound();
            }
            return View(tipoFormacao);
        }

        // GET: tipoFormacaos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: tipoFormacaos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,descricao,dependencia")] tipoFormacao tipoFormacao)
        {
            if (ModelState.IsValid)
            {
                db.tipoFormacaos.Add(tipoFormacao);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipoFormacao);
        }

        // GET: tipoFormacaos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tipoFormacao tipoFormacao = db.tipoFormacaos.Find(id);
            if (tipoFormacao == null)
            {
                return HttpNotFound();
            }
            return View(tipoFormacao);
        }

        // POST: tipoFormacaos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,descricao,dependencia")] tipoFormacao tipoFormacao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoFormacao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoFormacao);
        }

        // GET: tipoFormacaos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tipoFormacao tipoFormacao = db.tipoFormacaos.Find(id);
            if (tipoFormacao == null)
            {
                return HttpNotFound();
            }
            return View(tipoFormacao);
        }

        // POST: tipoFormacaos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tipoFormacao tipoFormacao = db.tipoFormacaos.Find(id);
            db.tipoFormacaos.Remove(tipoFormacao);
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
