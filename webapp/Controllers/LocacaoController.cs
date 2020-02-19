using Aplicacao;
using Aplicacao.ViewModels;
using AutoMapper;
using Core.Exceptions;
using Core.Extensions;
using Entidade;
using Entidade.Uteis;
using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Portal.Controllers
{
    public class LocacaoController : GenericController<PedidoLocacao>
    {

        public List<PedidoLocacao> ListaPedidosLocacao { get; set; }

        public List<PedidoLocacaoViewModel> ListaPedidoLocacao => _pedidoLocacaoAplicacao.Buscar()?.Select(x => new PedidoLocacaoViewModel(x)).ToList();


        public PedidoLocacaoViewModel PedidoLocacaoFiltro
        {
            get { return (PedidoLocacaoViewModel)Session["PedidoLocacaoFiltro"] ?? new PedidoLocacaoViewModel(); }
            set { Session["PedidoLocacaoFiltro"] = value; }
        }

        private readonly IUnidadeAplicacao _unidadeAplicacao;
        public List<UnidadeViewModel> ListaUnidade => _unidadeAplicacao?.Buscar()?.Select(x => new UnidadeViewModel(x))?.ToList() ?? new List<UnidadeViewModel>();

        private readonly ITipoLocacaoAplicacao _tipoLocacaoAplicacao;
        public List<TipoLocacaoViewModel> ListaTipoLocacao => _tipoLocacaoAplicacao?.Buscar()?.Select(x => new TipoLocacaoViewModel(x))?.ToList() ?? new List<TipoLocacaoViewModel>();

        private readonly IPedidoLocacaoAplicacao _pedidoLocacaoAplicacao;

        public LocacaoController(IPedidoLocacaoAplicacao pedidoLocacaoAplicacao, 
                                 ITipoLocacaoAplicacao tipoLocacaoAplicacao,
                                 IUnidadeAplicacao unidadeAplicacao)
        {
            Aplicacao = pedidoLocacaoAplicacao;
            _pedidoLocacaoAplicacao = pedidoLocacaoAplicacao;
            _unidadeAplicacao = unidadeAplicacao;
            _tipoLocacaoAplicacao = tipoLocacaoAplicacao;

        }

        [CheckSessionOut]
        public override ActionResult Index()
        {
            ModelState.Clear();

            return View("Index");
        }


        public ActionResult Pesquisar(PedidoLocacaoViewModel filtro)
        {
            var listaEmisaoSeloVM = new List<PedidoLocacaoViewModel>();

            PedidoLocacaoFiltro = filtro;

            try
            {
                PedidoLocacaoFiltro = filtro;
                var listaEmisaoSelo = _pedidoLocacaoAplicacao.ListarPedidoLocacaoFiltro(PedidoLocacaoFiltro)?.ToList() ?? new List<PedidoLocacao>();
                listaEmisaoSeloVM = listaEmisaoSelo?.Select(x => new PedidoLocacaoViewModel(x))?.ToList() ?? new List<PedidoLocacaoViewModel>();
            }
            catch (Exception ex)
            {
                return Json(new Resultado<object>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Warning.ToDescription(),
                    Titulo = "Atenção",
                    Mensagem = "Ocorreu um erro ao pesquisar: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }

            return PartialView("_GridPedidosLote", listaEmisaoSeloVM);
        }

        [CheckSessionOut]
        public ActionResult ExecutarModal(int idsLancamentosCobranca)
        {
            try
            {
                var ObjView = new PedidoLocacaoViewModel(Aplicacao.BuscarPorId(idsLancamentosCobranca));

                Session["Itens"] = ObjView.PedidoLocacaoLancamentosAdicionais;


                

                return PartialView("_ModalValidadeLote", ObjView);
            }
            catch (Exception ex)
            {
                return Json(new Resultado<object>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Warning.ToDescription(),
                    Titulo = "Atenção",
                    Mensagem = "Ocorreu um erro ao salvar: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult ExecutarPagamento(int id)
        {
            var lancamentoContasPagar = new List<PedidoLocacaoViewModel>();

            try
            {
                ModelState.Clear();
                System.Threading.Thread.Sleep(1500);

            }
            catch (Exception ex)
            {
                return Json(new Resultado<object>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Warning.ToDescription(),
                    Titulo = "Atenção",
                    Mensagem = "Ocorreu um erro ao efetivar a alteração: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }

            return PartialView("_GridEmissoesLote", ListaPedidoLocacao);
        }

        [CheckSessionOut]
        public virtual ActionResult ConfirmarAlteracao(PedidoLocacaoViewModel model)
        {

            var listaEmisaoSeloVM = new List<PedidoLocacaoViewModel>();

            if(_pedidoLocacaoAplicacao.LiberarControles(model.Id))
            {
                return Json(new Resultado<PedidoLocacaoViewModel>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Warning.ToDescription(),
                    Titulo = "Salvar Alterações",
                    Mensagem = "Locação já aguarda por Aprovação em notificações!"
                });
            }

            try
            {
                var items = !string.IsNullOrEmpty(Session["Itens"]?.ToString()) ? (List<PedidoLocacaoLancamentoAdicionalViewModel>)Session["Itens"] : new List<PedidoLocacaoLancamentoAdicionalViewModel>(); ;

                if (items == null || items.Count <= 0)
                {
                    return Json(new Resultado<PedidoLocacaoViewModel>()
                    {
                        Sucesso = false,
                        TipoModal = TipoModal.Warning.ToDescription(),
                        Titulo = "Salvar Alterações",
                        Mensagem = "Nenhum item foi adicionado"
                    });
                }

                var usuarioLogado = HttpContext.User as CustomPrincipal;

                var obj = _pedidoLocacaoAplicacao.BuscarPorId(model.Id);

                obj.DataVigenciaFim = model.DataVigenciaFim;
                obj.DataReajuste = model.DataReajuste;
                obj.ValorReajuste = Convert.ToDecimal(model.ValorReajuste);

                var objVM = new PedidoLocacaoViewModel(obj);

                objVM.PedidoLocacaoLancamentosAdicionais = new List<PedidoLocacaoLancamentoAdicionalViewModel>();

                objVM.PedidoLocacaoLancamentosAdicionais = items;

                _pedidoLocacaoAplicacao.SalvarLocacao(objVM, usuarioLogado.UsuarioId);

                ModelState.Clear();

                //atualiza grid conforme filtro anterior
                var listaPedidoLocacao = _pedidoLocacaoAplicacao.ListarPedidoLocacaoFiltro(PedidoLocacaoFiltro)?.ToList() ?? new List<PedidoLocacao>();
                listaEmisaoSeloVM = listaPedidoLocacao?.Select(x => new PedidoLocacaoViewModel(x))?.ToList() ?? new List<PedidoLocacaoViewModel>();

            }
            catch (BusinessRuleException br)
            {
                return Json(new Resultado<object>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Warning.ToDescription(),
                    Titulo = "Atenção",
                    Mensagem = br.Message
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new Resultado<object>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Danger.ToDescription(),
                    Titulo = "Atenção",
                    Mensagem = "Ocorreu um erro: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new Resultado<object>()
            {
                Sucesso = false,
                TipoModal = TipoModal.Success.ToDescription(),
                Titulo = "Sucesso",
                Mensagem = "Solicitação realizada com sucesso!"
            }, JsonRequestBehavior.AllowGet);
        }


        //subitens
        [CheckSessionOut]
        public ActionResult Atualizaritems(List<PedidoLocacaoLancamentoAdicionalViewModel> items)
        {
            Session["Itens"] = items;
            return PartialView("~/Views/Locacao/_GridVigencia.cshtml", items);
        }

        
        public ActionResult InativarPedido(int id)
        {
            var listaEmisaoSeloVM = new List<PedidoLocacaoViewModel>();

            try
            {
                var objeto = Aplicacao.BuscarPorId(id);

                objeto.Ativo = false;

                Aplicacao.Salvar(objeto);

                var listaEmisaoSelo = _pedidoLocacaoAplicacao.ListarPedidoLocacaoFiltro(PedidoLocacaoFiltro)?.ToList() ?? new List<PedidoLocacao>();
                listaEmisaoSeloVM = listaEmisaoSelo?.Select(x => new PedidoLocacaoViewModel(x))?.ToList() ?? new List<PedidoLocacaoViewModel>();

                DadosModal = new DadosModal
                {
                    Titulo = "Sucesso",
                    Mensagem = "Solicitação realizada com sucesso",
                    TipoModal = TipoModal.Success
                };
            }
            catch (Exception ex)
            {
                return Json(new Resultado<object>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Warning.ToDescription(),
                    Titulo = "Atenção",
                    Mensagem = "Ocorreu um erro ao inativar: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }

            return PartialView("_GridPedidosLote", listaEmisaoSeloVM);
        }
        public JsonResult BuscarDadosDosGrids(int id)
        {
            var cliente = new PedidoLocacaoViewModel(Aplicacao.BuscarPorId(id));

            Session["Itens"] = cliente.PedidoLocacaoLancamentosAdicionais.ToList();

            return Json(
                new
                {
                    items = cliente?.PedidoLocacaoLancamentosAdicionais,
                }
            );
        }
    }
}