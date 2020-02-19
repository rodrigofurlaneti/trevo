using Aplicacao;
using Aplicacao.ViewModels;
using AutoMapper;
using Core.Exceptions;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class ControleCompraController : GenericController<ControleCompra>
    {
        private readonly IControleCompraAplicacao _controleCompraAplicacao;
        private readonly IOrcamentoSinistroAplicacao _orcamentoSinistroAplicacao;

        public List<ChaveValorViewModel> ListaStatus => Aplicacao.BuscarValoresDoEnum<StatusCompraServico>().ToList();

        public ControleCompraController(
            IControleCompraAplicacao controleCompraAplicacao
            , IOrcamentoSinistroAplicacao orcamentoSinistroAplicacao)
        {
            Aplicacao = controleCompraAplicacao;
            _controleCompraAplicacao = controleCompraAplicacao;
            _orcamentoSinistroAplicacao = orcamentoSinistroAplicacao;
        }

        public ActionResult Editar(int orcamentoCotacaoId, int pecaServicoId)
        {
            var controleCompraViewModel = _controleCompraAplicacao.PrimeiroPor(orcamentoCotacaoId, pecaServicoId);

            return View("Index", controleCompraViewModel);
        }

        [HttpPost]
        public ActionResult SalvarDados(ControleCompraViewModel controleCompraViewModel)
        {
            try
            {
                _controleCompraAplicacao.Salvar(controleCompraViewModel);
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

            return View("Index", controleCompraViewModel);
        }

        public ActionResult BuscarCotacoes(DateTime? data, StatusCompraServico? statusCompraServico)
        {
            var itensViewModel = _controleCompraAplicacao.BuscarCotacoes(data, statusCompraServico);

            return PartialView("_Grid", itensViewModel);
        }
    }
}