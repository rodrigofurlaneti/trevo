using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class TabelaPrecoMensalistaController : GenericController<TabelaPrecoMensalista>
    {
        private readonly IEquipeAplicacao _equipeAplicacao;
        private readonly IUnidadeAplicacao _unidadeAplicacao;
        private readonly ITipoEquipeAplicacao _tipoEquipeAplicacao;
        private readonly ITabelaPrecoMensalistaUnidadeAplicacao _TabelaPrecoMensalistaUnidadeAplicacao;
        private readonly ITipoNotificacaoAplicacao _tipoNotificacaoAplicacao;
        private readonly IUsuarioAplicacao _usuarioAplicacao;
        private readonly ITabelaPrecoMensalistaAplicacao _TabelaPrecoMensalistaAplicacao;

        public List<EquipeViewModel> ListaEquipe;
        public List<TipoEquipeViewModel> ListaTipoEquipe;

        public TabelaPrecoMensalistaController(ITabelaPrecoMensalistaAplicacao TabelaPrecoMensalistaAplicacao,
                                         IUnidadeAplicacao unidadeAplicacao,
                                         ITipoEquipeAplicacao tipoEquipeAplicacao,
                                         IEquipeAplicacao equipeAplicacao,
                                         ITabelaPrecoMensalistaUnidadeAplicacao TabelaPrecoMensalistaUnidadeAplicacao,
                                         ITipoNotificacaoAplicacao tipoNotificacaoAplicacao,
                                         IUsuarioAplicacao usuarioAplicacao)
        {
            _unidadeAplicacao = unidadeAplicacao;
            _tipoEquipeAplicacao = tipoEquipeAplicacao;
            Aplicacao = TabelaPrecoMensalistaAplicacao;
            _equipeAplicacao = equipeAplicacao;
            _TabelaPrecoMensalistaUnidadeAplicacao = TabelaPrecoMensalistaUnidadeAplicacao;
            _tipoNotificacaoAplicacao = tipoNotificacaoAplicacao;
            _usuarioAplicacao = usuarioAplicacao;
            ListaEquipe = new List<EquipeViewModel>();
            ListaTipoEquipe = new List<TipoEquipeViewModel>();
            _TabelaPrecoMensalistaAplicacao = TabelaPrecoMensalistaAplicacao;
        }

        public List<UnidadeViewModel> ListaUnidade => _unidadeAplicacao.ListarOrdenadas();
        public List<TabelaPrecoMensalistaViewModel> ListaTabelaPrecoMensalista => Aplicacao?.Buscar()?.Select(x => new TabelaPrecoMensalistaViewModel(x)).ToList() ?? new List<TabelaPrecoMensalistaViewModel>();

        // GET: TabelaPrecoMensalista
        public override ActionResult Index()
        {
            return View();
        }

        [CheckSessionOut]
        [HttpPost]
        public JsonResult SalvarDados(TabelaPrecoMensalistaViewModel model)
        {
            try
            {
                var items = !string.IsNullOrEmpty(Session["Itens"]?.ToString()) ? (List<TabelaPrecoMensalistaUnidadeViewModel>)Session["Itens"] : new List<TabelaPrecoMensalistaUnidadeViewModel>(); ;

                if(items == null || items.Count <= 0)
                {
                    CriarDadosModalAviso("Nenhum item foi adicionado");
                }

                if (model.TabelaPrecoUnidade != null && model.TabelaPrecoUnidade.Count > 0)
                    model.TabelaPrecoUnidade.Clear();
                else
                    model.TabelaPrecoUnidade = new List<TabelaPrecoMensalistaUnidadeViewModel>();

                foreach (var item in items)
                {
                    model.TabelaPrecoUnidade.Add(item);
                }

                var entity = Aplicacao.BuscarPorId(model.Id) ?? model.ToEntity();

                var usuarioLogado = HttpContext.User as CustomPrincipal;

                var valor = Convert.ToDecimal(model.Valor.Replace(".", ""));

                entity.Valor = valor;
                entity.Nome = model.Nome;
                entity.DiasCalculo = model.DiasCalculo;
                entity.TabelaPrecoUnidade = model.TabelaPrecoUnidade.Select(x => x.ToEntity()).ToList() ?? new List<TabelaPrecoMensalistaUnidade>();

                _TabelaPrecoMensalistaAplicacao.Salvar(entity, usuarioLogado.UsuarioId);

                ModelState.Clear();

                CriarDadosModalSucesso("Registro Salvo com Sucesso");
            }
            catch (SoftparkIntegrationException sf)
            {
                CriarModalAvisoComRetornoParaIndex(sf.Message);
            }
            catch (BusinessRuleException br)
            {
                CriarDadosModalAviso(br.Message);
            }
            catch (Exception ex)
            {
                CriarDadosModalErro(ex.Message);
            }

            return Json(DadosModal);
        }

        public override ActionResult Edit(int id)
        {
            var cliente = new TabelaPrecoMensalistaViewModel(Aplicacao.BuscarPorId(id));

            Session["Itens"] = cliente.TabelaPrecoUnidade;

            return View("Index", cliente);
        }

        [HttpPost]
        public ActionResult AtualizaEquipamentos(int Id)
        {

            var cliente = new TabelaPrecoMensalistaViewModel(Aplicacao.BuscarPorId(Id));

            return PartialView("_GridTabelaPrecoMensalistaUnidade", cliente.TabelaPrecoUnidade);
        }


        [CheckSessionOut]
        [HttpPost]
        public ActionResult DeletaEquipamento(int Index, List<TabelaPrecoMensalistaUnidadeViewModel> listahorarioparametro)
        {
            try
            {
                if (listahorarioparametro[Index].Id != 0)
                    _TabelaPrecoMensalistaUnidadeAplicacao.ExcluirPorId(listahorarioparametro[Index].Id);

                listahorarioparametro.RemoveAt(Index);

                ModelState.Clear();

                DadosModal = new DadosModal
                {
                    Titulo = "Sucesso",
                    Mensagem = "Registro excluído com sucesso",
                    TipoModal = TipoModal.Success
                };
            }
            catch (BusinessRuleException br)
            {
                DadosModal = new DadosModal
                {
                    Titulo = "Atenção",
                    Mensagem = br.Message,
                    TipoModal = TipoModal.Warning
                };
                return View("Index");
            }
            catch (Exception ex)
            {
                DadosModal = new DadosModal
                {
                    Titulo = "Atenção",
                    Mensagem = "Ocorreu um erro ao salvar: " + ex.Message,
                    TipoModal = TipoModal.Danger
                };
                return View("Index");
            }

            return PartialView("_GridTabelaPrecoMensalistaUnidade", listahorarioparametro);
        }

        [CheckSessionOut]
        public ActionResult Atualizaritems(List<TabelaPrecoMensalistaUnidadeViewModel> items)
        {
            Session["Itens"] = items;
            return PartialView("~/Views/TabelaPrecoMensalista/_GridVigencia.cshtml", items);
        }

        public JsonResult CarregarModelos()
        {
            var lista = new List<Unidade>
            {
                new Unidade()
                {
                    Nome = @"Selecione uma Unidade.",
                    Id = 0
                }
            };

            var busca = _unidadeAplicacao.Buscar().ToList();

            if (busca == null || busca.Count() <= 0) return Json(new SelectList(lista, "Id", "Nome"));

            lista.AddRange(busca);

            return Json(new SelectList(lista, "Id", "Nome", 0));
        }

        public JsonResult BuscarDadosDosGrids(int id)
        {
            var cliente = new TabelaPrecoMensalistaViewModel(Aplicacao.BuscarPorId(id));

            cliente.TabelaPrecoUnidade.ToList().ForEach(x =>
            {
                var unidade = new Unidade { Id = x.Unidade.Id, Nome = x.Unidade.Nome };
                x.Unidade = unidade;
            });

            Session["Itens"] = cliente.TabelaPrecoUnidade.ToList();

            return Json(
                new
                {
                    items = cliente?.TabelaPrecoUnidade,
                }
            );
        }
    }
}
