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
using System.Net;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class ContaCorrenteClienteController : GenericController<ContaCorrenteCliente>
    {
        private readonly IContaCorrenteClienteAplicacao _contaCorrenteClienteAplicacao;
        private readonly IPlanoCarreiraAplicacao _planoCarreiraAplicacao;
        private readonly IClienteAplicacao _clienteAplicacao;

        public List<ContaCorrenteClienteDetalheViewModel> ListaContaCorrenteClienteDetalhes
        {
            get => (List<ContaCorrenteClienteDetalheViewModel>)Session["ListaContaCorrenteItemDetalhes"] ?? new List<ContaCorrenteClienteDetalheViewModel>();
            set => Session["ListaContaCorrenteItemDetalhes"] = value;
        }

        public ContaCorrenteClienteController(
            IContaCorrenteClienteAplicacao contaCorrenteClienteAplicacao
            , IPlanoCarreiraAplicacao planoCarreiraAplicacao
            , IClienteAplicacao clienteAplicacao
        )
        {
            Aplicacao = contaCorrenteClienteAplicacao;
            _contaCorrenteClienteAplicacao = contaCorrenteClienteAplicacao;
            _planoCarreiraAplicacao = planoCarreiraAplicacao;
            _clienteAplicacao = clienteAplicacao;

            ViewBag.ListaTipoOperacaoContaCorrente = Aplicacao?.BuscarValoresDoEnum<TipoOperacaoContaCorrente>();
        }

        [CheckSessionOut]
        public override ActionResult Index()
        {
            ListaContaCorrenteClienteDetalhes = new List<ContaCorrenteClienteDetalheViewModel>();
            return View("Index");
        }

        [CheckSessionOut]
        [HttpPost]
        public ActionResult SalvarDados(ContaCorrenteClienteViewModel model)
        {
            try
            {
                model.ContaCorrenteClienteDetalhes = ListaContaCorrenteClienteDetalhes;
                var contaCorrenteCliente = Mapper.Map<ContaCorrenteCliente>(model);
                _contaCorrenteClienteAplicacao.Salvar(contaCorrenteCliente);
                ModelState.Clear();
                CriarDadosModalSucesso("Registro salvo com sucesso");
            }
            catch (BusinessRuleException br)
            {
                CriarDadosModalAviso(br.Message);
                return View("Index", model);
            }
            catch (Exception ex)
            {
                CriarDadosModalErro("Ocorreu um erro ao salvar: " + ex.Message);
                return View("Index", model);
            }
            return View("Index");
        }

        public override ActionResult Edit(int id)
        {
            var contaCorrenteCliente = _contaCorrenteClienteAplicacao.BuscarPorId(id);
            var contaCorrenteClienteVM = Mapper.Map<ContaCorrenteClienteViewModel>(contaCorrenteCliente);
            
            ListaContaCorrenteClienteDetalhes = contaCorrenteClienteVM.ContaCorrenteClienteDetalhes;
            return View("Index", contaCorrenteClienteVM);
        }

        public PartialViewResult BuscarContaCorrenteCliente()
        {
            var contaCorrenteClientes = _contaCorrenteClienteAplicacao.BuscarPor(x => x.ContaCorrenteClienteDetalhes.Any());
            var contaCorrenteClientesVM = Mapper.Map<List<ContaCorrenteClienteViewModel>>(contaCorrenteClientes);

            return PartialView("_Grid", contaCorrenteClientesVM);
        }

        public ActionResult AdicionarContaCorrenteCliente(int tipoOperacaoId, string dataReferencia, string valor, int contratoId, int numeroContrato)
        {
            if (ListaContaCorrenteClienteDetalhes.Any(x => (int)x.TipoOperacaoContaCorrente == tipoOperacaoId && x.DataCompetencia == DateTime.Parse(dataReferencia)))
                return new HttpStatusCodeResult(HttpStatusCode.PreconditionFailed, "Já foi adicionado informações para este Tipo Benefício");

            var tipoBeneficioVM = (TipoOperacaoContaCorrente)tipoOperacaoId;
            
            var contaCorrenteCliente = new ContaCorrenteClienteDetalheViewModel(tipoBeneficioVM, DateTime.Parse(dataReferencia), valor, contratoId, numeroContrato);
            var listaContaCorrenteClienteDetalhes = ListaContaCorrenteClienteDetalhes;

            listaContaCorrenteClienteDetalhes.Add(contaCorrenteCliente);
            ListaContaCorrenteClienteDetalhes = listaContaCorrenteClienteDetalhes;

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridContaCorrenteClienteDetalhes", ListaContaCorrenteClienteDetalhes);
            return Json(new
            {
                Grid = grid,
            });
        }

        public ActionResult RemoverContaCorrenteCliente(int tipoBeneficioId, string dataReferencia)
        {
            var item = ListaContaCorrenteClienteDetalhes.FirstOrDefault(x => (int)x.TipoOperacaoContaCorrente == tipoBeneficioId && x.DataCompetencia == DateTime.Parse(dataReferencia));
            var listaContaCorrenteClienteDetalhes = ListaContaCorrenteClienteDetalhes;

            listaContaCorrenteClienteDetalhes.Remove(item);
            ListaContaCorrenteClienteDetalhes = listaContaCorrenteClienteDetalhes;

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridContaCorrenteClienteDetalhes", ListaContaCorrenteClienteDetalhes);
            return Json(new
            {
                Grid = grid,
            });
        }

        public ActionResult EditarContaCorrenteCliente(int tipoBeneficioId, string dataReferencia)
        {
            var item = ListaContaCorrenteClienteDetalhes.FirstOrDefault(x => (int)x.TipoOperacaoContaCorrente == tipoBeneficioId && x.DataCompetencia == DateTime.Parse(dataReferencia));
            var listaContaCorrenteClienteDetalhes = ListaContaCorrenteClienteDetalhes;

            listaContaCorrenteClienteDetalhes.Remove(item);
            ListaContaCorrenteClienteDetalhes = listaContaCorrenteClienteDetalhes;

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridContaCorrenteClienteDetalhes", ListaContaCorrenteClienteDetalhes);
            return Json(new
            {
                Grid = grid,
                Item = RemoverLoopDoJson(item)
            });
        }

        public ActionResult EditarSeJaExiste(int clienteId)
        {
            var contaCorrenteCliente = _contaCorrenteClienteAplicacao.PrimeiroPor(x => x.Cliente.Id == clienteId);

            if(contaCorrenteCliente != null)
            {
                var contaCorrenteClienteVM = Mapper.Map<ContaCorrenteClienteViewModel>(contaCorrenteCliente);
                ListaContaCorrenteClienteDetalhes = contaCorrenteClienteVM.ContaCorrenteClienteDetalhes;
            }
            else
            {
                ListaContaCorrenteClienteDetalhes = new List<ContaCorrenteClienteDetalheViewModel>();
            }

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridContaCorrenteClienteDetalhes", ListaContaCorrenteClienteDetalhes);

            return Json(new
            {
                Existe = contaCorrenteCliente != null,
                contaCorrenteCliente?.Id,
                Grid = grid
            }, JsonRequestBehavior.AllowGet);
        }
    }
}