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
    public class produtoesController : Controller
    {
        private SiproEntities db = new SiproEntities();

        // GET: Produtoes
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

            var produtoes = from s in db.produtoes select s;
            //var produtoes = db.var ordemFabricoes = from s in db.OrdemFabricoes select s;.Include(p => p.componente_id).Include(p => p.morada1).Include(p => p.TipoProduto).Include(p => p.rastrealibidade);

            if (!String.IsNullOrEmpty(criterioPesquisa))
            {
                produtoes = produtoes.Where(s => s.codcomercial.Equals(criterioPesquisa) ||
                        s.descricao.Contains(criterioPesquisa) ||
                        s.TipoProduto.Equals(criterioPesquisa) ||
                        s.ean13_produto.Equals(criterioPesquisa) ||
                        s.dun_itf14_subemb.Equals(criterioPesquisa) ||
                        s.itf_itf14_caixa.Equals(criterioPesquisa));
            }

            switch (sortOrder)
            {
                case "codcomercial_asc":
                    produtoes = produtoes.OrderByDescending(s => s.codcomercial);
                    break;
                case "codcomercial_desc":
                    produtoes = produtoes.OrderBy(s => s.codcomercial);
                    break;
                case "descricao_desc":
                    produtoes = produtoes.OrderByDescending(s => s.descricao);
                    break;
                case "descricao_asc":
                    produtoes = produtoes.OrderBy(s => s.descricao);
                    break;
                case "cliente_desc":
                    produtoes = produtoes.OrderByDescending(s => s.morada1.nome);
                    break;
                case "cliente_asc":
                    produtoes = produtoes.OrderBy(s => s.morada1.nome);
                    break;
                case "tipo_produto_desc":
                    produtoes = produtoes.OrderByDescending(s => s.TipoProduto);
                    break;
                case "tipo_produto_asc":
                    produtoes = produtoes.OrderBy(s => s.TipoProduto);
                    break;
                case "ean13_produto_desc":
                    produtoes = produtoes.OrderByDescending(s => s.ean13_produto);
                    break;
                case "ean13_produto_asc":
                    produtoes = produtoes.OrderBy(s => s.ean13_produto);
                    break;
                case "dun_itf14_subemb_desc":
                    produtoes = produtoes.OrderByDescending(s => s.dun_itf14_subemb);
                    break;
                case "dun_itf14_subemb_asc":
                    produtoes = produtoes.OrderBy(s => s.dun_itf14_subemb);
                    break;
                case "itf_itf14_caixa_desc":
                    produtoes = produtoes.OrderByDescending(s => s.itf_itf14_caixa);
                    break;
                case "itf_itf14_caixa_asc":
                    produtoes = produtoes.OrderBy(s => s.itf_itf14_caixa);
                    break;
                default:
                    produtoes = produtoes.OrderByDescending(s => s.id);
                    break;
            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(produtoes.ToPagedList(pageNumber, pageSize));

        }

        //AssociarComposto
        public ActionResult AssociarComposto(){
            ViewBag.codigoProduto_id = new SelectList(db.produtoes, "id", "codcomercial");
            ViewBag.codigoProduto_desc = new SelectList(db.produtoes, "id", "descricao");
            
            return View();
        }

        //AssociarComposto
        public ActionResult AssociarCompostoEdit(string obs)
        {
            ViewBag.codigoProduto_id = new SelectList(db.produtoes, "id", "codcomercial");
            ViewBag.codigoProduto_desc = new SelectList(db.produtoes, "id", "descricao");
            var idProduto = db.produtoes.Find(Convert.ToInt32(obs)).codcomercial.ToString();
            return View(subProdutosEdit(idProduto).ToPagedList(1,10));
        }



        private Boolean editaSubprodutos() {
            var subprodutos_codigoEdit = Request.Form["codcomercial"];
             var codigoEdit =new String[10];
             var qtdEdit = new String[10];
             var subprodutos_codigo = new String[10];
             var subprodutos_qtd = new String[10];

            if (Request.Form["item.codcomercial"] != null)
            {
                codigoEdit = Request.Form["item.codcomercial"].Split(',');
                qtdEdit = Request.Form["item.qtd"].Split(',');
                subProdEdit(subprodutos_codigoEdit);
            }
            if (Request.Form["codigoProd"] != null) { 
                 subprodutos_codigo = Request.Form["codigoProd"].Split(','); 
                 subprodutos_qtd = Request.Form["qtdProd"].Split(',');
                 subProdEditInsert(subprodutos_codigoEdit);
            }
            db.SaveChanges();
            return true;
        }

        private Boolean subProdEdit(string idProduto)
        {
            //elimina elementos já existentes
            List<produto_new> produto = db.produto_new.ToList();
            List<produto_new> codigos = new List<produto_new>();
            var indiceCodigos = 0;
            for (int i = 0; i < produto.Count(); i++)
            {
                produto_new produtoRelacionado = new produto_new();
                produtoRelacionado = produto[i];
                if (produto[i].@ref.Equals(idProduto) && (produto[i].numero_guia == null || produto[i].numero_guia == "") &&
                    (produto[i].id_numeroEncomenda == null || produto[i].id_numeroEncomenda == 0))
                {
                    db.produto_new.Remove(produto[i]);
                    indiceCodigos++;
                }
            }

            ViewBag.codcomercial = codigos;
            //cria novamente elementos
            var codigoEdit = Request.Form["item.codcomercial"].Split(',');
            var qtdEdit = Request.Form["item.qtd"].Split(',');
            
            for (int i = 0; i < codigoEdit.Count(); i++)
            {
                var index = 0;
                if (!codigoEdit[i].Equals(""))
                {
                    var encontrou = false;
                    produto_new sub = new produto_new();
                    while (!encontrou && index < produto.Count())
                    {
                        if (produto[index].codcomercial.Equals(codigoEdit[i]))
                        {
                            encontrou = true;
                            sub.codcomercial = codigoEdit[i].ToString();
                            sub.descricao = produto[index].descricao;
                            sub.qtd = Convert.ToInt16(qtdEdit[i]);
                            sub.@ref = idProduto;
                            sub.refProduto = idProduto;
                            db.produto_new.Add(sub);
                        }
                        index++;
                    }
                }
            }
            return true;
        }

        private Boolean subProdEditInsert(string idProduto)
        {
            var subprodutos_codigo = Request.Form["codigoProd"].Split(',');
            var subprodutos_qtd = Request.Form["qtdProd"].Split(',');
            List<produto> produto = db.produtoes.ToList();
            for (int i = 0; i < subprodutos_codigo.Count(); i++){
                var index = 0;
                if (!subprodutos_codigo[i].Equals("")){
                    var encontrou = false;
                    produto_new sub = new produto_new();
                    while (!encontrou && index < produto.Count()){
                        if (produto[index].codcomercial.Equals(subprodutos_codigo[i])){
                            encontrou = true;
                            sub.codcomercial = subprodutos_codigo[i].ToString();
                            sub.descricao = produto[index].descricao;
                            sub.qtd = Convert.ToInt16(subprodutos_qtd[i]);
                            sub.@ref = idProduto;
                            sub.refProduto = idProduto;
                            db.produto_new.Add(sub);
                        }
                        index++;
                    }
                }
            }
            return true;
        }


        //AssociarComposto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssociarComposto([Bind(Include = "id,codcomercial,descricao,qtd")] produto_new produto)
        {
            Request["qtd"].ToString();
            return View();
        }

        // GET: produtoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            produto produto = db.produtoes.Find(id);
            if (produto == null)
            {
                return HttpNotFound();
            }
            return View(produto);
        }

        // GET: Produtoes/Create
        public ActionResult Create()
        {
            ViewBag.manipulação = new SelectList(db.manipulacaos, "id", "nome");
            ViewBag.morada = new SelectList(db.moradas, "id", "nome");
            ViewBag.componente = new SelectList(db.produtoes, "id", "codcomercial");
            ViewBag.componente_id = new SelectList(db.produtoes, "id", "codcomercial");
            ViewBag.tipoproduto_id = new SelectList(db.TipoProdutoes, "id", "tipo_produto");
            ViewBag.restrealib = new SelectList(db.rastrealibidades, "id", "tipo");
            ViewBag.verificacao = new SelectList(db.verificacaos, "id", "nomeVerificacao");
            ViewBag.codigoProduto_id = new SelectList(db.produtoes, "id", "codcomercial");
            ViewBag.codigoProduto_desc = new SelectList(db.produtoes, "id", "descricao");
            return View();
        }

        private int verificaSubProdutos(string idProduto, string subproduto)
        {
            List<produto_new> produto = db.produto_new.ToList();

            for (int i = 0; i < produto.Count(); i++)
            {
                if (produto[i].codcomercial.Equals(subproduto) && produto[i].@ref.Equals(idProduto) 
                    && (produto[i].numero_guia == null || produto[i].numero_guia == "") && 
                    (produto[i].id_numeroEncomenda == null || produto[i].id_numeroEncomenda == 0)){
                    return produto[i].id;
                }
            }
            return 0;
        }

        private List<produto_new> subProdutosEdit(string idProduto){
            List<produto_new> produto = db.produto_new.ToList();
            List<produto_new> codigos=new List<produto_new>();
            var indiceCodigos = 0;
            for (int i = 0; i < produto.Count(); i++){
                produto_new produtoRelacionado = new produto_new();
                produtoRelacionado = produto[i];
                if (produto[i].@ref.Equals(idProduto) && (produto[i].numero_guia == null || produto[i].numero_guia == "") &&
                    (produto[i].id_numeroEncomenda == null || produto[i].id_numeroEncomenda == 0)){
                    codigos.Insert(indiceCodigos, produtoRelacionado);
                    indiceCodigos++;
                 }
            }
            ViewBag.codcomercial = codigos;
            return codigos;
        }


        private Boolean getsubProdutos(string idProduto)
        {
            var subprodutos_codigo = Request.Form["codcomercial"].Split(',');
            var subprodutos_qtd = Request.Form["qtd"].Split(',');
            List<produto> produto = db.produtoes.ToList();


            for (int i = 0; i < subprodutos_codigo.Count(); i++)
            {
                var index = 0;
                if (!subprodutos_codigo[i].Equals(""))
                {
                    var encontrou = false;
                    produto_new sub = new produto_new();
                    while (!encontrou && index < produto.Count())
                    {
                        produto_new update = db.produto_new.Find(verificaSubProdutos(idProduto, subprodutos_codigo[i]));
                        if (update == null)
                        {
                            if (produto[index].codcomercial.Equals(subprodutos_codigo[i]))
                            {
                                encontrou = true;
                                sub.codcomercial = subprodutos_codigo[i].ToString();
                                sub.descricao = produto[index].descricao;
                                sub.qtd = Convert.ToInt16(subprodutos_qtd[i - 1]);
                                sub.@ref = idProduto;
                                sub.refProduto = idProduto;
                                db.produto_new.Add(sub);
                                // i = 0;
                            }
                        }
                        else
                        {

                        }
                        index++;
                    }
                }
            }
            return true;
        }

        private Boolean subProdutos(string idProduto) {
            var subprodutos_codigo = Request.Form["codcomercial"].Split(',');
            var subprodutos_qtd = Request.Form["qtd"].Split(',');
            List<produto> produto = db.produtoes.ToList();
            
            for (int i = 0; i < subprodutos_codigo.Count(); i++) {
                var index = 0;
                if (!subprodutos_codigo[i].Equals("")) {
                    var encontrou = false;
                    produto_new sub = new produto_new();
                    while (!encontrou && index < produto.Count()) {
                        if (produto[index].codcomercial.Equals(subprodutos_codigo[i])) {
                            encontrou = true;
                            sub.codcomercial = subprodutos_codigo[i].ToString();
                            sub.descricao = produto[index].descricao;
                            sub.qtd = Convert.ToInt16(subprodutos_qtd[i-1]);
                            sub.@ref = idProduto;
                            sub.refProduto = idProduto;
                            db.produto_new.Add(sub);
                           // i = 0;
                        }
                        index++;
                    }
                }
            }
            return true;
        }


        private Boolean exists(String codComercial) {
            SelectList produtosExistentes = new SelectList(db.produtoes, "id", "codcomercial");
            var produtos =produtosExistentes.ToList();
            for (int i = 0; i < produtos.Count; i++) {

                if (produtos[i].Text.Equals(codComercial)){
                    ViewBag.exists = "Produto já registado";
                    return true;
                }
                   
            }
            return false;
        }
        
        // POST: Produtoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create([Bind(Include = "id,codcomercial,descricao,ean13_produto,dun_itf14_subemb,itf_itf14_caixa,comprimento,largura,altura,peso,imagem,NrCxsPlano,NrPlanos,NrUnidadesCx,PesoPorCaixa,NrCxsPalete,UndsPorPalete,CompPalete,LargPalete,AlturaPalete,PesoPalete,ImagemPalete,activo,restrealib,fichaProdutoElaborado,fichaProdutoDataElaboracao,id_tipo_produto,morada,componente,manipulação,verificacao")] produto produto)
        public ActionResult Create([Bind(Include = "id,codcomercial,descricao,ean13_produto,dun_itf14_subemb,itf_itf14_caixa,comprimento,largura,altura,peso,imagem,NrCxsPlano,NrPlanos,NrUnidadesCx,PesoPorCaixa,NrCxsPalete,UndsPorPalete,CompPalete,LargPalete,AlturaPalete,PesoPalete,ImagemPalete,activo,restrealib,fichaProdutoElaborado,fichaProdutoDataElaboracao,id_tipo_produto,morada,componente,manipulação,verificacao")] produto produto){
            
            
            if (ModelState.IsValid && !exists(produto.codcomercial))
            {
                //produto.id = Int32.Parse((Guid.NewGuid()).ToString);
                db.produtoes.Add(produto);
                // await db.SaveChangesAsync();
                var subprodutos_codigo = Request.Form["codcomercial"];
                var subprodutos_desc = Request.Form["descProdutos"];
                var subprodutos_qtd = Request.Form["qtd"];

                subProdutos(produto.codcomercial);
                //for (int i = 0; i < Request.Form.AllKeys.Count(); i++){
                //    var result = Request.Form.Get(i);
                //if (result != null && result != "") {
                //var resultado2 = result;
             
                db.SaveChanges();
                return RedirectToAction("Index");
        }
        //ViewBag.manipulação = new SelectList(db.manipulacaos, "id", "nome", produto.manipulação);
        ViewBag.morada = new SelectList(db.moradas, "id", "nome", produto.morada);
        ViewBag.componente_id = new SelectList(db.produtoes, "id", "codcomercial", produto.componente_id);
        ViewBag.tipoproduto_id = new SelectList(db.TipoProdutoes, "id", "tipo_produto", produto.produto1);
        ViewBag.restrealib = new SelectList(db.rastrealibidades, "id", "tipo", produto.rastrealibidade);
        //ViewBag.verificacao = new SelectList(db.verificacaos, "id", "nomeVerificacao", produto.);
        ViewBag.codigoProduto_id = new SelectList(db.produtoes, "id", "codcomercial");
        ViewBag.codigoProduto_desc = new SelectList(db.produtoes, "id", "descricao");
       
        return View(produto);
    }

        // GET: produtoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            produto produto = db.produtoes.Find(id);
            if (produto == null)
            {
                return HttpNotFound();
            }
            //ViewBag.manipulação = new SelectList(db.manipulacaos, "id", "nome", produto.m);
            ViewBag.morada = new SelectList(db.moradas, "id", "nome", produto.morada);
            //ViewBag.componente = new SelectList(db.produtoes, "id", "codcomercial", produto.componente);
            ViewBag.componente_id = new SelectList(db.produtoes, "id", "codcomercial");
            ViewBag.tipoproduto_id = new SelectList(db.TipoProdutoes, "id", "tipo_produto", produto.produto1);
            ViewBag.restrealib = new SelectList(db.rastrealibidades, "id", "tipo", produto.rastrealibidade);
            ViewBag.codigoProduto_id = new SelectList(db.produtoes, "id", "codcomercial");
            ViewBag.id = id;
           
            //ViewBag.verificacao = new SelectList(db.verificacaos, "id", "nomeVerificacao", produto.verificacao);
            //subProdutosEdit(produto.codcomercial);
            return View(produto);
        }

        // POST: produtoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,codcomercial,descricao,ean13_produto,dun_itf14_subemb,itf_itf14_caixa,comprimento,largura,altura,peso,imagem,NrCxsPlano,NrPlanos,NrUnidadesCx,PesoPorCaixa,NrCxsPalete,UndsPorPalete,CompPalete,LargPalete,AlturaPalete,PesoPalete,ImagemPalete,activo,fichaProdutoElaborado_id,fichaProdutoDataElaboracao,tipoproduto_id,morada,componente_id")] produto produto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(produto).State = EntityState.Modified;
                editaSubprodutos();
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.manipulação = new SelectList(db.manipulacaos, "id", "nome", produto.m);
            ViewBag.morada = new SelectList(db.moradas, "id", "nome", produto.morada);
            //ViewBag.componente = new SelectList(db.produtoes, "id", "codcomercial", produto.componente);
            ViewBag.componente_id = new SelectList(db.produtoes, "id", "codcomercial", produto.componente_id);
            ViewBag.tipoproduto_id = new SelectList(db.TipoProdutoes, "id", "tipo_produto", produto.produto1);
            ViewBag.codigoProduto_id = new SelectList(db.produtoes, "id", "codcomercial");
            ViewBag.restrealib = new SelectList(db.rastrealibidades, "id", "tipo", produto.rastrealibidade);
            //ViewBag.verificacao = new SelectList(db.verificacaos, "id", "nomeVerificacao", produto.verificacao);

            return View(produto);
        }

        // GET: produtoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            produto produto = db.produtoes.Find(id);
            if (produto == null)
            {
                return HttpNotFound();
            }
            return View(produto);
        }

        // POST: produtoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            produto produto = db.produtoes.Find(id);
            db.produtoes.Remove(produto);
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
