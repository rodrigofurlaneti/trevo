using Aplicacao;
using Aplicacao.ViewModels;
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
    public class ValidadeSeloController : GenericController<EmissaoSelo>
    {
        public List<EmissaoSelo> ListaEmissoesSelo { get; set; }

        public List<EmissaoSeloViewModel> ListaEmissaoSelo => _emissaoseloAplicacao.BuscarPor(x => x.Validade != null && x.StatusSelo != StatusSelo.ExcluidoLote).Select(x => new EmissaoSeloViewModel(x)).ToList();

        public EmissaoSeloViewModel EmissaoSeloFiltro
        {
            get { return (EmissaoSeloViewModel)Session["EmissaoSeloFiltro"] ?? new EmissaoSeloViewModel(); }
            set { Session["EmissaoSeloFiltro"] = value; }
        }

        public IEnumerable<ChaveValorViewModel> ListaTipoServico;

        private readonly IEmissaoSeloAplicacao _emissaoseloAplicacao;


        private readonly IUnidadeAplicacao _unidadeAplicacao;
        public List<UnidadeViewModel> ListaUnidade => _unidadeAplicacao?.Buscar()?.Select(x => new UnidadeViewModel(x))?.ToList() ?? new List<UnidadeViewModel>();

        private readonly IClienteAplicacao _clienteAplicacao;

        private readonly ITipoSeloAplicacao _tiposeloAplicacao;
        public List<TipoSeloViewModel> ListaTipoSelo => _tiposeloAplicacao?.Buscar()?.Select(x => new TipoSeloViewModel(x))?.ToList() ?? new List<TipoSeloViewModel>();

        public List<SelectListItem> ListaNumerosLote;

        public IEnumerable<ChaveValorViewModel> ListaTipoPagamento;
        public IEnumerable<ChaveValorViewModel> ListaFormaPagamento;

        public ValidadeSeloController(
            IEmissaoSeloAplicacao emissaoseloAplicacao,
            IClienteAplicacao clienteAplicacao,
            ITipoSeloAplicacao tiposeloAplicacao,
            IUnidadeAplicacao unidadeAplicacao)
        {
            _emissaoseloAplicacao = emissaoseloAplicacao;
            _clienteAplicacao = clienteAplicacao;
            _tiposeloAplicacao = tiposeloAplicacao;
            _unidadeAplicacao = unidadeAplicacao;

            ListaNumerosLote = new List<SelectListItem>();


        }



        public ActionResult Pesquisar(EmissaoSeloViewModel filtro)
        {
            AlimentarViewBag();

            var listaEmisaoSeloVM = new List<EmissaoSeloViewModel>();

            EmissaoSeloFiltro = filtro;

            try
            {
                EmissaoSeloFiltro = filtro;
                var listaEmisaoSelo = _emissaoseloAplicacao.ListarEmissaoSeloFiltro(EmissaoSeloFiltro)?.Where(x => x.StatusSelo != StatusSelo.ExcluidoLote).ToList() ?? new List<EmissaoSelo>();
                listaEmisaoSeloVM = listaEmisaoSelo?.Select(x => new EmissaoSeloViewModel(x))?.ToList() ?? new List<EmissaoSeloViewModel>();
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

            return PartialView("_GridEmissoesLote", listaEmisaoSeloVM);
        }

        [CheckSessionOut]
        public override ActionResult Index()
        {
            AlimentarViewBag();

            ModelState.Clear();

            return View("Index");
        }

        [CheckSessionOut]
        public ActionResult ExecutarModal(int idsLancamentosCobranca)
        {

            try
            {
                AlimentarViewBag();

                var objConta = _emissaoseloAplicacao.BuscarPorId(idsLancamentosCobranca);
                var ObjView = new EmissaoSeloViewModel(objConta);

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
            AlimentarViewBag();
            var lancamentoContasPagar = new List<EmissaoSeloViewModel>();

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

            return PartialView("_GridEmissoesLote", ListaEmissaoSelo);
        }

        [CheckSessionOut]
        public virtual ActionResult ConfirmarAlteracao(EmissaoSeloViewModel emissaoselo)
        {
            AlimentarViewBag();

            var listaEmisaoSeloVM = new List<EmissaoSeloViewModel>();

            try
            {

                var objetoConta = _emissaoseloAplicacao.BuscarPorId(emissaoselo.Id);

                objetoConta.Validade = emissaoselo.Validade;

                _emissaoseloAplicacao.AlteraValidade(objetoConta);

                ModelState.Clear();

                var listaEmissaoSelo = _emissaoseloAplicacao.ListarEmissaoSeloFiltro(EmissaoSeloFiltro)?.Where(x => x.StatusSelo != StatusSelo.ExcluidoLote).ToList() ?? new List<EmissaoSelo>();
                listaEmisaoSeloVM = listaEmissaoSelo?.Select(x => new EmissaoSeloViewModel(x))?.ToList() ?? new List<EmissaoSeloViewModel>();

            }
            catch (BusinessRuleException br)
            {
                GerarDadosModal("Atenção", br.Message, TipoModal.Danger);
                return PartialView("Index");
            }
            catch (Exception ex)
            {
                GerarDadosModal("Atenção", new BusinessRuleException(ex.Message).Message, TipoModal.Danger);
                return PartialView("Index");
            }

            ModelState.Clear();

            return Json(new Resultado<object>()
            {
                Sucesso = false,
                TipoModal = TipoModal.Success.ToDescription(),
                Titulo = "Sucesso",
                Mensagem = "Validade alterada com sucesso!"
            }, JsonRequestBehavior.AllowGet);
        }


        public void AlimentarViewBag()
        {
            //var ListaCliente = AutoMapper.Mapper.Map<List<Cliente>, List<ClienteViewModel>>(_clienteAplicacao.Buscar().ToList());
            //foreach (var cliente in ListaCliente)
            //{
            //    if (cliente.Pessoa.Nome == null)
            //        cliente.Pessoa.Nome = cliente.NomeFantasia;

            //}
            //ViewBag.ListaClientes = ListaCliente;
        }

        //GTE-1810
        [HttpPost]
        public ActionResult AtualizaLotes(int? tiposeloid)
        {
            var retornoEquipes = _emissaoseloAplicacao.BuscarPor(x => x.PedidoSelo.TipoSelo.Id == tiposeloid);

            ListaNumerosLote = retornoEquipes.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Id.ToString().PadLeft(8, '0') }).ToList();

            return Json(ListaNumerosLote, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CheckSessionOut]
        public JsonResult BuscarCliente(string descricao)
        {
            var lista = _clienteAplicacao.BuscarPor(c => c.Pessoa.Nome.Contains(descricao) || c.NomeFantasia.Contains(descricao));

            return Json(lista.Select(c => new
            {
                c.Id,
                Descricao = c.TipoPessoa == TipoPessoa.Fisica ? c.Pessoa.Nome : c.NomeFantasia
            }));
        }
    }
}