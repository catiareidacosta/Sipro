using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Sipro_2017;
using PagedList;

namespace Sipro_2017.Controllers
{
    public class RHsController : Controller
    {
        private SiproEntities db = new SiproEntities();

        // GET: RHs
        public ActionResult Index(string sortOrder, string currentFilter, string criterioPesquisa, int? page)
        {

            ViewBag.CurrentSort = sortOrder;
            if (criterioPesquisa != null)
            {
                page = 1;
            }
            else
            {
                criterioPesquisa = currentFilter;
            }
            ViewBag.CurrentFilter = criterioPesquisa;

            var rHs = db.RHs.Include(r => r.nrFuncionario).Include(r => r.nome).Include(r => r.dataAdmissao)
                .Include(r => r.telefone).Include(r => r.docIdenfiticacao).Include(r => r.registoCriminal)
                .Include(r => r.temporario).Include(r => r.numeroContrato).Include(r => r.dataInicioContrato)
                .Include(r => r.dataFimContrato).Include(r => r.empresa.tipo).Include(r => r.role.tipo);

            if (!String.IsNullOrEmpty(criterioPesquisa))
            {
                rHs = rHs.Where(s => s.nrFuncionario.Equals(criterioPesquisa) || 
                        s.nome.Contains(criterioPesquisa) || s.dataAdmissao.Equals(criterioPesquisa) ||
                        s.telefone.Equals(criterioPesquisa) || s.nrFuncionario.Equals(criterioPesquisa) ||
                        s.empresa.tipo.Equals(criterioPesquisa) || s.role.tipo.Equals(criterioPesquisa) ||
                        s.dataInicioContrato.Equals(criterioPesquisa) || s.dataFimContrato.Equals(criterioPesquisa));
            }

            switch (sortOrder)
            {
                case "nrFuncionario_asc":
                    rHs = rHs.OrderByDescending(s => s.nrFuncionario);
                    break;
                case "nrFuncionario_desc":
                    rHs = rHs.OrderBy(s => s.nrFuncionario);
                    break;
                case "nome_asc":
                    rHs = rHs.OrderByDescending(s => s.nome);
                    break;
                case "nome_desc":
                    rHs = rHs.OrderBy(s => s.nome);
                    break;
                case "dataAdmissao_asc":
                    rHs = rHs.OrderByDescending(s => s.dataAdmissao);
                    break;
                case "dataAdmissao_desc":
                    rHs = rHs.OrderBy(s => s.dataAdmissao);
                    break;
                case "telefone_asc":
                    rHs = rHs.OrderByDescending(s => s.telefone);
                    break;
                case "telefone_desc":
                    rHs = rHs.OrderBy(s => s.telefone);
                    break;
                case "temporario_asc":
                    rHs = rHs.OrderByDescending(s => s.temporario);
                    break;
                case "temporario_desc":
                    rHs = rHs.OrderBy(s => s.temporario);
                    break;
                case "numeroContrato_asc":
                    rHs = rHs.OrderByDescending(s => s.numeroContrato);
                    break;
                case "numeroContrato_desc":
                    rHs = rHs.OrderBy(s => s.numeroContrato);
                    break;
                case "dataInicioContrato_asc":
                    rHs = rHs.OrderByDescending(s => s.dataInicioContrato);
                    break;
                case "dataInicioContrato_desc":
                    rHs = rHs.OrderBy(s => s.dataInicioContrato);
                    break;
                case "dataFimContrato_asc":
                    rHs = rHs.OrderByDescending(s => s.dataFimContrato);
                    break;
                case "dataFimContrato_desc":
                    rHs = rHs.OrderBy(s => s.dataFimContrato);
                    break;
                case "empresa_asc":
                    rHs = rHs.OrderByDescending(s => s.empresa.tipo);
                    break;
                case "empresa_desc":
                    rHs = rHs.OrderBy(s => s.empresa.tipo);
                    break;
                case "role_asc":
                    rHs = rHs.OrderByDescending(s => s.role.tipo);
                    break;
                case "role_desc":
                    rHs = rHs.OrderBy(s => s.role.tipo);
                    break;
                default:
                    rHs = rHs.OrderBy(s => s.nrFuncionario);
                    break;
            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(rHs.ToPagedList(pageNumber, pageSize));

        }

        // GET: RHs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RH rH = db.RHs.Find(id);
            if (rH == null)
            {
                return HttpNotFound();
            }
            return View(rH);
        }

        // GET: RHs/Create
        public ActionResult Create()
        {
            ViewBag.departamento_id = new SelectList(db.departamentoes, "id", "nome");
            ViewBag.empresa_id = new SelectList(db.empresas, "id", "tipo");
            ViewBag.role_id = new SelectList(db.roles, "id", "tipo");
            return View();
        }

        // POST: RHs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nrFuncionario,nome,ativo,role_id,departamento_id,empresa_id,custoHora,documentos,diasFerias,fardamentoNr,sapatos,calca,polo,casaco,bata,dataAdmissao,morada,telefone,email,dataNascimento,nif,docIdenfiticacao,fichaAptidao,curriculo,certHabilitacoes,registoCriminal,temporario,numeroContrato,nss")] RH rH)
        {
            if (ModelState.IsValid)
            {
                db.RHs.Add(rH);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.departamento_id = new SelectList(db.departamentoes, "id", "nome", rH.departamento_id);
            ViewBag.empresa_id = new SelectList(db.empresas, "id", "tipo", rH.empresa_id);
            ViewBag.role_id = new SelectList(db.roles, "id", "tipo", rH.role_id);
            return View(rH);
        }

        // GET: RHs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RH rH = db.RHs.Find(id);
            if (rH == null)
            {
                return HttpNotFound();
            }
            ViewBag.departamento_id = new SelectList(db.departamentoes, "id", "nome", rH.departamento_id);
            ViewBag.empresa_id = new SelectList(db.empresas, "id", "tipo", rH.empresa_id);
            ViewBag.role_id = new SelectList(db.roles, "id", "tipo", rH.role_id);
            return View(rH);
        }

        // POST: RHs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,nrFuncionario,nome,ativo,role_id,departamento_id,empresa_id,custoHora,documentos,diasFerias,fardamentoNr,sapatos,calca,polo,casaco,bata,dataAdmissao,morada,telefone,email,dataNascimento,nif,docIdenfiticacao,fichaAptidao,curriculo,certHabilitacoes,registoCriminal,temporario,numeroContrato,nss")] RH rH)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rH).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.departamento_id = new SelectList(db.departamentoes, "id", "nome", rH.departamento_id);
            ViewBag.empresa_id = new SelectList(db.empresas, "id", "tipo", rH.empresa_id);
            ViewBag.role_id = new SelectList(db.roles, "id", "tipo", rH.role_id);
            return View(rH);
        }

        // GET: RHs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RH rH = db.RHs.Find(id);
            if (rH == null)
            {
                return HttpNotFound();
            }
            return View(rH);
        }

        // POST: RHs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RH rH = db.RHs.Find(id);
            db.RHs.Remove(rH);
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
