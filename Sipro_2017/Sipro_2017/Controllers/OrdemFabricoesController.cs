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
    public class OrdemFabricoesController : Controller
    {
        private SiproEntities db = new SiproEntities();

        // GET: OrdemFabricoes
        public ActionResult Index(string sortOrder, string currentFilter, string criterioPesquisa, int? page){
            ViewBag.CurrentSort = sortOrder;

            if (criterioPesquisa != null){
                page = 1;
            }
            else{
                criterioPesquisa = currentFilter;
            }

            ViewBag.CurrentFilter = criterioPesquisa;
        
            var ordemFabricoes = from s in db.OrdemFabricoes select s;

            if (!String.IsNullOrEmpty(criterioPesquisa)){
                ordemFabricoes = ordemFabricoes.Where(s => s.id.Equals(criterioPesquisa) ||
                        s.numPessoas.Equals(criterioPesquisa) ||
                        s.racioPessoa.Equals(criterioPesquisa) ||
                        s.produto.id.Equals(criterioPesquisa) ||
                        s.produto.descricao.Equals(criterioPesquisa));
            }

            switch (sortOrder){
                case "of_desc": ordemFabricoes = ordemFabricoes.OrderByDescending(s => s.id); break;
                case "of_asc": ordemFabricoes = ordemFabricoes.OrderBy(s => s.id); break;
                case "numpessoa_desc": ordemFabricoes = ordemFabricoes.OrderByDescending(s => s.numPessoas); break;
                case "numpessoa_asc": ordemFabricoes = ordemFabricoes.OrderBy(s => s.numPessoas); break;
                case "raciopessoa_asc": ordemFabricoes = ordemFabricoes.OrderByDescending(s => s.racioPessoa); break;
                case "raciopessoa_desc": ordemFabricoes = ordemFabricoes.OrderBy(s => s.racioPessoa); break;
                case "referencia_asc": ordemFabricoes = ordemFabricoes.OrderByDescending(s => s.produto.id); break;
                case "referencia_desc": ordemFabricoes = ordemFabricoes.OrderBy(s => s.produto.id); break;    
                case "descricao_desc": ordemFabricoes = ordemFabricoes.OrderByDescending(s => s.produto.descricao); break;
                case "descricao_asc": ordemFabricoes = ordemFabricoes.OrderBy(s => s.produto.descricao); break;
                case "estado_dataFim": ordemFabricoes = ordemFabricoes.OrderByDescending(s => s.dataFim);break;
                case "estado_fimPausa": ordemFabricoes = ordemFabricoes.OrderByDescending(s => s.dataInicioPausa); break;
                case "estado_inicioPausa": ordemFabricoes = ordemFabricoes.OrderBy(s => s.dataInicioPausa); break;
                default: ordemFabricoes = ordemFabricoes.OrderByDescending(s => s.id); break;
            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);

            return View(ordemFabricoes.ToPagedList(pageNumber, pageSize));
        }

        // GET: OrdemFabricoes/Details/5
        public ActionResult Details(int? id){
            if (id != null){
                return View("InvalidAcess");
            }
            OrdemFabrico ordemFabrico = db.OrdemFabricoes.Find(id);
            if (ordemFabrico == null){
                return HttpNotFound();
            }
            return View(ordemFabrico);
        }
        
        // GET: OrdemFabricoes/OFpesquisaEncomenda
        public ActionResult OFpesquisaEncomenda(){
            ViewBag.produto_id = new SelectList(db.produtoes, "id", "codcomercial");
            ViewBag.encomenda = new SelectList(db.Encomendas, "id", "descricao_encomenda");
            return View();
        }

        // GET: OrdemFabricoes/OFpesquisaEncomenda
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OFpesquisaEncomenda([Bind(Include = "encomenda_id")] OrdemFabrico ordemFabrico) {
            if (ordemFabrico != null){
                ViewBag.produto_id = new SelectList(db.produtoes, "id", "codcomercial", ordemFabrico.produto_id);
                return RedirectToAction("OFpesquisaMenu", new { ordemFab = ordemFabrico.encomenda_id });
            }
            else
                return View("InvalidAcess");
        }

        // GET: OrdemFabricoes/Create
        public ActionResult Create(){
            ViewBag.produto_id = new SelectList(db.produtoes, "id", "codcomercial");
            return View();
        }

        // GET: OrdemFabricoes/OFpesquisaEncomenda
        public ActionResult OFpesquisaMenu(int ordemFab){
            if (ordemFab != 0){
                var ordensId = new SelectList(db.OrdemFabricoes, "id", "id").Count();
                List<produto_new> produtosEncomendas = new List<produto_new>();

                List<produto_new> produtosEncomenda = db.produto_new.ToList();
                for (int i = 0; i < produtosEncomenda.Count; i++){
                    if (produtosEncomenda[i].id_numeroEncomenda == ordemFab){
                        produtosEncomendas.Add(produtosEncomenda[i]);
                    }
                }
                ViewBag.produto_id = new SelectList(db.produtoes, "id", "codcomercial");
                ViewBag.encomenda = ordemFab;
                ViewBag.produtosList = new SelectList(produtosEncomendas, "id", "codcomercial");
                ViewBag.id = ordensId;
                ViewBag.erroProdut = "";
                return View();
            }
            else
                return View("InvalidAcess");
        }



        // GET: OrdemFabricoes/OFpesquisaEncomenda
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OFpesquisaMenu(String numeroEncomenda, [Bind(Include = "numeroEncomenda,encomenda,numPessoas,produto_id,produto,LOTE,produto,validade")] OrdemFabrico ordemFabrico) {
            if (ordemFabrico != null || numeroEncomenda != ""){
                var produtosList = db.produtoes;
                var validade = Request.Form["validade_of"];
                Boolean encontrou = false;
                if (produtosList.Count() > 0) {
                    var i = 0;
                    while (!encontrou && i < produtosList.Count()) {
                        produto produtos = (produtosList.ToArray())[i];
                        if (produtos.codcomercial.Equals(Request.Form["produto_id"])) {
                            encontrou = true;
                            ordemFabrico.produto = produtos;
                        }
                        i++;
                    }
                }
                ordemFabrico.encomenda_id = Int32.Parse(numeroEncomenda);
                ordemFabrico.dataInicio = DateTime.Now;
                try{
                    if (validade != null)
                        ordemFabrico.validade = Convert.ToDateTime(validade);
                }
                catch { }

                db.OrdemFabricoes.Add(ordemFabrico);
                db.SaveChanges();

                ViewBag.produto_id = new SelectList(db.produtoes, "id", "codcomercial", ordemFabrico.produto_id);
                return RedirectToAction("OFSubMenu", new { of = ordemFabrico.id });
            }
            else
                return View("InvalidAcess");
        }

        // GET: OrdemFabricoes/OFpesquisaEncomenda
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OFSubMenu(OrdemFabrico ordemFabrico){
            if (ordemFabrico != null){
                ViewBag.id = ordemFabrico.id;
                ViewBag.informativo = "Off:" + ordemFabrico.produto.codcomercial + ";" + ordemFabrico.produto.descricao;
                if (ordemFabrico.dataInicioPausa != null && ordemFabrico.dataFimPausa == null)
                    ViewBag.paused = "true";
                else
                    ViewBag.paused = "false";
                return View(ordemFabrico);
            }
            else
                return View("InvalidAcess");
    }

        // GET: OrdemFabricoes/OFSubMenu
        public ActionResult OFSubMenu(string of){
            if (of != ""){
                OrdemFabrico ordem = db.OrdemFabricoes.Find(Int32.Parse(of));
                ViewBag.id = ordem.id;
                //paused
                if (ordem.dataInicioPausa!=null)// && ordem.dataFimPausa == null)
                    ViewBag.paused = "true";
                else
                    ViewBag.paused = "false";
                if(ordem.produto!=null)
                    if(ordem.produto.codcomercial!=null && ordem.produto.descricao!=null)
                        ViewBag.informativo = "Off : "+ ordem.produto.codcomercial + " ; " +ordem.produto.descricao;
                return View(ordem);
            }
            else
                return View("InvalidAcess");
    }


// POST: OrdemFabricoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,produto_id,numPessoas,dataInicio,dataFim")] OrdemFabrico ordemFabrico){
            if (ordemFabrico != null){
                if (ModelState.IsValid){
                    db.OrdemFabricoes.Add(ordemFabrico);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.produto_id = new SelectList(db.produtoes, "id", "codcomercial", ordemFabrico.produto_id);
                return View(ordemFabrico);
            }
            else
                return View("InvalidAcess");
    }

        // GET: OrdemFabricoes/Edit/5
        public ActionResult Edit(int? id){
            if (id != 0){
                if (id == null){
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                OrdemFabrico ordemFabrico = db.OrdemFabricoes.Find(id);
                if (ordemFabrico == null){
                    return HttpNotFound();
                }
                ViewBag.produto_id = new SelectList(db.produtoes, "id", "codcomercial", ordemFabrico.produto_id);
                return View(ordemFabrico);
            }
            else
                return View("InvalidAcess");
    }

        // POST: OrdemFabricoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,produto_id,numPessoas,dataInicio,dataFim")] OrdemFabrico ordemFabrico){
            if (ordemFabrico != null){
                if (ModelState.IsValid){
                    db.Entry(ordemFabrico).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.produto_id = new SelectList(db.produtoes, "id", "codcomercial", ordemFabrico.produto_id);
                return View(ordemFabrico);
            }
            else
                return View("InvalidAcess");
    }

    // GET: OrdemFabricoes/Delete/5
    public ActionResult Delete(int? id){
            if (id != 0){
                if (id == null){
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                OrdemFabrico ordemFabrico = db.OrdemFabricoes.Find(id);
                if (ordemFabrico == null){
                    return HttpNotFound();
                }
                return View(ordemFabrico);
            }
            else
                return View("InvalidAcess");
    }

        // POST: OrdemFabricoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id){
            OrdemFabrico ordemFabrico = db.OrdemFabricoes.Find(id);
            var pausasOF = db.ParagemProducaos.ToList();
            for (int i = 0; i < pausasOF.Count(); i++) {
                if (pausasOF[i].OF_id == id){
                    db.ParagemProducaos.Remove(pausasOF[i]);
                }
            }
            db.OrdemFabricoes.Remove(ordemFabrico);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing){
            if (disposing){
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private string getDescricao(string numeroguia, string idRef) {
            var produtoOF = db.produtoes.ToList();
            for (int i = 0; i < produtoOF.Count(); i++)
            {
                if (produtoOF[i].descricao != null)
                {
                    if (produtoOF[i].codcomercial.Equals(idRef))
                    {
                        return produtoOF[i].descricao;
                    }
                }
            }
            return "";
        }
        //novos métodos SubMenu Ordem Fabrico
        // GET: OrdemFabricoes/OfColocacaoEmLinhaSegundo
        public ActionResult OfColocacaoEmLinhaSegundo(string obs){
            if (obs != ""){
                    var ordemF = obs.Split(';')[0];
                var idproduto = obs.Split(';')[1];
                OrdemFabrico ordem = db.OrdemFabricoes.Find(Int32.Parse(ordemF));
                ViewBag.id = ordem.id;
                ViewBag.idProduto= idproduto;
                produto_new produtoOF = db.produto_new.Find(Int32.Parse(idproduto));
                //ViewBag.referencia = produtoOF.@ref;
                //ViewBag.artigo = produtoOF.descricao;
                var produto = db.produto_new.Find(ordem.produto_id);
                ViewBag.referencia = produtoOF.@ref;
                //vai buscar a dewscricao associada ao REFERENCIA da OF
                ViewBag.artigo = getDescricao(produtoOF.numero_guia, produtoOF.@ref);
                ViewBag.validadeProduto = produtoOF.validade.ToString().Split(' ')[0];
                ViewBag.qtdProduto = produtoOF.NrUnidades;
                ViewBag.loteproduto = produtoOF.lote;
                ViewBag.gtinFabricados = idproduto;
                ViewBag.qtdProduto = produtoOF.NrUnidades;
                ViewBag.qtdPalteteProduto =  Int32.Parse(produtoOF.NrUnidades.ToString()) ;
                
                if (ordem.produto != null)
                    if (ordem.produto.codcomercial != null && ordem.produto.descricao != null)
                        ViewBag.informativo = "Off : " + ordem.produto.codcomercial + " ; " + ordem.produto.descricao;
                return View(ordem);
            }
            else
                return View("InvalidAcess");
    }

        // GET: OrdemFabricoes/OfColocacaoEmLinha
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OfColocacaoEmLinhaSegundo([Bind(Include = "id,produto_id,numPessoas,dataInicio,dataFim,qtdProduto")] OrdemFabrico ordemFabrico){
            return View(ordemFabrico);
        }

        //OfColocacaoEmLinhaSave , ActionName("SaveOF")
        // GET: OrdemFabricoes/OfColocacaoEmLinha
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult OfColocacaoEmLinhaSave(string obs){
            if (obs == ""){
                return View("InvalidAcess");
            }
            else{
                var ordemF = obs.Split(';')[0];
                var qtd = obs.Split(';')[1];
                var produto = obs.Split(';')[3];
                //duvida para guardar o produto tenho de ir buscar a referencia produto new e dps produto

                OrdemFabrico ordem = db.OrdemFabricoes.Find(Int32.Parse(ordemF));
                //ordem. = Int32.Parse(produto);
                ordem.qtdPalteteProduto =Int32.Parse(obs.Split(';')[2]);
                
                ViewBag.id = ordem.id;
                ordem.qtdFabricado = Decimal.Parse(qtd);
                db.SaveChanges();
                return RedirectToAction("OFSubMenu", new { of = ordem.id });
            }
        }
        //OfColocacaoEmLinha, obs  
        // GET: OrdemFabricoes/OfColocacaoEmLinha
        public ActionResult OfColocacaoEmLinha(string obs){
            if (obs == ""){
                return View("InvalidAcess");
            }
            else{
                OrdemFabrico ordem = db.OrdemFabricoes.Find(Int32.Parse(obs));
                ViewBag.id = ordem.id;
                if (ordem.produto != null)
                    if (ordem.produto.codcomercial != null && ordem.produto.descricao != null)
                        ViewBag.informativo = "Off : " + ordem.produto.codcomercial + " ; " + ordem.produto.descricao;
                ViewBag.numGtinRecebidos = new SelectList(db.produto_new, "id", "ref").Concat(new SelectList(db.gtinRecebidos, "id", "numGtin"));
                ViewBag.numProd = new SelectList(db.produto_new, "id", "codcomercial");
                return View(ordem);
            }
        }

        // GET: OrdemFabricoes/OfColocacaoEmLinha
        [HttpPost]
        [ValidateAntiForgeryToken]
        //OfColocacaoEmLinhaSaveç
        public ActionResult OfColocacaoEmLinha(OrdemFabrico ordemFabrico){
            if (ordemFabrico == null){
                return View("InvalidAcess");
            }
            else{
                ViewBag.id = ordemFabrico.id;
                if (ordemFabrico.produto != null)
                    if (ordemFabrico.produto.codcomercial != null && ordemFabrico.produto.descricao != null)
                        ViewBag.informativo = "Off : " + ordemFabrico.produto.codcomercial + " ; " + ordemFabrico.produto.descricao;
                ViewBag.numGtinRecebidos = new SelectList(db.produto_new, "id", "ref");
                return View(ordemFabrico);
            }
        }


       // GET: OrdemFabricoes/OfRegistoQuebrasFaltas
        public ActionResult OfRegistoQuebrasFaltas(string obs){
            if (obs == ""){
                return View("InvalidAcess");
            }
            else{
                OrdemFabrico ordem = db.OrdemFabricoes.Find(Int32.Parse(obs));
                ViewBag.id = ordem.id;
                if (ordem.produto != null)
                    if (ordem.produto.codcomercial != null && ordem.produto.descricao != null)
                        ViewBag.informativo = "Off : " + ordem.produto.codcomercial + " ; " + ordem.produto.descricao;
                ViewBag.numGtinRecebidos = new SelectList(db.produto_new, "id", "ref");
                return View(ordem);
            }
        }

        // GET: OrdemFabricoes/OFSubMenu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OfRegistoQuebrasFaltas(OrdemFabrico ordemFabrico){
            if (ordemFabrico == null){
                return View("InvalidAcess");
            }
            else{
                return View(ordemFabrico);
            }
        }
       
        // GET: OrdemFabricoes/OfControloQualidade
        public ActionResult OfControloQualidade(string obs){
            if (obs == ""){
                return View("InvalidAcess");
            }
            else{
                OrdemFabrico ordem = db.OrdemFabricoes.Find(Int32.Parse(obs));
                ViewBag.id = ordem.id;
                if (ordem.produto != null)
                    if (ordem.produto.codcomercial != null && ordem.produto.descricao != null)
                        ViewBag.informativo = "Off : " + ordem.produto.codcomercial + " ; " + ordem.produto.descricao;
                return View(ordem);
            }
        }

        // GET: OrdemFabricoes/OfControloQualidade
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OfControloQualidade(OrdemFabrico ordemFabrico){
            if (ordemFabrico == null){
                return View("InvalidAcess");
            }
            else{
                return View(ordemFabrico);
            }
        }
        
        private string numcaixasPalete(string referencia) {
            var produtoOF = db.produtoes.ToList();
            for (int i = 0; i < produtoOF.Count(); i++)
            {
                if (produtoOF[i].descricao != null)
                {
                    if (produtoOF[i].codcomercial.Equals(referencia))
                    {
                        return produtoOF[i].NrCxsPalete.ToString();
                    }
                }
            }
            return "";
        }
        // GET: OrdemFabricoes/OfFechoPalete
        public ActionResult OfFechoPalete(string obs){
            if (obs == ""){
                return View("InvalidAcess");
            }
            else
            {
                OrdemFabrico ordem = db.OrdemFabricoes.Find(Int32.Parse(obs));
                //produto produtoOF = db.produtoes.Find(ordem.produto_id);
                
                ViewBag.id = ordem.id;
                
                ViewBag.gtinFabricados = "";
                if (ordem.produto != null)
                    if (ordem.produto.codcomercial != null && ordem.produto.descricao != null)
                        ViewBag.informativo = "Off : " + ordem.produto.codcomercial + " ; " + ordem.produto.descricao;
                var lista = new SelectList(db.produtoes, "id", "codcomercial");
                var listagem = new SelectList(db.produtoes, "id", "codcomercial").Concat(new SelectList(db.gtinFabricados, "id", "gtinrecebido_id").Concat(new SelectList(db.gtinRecebidos, "id", "numGtin")));
                ViewBag.componente_id = listagem;
                
                return View(ordem);
            }
        }

        // GET: OrdemFabricoes/OfFechoPalete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OfFechoPalete([Bind(Include = "id,gtinFabricados,LOTE,caixasproduzidas")] OrdemFabrico ordemFabrico, String gtinFabricados, string idOf){
            if (ordemFabrico == null || gtinFabricados=="" || idOf==""){
                return View("InvalidAcess");
            }
            else{
                OrdemFabrico ordemRecebida = db.OrdemFabricoes.Find(Int32.Parse(idOf));
                gtinFabricado gtin = new gtinFabricado();
              
                gtin.encomenda_id = ordemRecebida.encomenda_id;
                if (ordemRecebida.produto != null){
                    if (ordemRecebida.produto_id != null){
                        var produtoRecebidoLista = db.produto_new;
                        var encontrou = false;
                        var indice = 0;

                        gtin.gtinFabricado1 = gtinFabricados;
                        
                        gtin.encomenda_id = ordemRecebida.encomenda_id;
                        gtin.validade = ordemRecebida.validade;
                        gtin.qtdFabricada = Convert.ToInt32(ordemFabrico.qtdFabricado);
                        gtin.loteProduzir = ordemRecebida.LOTE;
                        gtin.ordemFabrico_id = ordemRecebida.id;
                        db.gtinFabricados.Add(gtin);
                        db.SaveChanges(); 
                    }
                }
                return RedirectToAction("OFSubMenu", new { of = idOf });
            }
        }
        
        // GET: OrdemFabricoes/OfFimOperacao
        public ActionResult OfFimOperacao(string obs){
            if (obs == ""){
                return View("InvalidAcess");
            }
            else{
                OrdemFabrico ordem = db.OrdemFabricoes.Find(Int32.Parse(obs));
                ViewBag.qtdPropduzida = ordem.qtdFabricado;
                ViewBag.motivos = new SelectList(db.FimOperacoes, "id", "descricao");
                ParagemProducao paragemProducao = new ParagemProducao();
                ViewBag.id = ordem.id;
                paragemProducao.OF_id = ordem.id;
                paragemProducao.OrdemFabrico = ordem;
                db.ParagemProducaos.Add(paragemProducao);
                ordem.dataFim = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        // GET: OrdemFabricoes/OfFimOperacao
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OfFimOperacao(OrdemFabrico ordemFabrico, string listMotivosFecho){
            if (ordemFabrico == null || listMotivosFecho==""){
                return View("InvalidAcess");
            }
            else{
                ordemFabrico.dataFim = DateTime.Now;
                ParagemProducao paragemProducao = new ParagemProducao();

                paragemProducao.motivoParagem_id = Int32.Parse(listMotivosFecho);
                paragemProducao.OF_id = ordemFabrico.id;
                paragemProducao.OrdemFabrico = ordemFabrico;
                paragemProducao.qtdProduzida = Convert.ToInt64(ordemFabrico.qtdFabricado);
                db.ParagemProducaos.Add(paragemProducao);
                db.SaveChanges();
                return View(ordemFabrico);
            }
        }

        // GET: OrdemFabricoes/OfRegistoQuebras
        public ActionResult OfRegistoQuebras(string obs){
            if (obs == ""){
                return View("InvalidAcess");
            }
            else{
                OrdemFabrico ordem = db.OrdemFabricoes.Find(Int32.Parse(obs));
                var produtoRef = db.produto_new.ToList();
                var index = 0;
                while (index < produtoRef.Count){
                    if (produtoRef[0].codcomercial.Equals(ordem.produto) && produtoRef[0].id_numeroEncomenda.Equals(ordem.encomenda_id)){
                        if (produtoRef[0].validade != null)
                            ViewBag.validadeProduto = produtoRef[0].validade.ToString().Split(' ')[0];
                        else
                            ViewBag.validadeProduto = "";
                    }
                    else
                        ViewBag.validadeProduto = "";
                    index++;
                }
                ViewBag.id = ordem.id;
                if (ordem.produto != null)
                    if (ordem.produto.codcomercial != null && ordem.produto.descricao != null)
                        ViewBag.informativo = "Off : " + ordem.produto.codcomercial + " ; " + ordem.produto.descricao;
                ViewBag.motivosQuebra = new SelectList(db.MotivoQuebras, "id", "descricao");
                ViewBag.numGtinRecebidos = new SelectList(db.produto_new, "id", "ref");
                return View(ordem);
            }
        }

        // GET: OrdemFabricoes/OfRegistoQuebras
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OfRegistoQuebras(string numQuebras, string idOf, string motivosQuebra) {
            if (numQuebras == "" || idOf == ""){
                return View("InvalidAcess");
            }
            else{
                OrdemFabrico ordem = db.OrdemFabricoes.Find(Int32.Parse(idOf));
                ordem.numQuebras = Int32.Parse(numQuebras);
                if (motivosQuebra != null)
                    ordem.MotivoQuebra.id = Int32.Parse(motivosQuebra);
                db.SaveChanges();
                return RedirectToAction("OFSubMenu", new { of = ordem.id });
            }
        }


        // GET: OrdemFabricoes/OfPausa
        public ActionResult OfPausa(string obs){
            if (obs == ""){
                return View("InvalidAcess");
            }
            else{
                OrdemFabrico ordem = db.OrdemFabricoes.Find(Int32.Parse(obs));
                ordem.dataInicioPausa = DateTime.Now;
                ParagemProducao paragem = new ParagemProducao();
                paragem.qtdProduzida = Convert.ToInt32(ordem.qtdFabricado);
                paragem.OF_id = Int32.Parse(obs);
                db.ParagemProducaos.Add(paragem);
                db.SaveChanges();
                ViewBag.paused = true;
                if (ordem.produto != null)
                    if (ordem.produto.codcomercial != null && ordem.produto.descricao != null)
                        ViewBag.informativo = "Off : " + ordem.produto.codcomercial + " ; " + ordem.produto.descricao;
                return RedirectToAction("OFSubMenu", new { of = ordem.id });
            }
        }

        // GET: OrdemFabricoes/OfRetiraPaus
        public ActionResult OfRetiraPaus(string obs){
            if (obs == ""){
                return View("InvalidAcess");
            }
            else{
                OrdemFabrico ordem = db.OrdemFabricoes.Find(Int32.Parse(obs));
                ViewBag.id = ordem.id;
                ordem.dataFimPausa = DateTime.Now;
                var intervaloPausa = ordem.dataFimPausa - ordem.dataInicioPausa;
                ordem.tempoPausaMinutos = Convert.ToDecimal(intervaloPausa.Value.TotalMinutes);
                ordem.ParagemProducaos = null;
                ordem.dataInicioPausa = null;
                db.SaveChanges();
                ViewBag.paused = false;
                if (ordem.produto != null)
                    if (ordem.produto.codcomercial != null && ordem.produto.descricao != null)
                        ViewBag.informativo = "Off : " + ordem.produto.codcomercial + " ; " + ordem.produto.descricao;
                return RedirectToAction("OFSubMenu", new { of = ordem.id });
            }
        }
        

        // GET: OrdemFabricoes/OfRegistoFaltas
        public ActionResult OfRegistoFaltas(string obs){
            if (obs == ""){
                return View("InvalidAcess");
            }
            else{
                OrdemFabrico ordem = db.OrdemFabricoes.Find(Int32.Parse(obs));
                var produtoRef = db.produto_new.ToList();
                var index = 0;
                while (index < produtoRef.Count){
                    if (produtoRef[0].codcomercial.Equals(ordem.produto) && produtoRef[0].id_numeroEncomenda.Equals(ordem.encomenda_id)){
                        if (produtoRef[0].validade != null)
                            ViewBag.validadeProduto = produtoRef[0].validade.ToString().Split(' ')[0];
                        else
                            ViewBag.validadeProduto = "";
                    }
                    else
                        ViewBag.validadeProduto = "";
                    index++;
                }
                ViewBag.id = ordem.id;
                if (ordem.produto != null)
                    if (ordem.produto.codcomercial != null && ordem.produto.descricao != null)
                        ViewBag.informativo = "Off : " + ordem.produto.codcomercial + " ; " + ordem.produto.descricao;
                ViewBag.motivosFaltas = new SelectList(db.MotivoFaltas, "id", "descricao");
                ViewBag.numGtinRecebidos = new SelectList(db.produto_new, "id", "ref");
                return View(ordem);
            }
        }

        // GET: OrdemFabricoes/OfRegistoFaltas
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OfRegistoFaltass(string numFaltas, string idOf, string motivosFaltas){
            if (numFaltas == "" || idOf == ""){
                return View("InvalidAcess");
            }
            else{
                OrdemFabrico ordem = db.OrdemFabricoes.Find(Int32.Parse(idOf));
                ordem.numFaltas = Int32.Parse(numFaltas);
                if (motivosFaltas != null)
                    ordem.MotivoQuebra.id = Int32.Parse(motivosFaltas);
                db.SaveChanges();
                return RedirectToAction("OFSubMenu", new { of = ordem.id });
            }
        }

    }
}
