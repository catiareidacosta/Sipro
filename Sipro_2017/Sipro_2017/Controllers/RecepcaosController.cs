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
using System.Data.Entity.Validation;

namespace Sipro_2017.Controllers
{
    public class RecepcaosController : Controller
    {
        private SiproEntities db = new SiproEntities();

        // GET: Recepcaos
        public ActionResult Index(string sortOrder, string currentFilter, string criterioPesquisa, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            if (criterioPesquisa != null) {
                page = 1;
            }
            else {
                criterioPesquisa = currentFilter;
            }

            ViewBag.CurrentFilter = criterioPesquisa;

            var recepcaos = db.Recepcaos.Include(r => r.camposControlo).
                Include(r => r.camposControlo1).Include(r => r.camposControlo2).
                Include(r => r.camposControlo3).Include(r => r.Encomenda).
                Include(r => r.morada1).Include(r => r.morada2);


            if (!String.IsNullOrEmpty(criterioPesquisa)) {
                recepcaos = recepcaos.Where(s => s.numeroGuia.Equals(criterioPesquisa) || s.dataRecepcao.Equals(criterioPesquisa) || s.morada.Equals(criterioPesquisa));
            }

            switch (sortOrder) {
                case "id_asc":
                    recepcaos = recepcaos.OrderByDescending(s => s.id);
                    break;
                case "id_desc":
                    recepcaos = recepcaos.OrderBy(s => s.id);
                    break;
                case "morada_asc":
                    recepcaos = recepcaos.OrderByDescending(s => s.morada);
                    break;
                case "morada_desc":
                    recepcaos = recepcaos.OrderBy(s => s.morada);
                    break;
                case "guia_desc":
                    recepcaos = recepcaos.OrderByDescending(s => s.numeroGuia);
                    break;
                case "guia_asc":
                    recepcaos = recepcaos.OrderBy(s => s.numeroGuia);
                    break;
                case "dataRecepcao_asc":
                    recepcaos = recepcaos.OrderByDescending(s => s.dataRecepcao);
                    break;
                case "dataRecepcao_desc":
                    recepcaos = recepcaos.OrderBy(s => s.dataRecepcao);
                    break;
                default:
                    recepcaos = recepcaos.OrderByDescending(s => s.dataRecepcao);
                    break;
            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(recepcaos.ToPagedList(pageNumber, pageSize));
        }

        // GET: Recepcaos/Details/5
        public ActionResult Details(Guid? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recepcao recepcao = db.Recepcaos.Find(id);
            if (recepcao == null) {
                return HttpNotFound();
            }
            return View(recepcao);
        }

        // GET: Recepcaos/EditaRecepMercadoria/5
        public ActionResult EditaRecepMercadoria(Guid? id) {
            var recepcao = db.Recepcaos.Find(id);
            if (recepcao != null)
            {
                ViewBag.entrada = id;
                ViewBag.id_numeroEncomenda = recepcao.id_numeroEncomenda;
                ViewBag.guia = recepcao.id;
                ViewBag.numeroguia = recepcao.numeroGuia;
                ViewBag.id_numeroEncomendasAlter = recepcao.id_numeroEncomenda;

                var RefProduto = recepcao.id_produtos;
                ViewBag.produtos_id = RefProduto.ToString();
                ViewBag.id_produtosDesc = new SelectList(db.produtoes, "codcomercial", "descricao");
                ViewBag.id_produtosUniCx = new SelectList(db.produtoes, "codcomercial", "NrUnidadesCx");
                ViewBag.id_produtosNrCxPal = new SelectList(db.produtoes, "codcomercial", "NrCxsPalete");
                ViewBag.id_produtosUnidaPal = new SelectList(db.produtoes, "codcomercial", "UndsPorPalete");
                var produtoRef = db.produto_new.ToList();
                for (var i = 0; i < produtoRef.Count; i++){
                    produto_new subProduto = produtoRef[i];
                    if (subProduto.id_numeroEncomenda == recepcao.id_numeroEncomenda
                    && subProduto.numero_guia == recepcao.numeroGuia){

                        if (subProduto.codcomercial == subProduto.@ref &&
                        subProduto.@ref == subProduto.refProduto){
                            var produtos = db.produtoes.ToList();
                            if (produtos.Count() < 0){
                                produto referencia = produtos[0];
                                ViewBag.id_produtosDesc = referencia.descricao;
                                ViewBag.id_produtosUniCx = referencia.NrUnidadesCx.ToString();
                                ViewBag.id_produtosNrCxPal = referencia.NrCxsPalete.ToString();
                                ViewBag.id_produtosUnidaPal = referencia.UndsPorPalete.ToString();
                                var RefProdDesc = referencia.descricao;
                                ViewBag.Lote = produtoRef[0].lote;
                                ViewBag.Validade = produtoRef[0].validade.ToString();
                                ViewBag.id_produtosDesc = new SelectList(db.produtoes, "codcomercial", "descricao", referencia.descricao);
                                ViewBag.id_produtosUniCx = new SelectList(db.produtoes, "codcomercial", "NrUnidadesCx", referencia.NrUnidadesCx.ToString());
                                ViewBag.id_produtosNrCxPal = new SelectList(db.produtoes, "codcomercial", "NrCxsPalete", referencia.NrCxsPalete.ToString());
                                ViewBag.id_produtosUnidaPal = new SelectList(db.produtoes, "codcomercial", "UndsPorPalete", referencia.UndsPorPalete.ToString());
                            }
                        }
                    }
                }

                ViewBag.id_Moradas = new SelectList(db.moradas, "id", "nome", recepcao.morada);
                ViewBag.morada = new SelectList(db.moradas, "id", "nome", recepcao.morada);
                ViewBag.id_produtos = new SelectList(db.produtoes, "codcomercial", "descricao", recepcao.id_produtos);
                return View(recepcao);
            }
            else
                return View("InvalidAcess");
        }


        private produto getProdutosCodComercial(string codComercial) {
            var produtos = db.produtoes;
            var listaprodutos = db.produtoes.ToList();
            var index = 0;
            Boolean found = false;
            while (index < listaprodutos.Count() && !found) {
                if (listaprodutos[index].codcomercial.Equals(codComercial)) {
                    found = true;
                }
                else
                    index++;
            }
            return listaprodutos[index];
        }

        // GET: Recepcaos/EditaRecepMercadoria/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditaRecepMercadoria([Bind(Include ="id,id_Moradas,comentarios,id_camposControlLivre,morada,id_produtos,produtos_id")] Recepcao recepcao) {
            var encomendas = Request.Form["id_numeroEncomendasAlter"];
            var guia = Request.Form["numeroguia"]; 
            var idRecepcao = Request.Form["guia"];
            Recepcao actualizaDados = new Recepcao();
            try{
                actualizaDados = db.Recepcaos.Find(new Guid(Request.Form["numeroguia"]));
            }
            catch {
                actualizaDados = db.Recepcaos.Find(new Guid(Request.Form["guia"]));
            }

            actualizaDados.id_Moradas = recepcao.id_Moradas;
            actualizaDados.comentarios = recepcao.comentarios;
            actualizaDados.morada = recepcao.morada;
            if (Request.Form["id_numeroEncomendasAlter"]!="")
                actualizaDados.id_numeroEncomenda = Convert.ToInt32(Request.Form["id_numeroEncomendasAlter"]);
            
            if (recepcao.dataRecepcao == null) {
                actualizaDados.dataRecepcao = DateTime.Now;
            }

            if (ModelState.IsValid) {
                db.Entry(actualizaDados).State = EntityState.Modified;
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private Boolean apagaProdutos(string guia) {
            List<produto_new> listagemProdutos = db.produto_new.ToList();
            var index = 0;
            while (index < listagemProdutos.Count()) {
                if (guia.Equals(listagemProdutos[index].numero_guia)) {
                    if (listagemProdutos[index].id_numeroEncomenda != null && listagemProdutos[index].id_numeroEncomenda != 0) {
                        db.produto_new.Remove(listagemProdutos[index]);
                    }
                }
                index++;
            }
            return true;
        }

        private Boolean validaProdutos(string referencia) {
            var guia = Request.Form["numeroguia"];
            List<produto_new> listagemProdutos = db.produto_new.ToList();
            var index = 0;
            Boolean exists = false;
            while (index < listagemProdutos.Count()) {
                if (guia.Equals(listagemProdutos[index].numero_guia)) {
                    if (listagemProdutos[index].codcomercial == referencia)
                        exists = true;
                }
                index++;
            }
            return exists;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecepcionaMercadoria([Bind(Include = "id,numeroGuia,dataRecepcao,id_camposControl,camposControl_id,id_numeroEncomenda,id_Moradas,comentarios,id_camposControlLivre,morada,id_produtos,produtos_id")] Recepcao recepcao) {
           ViewBag.id_numeroEncomenda = new SelectList(db.Encomendas, "id", "descricao_encomenda");
           var idguia = Request.Form["guia"];
           var numEncomenda = Request.Form["id_numeroEncomendasAlter"];
           var morada = Request.Form["morada"];
           var comentarios = Request.Form["comentarios"];
           
           recepcao.id = Guid.NewGuid();
           if (numEncomenda != null && numEncomenda != ""){
                recepcao.id_numeroEncomenda = Convert.ToInt32(numEncomenda);
            }
            if (morada != null && morada != ""){
                recepcao.morada = Convert.ToInt32(morada);
            }
            if (comentarios != null && comentarios != "")
                recepcao.comentarios = comentarios;


            ViewBag.guia = recepcao.id;
            recepcao.dataRecepcao = DateTime.Now;
            recepcao.dataInicioRecepcao = DateTime.Now;

            recepcao.numeroGuia = idguia;
            db.Recepcaos.Add(recepcao);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        // GET: Recepcaos/Create Apagar
        public ActionResult Create() {
            ViewBag.camposControl_id = new SelectList(db.camposControloes, "id", "descricao");
            ViewBag.camposControl_id = new SelectList(db.camposControloes, "id", "descricao");
            ViewBag.id_camposControl = new SelectList(db.camposControloes, "id", "descricao");
            ViewBag.id_produtos = new SelectList(db.produtoes, "id", "descricao");
            ViewBag.id_numeroEncomenda = new SelectList(db.Encomendas, "id", "descricao_encomenda");
            ViewBag.id_produto_new = new SelectList(db.produto_new, "id", "ref", "referencia");
            ViewBag.id_Moradas = new SelectList(db.moradas, "id", "nome");
            ViewBag.morada = new SelectList(db.moradas, "id", "nome");
            return View();
        }

        private Boolean guardaGuias(string obs, Boolean recepcao) {
            try {
                naoConforRecepcao viewModel = new naoConforRecepcao();
                if (obs.Split(';').Count() > 0){
                    var guia = obs.Split(';')[obs.Split(';').Count() - 1];
                    obs = obs.Split(';')[0];
                    var naoConforRecepcaos = db.naoConforRecepcaos;

                    var guias = naoConforRecepcaos.Where(g => g.guia.Equals(guia));
                    guias = guias.Where(g => g.guia.Equals(guia));

                    List<naoConforRecepcao> listaOcorrencias = guias.ToList();
                    var numeElem = listaOcorrencias.Count();

                    for (int i = 0; i < numeElem; i++){
                        var guiaAlterar = listaOcorrencias[i];
                        guiaAlterar.obs = obs;
                        guiaAlterar.dataInicio = DateTime.Now;
                        if (recepcao){
                            guiaAlterar.recepcionado = true;
                        }
                        else{
                            guiaAlterar.recepcionado = false;
                        }
                        db.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception except) {
                return false;
            }
        }
        // Recepção de Mercadorias
        public ActionResult RecepcionaMercadoria(string obs) {
            if (guardaGuias(obs, true)){
                ViewBag.data = DateTime.Now.ToString().Replace(" ", "&nbsp;");
                ViewBag.guia = obs.Split(';')[obs.Split(';').Count() - 1];
                ViewBag.numeroGuia = obs.Split(';')[obs.Split(';').Count()-1];
                ViewBag.camposControl_id = new SelectList(db.camposControloes, "id", "descricao");
                ViewBag.id_camposControl = new SelectList(db.camposControloes, "id", "descricao");
                ViewBag.id_Moradas = new SelectList(db.moradas, "id", "nome");
                ViewBag.morada = new SelectList(db.moradas, "id", "nome");
                ViewBag.id_produtos = new SelectList(db.produtoes, "codcomercial", "descricao");
                ViewBag.id_produtosDesc = new SelectList(db.produtoes, "codcomercial", "descricao");
                ViewBag.id_produtosUniCx = new SelectList(db.produtoes, "codcomercial", Convert.ToString("NrUnidadesCx"));
                ViewBag.id_produtosNrCxPal = new SelectList(db.produtoes, "codcomercial", Convert.ToString("NrCxsPalete"));
                ViewBag.id_produtosUnidaPal = new SelectList(db.produtoes, "codcomercial", Convert.ToString("UndsPorPalete"));
                ViewBag.id_numeroEncomenda = new SelectList(db.Encomendas, "id", "descricao_encomenda");
                ViewBag.encomendas = new SelectList(db.Encomendas, "id", "id");
                return View();
            }
            else
                return View("InvalidAcess");
        }

        //listaprodutosPai
        public ActionResult listaprodutosPai(string obs, string entrada) {
            ViewBag.guia = obs;
            ViewBag.numeroGuia = obs;
            
            var produto_new = db.produto_new.Include(p => p.Encomenda);
            List<produto_new> listaprodutosguia = new List<produto_new>();
            var index = 0;
            var totalunidades = 0;
            produto_new produtoPai = null;
            while (produto_new.Count() > index) {
                if (produto_new.ToList()[index].numero_guia != null &&
                    produto_new.ToList()[index].numero_guia.Equals(obs)) {
                    if (produto_new.ToList()[index].codcomercial.Equals(produto_new.ToList()[index].@ref) &&
                        produto_new.ToList()[index].refProduto.Equals(produto_new.ToList()[index].refProduto)) {
                        listaprodutosguia.Add(produto_new.ToList()[index]);
                        produtoPai = produto_new.ToList()[index];
                    }
                    else {
                        totalunidades = totalunidades + Convert.ToInt32(produto_new.ToList()[index].NrUnidades);
                    }
                }
                index++;
            }
            if (produtoPai != null){
                produtoPai.NrUnidades = totalunidades;
                db.Entry(produtoPai).State = EntityState.Modified;
                db.SaveChanges();
            }
            return View(listaprodutosguia);
        }

        //listaprodutosPai
        public ActionResult listaprodutosPaiEdit(string obs, string entrada){
            ViewBag.numeroGuia = obs.Split(';')[1];
           // entrada = obs.Split(';')[0];
            var guiaNum = new Recepcao();
            try{
                guiaNum = db.Recepcaos.Find(new Guid(entrada));
            }
            catch {
                guiaNum = db.Recepcaos.Find(new Guid(obs.Split(';')[1]));
                entrada = obs.Split(';')[1];
            }
            ViewBag.guia = obs.Split(';')[0]; 
            var produto_new = db.produto_new.Include(p => p.Encomenda);
            List<produto_new> listaprodutosguia = new List<produto_new>();
            var index = 0;
            var totalunidades = 0;
            produto_new produtoPai = null;
            while (produto_new.Count() > index){
                if (produto_new.ToList()[index].numero_guia != null &&
                    produto_new.ToList()[index].numero_guia.Equals(entrada))
                {
                    if (produto_new.ToList()[index].codcomercial.Equals(produto_new.ToList()[index].@ref)){
                        listaprodutosguia.Add(produto_new.ToList()[index]);
                        //em testes
                        totalunidades = Convert.ToInt32(produto_new.ToList()[index].NrUnidades);
                        totalunidades = 0;
                        //acabou testes
                        produtoPai = produto_new.ToList()[index];
                    }
                    else{
                        //totalunidades = totalunidades + Convert.ToInt32(produto_new.ToList()[index].NrUnidades);
                        totalunidades = totalunidades + Convert.ToInt32(produto_new.ToList()[index].NrUnidades);
                        /* totalunidades = 
                             Convert.ToInt32(registoExists(produtoPai.@ref, produtoPai.numero_guia).NrUnidades) 
                             +Convert.ToInt32(produto_new.ToList()[index].NrUnidades);*/
                    }
                }
                index++;
            }
            if (produtoPai != null){
                //produtoPai.NrUnidades = totalunidades;
                db.Entry(produtoPai).State = EntityState.Modified;
                db.SaveChanges();
            }
            return View(listaprodutosguia);
        }


        //listaprodutosPai
        public ActionResult listaprodutosSub(string obs, string entrada, string numeroGuia){
            ViewBag.numeroGuia = obs;
            ViewBag.guia = numeroGuia;
            ViewBag.entrada = entrada;
            var produto_new = db.produto_new.Include(p => p.Encomenda);
            List<produto_new> listaprodutosguia = new List<produto_new>();
            var index = 0;
            var totalunidades = 0;
            produto_new produtoPai = null;
            while (produto_new.Count() > index){
                if (produto_new.ToList()[index].numero_guia != null &&
                    produto_new.ToList()[index].numero_guia.Equals(obs) &&
                    produto_new.ToList()[index].@ref.Equals(entrada)){
                    if (!produto_new.ToList()[index].codcomercial.Equals(produto_new.ToList()[index].@ref)){
                        listaprodutosguia.Add(produto_new.ToList()[index]);
                        //teste validar se nao soma os demais
                        totalunidades = totalunidades + Convert.ToInt32(produto_new.ToList()[index].NrUnidades);
                    }
                    else{
                        produtoPai = produto_new.ToList()[index];
                    }
                }
                index++;
            }
            if (produtoPai != null) {
                produtoPai.NrUnidades = totalunidades;
                db.Entry(produtoPai).State = EntityState.Modified;
                db.SaveChanges();
            }
            return View(listaprodutosguia);
        }

        //listaprodutosEdit
        public ActionResult listaprodutosEdit(string obs, string referencia)
        {
            Recepcao recep = db.Recepcaos.Find(new Guid (obs));
            return listaprodutosSub(obs, referencia, recep.numeroGuia);
        }
        //listaprodutosGuia
        public ActionResult listaprodutosGuia(string obs, string entrada) {
            ViewBag.numeroGuia = obs;
            ViewBag.entrada = entrada;
            var produto_new = db.produto_new.Include(p => p.Encomenda);
            List<produto_new> listaprodutosguia = new List<produto_new>();
            var index = 0;

            while (produto_new.Count() > index) {
                if (produto_new.ToList()[index].numero_guia != null &&
                    produto_new.ToList()[index].numero_guia.Equals(obs)) {
                    listaprodutosguia.Add(produto_new.ToList()[index]);
                }
                index++;
            }
            return View(listaprodutosguia);
        }

        private produto_new registoExists(string referencias, string guia)
        {
            try{
                var produtoReferencia = db.produto_new;//
                var encontrou = false;
                var index = 0;
                while (!encontrou && index < produtoReferencia.Count()){
                    var produto = produtoReferencia.ToList()[index];
                    if (produto.codcomercial != null && produto.numero_guia!=null){
                        if (produto.codcomercial.Equals(referencias)
                            && produto.numero_guia.Equals(guia)){
                            return produto;
                        }
                    }
                    index++;
                }
            }
            catch (NullReferenceException ex)
            { }
            return null;
        }

        private long registoExists(string referencias, string guia, string lote, string validade) {
            try{
                var produtoReferencia = db.produto_new;//
                var encontrou = false;
                var index = 0;
                while (!encontrou && index < produtoReferencia.Count()){
                    var produto = produtoReferencia.ToList()[index];
                    if (produto.codcomercial != null){
                        if (produto.codcomercial.Equals(referencias)
                            && produto.lote.Equals(lote)
                            && produto.numero_guia.Equals(guia)){
                            long unidades = Convert.ToInt64(produto.NrUnidades);
                            db.produto_new.Remove(produto);
                            return unidades;
                        }
                    }
                    index++;
                }
            }
            catch (NullReferenceException ex)
            { }
            return 0;
        }


        private long apagaRegistoEntrada(string guia){
            try{
                var recepcaoList = new SelectList(db.saidasEntradas, "id", "numGuiaRecepcao").ToList();
                for (int i = 0; i < recepcaoList.Count; i++) {
                    if (recepcaoList[i].Value == guia) {
                        saidasEntrada saida = db.saidasEntradas.Find(recepcaoList[i].Text);
                        db.saidasEntradas.Remove(saida);
                    }
                }
            }
            catch (NullReferenceException ex)
            { }
            return 0;
        }


        private Boolean registoSubExists(string referencias, string guia, string referencia, string lote, string validade){
            var produtoReferencia = db.produto_new;//
            var encontrou = false;
            var index = 0;
            while (!encontrou && index < produtoReferencia.Count()){
                var produto = produtoReferencia.ToList()[index];
                if (produto.codcomercial != null){
                    if (produto.codcomercial.Equals(referencias)
                        && produto.numero_guia.Equals(guia)
                        && produto.@ref.Equals(referencia)){
                        db.produto_new.Remove(produto);
                        encontrou = true;
                    }
                }
                index++;
            }
            return encontrou;
        }


        // Adicionar produto na Recepção de Mercadorias
        public ActionResult EditSubProduto(string obs){
            var numeroguia = obs.Split(';')[0];
            Boolean encontrou = false;
            ViewBag.numeroGuia = obs.Split(';')[0];
            ViewBag.guia=db.Recepcaos.Find(new Guid(obs.Split(';')[0])).numeroGuia;
            
            if (obs != null && obs != ""){
                var produtoReferencia = db.produtoes;//

                var index = 0;
                while (!encontrou && index < produtoReferencia.Count()){
                    var produto = produtoReferencia.ToList()[index];
                    if (produto.codcomercial.Equals(obs.Split(';')[1])){
                        ViewBag.id_ref = obs.Split(';')[4];
                        ViewBag.refproduto = produto.codcomercial;
                        ViewBag.id_produtosDesc = produto.descricao;
                        ViewBag.id_produtosUniCx = produto.NrUnidadesCx;
                        ViewBag.id_produtosNrCxPal = produto.NrCxsPalete;
                        ViewBag.id_produtosUnidaPal = produto.UndsPorPalete;
                        ViewBag.Lote = obs.Split(';')[2];
                        ViewBag.Validade = obs.Split(';')[3];
                        ViewBag.numeroGuia = obs.Split(';')[0];
                        ViewBag.totalprodutos = Convert.ToInt32(ViewBag.totalprodutos) + Convert.ToInt32(produto.NrUnidadesCx); 
                        encontrou = true;
                    }
                    index++;
                }
                ViewBag.id_produtos = new SelectList(db.produtoes, "codcomercial", "descricao");
                ViewBag.id_produtosDesc = new SelectList(db.produtoes, "codcomercial", "descricao");
                ViewBag.id_produtosUniCx = new SelectList(db.produtoes, "codcomercial", "NrUnidadesCx");
                ViewBag.id_produtosNrCxPal = new SelectList(db.produtoes, "codcomercial", "NrCxsPalete");
                ViewBag.id_produtosUnidaPal = new SelectList(db.produtoes, "codcomercial", "UndsPorPalete");
                return View();
            }
            else
                return View("InvalidAcess");
        }

        //AddSSCC
        public ActionResult AddSSCC(string obs){
            var numeroguia = obs.Split(';')[0];
            var guia = obs.Split(';')[1];
            var referencia = obs.Split(';')[2];
            var qtd = obs.Split(';')[3];
            var lote = obs.Split(';')[4];
            var validade = obs.Split(';')[5];
            try{
                Convert.ToDateTime(validade);
            }
            catch {
                validade = "";
            }
            ViewBag.numeroGuia = numeroguia;
            ViewBag.guia = guia;
            ViewBag.id_ref = referencia;
            ViewBag.numUnidades = qtd;
            ViewBag.lote = lote;
            ViewBag.vald_ref = validade;
            ViewBag.lote_ref = lote;
            ViewBag.validade = validade;

            Recepcao recepcao = db.Recepcaos.Find(new Guid(guia));
            if (recepcao != null){
                ViewBag.morada = recepcao.morada;
                if (recepcao.Encomenda != null)
                    ViewBag.encomendas = recepcao.Encomenda.id;
                ViewBag.comentarios = recepcao.comentarios;
            }

            ViewBag.sscc = new SelectList(db.produto_new, "codcomercial", "descricao");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult AddSSCC([Bind(Include = "id,numeroGuia,dataRecepcao,id_camposControl,camposControl_id,id_numeroEncomenda,id_Moradas,comentarios,id_camposControlLivre,morada,id_produtos,produtos_id")] Recepcao recepcao){
            var gravado = false;
           
            var sscc = Request.Form["ssccField"];
            var idGuia = Request.Form["guia"];
            var RefProduto = Request.Form["id_ref"];
            var prodLote = Request.Form["lote"];
            var unidades = Request.Form["numUnidades"];
            var vldRef = Request.Form["vald_ref"];
            var prodValidade = Request.Form["Validade"];
            var produtoReferencia = db.produtoes;//
            var index = 0;
            while (index < produtoReferencia.Count()){
                var produto = produtoReferencia.ToList()[index];
                if (produto.codcomercial.Equals(RefProduto)){
                    var lista = produtoReferencia.ToList()[0];
                    var id_prod = lista.id;

                    produto_new subProduto = new produto_new();
                    subProduto.codcomercial = RefProduto;
                    subProduto.descricao = produto.descricao;
                    subProduto.@ref = RefProduto;
                    subProduto.lote = Request.Form["lote_ref"];
                    try{
                        if (vldRef != null && vldRef != "")
                            subProduto.validade = Convert.ToDateTime(vldRef);
                    }
                    catch (Exception ex) {
                        subProduto.validade = Convert.ToDateTime(DateTime.MaxValue);
                    }
                    subProduto.numero_guia = idGuia;
                    
                    produto_new produtoSSCC = new produto_new();
                    produtoSSCC.codcomercial = sscc;
                    produtoSSCC.@ref = RefProduto;
                    produtoSSCC.lote = prodLote;
                    produtoSSCC.NrUnidades = Convert.ToInt32(unidades);
                    if (subProduto.NrUnidades == null)
                        subProduto.NrUnidades = produtoSSCC.NrUnidades;
                    else
                        subProduto.NrUnidades = subProduto.NrUnidades + produtoSSCC.NrUnidades;
                    ViewBag.totalprodutos = subProduto.NrUnidades;
                    try{
                        if (prodValidade != null && prodValidade != "")
                                produtoSSCC.validade = Convert.ToDateTime(prodValidade);
                    }
                    catch (Exception ex){
                        produtoSSCC.validade = Convert.ToDateTime(DateTime.MaxValue);
                    }
                    produtoSSCC.numero_guia = idGuia;

                    db.produto_new.Add(produtoSSCC);
                    gtinRecebido recebido = new gtinRecebido();
                    recebido.descricao = produto.descricao;
                    recebido.numUnidades = Convert.ToInt32(produtoSSCC.NrUnidades);
                    recebido.validade = produtoSSCC.validade;
                    recebido.@ref = Convert.ToInt32(RefProduto);
                    recebido.numGtin = Convert.ToInt32(produto.codcomercial);
                    recebido.Lote = produtoSSCC.lote;
                    db.gtinRecebidos.Add(recebido);

                    db.SaveChanges();

                    saidasEntrada movimentoRecepcao = new saidasEntrada();
                    movimentoRecepcao.codcomercial = produtoSSCC.codcomercial;
                    if(subProduto.descricao!=null)
                        movimentoRecepcao.descricao = subProduto.descricao;
                    movimentoRecepcao.descricao = "";
                    movimentoRecepcao.gtinRef = RefProduto;
                    movimentoRecepcao.lote = produtoSSCC.lote;
                    movimentoRecepcao.validade = produtoSSCC.validade;
                    movimentoRecepcao.qtd = Convert.ToInt32(produtoSSCC.NrUnidades);
                    movimentoRecepcao.numGuiaRecepcao = subProduto.numero_guia;

                    db.saidasEntradas.Add(movimentoRecepcao);
                    db.SaveChanges();
                    var idRef = registoExists(RefProduto, subProduto.numero_guia);
                    if (idRef == null)
                        db.produto_new.Add(subProduto);
                    else{
                        idRef.NrUnidades = idRef.NrUnidades+ subProduto.NrUnidades;
                        db.Entry(idRef).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    index = produtoReferencia.Count();
                }
                index++;
            }
            
            var obs = Request.Form["numeroGuia"] +"; " + 
                    Request.Form["guia"] + "; " +  
                    Request.Form["id_ref"] + ";" + 
                    Request.Form["numUnidades"] + "; " + 
                    Request.Form["lote_ref"] + "; " +  
                    Request.Form["vald_ref"];
            return AddSSCC(obs);
        }

        // Adicionar produto na Recepção de Mercadorias
        public ActionResult AddSubProduto(string obs){
            db = new SiproEntities();
            var idguia = "";
            var numeroguia = "";
            var numEncomenda = "";
            var morada="";
            var comentario = "";
            if (obs.Split(';')[2] == "" && obs.Split(';')[1].Split('-').Count()<0) {
                ViewBag.erros = "Morada obrigatória";
                return RedirectToAction("RecepcionaMercadoria", new { obs = obs.Split(';')[0] });
            }
            else { 
                if (obs.Split(';').Count() > 4){
                    morada = obs.Split(';')[3];
                    numEncomenda = obs.Split(';')[2];
                    comentario = obs.Split(';')[4];
                    idguia = obs.Split(';')[1];
                    numeroguia = obs.Split(';')[0];
                }
                else {
                    numEncomenda = obs.Split(';')[1];
                    morada = obs.Split(';')[2];
                    comentario = obs.Split(';')[3];
                    numeroguia = obs.Split(';')[0];
                }
                ViewBag.guia = idguia;
                ViewBag.numeroGuia = numeroguia;
                Recepcao recepcao = new Recepcao();
                if (idguia == null || idguia == ""){
                    recepcao.id = Guid.NewGuid();
                    if (numEncomenda != null && numEncomenda != ""){
                        recepcao.id_numeroEncomenda = Convert.ToInt32(numEncomenda);
                    }
                    if (morada != null && morada != ""){
                        recepcao.morada = Convert.ToInt32(morada);
                    }
                    if (comentario != null && comentario != "")
                        recepcao.comentarios = comentario;

                    ViewBag.guia = recepcao.id;
                    recepcao.dataRecepcao = DateTime.Now;
                    recepcao.dataInicioRecepcao = DateTime.Now;
                    recepcao.numeroGuia = numeroguia;
                    db.Recepcaos.Add(recepcao);
                    db.GetValidationErrors();
                    try{
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException e){
                        foreach (var eve in e.EntityValidationErrors){
                            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors){
                                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                        throw;
                    }
                }
                else{
                    recepcao = db.Recepcaos.Find(new Guid(idguia));
                    if (numEncomenda != null && numEncomenda != ""){
                        recepcao.id_numeroEncomenda = Convert.ToInt32(numEncomenda);
                    }
                    if (morada != null && morada != ""){
                        recepcao.morada = Convert.ToInt32(morada);
                    }
                    if (comentario != null && comentario != "")
                        recepcao.comentarios = comentario;

                    ViewBag.guia = recepcao.id;
                    recepcao.numeroGuia = numeroguia;
                    db.Entry(recepcao).State = EntityState.Modified;
                    try{
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException e){
                        foreach (var eve in e.EntityValidationErrors){
                            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors){
                                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                        throw;
                    }
                }
            }
            Boolean encontrou = false;
            if (obs != null && obs != "") {
                var produtoReferencia = db.produtoes;//
                var index = 0;

                ViewBag.ean = new SelectList(db.produtoes, "codcomercial", "descricao");
                ViewBag.refEan = new SelectList(db.produtoes, "ean13_produto", "descricao");
                ViewBag.refunidade = new SelectList(db.produtoes, "codcomercial", "UndsPorPalete");
                return View();
            }
            else
                return View("InvalidAcess");
        }

        // Guardar produto adicionado na Recepção de Mercadorias
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult EditSubProduto([Bind(Include = "id,numeroGuia,dataRecepcao,id_camposControl,camposControl_id,id_numeroEncomenda,id_Moradas,comentarios,id_camposControlLivre,morada,id_produtos,produtos_id")] Recepcao recepcao){
            var gravado = false;
            var valorRec = Request.Form["numeroGuia"];
            var RefProduto = Request.Form["refproduto"];
            var RefProdDesc = Request.Form["descRef"];
            var prodUniCx = Request.Form["id_produtosUniCx"];
            var prodCxPal = Request.Form["id_produtosNrCxPal"];
            var prodUniPal = Request.Form["id_produtosUnidaPal"];
            var prodLote = Request.Form["Lote"];
            var unidades = Request.Form["subProdutoTotal"];
            var prodValidade = Request.Form["Validade"];
            var produtoReferencia = db.produtoes;//
            var index = 0;
            while (index < produtoReferencia.Count()){
                var produto = produtoReferencia.ToList()[index];
                if (produto.codcomercial.Equals(RefProduto)){
                    var lista = produtoReferencia.ToList()[0];
                    var id_prod = lista.id;
                    produto_new subProduto = new produto_new();
                    subProduto.codcomercial = RefProduto;
                    subProduto.descricao = produto.descricao;
                    subProduto.@ref = RefProduto;
                    subProduto.lote = prodLote;
                    if (prodValidade != null && prodValidade != "")
                        subProduto.validade = Convert.ToDateTime(prodValidade);
                    subProduto.numero_guia = Request.Form["numeroGuia"];
                   
                    for (var i = 0; i < Request.Form.Count; i++){
                        if (Request.Form["subProduto" + i] != null && Request.Form["subProduto" + i].Count() != 0){
                            produto_new produtoRef = new produto_new();
                            produtoRef.codcomercial = Request.Form["subProduto" + i];
                            produtoRef.@ref = RefProduto;
                            produtoRef.lote = Request.Form["subProdutolote" + i];
                            produtoRef.NrUnidades = Convert.ToInt32(Request.Form["subProdutoTotal" + i]);

                            subProduto.NrUnidades = subProduto.NrUnidades + produtoRef.NrUnidades;
                            ViewBag.totalprodutos = subProduto.NrUnidades;
                            if (Request.Form["subProdutoValidade" + i] != null && Request.Form["s" + i] != "")
                                produtoRef.validade = Convert.ToDateTime(Request.Form["subProdutoValidade" + i]);
                            produtoRef.numero_guia = Request.Form["numeroguia"];
                            db.produto_new.Add(produtoRef);
                          
                            saidasEntrada movimentoRecepcao = new saidasEntrada();
                            movimentoRecepcao.codcomercial = produtoRef.codcomercial;
                            movimentoRecepcao.descricao = subProduto.descricao;
                            movimentoRecepcao.gtinRef = RefProduto;
                            movimentoRecepcao.lote = produtoRef.lote;
                            movimentoRecepcao.validade = produtoRef.validade;
                            movimentoRecepcao.qtd = Convert.ToInt32(produtoRef.NrUnidades);
                            movimentoRecepcao.numGuiaRecepcao = subProduto.numero_guia;

                            db.saidasEntradas.Add(movimentoRecepcao);
                            gravado = true;
                        }
                
                    }
                   if (!gravado){
                        db.produto_new.Add(subProduto);
                        gravado = true;
                    }
                }
                index++;
            }
            db.SaveChanges();
            return RedirectToAction("EditaRecepMercadoria", new { id = Request.Form["numeroGuia"] });
        }

        // Guardar produto adicionado na Recepção de Mercadorias
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult AddSubProduto([Bind(Include = "id,numeroGuia,dataRecepcao,id_camposControl,camposControl_id,id_numeroEncomenda,id_Moradas,comentarios,id_camposControlLivre,morada,id_produtos,produtos_id")] Recepcao recepcao1){
            var gravado = false;
            var valor = Request.Form["refproduto"];
            var idGuia = Request.Form["guia"];
            var RefProduto = Request.Form["produtos_id"];
            var RefProdDesc = Request.Form["descRef"];
            var prodUniCx = Request.Form["id_produtosUniCx"];
            var prodCxPal = Request.Form["id_produtosNrCxPal"];
            var prodUniPal = Request.Form["id_produtosUnidaPal"];
            var prodLote = Request.Form["Lote"];
            var unidades = Request.Form["subProdutoTotal"];
            var prodValidade = Request.Form["Validade"];
            var produtoReferencia = db.produtoes;//
            var index = 0;
            while (index < produtoReferencia.Count()){
                var produto = produtoReferencia.ToList()[index];
                if (produto.codcomercial.Equals(RefProduto)){
                    var lista = produtoReferencia.ToList()[0];
                    var id_prod = lista.id;

                    produto_new subProduto = new produto_new();
                    subProduto.codcomercial = RefProduto;
                    subProduto.descricao = produto.descricao;
                    subProduto.@ref = RefProduto;
                    subProduto.lote = prodLote;
                    subProduto.NrUnidades = Convert.ToInt32(unidades);
                    if (prodValidade != null && prodValidade != "")
                        subProduto.validade = Convert.ToDateTime(prodValidade);
                    subProduto.numero_guia = idGuia;
                   
                    for (var i = 0; i < Request.Form.Count; i++){
                        if (Request.Form["subProduto" + i] != null && Request.Form["subProduto" + i].Count() != 0){
                            produto_new produtoRef = new produto_new();
                            produtoRef.codcomercial = Request.Form["subProduto" + i];
                            produtoRef.@ref = RefProduto;
                            produtoRef.lote = Request.Form["subProdutolote" + i];
                            produtoRef.NrUnidades = Convert.ToInt32(Request.Form["subProdutoTotal" + i]);
                            subProduto.NrUnidades = subProduto.NrUnidades + produtoRef.NrUnidades;
                            
                            ViewBag.totalprodutos = subProduto.NrUnidades;
                            if (Request.Form["subProdutoValidade" + i] != null && Request.Form["subProdutoValidade" + i] != "")
                                produtoRef.validade = Convert.ToDateTime(Request.Form["subProdutoValidade" + i]);
                            produtoRef.numero_guia = idGuia;
                            registoSubExists(produtoRef.codcomercial, subProduto.numero_guia, RefProduto, produtoRef.lote, produtoRef.validade.ToString());

                            db.produto_new.Add(produtoRef);
                            db.SaveChanges();

                            saidasEntrada movimentoRecepcao = new saidasEntrada();
                            movimentoRecepcao.codcomercial = produtoRef.codcomercial;
                            movimentoRecepcao.descricao = subProduto.descricao;
                            movimentoRecepcao.gtinRef = RefProduto;
                            movimentoRecepcao.lote = produtoRef.lote;
                            movimentoRecepcao.validade = produtoRef.validade;
                            movimentoRecepcao.qtd = Convert.ToInt32(produtoRef.NrUnidades);
                            movimentoRecepcao.numGuiaRecepcao = subProduto.numero_guia;

                            db.saidasEntradas.Add(movimentoRecepcao);
                            db.SaveChanges();
                            gravado = true;
                        }
                    }
                    subProduto.NrUnidades = subProduto.NrUnidades + registoExists(RefProduto, subProduto.numero_guia, subProduto.lote, subProduto.validade.ToString());
                    db.produto_new.Add(subProduto);
                    db.SaveChanges();
                    if (!gravado){
                        db.produto_new.Add(subProduto);
                        gravado = true;
                    }
                }
            index++;
            }
            return RedirectToAction("EditaRecepMercadoria", new { id = idGuia });
        }

        // GET: ArquivaObs
        public ActionResult ArquivaObs(string obs){
            guardaGuias(obs, false);

            return RedirectToAction("Index");
        }


        // GET: Recepcaos/CreateRecepcaoMercadoria
        public ActionResult NaoConformidadeRecepcao(string guia){
            if (guia != ""){
                var naoconformidades = guia.Split(';');
                var guiaRecebida = naoconformidades[0];
                List<SelectListItem> naoConformidades = new List<SelectListItem>();
                for (int i = 1; i < naoconformidades.Count(); i++){
                    SelectListItem item = new SelectListItem();
                    if (naoconformidades[i] != "" && naoconformidades[i] != ";"){
                        var nova = new naoConforRecepcao();
                        nova.guia = guiaRecebida;
                        nova.tiponaoconformidade_id = Int32.Parse(naoconformidades[i]);
                        nova.obs = "";
                        naoConformidades.Add(new SelectListItem { Value = naoconformidades[i], Text = db.camposControloes.Find(nova.tiponaoconformidade_id).descricao });

                        db.naoConforRecepcaos.Add(nova);
                        ViewBag.listNaoConformidades = naoConformidades;
                        ViewBag.guia = guiaRecebida;
                    }
                    db.SaveChanges();
                }
                return View();
            }
            else return View("InvalidAcess");
        }

        // GET: Recepcaos/CreateRecepcaoMercadoria
        public ActionResult CreateRecepcaoMercadoria(string guia, string obs){
            if (guia != ""){
                int? page;
                page = 1;
                ViewBag.camposControl_id = new SelectList(db.camposControloes, "id", "descricao");
                ViewBag.camposControl_id = new SelectList(db.camposControloes, "id", "descricao");
                ViewBag.id_camposControl = new SelectList(db.camposControloes, "id", "descricao");

                ViewBag.id_numeroEncomenda = new SelectList(db.Encomendas, "id", "descricao_encomenda");
                ViewBag.id_Moradas = new SelectList(db.moradas, "id", "nome");
                ViewBag.id_produtos = new SelectList(db.produtoes, "codcomercial", "descricao");
                ViewBag.id_produtosDesc = new SelectList(db.produtoes, "codcomercial", "descricao");
                ViewBag.id_produtosUniCx = new SelectList(db.produtoes, "codcomercial", "NrUnidadesCx");
                ViewBag.id_produtosNrCxPal = new SelectList(db.produtoes, "codcomercial", "NrCxsPalete");
                ViewBag.id_produtosUnidaPal = new SelectList(db.produtoes, "codcomercial", "UndsPorPalete");
                ViewBag.morada = new SelectList(db.moradas, "id", "nome");
                ViewBag.Message = "Mensagem ViewBab";

                var request = Request;
                var camposControl = db.camposControloes.ToList();
                int pageSize = 20;
                int pageNumber = (page ?? 1);
                return View(camposControl.ToPagedList(pageNumber, pageSize));
            }
            else return View("InvalidAcess");
        }

        // GET: Recepcaos/CreateCamposControl
        public ActionResult CreateCamposControl(String motivo){
            if (motivo != ""){
                int? page;
                page = 1;
                ViewBag.camposControl_id = new SelectList(db.camposControloes, "id", "descricao");
                ViewBag.camposControl_id = new SelectList(db.camposControloes, "id", "descricao");
                ViewBag.id_camposControl = new SelectList(db.camposControloes, "id", "descricao");
                ViewBag.id_produtos = new SelectList(db.camposControloes, "id", "descricao");
                ViewBag.id_Moradas = new SelectList(db.moradas, "id", "nome");
                ViewBag.morada = new SelectList(db.moradas, "id", "nome");
                var camposControl = db.camposControloes.ToList();
                int pageSize = 20;
                int pageNumber = (page ?? 1);
                return View(camposControl.ToPagedList(pageNumber, pageSize));
            }
            else
                return View("InvalidAcess");
        }

        // POST: Recepcaos/Create
        // Apagar
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "id,numeroGuia,dataRecepcao,id_camposControl,camposControl_id,id_numeroEncomenda,id_Moradas,comentarios,id_camposControlLivre,morada,id_produtos,produtos_id")] Recepcao recepcao){
        public ActionResult Create([Bind(Include = "id,id_numeroEncomenda,id_Moradas,comentarios")] Recepcao recepcao)
        {
            if (ModelState.IsValid){
                recepcao.id = Guid.NewGuid();
                recepcao.dataRecepcao = DateTime.Now;
                recepcao.dataInicioRecepcao = DateTime.Now;
                db.Recepcaos.Add(recepcao);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.camposControl_id = new SelectList(db.camposControloes, "id", "descricao", recepcao.camposControl_id);
            ViewBag.camposControl_id = new SelectList(db.camposControloes, "id", "descricao", recepcao.camposControl_id);
            ViewBag.id_camposControl = new SelectList(db.camposControloes, "id", "descricao", recepcao.id_camposControl);
            ViewBag.id_produtos = new SelectList(db.camposControloes, "id", "descricao", recepcao.id_produtos);
            ViewBag.id_numeroEncomenda = new SelectList(db.Encomendas, "id", "descricao_encomenda", recepcao.id_numeroEncomenda);
            ViewBag.id_Moradas = new SelectList(db.moradas, "id", "nome", recepcao.id_Moradas);
            ViewBag.morada = new SelectList(db.moradas, "id", "nome", recepcao.morada);
            return View(recepcao);
        }

        // GET: Recepcaos/Edit/5
        public ActionResult Edit(Guid? id){
            if (id == null){
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recepcao recepcao = db.Recepcaos.Find(id);
            if (recepcao == null){
                return HttpNotFound();
            }
            ViewBag.camposControl_id = new SelectList(db.camposControloes, "id", "descricao", recepcao.camposControl_id);
            ViewBag.camposControl_id = new SelectList(db.camposControloes, "id", "descricao", recepcao.camposControl_id);
            ViewBag.id_camposControl = new SelectList(db.camposControloes, "id", "descricao", recepcao.id_camposControl);
            ViewBag.id_produtos = new SelectList(db.camposControloes, "id", "descricao", recepcao.id_produtos);
            ViewBag.id_numeroEncomenda = new SelectList(db.Encomendas, "id", "descricao_encomenda", recepcao.id_numeroEncomenda);
            ViewBag.id_Moradas = new SelectList(db.moradas, "id", "nome", recepcao.id_Moradas);
            ViewBag.morada = new SelectList(db.moradas, "id", "nome", recepcao.morada);
            return View(recepcao);
        }

        // POST: Recepcaos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,numeroGuia,dataRecepcao,id_camposControl,camposControl_id,id_numeroEncomenda,id_Moradas,comentarios,id_camposControlLivre,morada,id_produtos,produtos_id")] Recepcao recepcao){
            if (ModelState.IsValid){
                db.Entry(recepcao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.camposControl_id = new SelectList(db.camposControloes, "id", "descricao", recepcao.camposControl_id);
            ViewBag.camposControl_id = new SelectList(db.camposControloes, "id", "descricao", recepcao.camposControl_id);
            ViewBag.id_camposControl = new SelectList(db.camposControloes, "id", "descricao", recepcao.id_camposControl);
            ViewBag.id_produtos = new SelectList(db.camposControloes, "id", "descricao", recepcao.id_produtos);
            ViewBag.id_numeroEncomenda = new SelectList(db.Encomendas, "id", "descricao_encomenda", recepcao.id_numeroEncomenda);
            ViewBag.id_Moradas = new SelectList(db.moradas, "id", "nome", recepcao.id_Moradas);
            ViewBag.morada = new SelectList(db.moradas, "id", "nome", recepcao.morada);
            return View(recepcao);
        }


        // GET: Recepcaos/DeleteProsuto_new/5
        public ActionResult DeleteProsuto_new(string id)
        {
            if (id == null){
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            produto_new prod = db.produto_new.Find(id);
            var guia = prod.numero_guia;
            if (prod == null)
            {
                return HttpNotFound();
            }
            return listaprodutosGuia(guia, "");
        }

        // GET: Recepcaos/Delete/5
        public ActionResult DeleteRecepcao(Guid? id){
            if (id == null){
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recepcao recepcao = db.Recepcaos.Find(id);
            if(recepcao!=null)
                db.Recepcaos.Remove(recepcao);
            apagaProdutos(recepcao.numeroGuia);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Recepcaos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id){
            Recepcao recepcao = db.Recepcaos.Find(id);
            db.Recepcaos.Remove(recepcao);
            apagaProdutos(recepcao.id.ToString());
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing){
            if (disposing){
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: produto_new/Edit/5
        public ActionResult EditProdutos(int? id, string obs){
            if (id == null){
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            produto_new produto_new = db.produto_new.Find(id);
            if (produto_new == null){
                return HttpNotFound();
            }
            ViewBag.id_numeroEncomenda = new SelectList(db.Encomendas, "id", "descricao_encomenda", produto_new.id_numeroEncomenda);
            //produto_new.validade = produto_new.validade.
            return View(produto_new);
        }

        // POST: produto_new/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProdutos([Bind(Include = "id,codcomercial,descricao,qtd,id_numeroEncomenda,ref,NrUnidades,lote,validade,refProduto,numero_guia")] produto_new produto_new, string obs){
            if (ModelState.IsValid){
                db.Entry(produto_new).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("EditaRecepMercadoria", new { obs = produto_new.numero_guia });
            }
            ViewBag.id_numeroEncomenda = new SelectList(db.Encomendas, "id", "descricao_encomenda", produto_new.id_numeroEncomenda);
            return View(produto_new);
        }
        
        // GET: produto_new/Delete/5
        public ActionResult DeleteProdutosSub(int? id, string obs){
            if (id == null){
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            produto_new produto_new = db.produto_new.Find(id);
            var numero_guia = produto_new.numero_guia;
            var refPai = produto_new.@ref;
            var returnar = produto_new.numero_guia + ";"+ produto_new.@ref + ";"+ produto_new.lote+ ";" + produto_new.validade + ";" + produto_new.@ref + ";";
            if (produto_new == null){
                return HttpNotFound();
            }

            db.produto_new.Remove(produto_new);
            db.SaveChanges();
            return RedirectToAction("listaprodutosEdit", new { obs = produto_new.numero_guia, referencia = produto_new.@ref });
        }

        // GET: produto_new/Delete/5
        public ActionResult DeleteProdutos(int? id, string obs){
            if (id == null){
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            produto_new produto_new = db.produto_new.Find(id);
            if (produto_new == null){
                return HttpNotFound();
            }
           
            db.produto_new.Remove(produto_new);
            db.SaveChanges();
            
            return RedirectToAction("RecepcionaMercadoria", new { obs = obs });
        }

        // GET: produto_new/Delete/5
        public ActionResult DeleteProdutosEdit(int? id, string obs){
            if (id == null){
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            produto_new produto_new = db.produto_new.Find(id);
            if (produto_new == null){
                return HttpNotFound();
            }

            db.produto_new.Remove(produto_new);
            db.SaveChanges();
            return RedirectToAction("EditaRecepMercadoria", new { id = produto_new.numero_guia });
        }

        // POST: produto_new/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteProdutosConfirmed(int id,string obs){
            produto_new produto_new = db.produto_new.Find(id);
            db.produto_new.Remove(produto_new);
            db.SaveChanges();
            return RedirectToAction("EditaRecepMercadoria", new { obs = produto_new.numero_guia });
        }
    }
}
