using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Sipro_2017;

namespace Sipro_2017.Controllers{
    public class orcamentoesController : Controller{
        private SiproEntities db = new SiproEntities();

        // GET: orcamentoes
        public ActionResult Index(string sortOrder, string currentFilter, string criterioPesquisa, int? page){
            ViewBag.CurrentSort = sortOrder;

            if (criterioPesquisa != null){
                page = 1;
            }
            else{
                criterioPesquisa = currentFilter;
            }

            ViewBag.CurrentFilter = criterioPesquisa;

            var orcamentos = from s in db.orcamentoes
                         select s;

            if (!String.IsNullOrEmpty(criterioPesquisa)){
                orcamentos = orcamentos.Where(s => s.produto.Contains(criterioPesquisa) ||
                        s.id.Equals(criterioPesquisa) || 
                        s.moradas.Equals(criterioPesquisa) ||
                        s.estadoOrcamento.tipo.Equals(criterioPesquisa) ||
                        s.data_alteracao.Equals(criterioPesquisa) ||
                        s.data_aprovacao.Equals(criterioPesquisa));
            }

            switch (sortOrder){
                case "numeroOrcamento_asc":
                    orcamentos = orcamentos.OrderByDescending(s => s.id);
                    break;
                case "numeroOrcamento_desc":
                    orcamentos = orcamentos.OrderBy(s => s.id);
                    break;
                case "produto_asc":
                    orcamentos = orcamentos.OrderByDescending(s => s.produto);
                    break;
                case "produto_desc":
                    orcamentos = orcamentos.OrderBy(s => s.produto);
                    break;
                case "moradas_asc":
                    orcamentos = orcamentos.OrderByDescending(s => s.moradas);
                    break;
                case "moradas_desc":
                    orcamentos = orcamentos.OrderBy(s => s.moradas);
                    break;
                case "tipo_asc":
                    orcamentos = orcamentos.OrderByDescending(s => s.estadoOrcamento.tipo);
                    break;
                case "tipo_desc":
                    orcamentos = orcamentos.OrderBy(s => s.estadoOrcamento.tipo);
                    break;
                case "dtAlteracao_asc":
                    orcamentos = orcamentos.OrderByDescending(s => s.data_alteracao);
                    break;
                case "dtAlteracao_desc":
                    orcamentos = orcamentos.OrderBy(s => s.data_alteracao);
                    break;
                case "dtAprovacao_asc":
                    orcamentos = orcamentos.OrderByDescending(s => s.data_aprovacao);
                    break;
                case "dtAprovacao_desc":
                    orcamentos = orcamentos.OrderBy(s => s.data_aprovacao);
                    break;
                default:
                    orcamentos = orcamentos.OrderByDescending(s => s.id);
                    break;
            }

            int pageSize = 20;
            int pageNumber = 1;
            return View(orcamentos.ToPagedList(pageNumber, pageSize));
        }

