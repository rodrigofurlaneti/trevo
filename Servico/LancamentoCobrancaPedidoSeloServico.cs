using BoletoNet;
using Core.Exceptions;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dominio
{
    public interface ILancamentoCobrancaPedidoSeloServico : IBaseServico<LancamentoCobrancaPedidoSelo>
    {
        void Salvar(PedidoSelo pedidoSelo, decimal valor);
        string GerarBoletoBancarioHtml(LancamentoCobranca lancamento);
        LancamentoCobrancaPedidoSelo RetornaUltimoLancamentoCobrancaPorPedidoSelo(int idPedido);
        string GerarBoletoBancarioImpressao(LancamentoCobranca lancamento);
        StatusLancamentoCobranca RetornaStatusPorPedidoSelo(int idPedidoSelo);
    }

    public class LancamentoCobrancaPedidoSeloServico : BaseServico<LancamentoCobrancaPedidoSelo, ILancamentoCobrancaPedidoSeloRepositorio>, ILancamentoCobrancaPedidoSeloServico
    {
        private readonly ILancamentoCobrancaServico _lancamentoCobrancaServico;
        private readonly ILancamentoCobrancaPedidoSeloRepositorio _lancamentoCobrancaPedidoSeloRepositorio;
        private readonly IContaFinanceiraRepositorio _contaFinanceiraRepositorio;
        private readonly IConvenioRepositorio _convenioRepositorio;
        private readonly IPedidoSeloRepositorio _pedidoSeloRepositorio;
        private readonly IParametroBoletoBancarioRepositorio _parametroBoletoBancarioRepositorio;

        public LancamentoCobrancaPedidoSeloServico(
            ILancamentoCobrancaServico lancamentoCobrancaServico,
            ILancamentoCobrancaPedidoSeloRepositorio lancamentoCobrancaPedidoSeloRepositorio,
            IContaFinanceiraRepositorio contaFinanceiraRepositorio,
            IConvenioRepositorio convenioRepositorio,
            IPedidoSeloRepositorio pedidoSeloRepositorio,
            IParametroBoletoBancarioRepositorio parametroBoletoBancarioRepositorio)
        {
            _lancamentoCobrancaServico = lancamentoCobrancaServico;
            _lancamentoCobrancaPedidoSeloRepositorio = lancamentoCobrancaPedidoSeloRepositorio;
            _contaFinanceiraRepositorio = contaFinanceiraRepositorio;
            _convenioRepositorio = convenioRepositorio;
            _pedidoSeloRepositorio = pedidoSeloRepositorio;
            _parametroBoletoBancarioRepositorio = parametroBoletoBancarioRepositorio;
        }

        public void Salvar(PedidoSelo pedidoSelo, decimal valor)
        {
            var entidade = new LancamentoCobrancaPedidoSelo
            {
                StatusLancamentoCobranca = StatusLancamentoCobranca.EmAberto,
                Cliente = pedidoSelo.Cliente,
                ContaFinanceira = _contaFinanceiraRepositorio.BuscarContaPadrao(),
                DataBaixa = null,
                DataGeracao = DateTime.Now,
                DataInsercao = DateTime.Now,
                DataVencimento = pedidoSelo.DataVencimento, //RetornaDataVencimento(DateTime.Now, pedidoSelo.DiasVencimento),
                DataCompetencia = new DateTime(pedidoSelo.DataVencimento.Year, pedidoSelo.DataVencimento.Month, 1),
                PedidoSelo = pedidoSelo,
                TipoServico = TipoServico.Convenio,
                Unidade = pedidoSelo.Unidade,
                ValorContrato = valor,
                PossueCnab = false,
                Recebimento = null,
                ValorJuros = 0,
                ValorMulta = 0
            };

            _lancamentoCobrancaPedidoSeloRepositorio.Save(entidade);
        }

        private DateTime RetornaDataVencimento(DateTime data, int diasParaVencimento)
        {
            data = data.AddDays(diasParaVencimento);
            while (FinalDeSemanaOuFeriado(data))
                data = data.AddDays(1);

            return data;
        }

        private bool FinalDeSemanaOuFeriado(DateTime data)
        {
            return FinalDeSemana(data) || Feriado(data);
        }

        private bool FinalDeSemana(DateTime data)
        {
            return data.DayOfWeek == DayOfWeek.Saturday || data.DayOfWeek == DayOfWeek.Monday;
        }

        private bool Feriado(DateTime data)
        {
            return false;
        }

        public LancamentoCobrancaPedidoSelo RetornaUltimoLancamentoCobrancaPorPedidoSelo(int idPedido)
        {
            return _lancamentoCobrancaPedidoSeloRepositorio.RetornaUltimoLancamentoCobrancaPorPedidoSelo(idPedido);
        }

        public string GerarBoletoBancarioHtml(LancamentoCobranca lancamento)
        {
            var contaFinanceira = lancamento?.ContaFinanceira ?? new ContaFinanceira();

            if (lancamento.Cliente.Pessoa.Enderecos == null || !lancamento.Cliente.Pessoa.Enderecos.Any())
                throw new BusinessRuleException($"O cadastro de [{lancamento.Cliente.Pessoa.Nome}], não possui Endereço. É necessário esta informação para prosseguir!");

            var listaParametroBoletoBancario = _parametroBoletoBancarioRepositorio.List()?.ToList() ?? new List<ParametroBoletoBancario>();

            var boleto = _lancamentoCobrancaServico.ImprimirBoletoBancario(lancamento, contaFinanceira, lancamento.DataVencimento, TipoValor.Monetario, null, TipoValor.Monetario, null, listaParametroBoletoBancario, TipoOcorrenciaCNAB.ENTRADA);
            var boletoBancario = new BoletoBancario()
            {
                CodigoBanco = Convert.ToInt16(contaFinanceira.Banco.CodigoBanco),
                Boleto = boleto,
                MostrarCodigoCarteira = true
            };

            boletoBancario.Boleto.Valida();
            boletoBancario.MostrarComprovanteEntrega = false;
            boletoBancario.FormatoCarne = true;
            boletoBancario.GeraImagemCodigoBarras(boleto);

            lancamento.PossueCnab = true;
            _lancamentoCobrancaServico.Salvar(lancamento);

            var objCedente = new Cedente(
                contaFinanceira.Cpf,
                contaFinanceira.Descricao,
                contaFinanceira.Agencia,
                contaFinanceira.Conta,
                contaFinanceira.DigitoConta
            );
            var aqvRemessa = new BoletoNet.ArquivoRemessa(BoletoNet.TipoArquivo.CNAB400);
            var banco = new BoletoNet.Banco(Convert.ToInt32(contaFinanceira.Banco.CodigoBanco));

            return boletoBancario.MontaHtmlEmbedded();
        }

        public string GerarBoletoBancarioImpressao(LancamentoCobranca lancamento)
        {
            var contaFinanceira = lancamento?.ContaFinanceira ?? new ContaFinanceira();

            if (lancamento.Cliente.Pessoa.Enderecos == null || !lancamento.Cliente.Pessoa.Enderecos.Any())
                throw new BusinessRuleException($"O cadastro de [{lancamento.Cliente.Pessoa.Nome}], não possui Endereço. É necessário esta informação para prosseguir!");

            var listaParametroBoletoBancario = _parametroBoletoBancarioRepositorio.List()?.ToList() ?? new List<ParametroBoletoBancario>();

            var boleto = _lancamentoCobrancaServico.ImprimirBoletoBancario(lancamento, contaFinanceira, lancamento.DataVencimento, TipoValor.Monetario, null, TipoValor.Monetario, null, listaParametroBoletoBancario, TipoOcorrenciaCNAB.ENTRADA);
            var boletoBancario = new BoletoBancario()
            {
                CodigoBanco = Convert.ToInt16(contaFinanceira.Banco.CodigoBanco),
                Boleto = boleto,
                MostrarCodigoCarteira = true
            };

            boletoBancario.Boleto.Valida();
            boletoBancario.MostrarComprovanteEntrega = false;
            boletoBancario.FormatoCarne = true;
            boletoBancario.GeraImagemCodigoBarras(boleto);
            
            return boletoBancario.MontaHtmlEmbedded();
        }

        public StatusLancamentoCobranca RetornaStatusPorPedidoSelo(int idPedidoSelo)
        {
            return _lancamentoCobrancaPedidoSeloRepositorio.RetornaStatusPorPedidoSelo(idPedidoSelo);
        }
    }
}