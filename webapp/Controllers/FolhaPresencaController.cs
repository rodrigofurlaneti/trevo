using Aplicacao;
using Aplicacao.ViewModels;
using AutoMapper;
using Core.Exceptions;
using Core.Extensions;
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
    public class FolhaPresencaController : GenericController<Funcionario>
    {
        private readonly IFuncionarioAplicacao _funcionarioAplicacao;
        private readonly ICalendarioRHAplicacao _calendarioRHAplicacao;
        private readonly IControleFeriasAplicacao _controleFeriasAplicacao;

        public FolhaPresencaViewModel FolhaPresenca
        {
            get => (FolhaPresencaViewModel)Session["FolhaPresenca"] ?? new FolhaPresencaViewModel();
            set => Session["FolhaPresenca"] = value;
        }

        public List<FolhaPresencaFuncionarioViewModel> FolhaPresencaFuncionarios
        {
            get => (List<FolhaPresencaFuncionarioViewModel>)Session["ItensSelecionados"] ?? new List<FolhaPresencaFuncionarioViewModel>();
            set => Session["ItensSelecionados"] = value;
        }

        public List<FolhaPresencaFuncionarioViewModel> ListaPresencaItem
        {
            get => (List<FolhaPresencaFuncionarioViewModel>)Session["ListaPresencaItem"] ?? new List<FolhaPresencaFuncionarioViewModel>();
            set => Session["ListaPresencaItem"] = value;
        }

        public FolhaPresencaController(
            IFuncionarioAplicacao funcionarioAplicacao,
            ICalendarioRHAplicacao calendarioRHAplicacao,
            IControleFeriasAplicacao controleFeriasAplicacao
        )
        {
            Aplicacao = funcionarioAplicacao;
            _funcionarioAplicacao = funcionarioAplicacao;
            _calendarioRHAplicacao = calendarioRHAplicacao;
            _controleFeriasAplicacao = controleFeriasAplicacao;

            ViewBag.Anos = RetornarAnos();
            ViewBag.Meses = RetornarMeses();
        }

        [CheckSessionOut]
        public override ActionResult Index()
        {
            FolhaPresencaFuncionarios = new List<FolhaPresencaFuncionarioViewModel>();
            ListaPresencaItem = new List<FolhaPresencaFuncionarioViewModel>();
            return View("Index");
        }

        public HttpStatusCodeResult AlternarItemSelecionado(int funcionarioId)
        {
            var itensSelecionado = FolhaPresencaFuncionarios;

            if (itensSelecionado.Any(x => x.Funcionario.Id == funcionarioId))
            {
                FolhaPresencaFuncionarios.RemoveAll(x => x.Funcionario.Id == funcionarioId);
            }
            else
            {
                if(itensSelecionado.Count == 10)
                    return new HttpStatusCodeResult(HttpStatusCode.PreconditionFailed, "Selecione no máximo 10");

                var itemSelecionado = ListaPresencaItem.FirstOrDefault(x => x.Funcionario.Id == funcionarioId);
                itensSelecionado.Add(itemSelecionado);
            }

            FolhaPresencaFuncionarios = itensSelecionado;
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public HttpStatusCodeResult AltenarSelecionarTudo(bool selecionado)
        {
            if (selecionado)
            {
                var itens = new List<FolhaPresencaFuncionarioViewModel>(FolhaPresencaFuncionarios);
                itens.AddRange(ListaPresencaItem);
                itens = itens.DistinctBy(x => x.Funcionario.Id).ToList();
                if (itens.Count > 10)
                    return new HttpStatusCodeResult(HttpStatusCode.PreconditionFailed, "Selecione no máximo 10");

                FolhaPresencaFuncionarios = ListaPresencaItem;
            }
            else
            {
                FolhaPresencaFuncionarios = new List<FolhaPresencaFuncionarioViewModel>();
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public HttpStatusCodeResult ArmazenarDadosImpressao(int? ano, int? mes, string observacao)
        {
            if (!ano.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.PreconditionFailed, "Informe o Ano");

            if (!mes.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.PreconditionFailed, "Informe o Mês");

            if (!FolhaPresencaFuncionarios.Any())
                return new HttpStatusCodeResult(HttpStatusCode.PreconditionFailed, "Nenhum Funcionário Selecionado");

            var folhaPresenca = new FolhaPresencaViewModel(ano, mes, observacao, FolhaPresencaFuncionarios);
            foreach (var folhaPresencaFuncionario in folhaPresenca.FolhaPresencaFuncionarios)
            {
                folhaPresencaFuncionario.FolhaPresencaDias = RetornarFolhaPresencaDias(folhaPresencaFuncionario.Funcionario.Id, ano, mes);
            }
            FolhaPresenca = folhaPresenca;

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public ActionResult Impressao()
        {
            return PartialView("_Impressao", FolhaPresenca);
        }

        public PartialViewResult BuscarFolhaPresenca(int? supervisorId, int? funcionarioId, int pagina = 1, bool novabusca = false)
        {
            if(novabusca)
                FolhaPresencaFuncionarios = new List<FolhaPresencaFuncionarioViewModel>();

            var funcionarios = new List<Funcionario>();
            PaginacaoGenericaViewModel paginacao;
            var quantidadePorPagina = 10;

            if (!supervisorId.HasValue && !funcionarioId.HasValue)
            {
                var quantidadeFuncionarios = _funcionarioAplicacao.Contar();
                paginacao = new PaginacaoGenericaViewModel(quantidadePorPagina, pagina, quantidadeFuncionarios);
                funcionarios = _funcionarioAplicacao.BuscarPorIntervaloOrdernadoPorAlias(paginacao.RegistroInicial, paginacao.RegistrosPorPagina, "Pessoa.Nome").ToList();
            }
            else
            {
                var predicate = PredicateBuilder.True<Funcionario>();

                if (supervisorId.HasValue)
                    predicate = predicate.And(x => x.Supervisor.Id == supervisorId.Value);

                if (funcionarioId.HasValue)
                    predicate = predicate.And(x => x.Id == funcionarioId.Value);

                funcionarios = _funcionarioAplicacao.BuscarPor(predicate).ToList();
                paginacao = new PaginacaoGenericaViewModel(quantidadePorPagina, pagina, funcionarios.Count);

                funcionarios = funcionarios.OrderBy(x => x.Pessoa.Nome).ToList();
                funcionarios = funcionarios.ItensDaPagina(paginacao.RegistroInicial, paginacao.RegistrosPorPagina).ToList();
            }

            ViewBag.Paginacao = paginacao;
            var presencaItensVM = funcionarios.Select(x => new FolhaPresencaFuncionarioViewModel(x)).ToList();

            foreach (var item in presencaItensVM)
            {
                if (FolhaPresencaFuncionarios.Any(x => x.Funcionario.Id == item.Funcionario.Id))
                    item.Selecionado = true;
            }

            ListaPresencaItem = presencaItensVM;

            return PartialView("_Grid", presencaItensVM);
        }

        public List<ChaveValorViewModel> RetornarAnos()
        {
            var anos = new List<ChaveValorViewModel>();
            var anoAtual = DateTime.Now.Year;

            for (int i = anoAtual; i < (anoAtual + 100); i++)
            {
                anos.Add(new ChaveValorViewModel { Id = i, Descricao = i.ToString() });
            }

            return anos;
        }

        public List<ChaveValorViewModel> RetornarMeses()
        {
            var meses = new List<ChaveValorViewModel>();

            for (int i = 1; i <= 12; i++)
            {
                var data = new DateTime(2000, i, 1);
                var nomeMes = data.ToString("MMMM").ToUpperCamelCase();
                meses.Add(new ChaveValorViewModel { Id = i, Descricao = $"{i.ToString()} - {nomeMes}" });
            }

            return meses;
        }

        public List<FolhaPresencaDiaViewModel> RetornarFolhaPresencaDias(int funcionarioId, int? ano = null, int? mes = null)
        {
            var controleFerias = _controleFeriasAplicacao.BuscarPor(x => x.Funcionario.Id == funcionarioId)?.ToList();

            var dataAtual = DateTime.Now;

            if (ano.HasValue && mes.HasValue)
                dataAtual = new DateTime(ano.Value, mes.Value, 20);
            else if (ano.HasValue)
                dataAtual = new DateTime(ano.Value, dataAtual.Month, 20);
            else if (mes.HasValue)
                dataAtual = new DateTime(dataAtual.Year, mes.Value, 20);

            var diaInicial = 20;

            if (dataAtual.Day < diaInicial)
                dataAtual = dataAtual.AddMonths(-1);

            var diasDiferenca = diaInicial - dataAtual.Day;
            dataAtual = dataAtual.AddDays(diasDiferenca);

            var dias = new List<FolhaPresencaDiaViewModel>();

            while (dataAtual.Day < diaInicial || dias.Count < 25)
            {
                var novoDia = new FolhaPresencaDiaViewModel
                {
                    Data = dataAtual,
                };

                novoDia.EhFeriado = _calendarioRHAplicacao.PrimeiroPor(x => (!x.DataFixa && x.Data.Date == novoDia.Data.Date) ||
                                                                        (x.DataFixa && x.Data.Day == novoDia.Data.Day && x.Data.Month == novoDia.Data.Month)) != null;

                novoDia.EhFerias = controleFerias.Any(x => novoDia.Data.Date >= x.DataInicial.Date && novoDia.Data.Date <= x.DataFinal.Date);
                dias.Add(novoDia);

                dataAtual = dataAtual.AddDays(1);
            }

            return dias;
        }
    }
}