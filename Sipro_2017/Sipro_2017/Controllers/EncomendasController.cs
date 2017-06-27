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
using System.Web.Mvc.Html;

namespace Sipro_2017.Controllers
{
    public class EncomendasController : Controller
    {

        private SiproEntities db = new SiproEntities();
        //private produto[] listaProdutos;
        List<produto_new> produtos_encomenda = new List<produto_new>();
      
        // GET: orcamentoes
        public ActionResult Index(string sortOrder, string currentFilter, string criterioPesquisa, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            if (criterioPesquisa != null){
                page = 1;
            } else{
                criterioPesquisa = currentFilter;
            }
            ViewBag.CurrentFilter = criterioPesquisa;
            var encomendas = db.Encomendas.Include(e => e.produto).Include(e => e.tipoMorada).Include(e => e.orcamento);

            if (!String.IsNullOrEmpty(criterioPesquisa)){
                encomendas = encomendas.Where(s => s.id.Equals(criterioPesquisa) || 
                    s.encomendaExterna.Contains(criterioPesquisa) ||s.morada.tipo.Equals(criterioPesquisa) || 
                    s.dataEncomenda.Equals(criterioPesquisa) || s.dataPrevista.Equals(criterioPesquisa) || 
                    s.quantidade.Equals(criterioPesquisa) ||s.Producaos.Equals(criterioPesquisa) || 
                    s.produto.codcomercial.Contains(criterioPesquisa) || s.quantidade.Equals(criterioPesquisa) || 
                    s.morada.nome.Contains(criterioPesquisa) ||s.dataConclusao.Equals(criterioPesquisa)|| 
                    s.descricao_encomenda.Equals(criterioPesquisa));
            }

            switch (sortOrder){
                case "enc_interna_desc": encomendas = encomendas.OrderByDescending(s => s.id); break;
                case "enc_interna_asc": encomendas = encomendas.OrderBy(s => s.id); break;
                case "id_asc": encomendas = encomendas.OrderByDescending(s => s.id); break;
                case "id_desc": encomendas = encomendas.OrderBy(s => s.id); break;
                case "encomendaExterna_desc": encomendas = encomendas.OrderByDescending(s => s.encomendaExterna); break;
                case "encomendaExterna_asc": encomendas = encomendas.OrderBy(s => s.encomendaExterna); break;
                case "nomeMorada_desc": encomendas = encomendas.OrderByDescending(s => s.morada.nome); break;
                case "nomeMorada_asc": encomendas = encomendas.OrderBy(s => s.morada.nome); break;
                case "desc_asc": encomendas = encomendas.OrderByDescending(s => s.descricao_encomenda); break;
                case "desc_desc": encomendas = encomendas.OrderBy(s => s.descricao_encomenda); break;
                case "dataEncomenda_asc": encomendas = encomendas.OrderByDescending(s => s.dataEncomenda); break;
                case "dataEncomenda_desc": encomendas = encomendas.OrderBy(s => s.dataEncomenda); break;
                case "dataPrevista_asc": encomendas = encomendas.OrderByDescending(s => s.dataPrevista); break;
                case "dataPrevista_desc": encomendas = encomendas.OrderBy(s => s.dataPrevista); break;
                case "qtd_asc": encomendas = encomendas.OrderByDescending(s => s.quantidade); break;
                case "qtd_desc": encomendas = encomendas.OrderBy(s => s.quantidade); break;
                case "qtdFabricada_asc": encomendas = encomendas.OrderByDescending(s => s.controle_qualidade_producao); break;
                case "qtdFabricada_desc": encomendas = encomendas.OrderBy(s => s.controle_qualidade_producao); break;
                case "codcomercial_asc": encomendas = encomendas.OrderByDescending(s => s.Producaos); break;
                case "codcomercial_desc": encomendas = encomendas.OrderBy(s => s.Producaos); break;
                case "dataConclusao_asc": encomendas = encomendas.OrderByDescending(s => s.dataConclusao); break;
                case "dataConclusao_desc": encomendas = encomendas.OrderBy(s => s.dataConclusao); break;
                default: encomendas = encomendas.OrderByDescending(s => s.id); break;
            }
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(encomendas.ToPagedList(pageNumber, pageSize));
        }

