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
    public interface IControlePontoFeriasAplicacao : IBaseAplicacao<ControlePontoFerias>
    {
        List<ControlePontoFeriasDiaViewModel> RetornarControlePontoFeriasDias(ControlePontoFeriasViewModel controlePontoFeriasViewModel, int? ano = null, int? mes = null);
        void BuscarFuncionarios(int quantidadePorPagina, ref PaginacaoGenericaViewModel paginacao, ref List<Funcionario> funcionarios, BuscarGridControlePontoFeriasFuncionarioViewModel busca, int pagina = 1);
        ControlePontoFerias Salvar(int funcionarioId, ControlePontoFeriasDiaViewModel dto, List<ControlePontoFeriasDiaViewModel> listaControlePontoFeriasDia);
    }

    public class ControlePontoFeriasAplicacao : BaseAplicacao<ControlePontoFerias, IControlePontoFeriasServico>, IControlePontoFeriasAplicacao
    {
        private readonly IFuncionarioServico _funcionarioServico;
        private readonly IControlePontoFeriasServico _controlePontoFeriasServico;
        private readonly IControleFeriasServico _controleFeriasServico;
        private readonly ICalendarioRHServico _calendarioRHServico;

        public ControlePontoFeriasAplicacao(
            IFuncionarioServico funcionarioServico,
            IControlePontoFeriasServico controlePontoFeriasServico,
            IControleFeriasServico controleFeriasServico,
            ICalendarioRHServico calendarioRHServico
            )
        {
            _funcionarioServico = funcionarioServico;
            _controlePontoFeriasServico = controlePontoFeriasServico;
            _controleFeriasServico = controleFeriasServico;
            _calendarioRHServico = calendarioRHServico;
        }

        public void BuscarFuncionarios(int quantidadePorPagina, ref PaginacaoGenericaViewModel paginacao, ref List<Funcionario> funcionarios, BuscarGridControlePontoFeriasFuncionarioViewModel busca, int pagina = 1)
        {
            if (!busca.UnidadeId.HasValue &&
                !busca.SupervisorId.HasValue &&
                !busca.FuncionarioId.HasValue &&
                string.IsNullOrEmpty(busca.ColunaUnidade) &&
                string.IsNullOrEmpty(busca.ColunaSupervisor) &&
                string.IsNullOrEmpty(busca.ColunaFuncionario))
            {
                var quantidadeFuncionarios = _controleFeriasServico.BuscarPor(x => x.AutorizadoTrabalhar).Count;
                paginacao = new PaginacaoGenericaViewModel(quantidadePorPagina, pagina, quantidadeFuncionarios);
                var controleferias = _controleFeriasServico.BuscarPor(x => x.AutorizadoTrabalhar && x.ListaPeriodoPermitido.Any()).ToList();
                funcionarios = controleferias.Select(x => x.Funcionario).Distinct().ToList();
                funcionarios = funcionarios.OrderBy(x => x.Pessoa.Nome)?.ItensDaPagina(paginacao.RegistroInicial, paginacao.RegistrosPorPagina)?.ToList();
            }
            else
            {
                var predicate = PredicateBuilder.True<ControleFerias>();

                predicate = predicate.And(x => x.AutorizadoTrabalhar && x.ListaPeriodoPermitido.Any());

                if (busca.UnidadeId.HasValue)
                    predicate = predicate.And(x => x.Funcionario.Unidade.Id == busca.UnidadeId.Value);

                if (busca.SupervisorId.HasValue)
                    predicate = predicate.And(x => x.Funcionario.Supervisor.Id == busca.SupervisorId.Value);

                if (busca.FuncionarioId.HasValue)
                    predicate = predicate.And(x => x.Funcionario.Id == busca.FuncionarioId.Value);

                if (!string.IsNullOrEmpty(busca.ColunaUnidade))
                    predicate = predicate.And(x => x.Funcionario.Unidade.Nome.ToLower().Contains(busca.ColunaUnidade.ToLower()));

                if (!string.IsNullOrEmpty(busca.ColunaSupervisor))
                    predicate = predicate.And(x => x.Funcionario.Supervisor.Pessoa.Nome.ToLower().Contains(busca.ColunaSupervisor.ToLower()));

                if (!string.IsNullOrEmpty(busca.ColunaFuncionario))
                    predicate = predicate.And(x => x.Funcionario.Pessoa.Nome.ToLower().Contains(busca.ColunaFuncionario.ToLower()));

                funcionarios = _controleFeriasServico.BuscarPor(predicate)?.Select(x => x.Funcionario)?.ToList();
                paginacao = new PaginacaoGenericaViewModel(quantidadePorPagina, pagina, funcionarios.Count);

                funcionarios = funcionarios.Distinct().OrderBy(x => x.Pessoa.Nome)?.ItensDaPagina(paginacao.RegistroInicial, paginacao.RegistrosPorPagina)?.ToList();
            }
        }

        public List<ControlePontoFeriasDiaViewModel> RetornarControlePontoFeriasDias(ControlePontoFeriasViewModel controlePontoFeriasViewModel, int? ano = null, int? mes = null)
        {
            var controleFerias = _controleFeriasServico.BuscarPor(x => x.Funcionario.Id == controlePontoFeriasViewModel.Funcionario.Id)?.ToList();

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

            var controlePontoFeriasDias = new List<ControlePontoFeriasDiaViewModel>();

            if (controlePontoFeriasViewModel.ControlePontoFeriasDias != null && controlePontoFeriasViewModel.ControlePontoFeriasDias.Any()) 
            {
                var dias = controlePontoFeriasViewModel.ControlePontoFeriasDias.Where(x => x.Data.Date >= dataAtual.Date && x.Data.Date <= dataAtual.AddMonths(1).Date).ToList();

                foreach (var dia in dias)
                {
                    dia.EhFeriado = _calendarioRHServico.PrimeiroPor(x => (!x.DataFixa && x.Data.Date == dia.Data.Date) ||
                                                                        (x.DataFixa && x.Data.Day == dia.Data.Day && x.Data.Month == dia.Data.Month)) != null;
                }

                controlePontoFeriasDias.AddRange(dias);
            }

            var listaControleFerias = _controleFeriasServico.BuscarPor(x => x.Funcionario.Id == controlePontoFeriasViewModel.Funcionario.Id && 
                                                                            x.AutorizadoTrabalhar &&
                                                                            x.ListaPeriodoPermitido.Any(
                                                                                p => p.DataDe.Date >= dataAtual.Date && p.DataAte.Date <= dataAtual.AddMonths(1).Date ||
                                                                                p.DataDe.Date >= dataAtual.Date && p.DataAte.Date <= dataAtual.AddMonths(1).Date
                                                                            ));

            foreach (var item in listaControleFerias)
            {
                foreach (var periodo in item.ListaPeriodoPermitido)
                {
                    int diasParaAdd = 0;
                    var dias = new List<ControlePontoFeriasDiaViewModel>();

                    var novoDia = periodo.DataDe;
                    while (novoDia.Date < periodo.DataAte)
                    {
                        novoDia = periodo.DataDe.AddDays(diasParaAdd);
                        var controlePontoDia = new ControlePontoFeriasDiaViewModel
                        {
                            Data = novoDia
                        };

                        controlePontoDia.EhFeriado = _calendarioRHServico.PrimeiroPor(x => (!x.DataFixa && x.Data.Date == controlePontoDia.Data.Date) ||
                                                                                    (x.DataFixa && x.Data.Day == controlePontoDia.Data.Day && x.Data.Month == controlePontoDia.Data.Month)) != null;

                        dias.Add(controlePontoDia);
                        diasParaAdd++;
                    }

                    dias = dias.Where(x => x.Data.Date >= dataAtual.Date && x.Data.Date <= dataAtual.AddMonths(1).Date).ToList();
                    dias = dias.Where(x => !controlePontoFeriasDias.Any(cp => cp.Data.Date == x.Data.Date)).ToList();
                    controlePontoFeriasDias.AddRange(dias);
                }
            }

            return controlePontoFeriasDias.OrderBy(x => x.Data).ToList();
        }

        public ControlePontoFerias Salvar(int funcionarioId, ControlePontoFeriasDiaViewModel dto, List<ControlePontoFeriasDiaViewModel> listaControlePontoFeriasDia)
        {
            var controleRhDia = listaControlePontoFeriasDia.FirstOrDefault(x => x.Data.Date == dto.Data.Date);

            controleRhDia.Data = dto.Data;
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
            controleRhDia.CalcularHoraExtra();
            controleRhDia.CalcularHoraAtraso();
            controleRhDia.CalcularAdicionalNoturno();

            var controlePontoFerias = _controlePontoFeriasServico.PrimeiroPor(x => x.Funcionario.Id == funcionarioId);

            if (controlePontoFerias == null)
            {
                controlePontoFerias = new ControlePontoFerias();
                controlePontoFerias.Funcionario = _funcionarioServico.BuscarPorId(funcionarioId);
            }

            if (controlePontoFerias.ControlePontoFeriasDias == null)
                controlePontoFerias.ControlePontoFeriasDias = new List<ControlePontoFeriasDia>();

            var dias = Mapper.Map<IList<ControlePontoFeriasDia>>(listaControlePontoFeriasDia);
            foreach (var dia in dias)
            {
                var diaParaRemover = controlePontoFerias.ControlePontoFeriasDias.FirstOrDefault(x => x.Id != 0 && x.Id == dia.Id);
                if(diaParaRemover != null)
                    controlePontoFerias.ControlePontoFeriasDias.Remove(diaParaRemover);

                controlePontoFerias.ControlePontoFeriasDias.Add(dia);
            }

            _controlePontoFeriasServico.Salvar(controlePontoFerias);
            return _controlePontoFeriasServico.BuscarPorId(controlePontoFerias.Id);
        }
    }
}