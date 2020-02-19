using Aplicacao;
using Aplicacao.ViewModels;
using AutoMapper;
using Core.Exceptions;
using Entidade;
using Entidade.Uteis;
using Newtonsoft.Json;
using Portal.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class OrcamentoSinistroController : GenericController<OrcamentoSinistro>
    {
        private readonly IOrcamentoSinistroAplicacao _orcamentoSinistroAplicacao;
        private readonly IOficinaAplicacao _oficinaAplicacao;
        private readonly IFornecedorAplicacao _fornecedorAplicacao;
        private readonly IOISAplicacao _oisAplicacao;

        public List<Oficina> ListaOficina => _oficinaAplicacao.BuscarPor(x => !x.IndicadaPeloCliente).ToList();
        public List<Oficina> ListaOficinaCliente => _oficinaAplicacao.BuscarPor(x => x.IndicadaPeloCliente).ToList();
        public List<Fornecedor> ListaFornecedor => _fornecedorAplicacao.Buscar().ToList();
        public List<OIS> ListaOIS => _oisAplicacao.Buscar().ToList();

        public List<ChaveValorViewModel> ListaFormaPagamento => _fornecedorAplicacao.BuscarValoresDoEnum<FormaPagamentoOrcamentoSinistroCotacaoItem>().ToList();

        public List<OficinaViewModel> ListaOficinasModal
        {
            get => (List<OficinaViewModel>)Session["OficinasModal"] ?? new List<OficinaViewModel>();
            set => Session["OficinasModal"] = value;
        }

        public List<FornecedorViewModel> ListaFornecedoresModal
        {
            get => (List<FornecedorViewModel>)Session["FornecedoresModal"] ?? new List<FornecedorViewModel>();
            set => Session["FornecedoresModal"] = value;
        }

        public List<OrcamentoSinistroOficinaViewModel> ListaOrcamentoSinistroOficina
        {
            get => (List<OrcamentoSinistroOficinaViewModel>)Session["OrcamentoSinistroOficina"];
            set => Session["OrcamentoSinistroOficina"] = value;
        }

        public List<OrcamentoSinistroOficinaClienteViewModel> ListaOrcamentoSinistroOficinaCliente
        {
            get => (List<OrcamentoSinistroOficinaClienteViewModel>)Session["OrcamentoSinistroOficinaCliente"];
            set => Session["OrcamentoSinistroOficinaCliente"] = value;
        }

        public List<OrcamentoSinistroPecaServicoViewModel> ListaOrcamentoSinistroPecaServico
        {
            get => (List<OrcamentoSinistroPecaServicoViewModel>)Session["OrcamentoSinistroPecas"];
            set => Session["OrcamentoSinistroPecas"] = value;
        }

        public List<OrcamentoSinistroFornecedorViewModel> ListaOrcamentoSinistroFornecedor
        {
            get => (List<OrcamentoSinistroFornecedorViewModel>)Session["OrcamentoSinistroFornecedores"];
            set => Session["OrcamentoSinistroFornecedores"] = value;
        }

        public List<OrcamentoSinistroCotacaoItemViewModel> ListaOrcamentoSinistroCotacaoItem
        {
            get => (List<OrcamentoSinistroCotacaoItemViewModel>)Session["OrcamentoSinistroCotacaoItens"];
            set => Session["OrcamentoSinistroCotacaoItens"] = value;
        }

        public OrcamentoSinistroController(
                                IOrcamentoSinistroAplicacao orcamentoSinistroAplicacao
                                , IOficinaAplicacao oficinaAplicacao
                                , IFornecedorAplicacao fornecedorAplicacao
                                , IOISAplicacao oisAplicacao
                                )
        {
            Aplicacao = orcamentoSinistroAplicacao;
            _orcamentoSinistroAplicacao = orcamentoSinistroAplicacao;
            _oficinaAplicacao = oficinaAplicacao;
            _fornecedorAplicacao = fornecedorAplicacao;
            _oisAplicacao = oisAplicacao;
        }

        public override ActionResult Index()
        {
            ListaOrcamentoSinistroOficina = null;
            ListaOrcamentoSinistroOficinaCliente = null;
            ListaOrcamentoSinistroPecaServico = null;
            ListaOrcamentoSinistroFornecedor = null;

            return base.Index();
        }

        public override ActionResult Edit(int id)
        {
            var orcamentoSinistro = _orcamentoSinistroAplicacao.BuscarPorId(id);
            var orcamentoSinistroViewModel = Mapper.Map<OrcamentoSinistroViewModel>(orcamentoSinistro);

            ListaOrcamentoSinistroOficina = orcamentoSinistroViewModel.OrcamentoSinistroOficinas;
            ListaOrcamentoSinistroOficinaCliente = orcamentoSinistroViewModel.OrcamentoSinistroOficinaClientes;
            ListaOrcamentoSinistroPecaServico = orcamentoSinistroViewModel.OrcamentoSinistroPecaServicos;
            ListaOrcamentoSinistroFornecedor = orcamentoSinistroViewModel.OrcamentoSinistroFornecedores;

            return View("Index", orcamentoSinistroViewModel);
        }

        public ActionResult BuscarOrcamentoSinistroOficinas()
        {
            return Json(ListaOrcamentoSinistroOficina);
        }

        public ActionResult BuscarOrcamentoSinistroOficinasCliente()
        {
            return Json(ListaOrcamentoSinistroOficinaCliente);
        }

        public ActionResult BuscarOrcamentoSinistroPecaServicos()
        {
            return Json(ListaOrcamentoSinistroPecaServico);
        }

        public ActionResult BuscarOrcamentoSinistroFornecedores()
        {
            return Json(ListaOrcamentoSinistroFornecedor);
        }

        public ActionResult Cotacao(int id)
        {
            return Index();
        }

        [HttpPost]
        public ActionResult SalvarDados(OrcamentoSinistroViewModel orcamentoSinistroViewModel)
        {
            try
            {
                orcamentoSinistroViewModel.OrcamentoSinistroOficinas = ListaOrcamentoSinistroOficina;
                orcamentoSinistroViewModel.OrcamentoSinistroOficinaClientes = ListaOrcamentoSinistroOficinaCliente;
                orcamentoSinistroViewModel.OrcamentoSinistroPecaServicos = ListaOrcamentoSinistroPecaServico;
                orcamentoSinistroViewModel.OrcamentoSinistroFornecedores = ListaOrcamentoSinistroFornecedor;

                var orcamentoSinistro = Mapper.Map<OrcamentoSinistro>(orcamentoSinistroViewModel);

                _orcamentoSinistroAplicacao.Salvar(orcamentoSinistro, UsuarioLogado.UsuarioId);

                ModelState.Clear();
                CriarDadosModalSucesso("Registro salvo com sucesso");
            }
            catch (BusinessRuleException br)
            {
                CriarDadosModalAviso(br.Message);
            }
            catch (Exception ex)
            {
                CriarDadosModalErro(ex.Message);
            }

            return View("Index", orcamentoSinistroViewModel);
        }

        public ActionResult BuscarOrcamentoSinistro()
        {
            var orcamentoSinistros = Mapper.Map<List<OrcamentoSinistroViewModel>>(_orcamentoSinistroAplicacao.Buscar());

            return PartialView("_Grid", orcamentoSinistros);
        }

        public JsonResult BuscarDadosDoModal(int orcamentoSinistroId)
        {
            var orcamentoSinistroViewModel = Mapper.Map<OrcamentoSinistroViewModel>(_orcamentoSinistroAplicacao.BuscarPorId(orcamentoSinistroId));

            if (orcamentoSinistroViewModel.OrcamentoSinistroCotacao != null)
            {
                ListaOrcamentoSinistroCotacaoItem = orcamentoSinistroViewModel.OrcamentoSinistroCotacao.OrcamentoSinistroCotacaoItens;

                ListaOrcamentoSinistroCotacaoItem
                    .ForEach(x => x.OrcamentoSinistroCotacao?.OrcamentoSinistroCotacaoItens?.ForEach(item => item.OrcamentoSinistroCotacao = null));
            }
            else
            {
                var lista = new List<OrcamentoSinistroCotacaoItemViewModel>();
                foreach (var item in orcamentoSinistroViewModel.OrcamentoSinistroPecaServicos)
                {
                    lista.Add(new OrcamentoSinistroCotacaoItemViewModel
                    {
                        PecaServico = item.PecaServico
                    });
                }

                ListaOrcamentoSinistroCotacaoItem = lista;
            }

            ListaOficinasModal = BuscarOficinasModal(orcamentoSinistroViewModel);
            ListaFornecedoresModal = BuscarFornecedoresModal(orcamentoSinistroViewModel);

            return Json(new
            {
                Form = RazorHelper.RenderRazorViewToString(ControllerContext, "_FormCotacaoItens", null),
                Grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridCotacaoItens", ListaOrcamentoSinistroCotacaoItem),
                Itens = ListaOrcamentoSinistroCotacaoItem,
                Status = orcamentoSinistroViewModel.OrcamentoSinistroCotacao?.Status
            });
        }

        public ActionResult AtualizarCotacaoItens(List<OrcamentoSinistroCotacaoItemViewModel> cotacaoItens)
        {
            ListaOrcamentoSinistroCotacaoItem = cotacaoItens;

            if (cotacaoItens != null)
            {
                foreach (var item in cotacaoItens)
                {
                    if(item.Oficina != null)
                        item.Oficina.IndicadaPeloCliente = ListaOficinasModal.FirstOrDefault(x => x.Id == item.Oficina.Id).IndicadaPeloCliente;
                }
            }

            return PartialView("_GridCotacaoItens", ListaOrcamentoSinistroCotacaoItem);
        }

        public ActionResult AtualizarOrcamentoSinistroOficinas(List<OrcamentoSinistroOficinaViewModel> orcamentoSinistroOficinas)
        {
            ListaOrcamentoSinistroOficina = orcamentoSinistroOficinas;

            return PartialView("_GridOficinas", ListaOrcamentoSinistroOficina);
        }

        public ActionResult AtualizarOrcamentoSinistroOficinasCliente(List<OrcamentoSinistroOficinaClienteViewModel> orcamentoSinistroOficinasCliente)
        {
            ListaOrcamentoSinistroOficinaCliente = orcamentoSinistroOficinasCliente;

            return PartialView("_GridOficinasCliente", ListaOrcamentoSinistroOficinaCliente);
        }

        public ActionResult AtualizarOrcamentoSinistroPecaServicos(List<OrcamentoSinistroPecaServicoViewModel> orcamentoSinistroPecaServicos)
        {
            ListaOrcamentoSinistroPecaServico = orcamentoSinistroPecaServicos;

            return PartialView("_GridPecaServicos", ListaOrcamentoSinistroPecaServico);
        }

        public ActionResult AtualizarOrcamentoSinistroFornecedores(List<OrcamentoSinistroFornecedorViewModel> orcamentoSinistroFornecedores)
        {
            ListaOrcamentoSinistroFornecedor = orcamentoSinistroFornecedores;

            return PartialView("_GridFornecedores", ListaOrcamentoSinistroFornecedor);
        }

        public ActionResult AtualizarHistoricoData(List<OrcamentoSinistroCotacaoHistoricoDataItemViewModel> historicoData)
        {
            return PartialView("_GridHistoricoData", historicoData);
        }

        public JsonResult BuscarOisSelecionado(int oisId)
        {
            var ois = _oisAplicacao.BuscarPorId(oisId);

            return Json(
                new {
                    NomeUnidade = ois.Unidade?.Nome,
                    NomeCliente = ois.NomeCliente,
                    DescricaoModelo = ois.Modelo?.Descricao,
                    Placa = ois.Placa,
                });
        }

        public ActionResult SalvarCotacao(int orcamentoSinistroId)
        {
            try
            {
                _orcamentoSinistroAplicacao.SalvarCotacao(orcamentoSinistroId, ListaOrcamentoSinistroCotacaoItem, UsuarioLogado.UsuarioId);

                CriarDadosModalSucesso("Registro salvo com sucesso!");
            }
            catch (BusinessRuleException br)
            {
                CriarDadosModalAviso(br.Message);
            }
            catch (Exception ex)
            {
                CriarDadosModalAviso(ex.Message);
            }

            return PartialView("../Shared/_ModalPrincipal");
        }

        public void Cancelar(int id)
        {
            _orcamentoSinistroAplicacao.Cancelar(id);
        }

        private List<OficinaViewModel> BuscarOficinasModal(OrcamentoSinistroViewModel orcamentoSinistroViewModel)
        {
            var oficinas = new List<OficinaViewModel>();

            var oficinasNaoIndicadas = orcamentoSinistroViewModel.OrcamentoSinistroOficinas.Select(x => x.Oficina);

            if (oficinasNaoIndicadas.Any())
                oficinas.AddRange(oficinasNaoIndicadas);

            var oficinasIndicadas = orcamentoSinistroViewModel.OrcamentoSinistroOficinaClientes.Select(x => x.Oficina);
            if (oficinasIndicadas.Any())
                oficinas.AddRange(oficinasIndicadas);

            return oficinas.Any() ? oficinas : Mapper.Map<List<OficinaViewModel>>(_oficinaAplicacao.Buscar());
        }

        private List<FornecedorViewModel> BuscarFornecedoresModal(OrcamentoSinistroViewModel orcamentoSinistroViewModel)
        {
            var fornecedores = orcamentoSinistroViewModel.OrcamentoSinistroFornecedores.Select(x => x.Fornecedor).ToList() ?? new List<FornecedorViewModel>();

            return fornecedores.Any() ? fornecedores : Mapper.Map<List<FornecedorViewModel>>(_fornecedorAplicacao.Buscar());
        }
    }
}