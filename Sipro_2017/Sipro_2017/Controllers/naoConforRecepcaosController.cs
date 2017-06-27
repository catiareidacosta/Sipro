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
    public class naoConforRecepcaosController : Controller
    {
        private SiproEntities db = new SiproEntities();

        // GET: naoConforRecepcaos
        public ActionResult Index()
        {
            var naoConforRecepcaos = db.naoConforRecepcaos.Include(n => n.tiponaoconformidade_id);
            return View(naoConforRecepcaos.ToList());
        }


     

        // GET: naoConforRecepcaos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            naoConforRecepcao naoConforRecepcao = db.naoConforRecepcaos.Find(id);
            if (naoConforRecepcao == null)
            {
                return HttpNotFound();
            }
            return View(naoConforRecepcao);
        }

        // GET: naoConforRecepcaos/Create
        public ActionResult Create()
        {
            ViewBag.tiponaoconformidade_id = new SelectList(db.TipoNaoConformidades, "id", "descricao");
            return View();
        }

        // POST: naoConforRecepcaos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,guia,tiponaoconformidade_id,obs")] naoConforRecepcao naoConforRecepcao)
        {
            if (ModelState.IsValid)
            {
                db.naoConforRecepcaos.Add(naoConforRecepcao);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.tiponaoconformidade_id = new SelectList(db.TipoNaoConformidades, "id", "descricao", naoConforRecepcao.tiponaoconformidade_id);
            return View(naoConforRecepcao);
        }

        // GET: naoConforRecepcaos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            naoConforRecepcao naoConforRecepcao = db.naoConforRecepcaos.Find(id);
            if (naoConforRecepcao == null)
            {
                return HttpNotFound();
            }
            ViewBag.tiponaoconformidade_id = new SelectList(db.TipoNaoConformidades, "id", "descricao", naoConforRecepcao.tiponaoconformidade_id);
            return View(naoConforRecepcao);
        }

        // POST: naoConforRecepcaos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,guia,tiponaoconformidade_id,obs")] naoConforRecepcao naoConforRecepcao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(naoConforRecepcao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.tiponaoconformidade_id = new SelectList(db.TipoNaoConformidades, "id", "descricao", naoConforRecepcao.tiponaoconformidade_id);
            return View(naoConforRecepcao);
        }

        // GET: naoConforRecepcaos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            naoConforRecepcao naoConforRecepcao = db.naoConforRecepcaos.Find(id);
            if (naoConforRecepcao == null)
            {
                return HttpNotFound();
            }
            return View(naoConforRecepcao);
        }

        // POST: naoConforRecepcaos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            naoConforRecepcao naoConforRecepcao = db.naoConforRecepcaos.Find(id);
            db.naoConforRecepcaos.Remove(naoConforRecepcao);
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