        // GET: orcamentoes/Details/5
        public ActionResult Details(int? id){
            if (id == null){
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            orcamento orcamento = db.orcamentoes.Find(id);
            if (orcamento == null){
                return HttpNotFound();
            }
            return View(orcamento);
        }

        // GET: orcamentoes/Create
        public ActionResult Create(){
            ViewBag.estado_orcamento = new SelectList(db.estadoOrcamentoes, "id", "tipo");
            ViewBag.user_alteracao = new SelectList(db.funcionarios, "id", "nome_user");
            ViewBag.morada_orcamento = new SelectList(db.moradas, "id", "nome");
            ViewBag.motivo = new SelectList(db.motivoOrcamentoes, "id", "descricao");
            ViewBag.moradas = new SelectList(db.tipoMoradas, "id", "tipo");
            return View();
        }

        public ActionResult Search(orcamento model){
            //do something with model.Name
            return View(); //return "Search" view to the user
                           //return View(model); //You can also return view with the model to the user
                           //return View("SpecificView"); //You can specify a concrete view name as well 
        }


        public ActionResult FileUpload(){
            int arquivosSalvos = 0;
            for (int i = 0; i < Request.Files.Count; i++){
                HttpPostedFileBase arquivo = Request.Files[i];

                //Suas validações ......

                //Salva o arquivo
                if (arquivo.ContentLength > 0){
                    var uploadPath = Server.MapPath("~/Content/Uploads");
                    string caminhoArquivo = System.IO.Path.Combine(@uploadPath, 
                        System.IO.Path.GetFileName(arquivo.FileName));

                    arquivo.SaveAs(caminhoArquivo);
                    arquivosSalvos++;
                }
            }

            ViewData["Message"] = String.Format("{0} arquivo(s) salvo(s) com sucesso.",
                arquivosSalvos);
            return View("Upload");
        }
    
    // POST: orcamentoes/Create
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
        [ValidateAntiForgeryToken]
        public  ActionResult Create(
            [Bind(Include = "id,qtd_produto,motivo,data_aprovacao,ficheiro_orcamento,data_alteracao,observacoes,loja_id,moradas,produto,estado_orcamento,user_alteracao,morada_orcamento,qtd_produto1, qtd_produto2, qtd_produto3, qtd_produto4, preco_produto1, preco_produto2, preco_produto3, preco_produto4, metrica_produto1, metrica_produto2, metrica_produto3, metrica_produto4	")] orcamento orcamentoRecebido, 
            HttpPostedFileBase file){
            try{
                orcamentoRecebido.moradas = orcamentoRecebido.morada_orcamento;

                if (orcamentoRecebido.morada_orcamento!=0 && orcamentoRecebido.moradas!=0 && orcamentoRecebido.estado_orcamento!=0 ){
                    DateTime localDate = DateTime.Now;
                    orcamentoRecebido.data_alteracao = localDate;
                    int id_estado = orcamentoRecebido.estado_orcamento;

                    estadoOrcamento estado= db.estadoOrcamentoes.Find(id_estado);

                    orcamentoRecebido.moradas = orcamentoRecebido.morada_orcamento;
                    var preco1 = Request.Form["preco_produto1"].ToString();
                    var preco2 = Request.Form["preco_produto2"].ToString();
                    var preco3 = Request.Form["preco_produto3"].ToString();
                    var preco4 = Request.Form["preco_produto4"].ToString();
                    if(preco2!=null && preco2!="")
                        orcamentoRecebido.preco_produto1 = Convert.ToDecimal(preco1.Replace('.',','));
                    if (preco2 != null && preco2 != "")
                        orcamentoRecebido.preco_produto2 = Convert.ToDecimal(preco2.Replace('.', ','));
                    if (preco3 != null && preco3 != "")
                        orcamentoRecebido.preco_produto3 = Convert.ToDecimal(preco3.Replace('.', ','));
                    if (preco4 != null && preco4 != "")
                        orcamentoRecebido.preco_produto4 = Convert.ToDecimal(preco4.Replace('.', ','));


                    if (estado.tipo.Equals("Aprovado") && !orcamentoRecebido.data_aprovacao.HasValue)
                        orcamentoRecebido.data_aprovacao = localDate;

                    orcamentoRecebido.user_alteracao = 1;

                    db.orcamentoes.Add(orcamentoRecebido);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                    // db.orcamentoes.Add(orcamentoRecebido);
                    //db.SaveChanges();
                    //return RedirectToAction("Index");
                }
                
                ViewBag.estado_orcamento = new SelectList(db.estadoOrcamentoes, "id", "tipo", orcamentoRecebido.estado_orcamento);
                ViewBag.user_alteracao = new SelectList(db.funcionarios, "id", "nome_user", orcamentoRecebido.user_alteracao);
                ViewBag.morada_orcamento = new SelectList(db.moradas, "id", "nome", orcamentoRecebido.morada_orcamento);
                ViewBag.motivo = new SelectList(db.motivoOrcamentoes, "id", "descricao", orcamentoRecebido.motivo);
                ViewBag.moradas = new SelectList(db.tipoMoradas, "id", "tipo", orcamentoRecebido.moradas);
                
                return View(orcamentoRecebido);
            }
            catch (System.Data.SqlClient.SqlException e){
                //this.View.ViewData("ficheiro_orcamento");
                return new HttpStatusCodeResult(404, "Erro na criação de Orcamento" + e.Message);
            }

        }

        private void fileUpload(orcamento model){
            
            String fileName = model.ficheiro_orcamento;
            var fullPath = Server.MapPath("~/images/" + fileName);

            var fileSavePath = "";
            var uploadedFile = Request.Files[0];
            fileName = System.IO.Path.GetFileName(uploadedFile.FileName);
            fileSavePath = Server.MapPath("~/App_Data/UploadedFiles/" +
              fileName);
            uploadedFile.SaveAs(fileSavePath);

        }
        // GET: orcamentoes/Edit/5
        public ActionResult Edit(int? id){
            if (id == null){
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            orcamento orcamento = db.orcamentoes.Find(id);
            if (orcamento == null){
                return HttpNotFound();
            }
            ViewBag.estado_orcamento = new SelectList(db.estadoOrcamentoes, "id", "tipo", orcamento.estado_orcamento);
            ViewBag.user_alteracao = new SelectList(db.funcionarios, "id", "nome_user", orcamento.user_alteracao);
            ViewBag.morada_orcamento = new SelectList(db.moradas, "id", "nome", orcamento.morada_orcamento);
            ViewBag.motivo = new SelectList(db.motivoOrcamentoes, "id", "descricao", orcamento.motivo);
            ViewBag.moradas = new SelectList(db.tipoMoradas, "id", "tipo", orcamento.moradas);

            return View(orcamento);
        }

        // POST: orcamentoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
               [Bind(Include = "id,motivo,data_aprovacao,ficheiro_orcamento,data_alteracao,observacoes,loja_id,moradas,produto,estado_orcamento,user_alteracao,morada_orcamento,qtd_produto1, qtd_produto2, qtd_produto3, qtd_produto4, metrica_produto1, metrica_produto2, metrica_produto3, metrica_produto4")] orcamento orcamento){
            if (true){
                db.Entry(orcamento).State = EntityState.Modified;

                var preco1 = Request.Form["preco_produto1"];
                var preco2 = Request.Form["preco_produto2"];
                var preco3 = Request.Form["preco_produto3"];
                var preco4 = Request.Form["preco_produto4"];

                if(preco1!=null && preco1 != "")
                    orcamento.preco_produto1 = Convert.ToDecimal(preco1.Replace(".", ","));
                if (preco2 != "")
                    orcamento.preco_produto2 = Convert.ToDecimal(preco2.Replace(".", ","));
                if (preco3 != "")
                    orcamento.preco_produto3 = Convert.ToDecimal(preco3.Replace(".", ","));
                if (preco4 != "")
                    orcamento.preco_produto4 = Convert.ToDecimal(preco4.Replace(".", ","));

                DateTime localDate = DateTime.Now;
                orcamento.data_alteracao = localDate;
                int id_estado = orcamento.estado_orcamento;
                estadoOrcamento estado = db.estadoOrcamentoes.Find(id_estado);
                orcamento.moradas = orcamento.morada_orcamento;
                orcamento.tipoMorada = db.moradas.Find(orcamento.morada_orcamento).tipoMorada;

                if (estado.tipo.Equals("Aprovado") && db.orcamentoes.Find(orcamento.id).data_aprovacao==null)
                    orcamento.data_aprovacao = localDate;
                orcamento.user_alteracao = 1;
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        // GET: orcamentoes/Delete/5
        public ActionResult Delete(int? id){
            if (id == null){
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            orcamento orcamento = db.orcamentoes.Find(id);
            if (orcamento == null){
                return HttpNotFound();
            }
            return View(orcamento);
        }

        // POST: orcamentoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id){
            orcamento orcamento = db.orcamentoes.Find(id);
            db.orcamentoes.Remove(orcamento);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing){
            if (disposing){
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
