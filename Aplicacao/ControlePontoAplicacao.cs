using System;
using System.Collections.Generic;
using System.Linq;
using Aplicacao.Base;
using Aplicacao.ViewModels;
using AutoMapper;
using Core.Extensions;
using Dominio;
using Entidade;
using Entidade.Uteis;

namespace Aplicacao
{
    public interface IControlePontoAplicacao : IBaseAplicacao<ControlePonto>
    {
        List<ControlePontoDiaViewModel> RetornarControlePontoDias(ControlePontoViewModel controlePontoViewModel, int? ano = null, int? mes = null);
        void BuscarFuncionarios(int quantidadePorPagina, ref PaginacaoGenericaViewModel paginacao, ref List<Funcionario> funcionarios, BuscarGridControlePontoFuncionarioViewModel busca, int pagina = 1);
        List<ChaveValorViewModel> BuscarFuncionariosDoSupervisorChaveValor(int supervisorId);
        List<FuncionarioViewModel> BuscarFuncionariosDoSupervisor(int supervisorId);
        ControlePonto Salvar(int funcionarioId, ControlePontoDiaViewModel dto, List<ControlePontoDiaViewModel> listaControlePontoDia);
    }

    public class ControlePontoAplicacao : BaseAplicacao<ControlePonto, IControlePontoServico>, IControlePontoAplicacao
    {
        private readonly IFuncionarioServico _funcionarioServico;
        private readonly IControlePontoServico _controlePontoServico;
        private readonly IControleFeriasServico _controleFeriasServico;
        private readonly ICalendarioRHServico _calendarioRHServico;

        public ControlePontoAplicacao(
            IFuncionarioServico funcionarioServico,
            IControlePontoServico controlePontoServico,
            IControleFeriasServico controleFeriasServico,
            ICalendarioRHServico calendarioRHServico
            )
        {
            _funcionarioServico = funcionarioServico;
            _controlePontoServico = controlePontoServico;
            _controleFeriasServico = controleFeriasServico;
            _calendarioRHServico = calendarioRHServico;
        }

        public void BuscarFuncionarios(int quantidadePorPagina, ref PaginacaoGenericaViewModel paginacao, ref List<Funcionario> funcionarios, BuscarGridControlePontoFuncionarioViewModel busca, int pagina = 1)
        {
            if (!busca.UnidadeId.HasValue &&
                !busca.SupervisorId.HasValue &&
                !busca.FuncionarioId.HasValue &&
                string.IsNullOrEmpty(busca.ColunaUnidade) &&
                string.IsNullOrEmpty(busca.ColunaSupervisor) &&
                string.IsNullOrEmpty(busca.ColunaFuncionario))
            {
                var quantidadeFuncionarios = _funcionarioServico.Contar();
                paginacao = new PaginacaoGenericaViewModel(quantidadePorPagina, pagina, quantidadeFuncionarios);
                funcionarios = _funcionarioServico.BuscarPorIntervaloOrdernadoPorAlias(paginacao.RegistroInicial, paginacao.RegistrosPorPagina, "Pessoa.Nome").ToList();
            }
            else
            {
                var predicate = PredicateBuilder.True<Funcionario>();

                if (busca.UnidadeId.HasValue)
                    predicate = predicate.And(x => x.Unidade.Id == busca.UnidadeId.Value);

                if (busca.SupervisorId.HasValue)
                    predicate = predicate.And(x => x.Supervisor.Id == busca.SupervisorId.Value);

                if (busca.FuncionarioId.HasValue)
                    predicate = predicate.And(x => x.Id == busca.FuncionarioId.Value);

                if (!string.IsNullOrEmpty(busca.ColunaUnidade))
                    predicate = predicate.And(x => x.Unidade.Nome.ToLower().Contains(busca.ColunaUnidade.ToLower()));

                if (!string.IsNullOrEmpty(busca.ColunaSupervisor))
                    predicate = predicate.And(x => x.Supervisor.Pessoa.Nome.ToLower().Contains(busca.ColunaSupervisor.ToLower()));

                if (!string.IsNullOrEmpty(busca.ColunaFuncionario))
                    predicate = predicate.And(x => x.Pessoa.Nome.ToLower().Contains(busca.ColunaFuncionario.ToLower()));

                funcionarios = _funcionarioServico.BuscarPor(predicate).ToList();
                paginacao = new PaginacaoGenericaViewModel(quantidadePorPagina, pagina, funcionarios.Count);

                funcionarios = funcionarios.OrderBy(x => x.Pessoa.Nome).ToList();
                funcionarios = funcionarios.ItensDaPagina(paginacao.RegistroInicial, paginacao.RegistrosPorPagina).ToList();
            }
        }

        public List<ChaveValorViewModel> BuscarFuncionariosDoSupervisorChaveValor(int supervisorId)
        {
            var listaFuncionario = _funcionarioServico.BuscarFuncionariosDoSupervisor(supervisorId);

            var listaChaveValor = new List<ChaveValorViewModel>();

            foreach (var funcionario in listaFuncionario)
            {
                listaChaveValor.Add(new ChaveValorViewModel
                {
                    Id = funcionario.Id,
                    Descricao = $"{funcionario.Pessoa.Nome} - {funcionario.Unidade.Nome}"
                });
            }

            return listaChaveValor;
        }

