using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Core.Extensions;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class DescontoController : GenericController<Desconto>
    {
        public List<SelectListItem> ListaTipoDescontoAux { get; set; }

        private readonly ITipoNotificacaoAplicacao _tipoNotificacaoAplicacao;
        private readonly IUsuarioAplicacao _usuarioAplicacao;
        private readonly IDescontoAplicacao _negociacaoSeloDescontoAplicacao;

        public DescontoController(IDescontoAplicacao negociacaoSeloDescontoAplicacao,
                                                ITipoNotificacaoAplicacao tipoNotificacaoAplicacao,
                                                IUsuarioAplicacao usuarioAplicacao)
        {
            Aplicacao = negociacaoSeloDescontoAplicacao;
            _tipoNotificacaoAplicacao = tipoNotificacaoAplicacao;
            _usuarioAplicacao = usuarioAplicacao;
            _negociacaoSeloDescontoAplicacao = negociacaoSeloDescontoAplicacao;

            ListaTipoDescontoAux = new List<SelectListItem>();

            ListaTipoDescontoAux.Add(new SelectListItem { Value = "1", Text = "Monetário" });
            ListaTipoDescontoAux.Add(new SelectListItem { Value = "2", Text = "Percentual" });

            ViewBag.ListaParametroSelo = ListaTipoDescontoAux;
        }

        public List<Desconto> ListaNegociacaoSeloDesconto => Aplicacao?.Buscar()?.ToList() ?? new List<Desconto>();
        
        public override ActionResult Index()
        {
            //teste
            return View();
        }

        [CheckSessionOut]
        public override ActionResult Edit(int id)
        {
            var NegociacaoSeloDescontoDM = Aplicacao.BuscarPorId(id);
            var NegociacaoSeloDescontoVM = AutoMapper.Mapper.Map<Desconto, DescontoViewModel>(NegociacaoSeloDescontoDM) ?? new DescontoViewModel();

            NegociacaoSeloDescontoVM.Valor = NegociacaoSeloDescontoDM?.Valor.ToString("C2").Replace("R$", string.Empty).Replace(" ", string.Empty) ?? string.Empty;

            return View("Index", NegociacaoSeloDescontoVM);

        }

        public ActionResult SalvarDados(DescontoViewModel NegociacaoSeloDescontoVM)
        {
            if (NegociacaoSeloDescontoVM.Valor == null)
                NegociacaoSeloDescontoVM.Valor = "0";

            var valor = Convert.ToDecimal(NegociacaoSeloDescontoVM.Valor.Replace(".", ""));

            var NegociacaoSeloDescontoDM = AutoMapper.Mapper.Map<DescontoViewModel, Desconto>(NegociacaoSeloDescontoVM);

            var usuarioLogado = HttpContext.User as CustomPrincipal;

            _negociacaoSeloDescontoAplicacao.Salvar(NegociacaoSeloDescontoDM);

            ModelState.Clear();

            DadosModal = new DadosModal
            {
                Titulo = "Sucesso",
                Mensagem = "Registro salvo com sucesso",
                TipoModal = TipoModal.Success
            };

            return View("Index");
        }

        public ActionResult BuscarDescontos()
        {
            var NegociacaoSeloDescontos = AutoMapper.Mapper.Map<List<Desconto>, List<DescontoViewModel>>(ListaNegociacaoSeloDesconto);
            //foreach (var NegociacaoSeloDesconto in NegociacaoSeloDescontos)
            //    NegociacaoSeloDesconto.TipoDesconto = NegociacaoSeloDesconto.TipoDesconto.ToDescription();

            return PartialView("_GridDesconto", NegociacaoSeloDescontos);
        }
    }
}