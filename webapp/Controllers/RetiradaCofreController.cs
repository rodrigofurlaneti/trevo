using Aplicacao;
using Aplicacao.ViewModels;
using AutoMapper;
using Core.Extensions;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class RetiradaCofreController : GenericController<RetiradaCofre>
    {
        private readonly IRetiradaCofreAplicacao _retiradaCofreAplicacao;
        private readonly IContaPagarAplicacao _contaPagarAplicacao;
        private readonly IDepartamentoAplicacao _departamentoAplicacao;
        private readonly IContaFinanceiraAplicacao _contaFinanceira;
        private readonly IUsuarioAplicacao _usuarioAplicacao;

        public List<ContaFinanceiraViewModel> ListaContaFinanceira => _contaFinanceira?.Buscar()?.Select(x => new ContaFinanceiraViewModel(x))?.ToList() ?? new List<ContaFinanceiraViewModel>();
        public List<Usuario> ListaUsuarios => _usuarioAplicacao.BuscarPor(x => x.Ativo).ToList();

        public RetiradaCofreController(
            IContaPagarAplicacao contaPagarAplicacao,
            IDepartamentoAplicacao departamentoAplicacao,
            IRetiradaCofreAplicacao retiradaCofreAplicacao,
            IContaFinanceiraAplicacao contaFinanceiraAplicacao,
            IUsuarioAplicacao usuarioAplicacao)
        {
            Aplicacao = retiradaCofreAplicacao;
            _retiradaCofreAplicacao = retiradaCofreAplicacao;
            _contaPagarAplicacao = contaPagarAplicacao;
            _departamentoAplicacao = departamentoAplicacao;
            _contaFinanceira = contaFinanceiraAplicacao;
            _usuarioAplicacao = usuarioAplicacao;
        }

        // GET: RetiradaCofre
        public override ActionResult Index()
        {
            ViewBag.DepartamentoSelectList = new SelectList(
                _departamentoAplicacao.Buscar() ?? new List<Departamento>(),
                "Id", 
                "Nome");

            ViewBag.ListaTipoPagamentoSelectList = new SelectList(
                Enum.GetValues(typeof(TipoServico)).Cast<TipoServico>().Select(e => new ChaveValorViewModel { Id = (int)e, Descricao = e.ToDescription() }) ?? new List<ChaveValorViewModel>(),
                "Id",
                "Descricao");

            ViewBag.ListaContaFinanceiraSelectList = new SelectList(ListaContaFinanceira ?? new List<ContaFinanceiraViewModel>(), "Id", "Descricao");

            ViewBag.ListaUsuarioSelectList = new SelectList(ListaUsuarios ?? new List<Usuario>(), "Id", "Funcionario.Pessoa.Nome");

            return View();
        }

        public ActionResult BuscarRetiradaCofre(RetiradaCofreViewModel RetiradaCofreViewModel)
        {
            var model = new List<RetiradaCofreViewModel>();

            var retiradasCofre = Aplicacao.BuscarPor(x =>
                (x.StatusRetiradaCofre == StatusRetiradaCofre.Pendente && (RetiradaCofreViewModel.DepartamentoId == null || x.ContasAPagar.Departamento.Id == RetiradaCofreViewModel.DepartamentoId)) &&
                (x.StatusRetiradaCofre == StatusRetiradaCofre.Pendente && (RetiradaCofreViewModel.ContaFinanceiraId == null || x.ContasAPagar.ContaFinanceira.Id == RetiradaCofreViewModel.ContaFinanceiraId)) &&
                (x.StatusRetiradaCofre == StatusRetiradaCofre.Pendente && (RetiradaCofreViewModel.UsuarioId == null || x.Usuario.Id == RetiradaCofreViewModel.UsuarioId)) &&
                (x.StatusRetiradaCofre == StatusRetiradaCofre.Pendente && (!RetiradaCofreViewModel.DataInicio.HasValue || x.DataInsercao.Date >= RetiradaCofreViewModel.DataInicio)) &&
                (x.StatusRetiradaCofre == StatusRetiradaCofre.Pendente && (!RetiradaCofreViewModel.DataFim.HasValue || x.DataInsercao.Date <= RetiradaCofreViewModel.DataFim))
            );

            model = Mapper.Map<List<RetiradaCofreViewModel>>(retiradasCofre);

            return PartialView("_GridRetiradaCofre", model);
        }

        public ActionResult BuscarRetiradaCofrePeloId(int retiradaCofreId)
        {
            var retiradaCofre = _retiradaCofreAplicacao.BuscarPorId(retiradaCofreId);
            var retiradaCofreVM = Mapper.Map<RetiradaCofreViewModel>(retiradaCofre);

            var model = new List<RetiradaCofreViewModel> { retiradaCofreVM };

            return PartialView("_GridRetiradaCofre", model);
        }

        public ActionResult AtualizarStatus(List<RetiradaCofreViewModel> retiradasCofre, AcaoRetiradaCofre acao)
        {
            _retiradaCofreAplicacao.AtualizarStatus(retiradasCofre, acao, UsuarioLogado.UsuarioId);

            return Json(new
            {
                status = new HttpStatusCodeResult(HttpStatusCode.Created, acao == AcaoRetiradaCofre.Aprovar ? "Valor retirado do cofre com sucesso" : "Valor negado com sucesso"),
            });
        }

        public ActionResult Informacoes(int retiradaCofreId)
        {
            var retiradaCofre = Aplicacao.BuscarPorId(retiradaCofreId);

            var retiradaCofreVM = Mapper.Map<RetiradaCofreViewModel>(retiradaCofre);

            return PartialView("_ModalInformacoes", retiradaCofreVM);
        }
    }
}