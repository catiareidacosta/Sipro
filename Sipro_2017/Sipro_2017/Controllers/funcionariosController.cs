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
    public class funcionariosController : Controller
    {
        private SiproEntities db = new SiproEntities();

        // GET: funcionarios
        public ActionResult Index()
        {
            var funcionarios = db.funcionarios.Include(f => f.docQualidade).Include(f => f.listagemGrupo);
            return View(funcionarios.ToList());
        }

        // GET: funcionarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            funcionario funcionario = db.funcionarios.Find(id);
            if (funcionario == null)
            {
                return HttpNotFound();
            }
            return View(funcionario);
        }

        // GET: funcionarios/Create
        public ActionResult Create()
        {
            ViewBag.docs_funcionario = new SelectList(db.docQualidades, "id", "nome");
            ViewBag.grupo_funcionario = new SelectList(db.listagemGrupoes, "id", "nome_grupo");
            return View();
        }

        // POST: funcionarios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nome_user,pass_user,numero_user,data_entrada,grupo_funcionario,docs_funcionario")] funcionario funcionario)
        {
            if (ModelState.IsValid)
            {
                db.funcionarios.Add(funcionario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.docs_funcionario = new SelectList(db.docQualidades, "id", "nome", funcionario.docs_funcionario);
            ViewBag.grupo_funcionario = new SelectList(db.listagemGrupoes, "id", "nome_grupo", funcionario.grupo_funcionario);
            return View(funcionario);
        }

        // GET: funcionarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            funcionario funcionario = db.funcionarios.Find(id);
            if (funcionario == null)
            {
                return HttpNotFound();
            }
            ViewBag.docs_funcionario = new SelectList(db.docQualidades, "id", "nome", funcionario.docs_funcionario);
            ViewBag.grupo_funcionario = new SelectList(db.listagemGrupoes, "id", "nome_grupo", funcionario.grupo_funcionario);
            return View(funcionario);
        }

        // POST: funcionarios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nome_user,pass_user,numero_user,data_entrada,grupo_funcionario,docs_funcionario")] funcionario funcionario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(funcionario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.docs_funcionario = new SelectList(db.docQualidades, "id", "nome", funcionario.docs_funcionario);
            ViewBag.grupo_funcionario = new SelectList(db.listagemGrupoes, "id", "nome_grupo", funcionario.grupo_funcionario);
            return View(funcionario);
        }

        // GET: funcionarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            funcionario funcionario = db.funcionarios.Find(id);
            if (funcionario == null)
            {
                return HttpNotFound();
            }
            return View(funcionario);
        }

        // POST: funcionarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            funcionario funcionario = db.funcionarios.Find(id);
            db.funcionarios.Remove(funcionario);
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
