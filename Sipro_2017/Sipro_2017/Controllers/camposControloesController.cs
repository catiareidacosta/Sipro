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
    public class camposControloesController : Controller
    {
        private SiproEntities db = new SiproEntities();

        // GET: camposControloes
        public ActionResult Index()
        {
            return View(db.camposControloes.ToList());
        }

        // GET: camposControloes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            camposControlo camposControlo = db.camposControloes.Find(id);
            if (camposControlo == null)
            {
                return HttpNotFound();
            }
            return View(camposControlo);
        }

        // GET: camposControloes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: camposControloes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,descricao")] camposControlo camposControlo)
        {
            if (ModelState.IsValid)
            {
                db.camposControloes.Add(camposControlo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(camposControlo);
        }

        // GET: camposControloes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            camposControlo camposControlo = db.camposControloes.Find(id);
            if (camposControlo == null)
            {
                return HttpNotFound();
            }
            return View(camposControlo);
        }

        // POST: camposControloes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,descricao")] camposControlo camposControlo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(camposControlo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(camposControlo);
        }

        // GET: camposControloes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            camposControlo camposControlo = db.camposControloes.Find(id);
            if (camposControlo == null)
            {
                return HttpNotFound();
            }
            return View(camposControlo);
        }

        // POST: camposControloes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            camposControlo camposControlo = db.camposControloes.Find(id);
            db.camposControloes.Remove(camposControlo);
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
