using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Sipro_2017
{
    public class ExpedicaosController : Controller
    {
        private SiproEntities db = new SiproEntities();

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

            var expedicao = from s in db.Expedicaos
                            select s;

            if (!String.IsNullOrEmpty(criterioPesquisa))
            {
                expedicao = expedicao.Where(s => s.numeroGuia.Equals(criterioPesquisa) ||
                    s.dataExpedicao.Equals(criterioPesquisa) || s.morada.Equals(criterioPesquisa)
                    || s.id.Equals(criterioPesquisa)
                    || s.cmr.Equals(criterioPesquisa) || s.matricula.Equals(criterioPesquisa));
            }

            switch (sortOrder)
            {/**
              
                "
                "dataExpedicao
                 */
                case "morada_asc":
                    expedicao = expedicao.OrderByDescending(s => s.morada);
                    break;
                case "morada_desc":
                    expedicao = expedicao.OrderBy(s => s.morada);
                    break;
                case "numexp_asc":
                    expedicao = expedicao.OrderByDescending(s => s.id);
                    break;
                case "numexp_desc":
                    expedicao = expedicao.OrderBy(s => s.id);
                    break;
                case "matricula_asc":
                    expedicao = expedicao.OrderByDescending(s => s.matricula);
                    break;
                case "matricula_desc":
                    expedicao = expedicao.OrderBy(s => s.matricula);
                    break;
                case "cmr_asc":
                    expedicao = expedicao.OrderByDescending(s => s.cmr);
                    break;
                case "cmr_desc":
                    expedicao = expedicao.OrderBy(s => s.cmr);
                    break;
                case "guiaExp_asc":
                    expedicao = expedicao.OrderByDescending(s => s.numeroGuia);
                    break;
                case "guiaExp_desc":
                    expedicao = expedicao.OrderBy(s => s.numeroGuia);
                    break;
                case "dataExpedicao_asc":
                    expedicao = expedicao.OrderByDescending(s => s.dataExpedicao);
                    break;
                case "dataExpedicao_desc":
                    expedicao = expedicao.OrderBy(s => s.dataExpedicao);
                    break;

                default:
                    expedicao = expedicao.OrderByDescending(s => s.id);
                    break;
            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(expedicao.ToPagedList(pageNumber, pageSize));
        }

        // GET: Expedicaos/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expedicao expedicao = db.Expedicaos.Find(id);
            if (expedicao == null)
            {
                return HttpNotFound();
            }
            return View(expedicao);
        }

        // GET: Expedicaos/Create
        public ActionResult Create()
        {
            ViewBag.camposControl_id = new SelectList(db.camposControloes, "id", "descricao");
            ViewBag.id_camposControl = new SelectList(db.camposControloes, "id", "descricao");
            return View();
        }

        // POST: Expedicaos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,numeroGuia,num_expedicao,dataExpedicao,id_camposControl,camposControl_id,matricula,cmr")] Expedicao expedicao)
        {
            if (ModelState.IsValid)
            {
                expedicao.id = Guid.NewGuid();
                db.Expedicaos.Add(expedicao);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.camposControl_id = new SelectList(db.camposControloes, "id", "descricao", expedicao.camposControl_id);
            ViewBag.id_camposControl = new SelectList(db.camposControloes, "id", "descricao", expedicao.id_camposControl);
            return View(expedicao);
        }

        // GET: Expedicaos/ExpedicaoMercadoriaEdit/5
        public ActionResult ExpedicaoMercadoriaEdit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Expedicao expedicao = db.Expedicaos.Find(id);
            if (expedicao == null)
            {
                return HttpNotFound();
            }

            ViewBag.data = expedicao.dataExpedicao.ToString().Replace(" ", "&nbsp;");
            ViewBag.numeroGuia = expedicao.id;
            ViewBag.guia = expedicao.numeroGuia ;
            //ViewBag.camposControl_id = new SelectList(db.camposControloes, "id", "descricao");
            //ViewBag.id_camposControl = new SelectList(db.camposControloes, "id", "descricao");
            //ViewBag.id_Moradas = new SelectList(db.moradas, "id", "nome", expedicao.morada);
            ViewBag.morada = new SelectList(db.moradas, "id", "nome", expedicao.morada);
            //ViewBag.id_produtos = new SelectList(db.produtoes, "codcomercial", "descricao");
            //ViewBag.id_produtosDesc = new SelectList(db.produtoes, "codcomercial", "descricao");
            //ViewBag.id_produtosUniCx = new SelectList(db.produtoes, "codcomercial", Convert.ToString("NrUnidadesCx"));
            //ViewBag.id_produtosNrCxPal = new SelectList(db.produtoes, "codcomercial", Convert.ToString("NrCxsPalete"));
            //ViewBag.id_produtosUnidaPal = new SelectList(db.produtoes, "codcomercial", Convert.ToString("UndsPorPalete"));
            //ViewBag.encomendas = new SelectList(db.Encomendas, "id", "id");

            var listaprodutosdesc = new SelectList(db.produtoes, "codcomercial", "descricao");

            //ViewBag.id_produtosDescricao = listaprodutosdesc;
            //ViewBag.id_produtosCodComercial = new SelectList(db.produto_new, "codcomercial", "ref");
            //ViewBag.id_qtd = new SelectList(db.produto_new, "id", "NrUnidades");
            //ViewBag.id_lote = new SelectList(db.produto_new, "id", "lote");
            //ViewBag.id_validade = new SelectList(db.produto_new, "id", "validade");
            //ViewBag.id_encomenda = new SelectList(db.produto_new, "id", "id_numeroEncomenda");
            //ViewBag.id_produtosDesc = new SelectList(db.produtoes, "codcomercial", "descricao");
            //ViewBag.id_produtos = new SelectList(db.produtoes, "id", "codcomercial");
            //ViewBag.id_produtosGtinFabricado = new SelectList(db.gtinFabricados, "id", "gtinFabricado1");
            //ViewBag.id_refGtinFabricado = new SelectList(db.gtinFabricados, "id", "produto_id");
            //ViewBag.id_qtdFabricado = new SelectList(db.OrdemFabricoes, "id", "qtdFabricado");
            //ViewBag.id_loteFabricado = new SelectList(db.gtinFabricados, "id", "loteProduzir");
            //ViewBag.id_validadeFabricado = new SelectList(db.OrdemFabricoes, "id", "validade");
            //ViewBag.id_encomendaFabricado = new SelectList(db.gtinFabricados, "id", "encomenda_id");
            //ViewBag.id_produtosGtinRecebido = new SelectList(db.gtinRecebidos, "id", "numGtin");
            //ViewBag.id_refGtinRecebido = new SelectList(db.gtinRecebidos, "id", "ref");
            //ViewBag.id_OfGtinFabricado = new SelectList(db.gtinFabricados, "id", "ordemFabrico_id");
            //ViewBag.id_qtdGtinRecebido = new SelectList(db.gtinRecebidos, "id", "numUnidades");
            //ViewBag.id_loteGtinRecebido = new SelectList(db.gtinRecebidos, "id", "Lote");
            //ViewBag.id_validadeGtinRecebido = new SelectList(db.gtinRecebidos, "id", "validade");
            //ViewBag.id_encomendaGtinRecebido = new SelectList(db.gtinRecebidos, "id", "numGtin");
            //ViewBag.id_descricaoGtinRecebido = new SelectList(db.gtinRecebidos, "id", "descricao");
            //ViewBag.ofValidade = new SelectList(db.OrdemFabricoes, "id", "validade");
            //ViewBag.ofQtdProduzida = new SelectList(db.OrdemFabricoes, "id", "qtdFabricado");
            //ViewBag.ofQtdProduzida1 = new SelectList(db.gtinFabricados, "id", "qtdFabricada");
            //LOTE
            //ViewBag.ofLOTE = new SelectList(db.OrdemFabricoes, "id", "LOTE");
            //produto_id
            //ViewBag.ofProduto = new SelectList(db.OrdemFabricoes, "id", "produto_id");
            //ViewBag.camposControl_id = new SelectList(db.camposControloes, "id", "descricao", expedicao.camposControl_id);
            //ViewBag.id_camposControl = new SelectList(db.camposControloes, "id", "descricao", expedicao.id_camposControl);
            //ViewBag.id_OfGtinFabricado = new SelectList(db.gtinFabricados, "id", "ordemFabrico_id");
            //ViewBag.id_qtdFabricadoOF = new SelectList(db.OrdemFabricoes, "id", "qtdFabricado");
            //ViewBag.id_qtdFabricado = new SelectList(db.gtinFabricados, "id", "qtdFabricada");
            //ViewBag.id_loteFabricado = new SelectList(db.gtinFabricados, "id", "loteProduzir");
            //ViewBag.id_validadeFabricado = new SelectList(db.OrdemFabricoes, "id", "validade");
            //ViewBag.id_encomendaFabricado = new SelectList(db.gtinFabricados, "id", "encomenda_id");
            //ViewBag.id_refGtinFabricado = new SelectList(db.OrdemFabricoes, "id", "produto_id");
            //ViewBag.id_OfGtinFabricadoSSCC = new SelectList(db.gtinFabricados, "id", "gtinFabricado1");

            return View(expedicao);
        }

        // POST: Expedicaos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,numeroGuia,num_expedicao,dataExpedicao,id_camposControl,camposControl_id,matricula,cmr")] Expedicao expedicao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(expedicao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.camposControl_id = new SelectList(db.camposControloes, "id", "descricao", expedicao.camposControl_id);
            ViewBag.id_camposControl = new SelectList(db.camposControloes, "id", "descricao", expedicao.id_camposControl);
            return View(expedicao);
        }

        // GET: Expedicaos/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expedicao expedicao = db.Expedicaos.Find(id);
            if (expedicao == null)
            {
                return HttpNotFound();
            }
            return View(expedicao);
        }

        // POST: Expedicaos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Expedicao expedicao = db.Expedicaos.Find(id);
            db.Expedicaos.Remove(expedicao);
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

        // GET: Recepcaos/CreateCamposControl (Primeira Fase Expedição)
        public ActionResult CreateCamposControl(String motivo) {
            if (motivo != "") {
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

        // GET: Expedicao/NaoConformidadeExpedicao
        public ActionResult NaoConformidadeExpedicao(string guia) {
            if (guia != "") {
                var naoconformidades = guia.Split(';');
                var guiaRecebida = naoconformidades[0];
                List<SelectListItem> naoConformidades = new List<SelectListItem>();
                for (int i = 1; i < naoconformidades.Count(); i++) {
                    SelectListItem item = new SelectListItem();
                    if (naoconformidades[i] != "" && naoconformidades[i] != ";") {
                        var nova = new naoConforExpedicao();
                        nova.guiaExpedicao = guiaRecebida;
                        nova.tiponaoconformidade_id = Int32.Parse(naoconformidades[i]);
                        //nova.tiponaoconformidade = db.TipoNaoConformidades.Find(nova.tiponaoconformidade_id);
                        nova.obs = "";
                        naoConformidades.Add(new SelectListItem { Value = naoconformidades[i], Text = db.camposControloes.Find(nova.tiponaoconformidade_id).descricao });

                        db.naoConforExpedicaos.Add(nova);
                        ViewBag.listNaoConformidades = naoConformidades;
                        ViewBag.guia = guiaRecebida;
                    }
                    db.SaveChanges();
                }
                //ViewBag.Message = "Mensagem ViewBab";
                return View();
            }
            else return View("InvalidAcess");
        }

        private Boolean guardaGuias(string obs, Boolean expedido) {
            try {
                naoConforExpedicao viewModel = new naoConforExpedicao();
                if (obs.Split(';').Count() > 0) {
                    var guia = obs.Split(';')[obs.Split(';').Count() - 1];
                    obs = obs.Split(';')[0];
                    var naoConforExpedicao = db.naoConforExpedicaos;

                    var guias = naoConforExpedicao.Where(g => g.guiaExpedicao.Equals(guia));
                    guias = guias.Where(g => g.guiaExpedicao.Equals(guia));

                    List<naoConforExpedicao> listaOcorrencias = guias.ToList();
                    var numeElem = listaOcorrencias.Count();

                    for (int i = 0; i < numeElem; i++) {
                        var guiaAlterar = listaOcorrencias[i];
                        guiaAlterar.obs = obs;
                        guiaAlterar.dataInicio = DateTime.Now;
                        if (expedido) {
                            guiaAlterar.expedido = true;
                        }
                        else {
                            guiaAlterar.expedido = false;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExpedicaoMercadoriaEdit(String refPai, [Bind(Include = "id,morada,numeroGuia,num_expedicao,id_camposControl,camposControl_id,matricula,cmr")] Expedicao expedicao) {
            expedicao.dataExpedicao = Convert.ToDateTime(Request.Form["dataExpedicao"].Replace("&nbsp;", " "));
            expedicao.numeroGuia = Request.Form["guia"];
            expedicao.morada = Convert.ToInt32(expedicao.morada);
            db.Entry(expedicao).State = EntityState.Modified;
            db.SaveChanges();

            ViewBag.data = expedicao.dataExpedicao.ToString().Replace(" ", "&nbsp;");
            ViewBag.numeroGuia = expedicao.id;
            ViewBag.guia = expedicao.numeroGuia;
            ViewBag.morada = new SelectList(db.moradas, "id", "nome", expedicao.morada);

            return View(expedicao);
        }



        //MovimentoProdutoSaida
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public ActionResult ExpedicaoMercadoria(String refPai, [Bind(Include = "id,morada,numeroGuia,num_expedicao,dataExpedicao,id_camposControl,camposControl_id,matricula,cmr")] Expedicao expedicao) {
            try
            {
                MovimentoProdutoSaida();

                // db.Entry(expedicao).State = EntityState.Modified;
                expedicao.dataExpedicao = Convert.ToDateTime(Request.Form["dataExpedicao"].Replace("&nbsp;", " "));
                db.Expedicaos.Add(expedicao);
                expedicao.id = Guid.NewGuid();
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException excption) {
                ViewBag.camposControl_id = new SelectList(db.camposControloes, "id", "descricao", expedicao.camposControl_id);
                ViewBag.id_camposControl = new SelectList(db.camposControloes, "id", "descricao", expedicao.id_camposControl);
                return View(expedicao);
            }

        }

        public ActionResult ExpedicaoMercadoria(string obs) {
            if (guardaGuias(obs, true))
            {
                ViewBag.data = DateTime.Now.ToString().Replace(" ", "&nbsp;");
                ViewBag.guia = obs.Split(';')[obs.Split(';').Count() - 1];
                ViewBag.numeroGuia = obs.Split(';')[obs.Split(';').Count() - 1];
                //duvidas
                ViewBag.numeroRecep = obs.Split(';')[obs.Split(';').Count()-1];
                ViewBag.camposControl_id = new SelectList(db.camposControloes, "id", "descricao");
                ViewBag.id_camposControl = new SelectList(db.camposControloes, "id", "descricao");
                ViewBag.id_Moradas = new SelectList(db.moradas, "id", "nome");
                ViewBag.morada = new SelectList(db.moradas, "id", "nome");
                ViewBag.id_produtos = new SelectList(db.produtoes, "codcomercial", "descricao");
                ViewBag.id_produtosDesc = new SelectList(db.produtoes, "codcomercial", "descricao");
                ViewBag.id_produtosUniCx = new SelectList(db.produtoes, "codcomercial", Convert.ToString("NrUnidadesCx"));
                ViewBag.id_produtosNrCxPal = new SelectList(db.produtoes, "codcomercial", Convert.ToString("NrCxsPalete"));
                ViewBag.id_produtosUnidaPal = new SelectList(db.produtoes, "codcomercial", Convert.ToString("UndsPorPalete"));
                ViewBag.id_refGtinRecebido = new SelectList(db.gtinRecebidos, "id", "ref");
                ViewBag.encomendas = new SelectList(db.Encomendas, "id", "id");

                ViewBag.id_OfGtinFabricado = new SelectList(db.gtinFabricados, "id", "ordemFabrico_id");
                ViewBag.id_qtdFabricado = new SelectList(db.OrdemFabricoes, "id", "qtdFabricado");
                ViewBag.id_loteFabricado = new SelectList(db.gtinFabricados, "id", "loteProduzir");
                ViewBag.id_validadeFabricado = new SelectList(db.OrdemFabricoes, "id", "validade");
                ViewBag.id_encomendaFabricado = new SelectList(db.gtinFabricados, "id", "encomenda_id");
                ViewBag.id_refGtinFabricado = new SelectList(db.OrdemFabricoes, "id", "produto_id");
                ViewBag.id_OfGtinFabricadoSSCC = new SelectList(db.gtinFabricados, "id", "gtinFabricado1");
                /*

                var listaprodutosdesc = new SelectList(db.produtoes, "codcomercial", "descricao");
                    ViewBag.id_refGtinFabricado = new SelectList(db.gtinFabricados, "id", "produto_id");

                ViewBag.id_produtosDescricao = listaprodutosdesc;
                ViewBag.id_produtosCodComercial = new SelectList(db.produto_new, "codcomercial", "ref");
                ViewBag.id_qtd = new SelectList(db.produto_new, "id", "NrUnidades");
                ViewBag.id_lote = new SelectList(db.produto_new, "id", "lote");
                ViewBag.id_validade = new SelectList(db.produto_new, "id", "validade");
                ViewBag.id_encomenda = new SelectList(db.produto_new, "id", "id_numeroEncomenda");
                ViewBag.id_produtosDesc = new SelectList(db.produtoes, "codcomercial", "descricao");
                ViewBag.id_produtos = new SelectList(db.produtoes, "id", "codcomercial");
                ViewBag.id_produtosGtinFabricado = new SelectList(db.gtinFabricados, "id", "gtinrecebido_id");
                ViewBag.id_refGtinFabricado = new SelectList(db.gtinFabricados, "id", "produto_id");
                ViewBag.id_qtdFabricado = new SelectList(db.OrdemFabricoes, "id", "qtdFabricado");
                ViewBag.id_loteFabricado = new SelectList(db.gtinFabricados, "id", "loteProduzir");
                ViewBag.id_validadeFabricado = new SelectList(db.OrdemFabricoes, "id", "validade");
                ViewBag.id_encomendaFabricado = new SelectList(db.gtinFabricados, "id", "encomenda_id");
                ViewBag.id_produtosGtinRecebido = new SelectList(db.gtinRecebidos, "id", "numGtin");
                ViewBag.id_refGtinRecebido = new SelectList(db.gtinRecebidos, "id", "ref");
                ViewBag.id_OfGtinFabricado = new SelectList(db.gtinFabricados, "id", "ordemFabrico_id");
                ViewBag.id_qtdGtinRecebido = new SelectList(db.gtinRecebidos, "id", "numUnidades");
                ViewBag.id_loteGtinRecebido = new SelectList(db.gtinRecebidos, "id", "Lote");
                ViewBag.id_validadeGtinRecebido = new SelectList(db.gtinRecebidos, "id", "validade");
                ViewBag.id_encomendaGtinRecebido = new SelectList(db.gtinRecebidos, "id", "numGtin");
                ViewBag.id_descricaoGtinRecebido = new SelectList(db.gtinRecebidos, "id", "descricao");
                ViewBag.ofValidade = new SelectList(db.OrdemFabricoes, "id", "validade");
                ViewBag.ofQtdProduzida = new SelectList(db.OrdemFabricoes, "id", "qtdFabricado");
                */
                return View();
            }
            else
                return View("InvalidAcess");
        }

        private String saveExpedicao(String obs) {
            try
            {
                Expedicao exp = db.Expedicaos.Find(Guid.Parse(obs.Split(';')[0]));
                ViewBag.numeroGuia = exp.numeroGuia;
                return obs.Split(';')[0];
            }
            catch {
                Expedicao expedicao = new Expedicao();
                expedicao.cmr = obs.Split(';')[3];
                expedicao.morada = Convert.ToInt32(obs.Split(';')[1]);
                expedicao.dataExpedicao = DateTime.Now;
                expedicao.matricula = obs.Split(';')[2];
                expedicao.numeroGuia = obs.Split(';')[0];
                expedicao.id = Guid.NewGuid();
                db.Expedicaos.Add(expedicao);
                db.SaveChanges();
                ViewBag.numeroGuia = obs.Split(';')[0];
                return expedicao.id.ToString();
            }
        }

        //listaprodutosPai
        public ActionResult listaprodutosPai(string obs)
        {
            ViewBag.guia = obs.Split(';')[0];
            var guia = obs.Split(';')[0];
            if (obs.Split(';').Count() > 1)
                ViewBag.numeroGuia = obs.Split(';')[1];
            else {
                ViewBag.numeroGuia = db.Expedicaos.Find(Guid.Parse(guia)).numeroGuia;
            }
            
            var expedidos = db.produtosExpedidos;
            List<produtosExpedido> listaprodutosexpedidos = new List<produtosExpedido>();
            var index = 0;
            var totalunidades = 0;
          
            while (expedidos.Count() > index)
            {
                if (expedidos.ToList()[index].numExpedicao != null &&
                    expedidos.ToList()[index].numExpedicao.Equals(guia))
                {
                    listaprodutosexpedidos.Add(expedidos.ToList()[index]);
                }
                index++;
            }
            return View(listaprodutosexpedidos);
        }

        // GET: produto_new/Delete/5
        public ActionResult DeleteProdutos(int? id, String obs)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            produtosExpedido expedido = db.produtosExpedidos.Find(id);
            if (expedido == null)
            {
                return HttpNotFound();
            }

            var lote = expedido.lote ;
            var gtin = expedido.gtinRef;
            var numguia = expedido.numExpedicao;

            var movimentos = db.saidasEntradas;
            for (int i = 0; i < movimentos.Count(); i++) {
                var mov = movimentos.ToList()[i];
                if (mov.codcomercial == gtin && mov.lote == lote && mov.numGuiaExpedicao == numguia) {
                    db.saidasEntradas.Remove(mov);
                }
            }
            db.produtosExpedidos.Remove(expedido);
            db.SaveChanges();

            return RedirectToAction("listaprodutosPai", new { obs = expedido.numExpedicao });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSubProduto([Bind(Include = "id,numeroGuia,num_expedicao,dataExpedicao,id_camposControl,camposControl_id,matricula,cmr")] Expedicao expedicao){
            saidasEntrada movimento = new saidasEntrada();
            produtosExpedido expedido = new produtosExpedido();

            
            expedido.gtinRef = Request.Form["gtinRef"];
            expedido.codcomercial = Request.Form["gtinRef"];
            expedido.descricao = Request.Form["descricao"];
            expedido.qtd = Convert.ToInt32(Request.Form["qtd"]);
            expedido.lote = Request.Form["lote"];
            expedido.encomenda = Request.Form["encomenda"];
            movimento.gtinRef = Request.Form["gtinRef"];
            movimento.descricao = Request.Form["descricao"];
            movimento.lote = Request.Form["lote"];
            movimento.qtd = Convert.ToInt32(Request.Form["qtd"]);
            movimento.numGuiaExpedicao = Request.Form["guia"];
            movimento.codcomercial = Request.Form["gtinRef"];
            expedicao.numeroGuia = Request.Form["guia"];

            if (Request.Form["encomenda"] != null && Request.Form["encomenda"] != "")
            {
                expedido.encomenda = Request.Form["encomenda"];
            }
            expedido.numExpedicao = Request.Form["guia"];
            
            db.produtosExpedidos.Add(expedido);
            db.saidasEntradas.Add(movimento);

            db.SaveChanges();
            
            ViewBag.numeroGuia = Request.Form["numeroguia"];
            ViewBag.guia = Request.Form["guia"];
            var listaprodutosdesc = new SelectList(db.produtoes, "codcomercial", "descricao");
            var listaeandesc = new SelectList(db.produtoes, "ean13_produto", "descricao");
            ViewBag.id_produtosDescricao = listaprodutosdesc;
            ViewBag.id_produtosCodComercial = new SelectList(db.produto_new, "codcomercial", "ref");
            ViewBag.id_qtd = new SelectList(db.produto_new, "id", "NrUnidades");
            ViewBag.id_lote = new SelectList(db.produto_new, "id", "lote");
            ViewBag.id_validade = new SelectList(db.produto_new, "id", "validade");
            ViewBag.id_encomenda = new SelectList(db.produto_new, "id", "id_numeroEncomenda");
            ViewBag.id_produtosDesc = new SelectList(db.produtoes, "codcomercial", "descricao");
            ViewBag.id_produtos = new SelectList(db.produtoes, "id", "codcomercial");
            ViewBag.id_produtosGtinFabricado = new SelectList(db.gtinFabricados, "id", "gtinFabricado1");
            ViewBag.ofQtdProduzida1 = new SelectList(db.gtinFabricados, "id", "qtdFabricada");
            ViewBag.id_refGtinFabricado = new SelectList(db.gtinFabricados, "id", "produto_id");
            ViewBag.id_qtdFabricado = new SelectList(db.OrdemFabricoes, "id", "qtdFabricado");
            ViewBag.id_loteFabricado = new SelectList(db.gtinFabricados, "id", "loteProduzir");
            ViewBag.id_validadeFabricado = new SelectList(db.OrdemFabricoes, "id", "validade");
            ViewBag.id_encomendaFabricado = new SelectList(db.gtinFabricados, "id", "encomenda_id");
            ViewBag.id_OfGtinFabricados = new SelectList(db.gtinFabricados, "id", "ordemFabrico_id");

            ViewBag.id_produtosGtinRecebido = new SelectList(db.saidasEntradas, "id", "codcomercial");
            ViewBag.id_refGtinRecebido = new SelectList(db.saidasEntradas, "id", "gtinRef");
            ViewBag.id_qtdGtinRecebido = new SelectList(db.saidasEntradas, "id", "qtd");
            ViewBag.id_loteGtinRecebido = new SelectList(db.saidasEntradas, "id", "lote");
            ViewBag.id_validadeGtinRecebido = new SelectList(db.saidasEntradas, "id", "validade");
            ViewBag.id_encomendaGtinRecebido = new SelectList(db.saidasEntradas, "id", "gtinRef");
            ViewBag.id_descricaoGtinRecebido = new SelectList(db.saidasEntradas, "id", "descricao");

            ViewBag.ofValidade = new SelectList(db.OrdemFabricoes, "id", "validade");
            ViewBag.ofQtdProduzida = new SelectList(db.OrdemFabricoes, "id", "qtdFabricado");
            ViewBag.ofQtdProduzida1 = new SelectList(db.gtinFabricados, "id", "qtdFabricada");
            ViewBag.id_OfGtinFabricadoSSCC = new SelectList(db.gtinFabricados, "id", "gtinFabricado1");
            ViewBag.ofLOTE = new SelectList(db.OrdemFabricoes, "id", "LOTE");
            ViewBag.ofProduto = new SelectList(db.OrdemFabricoes, "id", "produto_id");
            ViewBag.id_OfGtinFabricado = new SelectList(db.gtinFabricados, "id", "ordemFabrico_id");        //ofFabricado	
            ViewBag.id_qtdFabricado = new SelectList(db.OrdemFabricoes, "id", "qtdFabricado");              //qtdFabricado
            ViewBag.id_loteFabricado = new SelectList(db.gtinFabricados, "id", "loteProduzir");             //loteFabricado
            ViewBag.id_validadeFabricado = new SelectList(db.OrdemFabricoes, "id", "validade");             //validadeFabricado
            ViewBag.id_encomendaFabricado = new SelectList(db.gtinFabricados, "id", "encomenda_id");        //encomendaFabricado
            ViewBag.id_refGtinFabricado = new SelectList(db.OrdemFabricoes, "id", "produto_id");            //refGtinFabricado
            ViewBag.id_OfGtinFabricadoSSCC = new SelectList(db.gtinFabricados, "id", "gtinFabricado1");		//gtinfabricado
            ViewBag.encomendaExterna = new SelectList(db.Encomendas, "id", "encomendaExterna");		//numeroEncomendaExterna
            //@Html.DropDownList("encomendaExterna", null, "--Select One--", htmlAttributes: new { @class = "hidden", @id = "encomendaExterna", name = "encomendaExterna" })
            return View();

        }
        // Adicionar produto na Recepção de Mercadorias
        public ActionResult AddSubProduto(string obs)
        {
            Boolean encontrou = false;
            
            ViewBag.guia = saveExpedicao(obs);
          
            if (obs != null && obs != "")
            {
                var listaprodutosdesc = new SelectList(db.produtoes, "codcomercial", "descricao");
                var listaeandesc  =  new SelectList(db.produtoes, "ean13_produto", "descricao");
                ViewBag.id_produtosDescricao = listaprodutosdesc;
                ViewBag.id_produtosCodComercial = new SelectList(db.produto_new, "codcomercial", "ref");
                ViewBag.id_qtd = new SelectList(db.produto_new, "id", "NrUnidades");
                ViewBag.id_lote = new SelectList(db.produto_new, "id", "lote");
                ViewBag.id_validade = new SelectList(db.produto_new, "id", "validade");
                ViewBag.id_encomenda = new SelectList(db.produto_new, "id", "id_numeroEncomenda");
                ViewBag.id_produtosDesc = new SelectList(db.produtoes, "codcomercial", "descricao");
                ViewBag.id_produtos = new SelectList(db.produtoes, "id", "codcomercial");
                ViewBag.id_produtosGtinFabricado = new SelectList(db.gtinFabricados, "id", "gtinFabricado1");
                ViewBag.id_refGtinFabricado = new SelectList(db.gtinFabricados, "id", "produto_id");
                ViewBag.id_qtdFabricado = new SelectList(db.OrdemFabricoes, "id", "qtdFabricado");
                ViewBag.id_loteFabricado = new SelectList(db.gtinFabricados, "id", "loteProduzir");
                ViewBag.id_validadeFabricado = new SelectList(db.OrdemFabricoes, "id", "validade");
                ViewBag.id_encomendaFabricado = new SelectList(db.gtinFabricados, "id", "encomenda_id");
                
                //ViewBag.id_refGtinRecebido = new SelectList(db.gtinRecebidos, "id", "ref");
                ViewBag.id_OfGtinFabricados = new SelectList(db.gtinFabricados, "id", "ordemFabrico_id");


                ViewBag.id_produtosGtinRecebido = new SelectList(db.saidasEntradas, "id", "codcomercial");
                ViewBag.id_refGtinRecebido = new SelectList(db.saidasEntradas, "id", "gtinRef");
                ViewBag.id_qtdGtinRecebido = new SelectList(db.saidasEntradas, "id", "qtd");
                ViewBag.id_loteGtinRecebido = new SelectList(db.saidasEntradas, "id", "lote");
                ViewBag.id_validadeGtinRecebido = new SelectList(db.saidasEntradas, "id", "validade");
                ViewBag.id_encomendaGtinRecebido = new SelectList(db.saidasEntradas, "id", "gtinRef");
                ViewBag.id_descricaoGtinRecebido = new SelectList(db.saidasEntradas, "id", "descricao");
                /*
                ViewBag.id_produtosGtinRecebido = new SelectList(db.gtinRecebidos, "id", "numGtin"); 
                ViewBag.id_qtdGtinRecebido = new SelectList(db.gtinRecebidos, "id", "numUnidades");
                ViewBag.id_loteGtinRecebido = new SelectList(db.gtinRecebidos, "id", "Lote");
                ViewBag.id_validadeGtinRecebido = new SelectList(db.gtinRecebidos, "id", "validade");
                ViewBag.id_encomendaGtinRecebido = new SelectList(db.gtinRecebidos, "id", "numGtin");
                ViewBag.id_descricaoGtinRecebido = new SelectList(db.gtinRecebidos, "id", "descricao");*/

                ViewBag.ofValidade = new SelectList(db.OrdemFabricoes, "id", "validade");
                ViewBag.ofQtdProduzida = new SelectList(db.OrdemFabricoes, "id", "qtdFabricado");
                ViewBag.ofQtdProduzida1 = new SelectList(db.gtinFabricados, "id", "qtdFabricada");
                ViewBag.id_OfGtinFabricadoSSCC = new SelectList(db.gtinFabricados, "id", "gtinFabricado1");
                //LOTE
                ViewBag.ofLOTE = new SelectList(db.OrdemFabricoes, "id", "LOTE");
                //produto_id
                ViewBag.ofProduto = new SelectList(db.OrdemFabricoes, "id", "produto_id");


                ViewBag.id_OfGtinFabricado = new SelectList(db.gtinFabricados, "id", "ordemFabrico_id");        //ofFabricado	
                ViewBag.id_qtdFabricado = new SelectList(db.OrdemFabricoes, "id", "qtdFabricado");              //qtdFabricado
                ViewBag.id_loteFabricado = new SelectList(db.gtinFabricados, "id", "loteProduzir");             //loteFabricado
                ViewBag.id_validadeFabricado = new SelectList(db.OrdemFabricoes, "id", "validade");             //validadeFabricado
                ViewBag.id_encomendaFabricado = new SelectList(db.gtinFabricados, "id", "encomenda_id");        //encomendaFabricado
                ViewBag.id_refGtinFabricado = new SelectList(db.OrdemFabricoes, "id", "produto_id");            //refGtinFabricado
                ViewBag.id_OfGtinFabricadoSSCC = new SelectList(db.gtinFabricados, "id", "gtinFabricado1");		//gtinfabricado
                ViewBag.encomendaExterna = new SelectList(db.Encomendas, "id", "encomendaExterna");		//numeroEncomendaExterna


                return View();
            }
            else
                return View("InvalidAcess");
        }


        // Adicionar produto na Recepção de Mercadorias
        public ActionResult EditSubProduto(string obs)
        {
            Boolean encontrou = false;
           // ViewBag.guia = obs.Split(';')[0];
            ViewBag.numeroGuia = obs.Split(';')[0];

            if (obs != null && obs != "")
            {
                //ler movimentos gravados.
                var movimentosGravados = db.produtosExpedidos.ToList();
                List<produtosExpedido> movimentosExp = new List<produtosExpedido>();

                for (int i = 0; i < movimentosGravados.Count(); i++) {
                    if (ViewBag.numeroRecep == movimentosGravados[i].numExpedicao) {
                        movimentosExp.Add(movimentosGravados[i]);
                    }
                }
                ViewBag.dadosExistentes = movimentosExp;

                var listaprodutosdesc = new SelectList(db.produtoes, "codcomercial", "descricao");
                ViewBag.id_produtosDescricao = listaprodutosdesc;
                ViewBag.id_produtosCodComercial = new SelectList(db.produto_new, "codcomercial", "ref");
                ViewBag.id_qtd = new SelectList(db.produto_new, "id", "NrUnidades");
                ViewBag.id_lote = new SelectList(db.produto_new, "id", "lote");
                ViewBag.id_validade = new SelectList(db.produto_new, "id", "validade");
                ViewBag.id_encomenda = new SelectList(db.produto_new, "id", "id_numeroEncomenda");
                ViewBag.id_produtosDesc = new SelectList(db.produtoes, "codcomercial", "descricao");
                ViewBag.id_produtos = new SelectList(db.produtoes, "id", "codcomercial");
                ViewBag.id_produtosGtinFabricado = new SelectList(db.gtinFabricados, "id", "gtinFabricado1");
                ViewBag.id_refGtinFabricado = new SelectList(db.gtinFabricados, "id", "produto_id");
                ViewBag.id_qtdFabricado = new SelectList(db.OrdemFabricoes, "id", "qtdFabricado");
                ViewBag.id_loteFabricado = new SelectList(db.gtinFabricados, "id", "loteProduzir");
                ViewBag.id_validadeFabricado = new SelectList(db.OrdemFabricoes, "id", "validade");
                ViewBag.id_encomendaFabricado = new SelectList(db.gtinFabricados, "id", "encomenda_id");
                
                ViewBag.id_OfGtinFabricado = new SelectList(db.gtinFabricados, "id", "ordemFabrico_id");
                ViewBag.id_produtosGtinRecebido = new SelectList(db.saidasEntradas, "id", "codcomercial");

                ViewBag.id_refGtinRecebido = new SelectList(db.saidasEntradas, "id", "gtinRef");
                ViewBag.id_qtdGtinRecebido = new SelectList(db.saidasEntradas, "id", "qtd");
                ViewBag.id_loteGtinRecebido = new SelectList(db.saidasEntradas, "id", "lote");
                ViewBag.id_validadeGtinRecebido = new SelectList(db.saidasEntradas, "id", "validade");
                ViewBag.id_encomendaGtinRecebido = new SelectList(db.saidasEntradas, "id", "gtinRef");
                ViewBag.id_descricaoGtinRecebido = new SelectList(db.saidasEntradas, "id", "descricao");

                /*ViewBag.id_refGtinRecebido = new SelectList(db.gtinRecebidos, "id", "ref");
                 ViewBag.id_produtosGtinRecebido = new SelectList(db.gtinRecebidos, "id", "numGtin");
                ViewBag.id_qtdGtinRecebido = new SelectList(db.gtinRecebidos, "id", "numUnidades");
                ViewBag.id_loteGtinRecebido = new SelectList(db.gtinRecebidos, "id", "Lote");
                ViewBag.id_validadeGtinRecebido = new SelectList(db.gtinRecebidos, "id", "validade");
                ViewBag.id_encomendaGtinRecebido = new SelectList(db.gtinRecebidos, "id", "numGtin");
                ViewBag.id_descricaoGtinRecebido = new SelectList(db.gtinRecebidos, "id", "descricao");*/
                ViewBag.ofValidade = new SelectList(db.OrdemFabricoes, "id", "validade");
                ViewBag.ofQtdProduzida = new SelectList(db.OrdemFabricoes, "id", "qtdFabricado");
                ViewBag.ofQtdProduzida1 = new SelectList(db.gtinFabricados, "id", "qtdFabricada");
                //LOTE
                ViewBag.ofLOTE = new SelectList(db.OrdemFabricoes, "id", "LOTE");
                //produto_id
                ViewBag.ofProduto = new SelectList(db.OrdemFabricoes, "id", "produto_id");

                ViewBag.id_OfGtinFabricado = new SelectList(db.gtinFabricados, "id", "ordemFabrico_id");        //ofFabricado	
                ViewBag.id_qtdFabricado = new SelectList(db.OrdemFabricoes, "id", "qtdFabricado");              //qtdFabricado
                ViewBag.id_loteFabricado = new SelectList(db.gtinFabricados, "id", "loteProduzir");             //loteFabricado
                ViewBag.id_validadeFabricado = new SelectList(db.OrdemFabricoes, "id", "validade");             //validadeFabricado
                ViewBag.id_encomendaFabricado = new SelectList(db.gtinFabricados, "id", "encomenda_id");        //encomendaFabricado
                ViewBag.id_refGtinFabricado = new SelectList(db.OrdemFabricoes, "id", "produto_id");            //refGtinFabricado
                ViewBag.id_OfGtinFabricadoSSCC = new SelectList(db.gtinFabricados, "id", "gtinFabricado1");		//gtinfabricado
                ViewBag.encomendaExterna = new SelectList(db.Encomendas, "id", "encomendaExterna");		//numeroEncomendaExterna

                return View(movimentosExp);
            }
            else
                return View("InvalidAcess");
        }

        private String getRefDescri(String gtin) {
            var listaCodComercialProdutoNew = new SelectList(db.produto_new, "codcomercial", "ref");
            var refpai = listaCodComercialProdutoNew.ToList().Find(a => a.Value == gtin);
            var referenciapai = "";
            if (refpai != null)
                referenciapai = refpai.Text;

            listaCodComercialProdutoNew = new SelectList(db.produto_new, "codcomercial", "descricao");
            //int theThingIActuallyAmInterestedIn = listaCodComercialDescricao.IndexOf(listaCodComercialDescricao.Single(i => i.Text == gtin));
            var index = listaCodComercialProdutoNew.ToList().Find(a => a.Value == gtin);
            var value = "";
            if (index != null)
                value = index.Text;

            var listaCodComercialDescricao = new SelectList(db.produtoes, "codcomercial", "descricao");
            //int theThingIActuallyAmInterestedIn = listaCodComercialDescricao.IndexOf(listaCodComercialDescricao.Single(i => i.Text == gtin));
            index = listaCodComercialDescricao.ToList().Find(a => a.Value == gtin);
            if (index != null && value == null)
                value = index.Text;


            var listaIdCodComercial = new SelectList(db.produtoes, "id", "codcomercial");
            index = listaIdCodComercial.ToList().Find(a => a.Value == gtin);
            if (index != null && value == null)
                value = index.Text;

            var listaGtinRecebido = new SelectList(db.gtinFabricados, "id", "gtinrecebido_id");
            var gtinRecebido = listaGtinRecebido.ToList().Find(a => a.Value == gtin);
            if (gtinRecebido != null)
                index = listaGtinRecebido.ToList().Find(a => a.Value == gtinRecebido.Text);
            if (index != null && value == null)
                value = index.Text;

            var listaIdProduto = new SelectList(db.gtinFabricados, "id", "produto_id");
            var produtoId = listaIdProduto.ToList().Find(a => a.Value == gtin);
            if (produtoId != null)
                index = listaCodComercialDescricao.ToList().Find(a => a.Value == produtoId.Text);
            if (index != null && value == null)
                value = index.Text;


            var listaIdProdutoRecebido = new SelectList(db.gtinRecebidos, "numGtin", "ref");
            produtoId = listaIdProduto.ToList().Find(a => a.Text == gtin);
            if (produtoId != null) {
                index = listaIdProdutoRecebido.ToList().Find(a => a.Value == produtoId.Text);
                listaIdCodComercial = new SelectList(db.produtoes, "id", "codcomercial");
                if (referenciapai == null)
                    referenciapai = listaIdCodComercial.ToList().Find(a => a.Value == gtin).Value;
            }
            listaIdProdutoRecebido = new SelectList(db.gtinRecebidos, "numGtin", "descricao");
            produtoId = listaIdProduto.ToList().Find(a => a.Text == gtin);
            if (produtoId != null && value == null)
                value = index.Text;

            return referenciapai + ";" + value;
        }
        // Guardar produto adicionado na Recepção de Mercadorias
        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None)]
        public Boolean MovimentoProdutoSaida()
        {
            /*
                refproduto; refPai; descricao; qtd; lote; validade; encomenda
			*/
            var dataExpedicao = Request.Form["dataExpedicao"];

            var comercial = Request.Form["codcProd"];
            //morada cliente
            var morada = Request.Form["morada"];
            //matricula
            var matricula = Request.Form["matricula"];
            //cmr
            var cmr = Request.Form["cmr"];
            //numero de guia
            var numeroguia = Request.Form["numeroguia"];
            var gtinRef = Request.Form["gtinRef"];
            var qtd = Request.Form["qtd"];
            var lote = Request.Form["lote"];
            var validade_prod = Request.Form["validade_prod"];

            var valor = Request.Form["refproduto"];
            var RefProduto = Request.Form["codcomercial"];
            var RefProdDesc = Request.Form["descricao"];
            var prodLote = Request.Form["lote"];
            var unidades = Request.Form["qtd"];
            var prodValidade = Request.Form["Validade"];
            var codComercial = Request.Form["codcomercial"];
            var encomenda = Request.Form["encomenda"];

            saidasEntrada movimento = new saidasEntrada();
            produtosExpedido expedido = new produtosExpedido();

            var resultadoDesc = getRefDescri(gtinRef);
            if (resultadoDesc != null) {
                expedido.gtinRef = resultadoDesc.Split(';')[0];
                expedido.descricao = resultadoDesc.Split(';')[1];
                movimento.gtinRef = resultadoDesc.Split(';')[0];
                movimento.descricao = resultadoDesc.Split(';')[1];
            }


            expedido.codcomercial = gtinRef;
            movimento.codcomercial = gtinRef;
            expedido.lote = Request.Form["lote"];
            movimento.lote = Request.Form["lote"];
            expedido.qtd = Convert.ToInt32(Request.Form["qtd"]);
            movimento.qtd = Convert.ToInt32(Request.Form["qtd"]);
            if (validade_prod != null && validade_prod != "")
            {
                expedido.validade = Convert.ToDateTime(validade_prod);
                movimento.validade = Convert.ToDateTime(validade_prod);
            }
            if (Request.Form["encomenda"] != null && Request.Form["encomenda"] != "")
            {
                movimento.numGuiaExpedicao = Request.Form["encomenda"];
                expedido.encomenda = Request.Form["encomenda"];
            }

            expedido.numExpedicao = Request.Form["numeroguia"];


            db.produtosExpedidos.Add(expedido);
            db.saidasEntradas.Add(movimento);

            var index = 0;

            for (var i = 0; i < Request.Form.Count; i++)
            {
                var teste = Request.Form["refproduto4"];
                if (Request.Form["refproduto" + i] != null && Request.Form["refproduto" + i].Count() != 0)
                {
                    saidasEntrada movimentos = new saidasEntrada();

                    produtosExpedido produtosExpedido = new produtosExpedido();
                    produtosExpedido.codcomercial = Request.Form["refproduto" + i];
                    movimentos.codcomercial = Request.Form["refproduto" + i];

                    resultadoDesc = getRefDescri(movimentos.gtinRef);
                    if (resultadoDesc != null)
                    {
                        produtosExpedido.gtinRef = resultadoDesc.Split(';')[0];
                        produtosExpedido.descricao = resultadoDesc.Split(';')[1];
                        movimentos.gtinRef = resultadoDesc.Split(';')[0];
                        movimentos.descricao = resultadoDesc.Split(';')[1];
                    }

                    produtosExpedido.codcomercial = Request.Form["codcomercial" + i];
                    movimentos.codcomercial = Request.Form["codcomercial" + i];
                    produtosExpedido.lote = Request.Form["lote" + i];
                    movimentos.lote = Request.Form["lote" + i];
                    produtosExpedido.qtd = Convert.ToInt32(Request.Form["qtd" + i]);
                    if (Request.Form["Validade" + i] != null && Request.Form["Validade" + i] != "") {
                        produtosExpedido.validade = Convert.ToDateTime(Request.Form["Validade" + i]);
                        movimentos.validade = Convert.ToDateTime(Request.Form["Validade" + i]);
                    }
                    if (Request.Form["encomenda" + i] != null && Request.Form["encomenda" + i] != "")
                    {
                        movimentos.numGuiaExpedicao = Request.Form["encomenda" + i];
                        produtosExpedido.encomenda = Request.Form["encomenda" + i];
                    }

                    produtosExpedido.numExpedicao = Request.Form["numeroguia"];


                    db.produtosExpedidos.Add(produtosExpedido);

                    db.saidasEntradas.Add(movimentos);

                    return true;
                }
            }
            //return RedirectToAction("ExpedicaoMercadoria", new { obs = Request.Form["numeroguia"] });
            return false;
        }

        private Boolean movimentoExist(string codcomercial, string numExpedicao) {
            var listaMovimentos = new SelectList(db.produtosExpedidos, "codcomercial", "numExpedicao");
            for (int i = 0; i < listaMovimentos.Count(); i++) {
                var listaCodigos = listaMovimentos.ToList().Find(a => a.Text == codcomercial);
                if (listaCodigos != null)
                {
                    var index = listaMovimentos.ToList().Find(a => a.Value == listaCodigos.Text);
                    if (listaCodigos.Value == numExpedicao)
                        return true;
                }
            }
            return false;
        }

        public Boolean MovimentoProdutoSaidaEdit()
        {
            var dataExpedicao = Request.Form["dataExpedicao"];
            var comercial = Request.Form["codcProd"];
            var morada = Request.Form["morada"];
            var matricula = Request.Form["matricula"];
            var cmr = Request.Form["cmr"];

            var numeroguia = Request.Form["numeroguia"];
            //valores já existentes na BD
            //var gtinRef = Request.Form["item.codcomercial"];
            //var qtd = Request.Form["item.qtd"];
            //var lote = Request.Form["item.lote"];
            //var encomenda = Request.Form["item.encomenda"];

            /*deteleRegistos();
            //inserir : 
            for (var indexExistentes = 0; indexExistentes < gtinRef.Count(); indexExistentes++) {
                //if (gtinRef[indexExistentes] != null) {
                saidasEntrada updateMovimento = new saidasEntrada();
                produtosExpedido updateExpedido = new produtosExpedido();
                var UpdateresultadoDesc = getRefDescri(gtinRef[indexExistentes].ToString());

                if (updateExpedido != null){
                    updateExpedido.gtinRef = gtinRef[indexExistentes].ToString();
                    updateExpedido.descricao = UpdateresultadoDesc.Split(';')[1];
                    updateMovimento.gtinRef = gtinRef[indexExistentes].ToString();
                    updateMovimento.descricao = UpdateresultadoDesc.Split(';')[1];
                }

                updateExpedido.codcomercial = gtinRef[indexExistentes].ToString();
                updateMovimento.codcomercial = gtinRef[indexExistentes].ToString();
                updateExpedido.lote = lote[indexExistentes].ToString();
                updateMovimento.lote = lote[indexExistentes].ToString();
                updateExpedido.qtd = Convert.ToInt32(qtd[indexExistentes]);
                updateMovimento.qtd = Convert.ToInt32(qtd[indexExistentes]);
                updateExpedido.numExpedicao = Request.Form["numeroguia"];
                updateMovimento.numGuiaExpedicao = Request.Form["numeroguia"];
                updateExpedido.encomenda = encomenda[indexExistentes].ToString();
                db.produtosExpedidos.Add(updateExpedido);
                db.saidasEntradas.Add(updateMovimento);
                
            }
            
            //segunda linha adicionado descricao + codcomercial
            var descRef = Request.Form["descRef"];
            var codProdNovo = Request.Form["codcomercial"];
            //GTIN nova
            var gtinNovo = Request.Form["gtinRef_novo"];
            //qtd nova
            var qtdNovo = Request.Form["qtd"];
            //lote nova
            var loteNovo = Request.Form["lote"];
            //validade nova
            var validadeNova = Request.Form["validade_prod"];
            //encomenda nova
            var encomendaNova = Request.Form["encomenda"];

            //inserir : 
            for (var indexNovos = 0; indexNovos < gtinNovo.Count(); indexNovos++)
            {
                saidasEntrada updateMovimento = new saidasEntrada();
                produtosExpedido updateExpedido = new produtosExpedido();
                var UpdateresultadoDesc = getRefDescri(gtinNovo[indexNovos].ToString());

                if (updateExpedido != null)
                {
                    updateExpedido.gtinRef = gtinNovo[indexNovos].ToString();
                    updateExpedido.descricao = UpdateresultadoDesc.Split(';')[1];
                    updateMovimento.gtinRef = gtinRef;
                    updateMovimento.descricao = UpdateresultadoDesc.Split(';')[1];
                }

                updateExpedido.codcomercial = gtinNovo[indexNovos].ToString();
                updateMovimento.codcomercial = gtinNovo[indexNovos].ToString();
                updateExpedido.lote = loteNovo[indexNovos].ToString();
                updateMovimento.lote = loteNovo[indexNovos].ToString();
                updateExpedido.qtd = Convert.ToInt32(qtdNovo[indexNovos]);
                updateMovimento.qtd = Convert.ToInt32(qtdNovo[indexNovos]);
                updateExpedido.numExpedicao = Request.Form["numeroguia"];
                updateMovimento.numGuiaExpedicao = Request.Form["numeroguia"];
                updateExpedido.encomenda = encomendaNova[indexNovos].ToString();
            }*/
            return false;
        }

        private Boolean deteleRegistos() {
            var listaMovimentos = new SelectList(db.produtosExpedidos, "id", "numExpedicao");
            var listasaidas = new SelectList(db.saidasEntradas, "id", "numGuiaExpedicao");
           
            for (int i = 0; i < listaMovimentos.Count(); i++)
            {
                var listaCodigos = listaMovimentos.ToList().Find(a => a.Text == Request.Form["numeroguia"]);
                if (listaCodigos != null)
                {
                   var elimina= db.produtosExpedidos.Find(Convert.ToInt32(listaCodigos.Value));
                   if (elimina != null)
                   {
                        db.produtosExpedidos.Remove(elimina);
                        db.SaveChanges();
                   }
                }
            }
            for (int i = 0; i < listasaidas.Count(); i++)
            {
                var listaCodigos = listasaidas.ToList().Find(a => a.Text == Request.Form["numeroguia"]);
                if (listaCodigos != null)
                {
                    var elimina = db.produtosExpedidos.Find(Convert.ToInt32(listaCodigos.Value));
                    if (elimina != null)
                    {
                        db.produtosExpedidos.Remove(elimina);
                        db.SaveChanges();
                    }
                }
            }
            return false;
        }

    }
}
