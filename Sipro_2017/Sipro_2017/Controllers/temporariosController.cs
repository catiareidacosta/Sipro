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
    public class temporariosController : Controller
    {
        private SiproEntities db = new SiproEntities();

        // GET: temporarios
        public ActionResult Index()
        {
            return View(db.temporarios.ToList());
        }

        // GET: temporarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            temporario temporario = db.temporarios.Find(id);
            if (temporario == null)
            {
                return HttpNotFound();
            }
            return View(temporario);
        }

        // GET: temporarios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: temporarios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,dataAdmissao")] temporario temporario)
        {
            if (ModelState.IsValid)
            {
                db.temporarios.Add(temporario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(temporario);
        }

        // GET: temporarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            temporario temporario = db.temporarios.Find(id);
            if (temporario == null)
            {
                return HttpNotFound();
            }
            return View(temporario);
        }

        // POST: temporarios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,dataAdmissao")] temporario temporario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(temporario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(temporario);
        }

        // GET: temporarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            temporario temporario = db.temporarios.Find(id);
            if (temporario == null)
            {
                return HttpNotFound();
            }
            return View(temporario);
        }

        // POST: temporarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            temporario temporario = db.temporarios.Find(id);
            db.temporarios.Remove(temporario);
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
