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
    public class tipoMoradasController : Controller
    {
        private SiproEntities db = new SiproEntities();

        // GET: tipoMoradas
        public ActionResult Index()
        {
            return View(db.tipoMoradas.ToList());
        }

        // GET: tipoMoradas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tipoMorada tipoMorada = db.tipoMoradas.Find(id);
            if (tipoMorada == null)
            {
                return HttpNotFound();
            }
            return View(tipoMorada);
        }

        // GET: tipoMoradas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: tipoMoradas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,tipo")] tipoMorada tipoMorada)
        {
            if (ModelState.IsValid)
            {
                db.tipoMoradas.Add(tipoMorada);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipoMorada);
        }

        // GET: tipoMoradas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tipoMorada tipoMorada = db.tipoMoradas.Find(id);
            if (tipoMorada == null)
            {
                return HttpNotFound();
            }
            return View(tipoMorada);
        }

        // POST: tipoMoradas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,tipo")] tipoMorada tipoMorada)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoMorada).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoMorada);
        }

        // GET: tipoMoradas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tipoMorada tipoMorada = db.tipoMoradas.Find(id);
            if (tipoMorada == null)
            {
                return HttpNotFound();
            }
            return View(tipoMorada);
        }

        // POST: tipoMoradas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tipoMorada tipoMorada = db.tipoMoradas.Find(id);
            db.tipoMoradas.Remove(tipoMorada);
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