        public List<FuncionarioViewModel> BuscarFuncionariosDoSupervisor(int supervisorId)
        {
            var listaFuncionario = _funcionarioServico.BuscarFuncionariosDoSupervisor(supervisorId);
            return listaFuncionario.Select(x => new FuncionarioViewModel(x)).ToList();
        }

        public List<ControlePontoDiaViewModel> RetornarControlePontoDias(ControlePontoViewModel controlePontoViewModel, int? ano = null, int? mes = null)
        {
            var controleFerias = _controleFeriasServico.BuscarPor(x => x.Funcionario.Id == controlePontoViewModel.Funcionario.Id)?.ToList();
            var dataAtual = DateTime.Now;

            var diaInicial = 20;

            if (ano.HasValue && mes.HasValue)
                dataAtual = new DateTime(ano.Value, mes.Value, 20);
            else if (ano.HasValue)
                dataAtual = new DateTime(ano.Value, dataAtual.Month, 20);
            else if (mes.HasValue)
                dataAtual = new DateTime(dataAtual.Year, mes.Value, 20);

            if (dataAtual.Day < diaInicial)
                dataAtual = dataAtual.AddMonths(-1);

            var diasDiferenca = diaInicial - dataAtual.Day;
            dataAtual = dataAtual.AddDays(diasDiferenca);

            if (controlePontoViewModel.ControlePontoDias != null && controlePontoViewModel.ControlePontoDias.Any())
            {
                var dias = controlePontoViewModel.ControlePontoDias.Where(x => x.Data.Date >= dataAtual.Date && x.Data.Date < dataAtual.AddMonths(1).Date).ToList();

                foreach (var dia in dias)
                {
                    dia.EhFeriado = _calendarioRHServico.PrimeiroPor(x => (!x.DataFixa && x.Data.Date == dia.Data.Date) ||
                                                                        (x.DataFixa && x.Data.Day == dia.Data.Day && x.Data.Month == dia.Data.Month)) != null;
                }

                if (dias.Any())
                    return dias;
            }

            var controlePontoDias = new List<ControlePontoDiaViewModel>();

            while (dataAtual.Day < diaInicial || controlePontoDias.Count < 25)
            {
                var novoDia = new ControlePontoDiaViewModel
                {
                    Data = dataAtual,
                };

                novoDia.EhFeriado = _calendarioRHServico.PrimeiroPor(x => (!x.DataFixa && x.Data.Date == novoDia.Data.Date) ||
                                                                        (x.DataFixa && x.Data.Day == novoDia.Data.Day && x.Data.Month == novoDia.Data.Month)) != null;

                novoDia.EhFerias = controleFerias.Any(x => novoDia.Data.Date >= x.DataInicial.Date && novoDia.Data.Date <= x.DataFinal.Date);
                controlePontoDias.Add(novoDia);

                dataAtual = dataAtual.AddDays(1);
            }

            return controlePontoDias;
        }

        public ControlePonto Salvar(int funcionarioId, ControlePontoDiaViewModel dto, List<ControlePontoDiaViewModel> listaControlePontoDia)
        {
            var controlePonto = _controlePontoServico.PrimeiroPor(x => x.Funcionario.Id == funcionarioId);

            if (controlePonto == null)
            {
                controlePonto = new ControlePonto();
                controlePonto.Funcionario = _funcionarioServico.BuscarPorId(funcionarioId);
            }

            if (controlePonto.ControlePontoDias == null)
                controlePonto.ControlePontoDias = new List<ControlePontoDia>();

            var controleRhDia = listaControlePontoDia.FirstOrDefault(x => x.Data.Date == dto.Data.Date);
            controleRhDia.Data = dto.Data;
            controleRhDia.Folga = dto.Folga;
            controleRhDia.Falta = dto.Falta;
            controleRhDia.FaltaJustificada = dto.FaltaJustificada;
            controleRhDia.Atraso = dto.Atraso;
            controleRhDia.Atestado = dto.Atestado;
            controleRhDia.AtrasoJustificado = dto.AtrasoJustificado;
            controleRhDia.Suspensao = dto.Suspensao;
            controleRhDia.Observacao = dto.Observacao;
            controleRhDia.HorarioEntrada = dto.HorarioEntrada;
            controleRhDia.HorarioSaidaAlmoco = dto.HorarioSaidaAlmoco;
            controleRhDia.HorarioRetornoAlmoco = dto.HorarioRetornoAlmoco;
            controleRhDia.HorarioSaida = dto.HorarioSaida;
            controleRhDia.CalcularHorasDiaTime();
            controleRhDia.CalcularHorasDia();
            controleRhDia.CalcularHoraExtra(controlePonto.Funcionario.TipoEscala);
            controleRhDia.CalcularHoraAtraso();
            controleRhDia.CalcularAdicionalNoturno(controlePonto.Funcionario.TipoEscala);

            var dias = Mapper.Map<IList<ControlePontoDia>>(listaControlePontoDia);
            foreach (var dia in dias)
            {
                var diaParaRemover = controlePonto.ControlePontoDias.FirstOrDefault(x => x.Id != 0 && x.Id == dia.Id);
                if(diaParaRemover != null)
                    controlePonto.ControlePontoDias.Remove(diaParaRemover);

                controlePonto.ControlePontoDias.Add(dia);
            }

            _controlePontoServico.Salvar(controlePonto);
            return _controlePontoServico.BuscarPorId(controlePonto.Id);
        }
    }
}