        // GET: Encomendas/Details/5
        public ActionResult Details(int? id){
            if (id == null){
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Encomenda encomenda = db.Encomendas.Find(id);
            if (encomenda == null){
                return HttpNotFound();
            }
            return View(encomenda);
        }

        //GET: Encomendas/Create
        [HttpPost]
        public ActionResult FindProdutos(string pesquisaProdutos){
            Request["pesquisaProdutos"].ToString();
            
            if (pesquisaProdutos != null){
                ViewBag.codigoProduto_id = new SelectList(
                     db.produtoes.Where(prod => prod.id.Equals(pesquisaProdutos) || prod.descricao.Contains(pesquisaProdutos)), "id", "codcomercial");

            }
            else{
                ViewBag.codigoProduto_id = new SelectList(db.produtoes, "id", "codcomercial");
            }
            ViewBag.encomenda_cliente = new SelectList(db.tipoMoradas, "id", "tipo");
            ViewBag.encomenda_orcamento = new SelectList(db.orcamentoes, "id", "ficheiro_orcamento");
            ViewBag.morada_cliente = new SelectList(db.moradas, "id", "nome");
            return View();
        }

        //GET: Encomendas/Create
        public ActionResult Create(){
            ViewBag.codigoProduto_id = new SelectList(db.produtoes, "id", "codcomercial");
            ViewBag.codigoProduto_desc = new SelectList(db.produtoes, "id", "descricao");
            ViewBag.encomenda_cliente = new SelectList(db.tipoMoradas, "id", "tipo");
            ViewBag.encomenda_orcamento = new SelectList(db.orcamentoes, "id", "id");
            ViewBag.morada_cliente = new SelectList(db.moradas, "id", "nome");
            produtos_encomenda.Add(new produto_new());
            ViewBag.produto_new =  new HashSet<produto_new>(produtos_encomenda);
            return View();
        }

        //GET: Encomendas/Create
        public ActionResult novoOrcamento(int orcamento){
            ViewBag.idorcamento = orcamento;
            return View();
        }

        // POST: Encomendas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string codigoproduto0, string descProduto0, string qtdProduto0, [Bind(Include = "descricao_encomenda,encomendaExterna,dataConclusao,concluido,dataEncomenda,dataPrevista,quantidade,controle_qualidade_producao,encomenda_cliente,encomenda_orcamento,morada_cliente,codigoProduto_id")] Encomenda encomenda){
            List<produto_new> produtosEncomenda = validaProduto(encomenda);
            if (produtosEncomenda != null)
            {
                if (produtosEncomenda.Count != 0)
                {
                    if (ModelState.IsValid)
                    {
                        encomenda.concluido = false;
                        encomenda.dataEncomenda = DateTime.Now;
                        if (encomenda.morada_cliente != null)
                        {
                            morada moradaCliente = db.moradas.Find(encomenda.morada_cliente);
                            encomenda.encomenda_cliente = moradaCliente.tipoMorada.id;
                        }
                        else
                        {
                            return View("create");
                        }

                        var numeroProdutos = encomenda.produto_new.Count;
                        for (int prod = 0; prod < numeroProdutos; prod++)
                        {
                            var produto_new = new produto_new();
                            var result = encomenda.produto_new.ToArray();
                            produto_new = result[prod];
                            if (prod == 0)
                            {
                                var lista = new SelectList(db.produtoes, "id", "codcomercial");
                                var list = lista.ToList();
                                for (int j = 0; j < lista.Count(); j++)
                                {
                                    if (list[j].Text.Equals(produto_new.codcomercial))
                                    {
                                        var id = list[j].Value;
                                        encomenda.codigoProduto_id = Int32.Parse(id);
                                    }
                                }
                            }
                            db.produto_new.Add(produto_new);
                        }
                        Boolean novoOrcamento = false;
                        orcamento orcamentos = new orcamento();
                        if (encomenda.encomenda_orcamento != null)
                        {
                           
                            if (db.orcamentoes.Find(encomenda.encomenda_orcamento) == null)
                            {

                                orcamentos.data_recepcao = DateTime.Now;
                                orcamentos.id = Convert.ToInt32(encomenda.encomenda_orcamento);
                                orcamentos.estadoOrcamento = db.estadoOrcamentoes.Find(4);
                                orcamentos.user_alteracao = 1;
                                orcamentos.morada = db.moradas.Find(encomenda.morada_cliente);
                                orcamentos.tipoMorada = orcamentos.morada.tipoMorada;
                                db.orcamentoes.Add(orcamentos);
                                novoOrcamento = true;
                            }
                        }
                        db.Encomendas.Add(encomenda);
                        for (int prod = 0; prod < produtosEncomenda.Count; prod++)
                        {
                            produto_new update = produtosEncomenda[prod];
                            update.id_numeroEncomenda = encomenda.id;
                        }

                        db.SaveChanges();
                        if (novoOrcamento)
                            return RedirectToAction("novoOrcamento", new { orcamento = orcamentos.id });
                        return RedirectToAction("Index");
                    }
                    ViewBag.codigoProduto_id = new SelectList(db.produtoes, "id", "codcomercial", encomenda.codigoProduto_id);
                    ViewBag.codigoProduto_desc = new SelectList(db.produtoes, "id", "descricao");
                    ViewBag.encomenda_cliente = new SelectList(db.tipoMoradas, "id", "tipo", encomenda.encomenda_cliente);
                    ViewBag.encomenda_orcamento = new SelectList(db.orcamentoes, "id", "ficheiro_orcamento", encomenda.encomenda_orcamento);
                    ViewBag.morada_cliente = new SelectList(db.moradas, "id", "nome", encomenda.morada_cliente);
                    return View("create");
                }
            }
            return View("create");
        }

        private string findDescProduto(string codcomercial){
            var prd = db.produtoes.ToList();
            string desc="";
            Boolean found = false;
            int i = 0;
            while (!found && i < prd.Count()) {
                produto produto_index = prd[i];
                if (produto_index.codcomercial == codcomercial) {//|| produto_index.ean13_produto == Int64.Parse(codcomercial)) {
                    desc = produto_index.descricao;
                    //found = true;
                    return desc;
                }
                i++;
            }
            return desc;
        }

        private Boolean eliminaRegistosEncomenda(int encomenda) {
            var produtos = db.produto_new.ToList();
            for (int i = 0; i < produtos.Count; i++) {
                produto_new produto = produtos[i];
                if (produto.id_numeroEncomenda == encomenda)
                    db.produto_new.Remove(produto);    
            }
            db.SaveChanges();
            return true;
        }

        private Boolean validaProdutoEdit(Encomenda encomenda){
            var list = encomenda.produto_new.ToList();
            eliminaRegistosEncomenda(encomenda.id);
            var indiceQtd = 0;
            int i = 1;
            if (!Request.Form["codigoproduto0"].Equals("0")) {
                i = 0;
            }
            for (; i < 11; i++) {
                var testing = Request.Form[i];
                produto_new produtosEncomenda = new produto_new();
                var idProduto = Request.Form["codigoproduto" + i];
                var codigos = Request.Form["codigoProduto_id"].Split(',');
                if (Request.Form["codcomercial"] != null)
                {
                    var idProdutoAlterados = Request.Form["codcomercial"].Split(',');
                    for (int alterados = 0; alterados < idProdutoAlterados.Count(); alterados++)
                    {
                        //variaável a guardar.
                        codigos[alterados] = idProdutoAlterados[alterados];
                    }

                }

                var teste = Request.Form["qtdproduto1"];
                if (idProduto != "" || idProduto != null){
                    idProduto = codigos[i];
                    var qtdsArray = Request.Form["qtd"] +","+ Request.Form["quantidade"];
                    var precosListagem = Request.Form["preco"];
                    var precos = new string[10];
                    try
                    {
                        precos = precosListagem.Split(',');
                    }
                    catch {
                        precos = Request.Form["precoProdutos"].Split(',');
                    }
                   
                  
                    var qtds = qtdsArray.Split(',');
                    if (Request.Form["qtd"]==null){
                        qtds = Request.Form["quantidade"].Split(',');
                    }
                   
                    if ((qtds[indiceQtd] != "" || qtds[indiceQtd] != null && qtds[indiceQtd] != "0")){
                        var quanti = qtdsArray.Split(',');
                   
                        var parames = Request.Params.AllKeys;
                        var testin = Request.Form;
                        var descricao = findDescProduto(idProduto);
                        var qtd = quanti[indiceQtd];
                        var preco = "";
                        try
                        {
                            preco = precos[indiceQtd];
                        }
                        catch
                        {}
                        int qtdProduto = 0;
                        try{
                            qtdProduto = Int32.Parse(qtd);
                            if (qtdProduto == 0)
                                qtdProduto = Int32.Parse(qtds[indiceQtd]);
                        }
                        catch{
                            qtdProduto = Int32.Parse(qtds[indiceQtd]);
                        }
                        if (idProduto != null && idProduto != "" && !idProduto.Equals("0")
                            && qtdProduto != 0){
                            try{
                                var produtos = db.produtoes;
                                produtosEncomenda.id_numeroEncomenda = encomenda.id;
                                produtosEncomenda.codcomercial = idProduto;
                                produtosEncomenda.descricao = descricao;
                                produtosEncomenda.qtd = qtdProduto;
                                produtosEncomenda.@ref = idProduto;
                                produtosEncomenda.preco = Convert.ToDecimal(preco.Replace('.',','));
                                db.produto_new.Add(produtosEncomenda);
                                //i++;
                                indiceQtd++;
                                try{
                                    var exists = encomenda.produto_new;
                                    db.produto_new.Add(produtosEncomenda);
                                    db.SaveChanges();
                                }
                                catch{
                                    db.Entry(produtosEncomenda).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                                list.Add(produtosEncomenda);
                            }
                            catch (Exception e){
                                string nome = "qtdProduto" + i;
                                ViewBag.nome = "Qtd obrigatória!";
                                return false;
                            }
                        }
                    }
                }
            }
            encomenda.produto_new = list;
            db.SaveChanges();
           
            return true;
        }

        private List<produto_new> validaProduto(Encomenda encomenda){
            encomenda.quantidade = 0;
            List<produto_new> list = new List<produto_new>();
            for (int i = 0; i < 10; i++) {
                produto_new produtosEncomenda = new produto_new();
                var idProduto = Request.Form["codigoproduto" + i];
                var qtds = Request.Form["quantidade"].Split(',');
                var precos = new string[10];
                var precosListagem = Request.Form["preco"];
                try
                {
                    precos = precosListagem.Split(',');
                }
                catch
                {
                    precos = Request.Form["precoProdutos"].Split(',');
                }

                var descricao = Request.Form["desproduto"+i];
                var qtd = qtds[i];
                //var preco = precos[i];

                var preco = "";
                try
                {
                    preco = precos[i];
                }
                catch
                { }

                int qtdProduto = 0;
                try
                {
                    qtdProduto = Int32.Parse(qtd);
                }
                catch { }
                   

                //                if (idProduto != null && idProduto!="" && idProduto != "0") {
                if (idProduto != null && idProduto != "" && !idProduto.Equals("0")
                    && qtdProduto != 0){
                    try{
                        var produtos = db.produtoes;
                        produtosEncomenda.id_numeroEncomenda = encomenda.id;
                        produtosEncomenda.codcomercial = idProduto;
                        produtosEncomenda.descricao = descricao;
                        produtosEncomenda.qtd = qtdProduto;
                        produtosEncomenda.@ref = idProduto;

                        produtosEncomenda.preco = Convert.ToDecimal(preco.Replace('.', ','));

                        db.produto_new.Add(produtosEncomenda);
                        var exists= encomenda.produto_new;
                        encomenda.quantidade = qtdProduto + encomenda.quantidade;
                        list.Add(produtosEncomenda);
                    }
                    catch (Exception e) {
                        string nome = "qtdProduto" + i;
                        ViewBag.nome = "Qtd obrigatória!";
                        return null;
                    }
                }
            }
            encomenda.produto_new = list;
            return list;
        }

        private Boolean actualizaQuantidades(Encomenda encomenda) {
            var prosdutosEncomenda = db.produto_new;
            var produtos_encomenda = prosdutosEncomenda.ToList();
            encomenda.quantidade = 0;
            for (int j = 0; j < prosdutosEncomenda.Count(); j++){
                var index = j;
                if(produtos_encomenda[index].id_numeroEncomenda == encomenda.id)
                    encomenda.quantidade = encomenda.quantidade +  Convert.ToInt32(produtos_encomenda[index].qtd);
            }
            return true;
        }

        //actualizaProdutos
        private Boolean actualizaProduto(Encomenda encomenda, String produtosAlterados ){
            var alterado = produtosAlterados.Split('$');
            for (int i = 0; i < alterado.Count(); i++){
                var alteradoSub = alterado[i].Split(';');
                var prosdutosEncomenda = db.produto_new;
                var produtos_encomenda = prosdutosEncomenda.ToList();
                var found = true;
                for (int j = 0; j < prosdutosEncomenda.Count(); j++) {
                    var index = j;
                    if (produtos_encomenda[index].codcomercial.Equals(alteradoSub[0]) 
                        && produtos_encomenda[index].id_numeroEncomenda == encomenda.id) {
                        encomenda.quantidade = encomenda.quantidade - Convert.ToInt32(produtos_encomenda[index].qtd);
                        produtos_encomenda[index].qtd = Int32.Parse(alteradoSub[2]);
                        if (encomenda.quantidade > 0)
                            encomenda.quantidade = encomenda.quantidade + Convert.ToInt32(produtos_encomenda[index].qtd);
                        else
                            encomenda.quantidade = Convert.ToInt32(produtos_encomenda[index].qtd);

                        if (produtos_encomenda[index].qtd == 0) {
                            db.produto_new.Remove(produtos_encomenda[index]);
                        }
                        db.Entry(produtos_encomenda[j]).State = EntityState.Modified;
                        return true;
                    }
                }      
            }
            return false;
        }

        public ActionResult pesquisaProduto(int? produto, [Bind(Include = "produto,id,dataEncomenda,descricao_encomenda,encomendaExterna,dataConclusao,concluido,dataEncomenda,dataPrevista,quantidade,controle_qualidade_producao,encomenda_cliente,encomenda_orcamento,morada_cliente,codigoProduto_id")] Encomenda encomenda){
            try{
                produto produtoBD= db.produtoes.Find(Request.QueryString["int"]);
                ViewBag.produto = produtoBD;
                encomenda.produto = produtoBD;
                ViewBag.codigoProduto_id = db.produtoes.Find(Request["morada_orcamento"].ToString());
                var categories = from category in db.produtoes orderby category.id
                                select new{category.id, category.descricao};
                return Json(categories);
            }
            catch (Exception ex){
                return null;
            }
        }
     
        // GET: Encomendas/Edit/5
        public ActionResult Edit(int? id){
            if (id == null){
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Encomenda encomenda = db.Encomendas.Find(id);
            if (encomenda == null){
                return HttpNotFound();
            }
            encomenda.codigoProduto_id = 0;
            encomenda.quantidade = 0;
            ViewBag.codigoProduto_id = new SelectList(db.produtoes, "id", "codcomercial", encomenda.codigoProduto_id);
            ViewBag.codigoProduto_desc = new SelectList(db.produtoes, "id", "descricao");
            ViewBag.encomenda_cliente = new SelectList(db.tipoMoradas, "id", "tipo", encomenda.encomenda_cliente);
            ViewBag.encomenda_orcamento = new SelectList(db.orcamentoes, "id", "ficheiro_orcamento", encomenda.ficheiro_encomenda);
            ViewBag.morada_cliente = new SelectList(db.moradas, "id", "nome", encomenda.morada_cliente);
            ViewBag.data = encomenda.dataEncomenda;
            //produto_new
            var teste = encomenda.produto_new;
            return View(encomenda);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(String data, String produtosAlterados, [Bind(Include = "id,descricao_encomenda,encomendaExterna,produto_new,dataConclusao,quantidade,controle_qualidade_producao,encomenda_orcamento,morada_cliente,codigoProduto_id,produto_new,tipoMorada,dataPrevista,dataEncomenda")] Encomenda encomenda){
            var id= encomenda.id;

            if (Convert.ToDateTime(data) == DateTime.MinValue)
                 encomenda.dataEncomenda = DateTime.Now;
             else
                 encomenda.dataEncomenda = Convert.ToDateTime(data);

            if (encomenda.morada_cliente != null){
                morada moradaCliente = db.moradas.Find(encomenda.morada_cliente);
                encomenda.encomenda_cliente = moradaCliente.tipoMorada.id;
            }

            var produtosAlterado= Request.Form["produtosAlterados"];
            if (encomenda.encomenda_cliente == 0){
                encomenda.encomenda_cliente = Convert.ToInt32(encomenda.morada_cliente);
            }
            if (validaProdutoEdit(encomenda)){
                if (ModelState.IsValid){
                    var numeroProdutos = encomenda.produto_new.Count;
                    if (produtosAlterados != null && produtosAlterados != "") {
                        actualizaProduto( encomenda,  produtosAlterados);
                    }
                    SelectList produtos_news = new SelectList(db.produto_new);
                    var prd_crc = db.produto_new.ToList();
                    var result = encomenda.produto_new.ToArray();
                    var actualiza = true;
                    for (int prod = 0; prod < numeroProdutos; prod++){
                        for (int ciclo = 0; ciclo < prd_crc.Count; ciclo++){
                            if (prd_crc[ciclo].codcomercial == result[prod].codcomercial && prd_crc[ciclo].id_numeroEncomenda == result[prod].id_numeroEncomenda){
                                if (prd_crc[ciclo].qtd != result[prod].qtd) {
                                    produto_new update = db.produto_new.Find(result[prod].id);
                                    db.Entry(update).State = EntityState.Modified;
                                    update.qtd = result[prod].qtd;
                                    encomenda.quantidade = encomenda.quantidade + Convert.ToInt32(update.qtd);
                                }
                                actualiza = false;
                            }
                        }
                        if (actualiza){
                            var produto_new = new produto_new();
                            produto_new = result[prod];
                        }
                    }
                    db.Entry(encomenda).State = EntityState.Modified;
                    db.SaveChanges();
                    var qtd = Request.Form["qtdproduto0"];
                    qtd = Request.Form["qtdProdutsExits"];
                    qtd = Request.Form["qtd"];
                    db.Entry(encomenda).State = EntityState.Modified;
                    if (actualizaQuantidades(encomenda))
                        db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.codigoProduto_id = new SelectList(db.produtoes, "id", "codcomercial");
                ViewBag.codigoProduto_desc = new SelectList(db.produtoes, "id", "descricao");
                ViewBag.encomenda_cliente = new SelectList(db.tipoMoradas, "id", "tipo");
                ViewBag.encomenda_orcamento = new SelectList(db.orcamentoes, "id", "ficheiro_orcamento");
                ViewBag.morada_cliente = new SelectList(db.moradas, "id", "nome");
                produtos_encomenda.Add(new produto_new());
                return View(encomenda);
            }
            ViewBag.morada_cliente = new SelectList(db.moradas, "id", "nome", encomenda.morada_cliente);
            return View(encomenda);
        }

        // GET: Encomendas/Delete/5
        public ActionResult Delete(int? id){
            if (id == null){
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Encomenda encomenda = db.Encomendas.Find(id);
            if (encomenda == null){
                return HttpNotFound();
            }
            return View(encomenda);
        }

        // POST: Encomendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id){
            Encomenda encomenda = db.Encomendas.Find(id);
            ProdEncomenda(id);
            db.Encomendas.Remove(encomenda);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ProdEncomenda(int id){
            var produtosEncomendas = db.produto_new;
            for(int i=0; i< produtosEncomendas.Count(); i++){
                if (produtosEncomendas.ToList()[i].id_numeroEncomenda.Equals(id))
                    db.produto_new.Remove(produtosEncomendas.ToList()[i]);
            }
            
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

