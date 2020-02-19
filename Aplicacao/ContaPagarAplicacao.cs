using Aplicacao.Base;
using Aplicacao.ViewModels;
using PagamentoNet;
using Core.Extensions;
using Dominio;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using Core.Exceptions;

namespace Aplicacao
{
    public interface IContaPagarAplicacao : IBaseAplicacao<ContasAPagar>
    {
        IList<ContasAPagar> ListarContasPagar(int mes, int idUnidade);

        IList<ContasAPagar> ListarContasPagar(ContasAPagarViewModel filtro);

        void ExecutarPagamento(int Id, int idUsuario);

        void NegarConta(int id, string observacao, int usuarioId);

        void ExcluirLogicamentePorId(int id);

        void SalvarRecibos(List<ContasAPagarViewModel> contasAPagar);

        decimal CalcularDespesaTotal(ConsolidaAjusteFaturamentoViewModel filtro);

        decimal CalcularDespesaFixa(ConsolidaAjusteFaturamentoViewModel filtro);

        decimal CalcularDespesaEscolhida(ConsolidaAjusteFaturamentoViewModel filtro);

        IList<ContasAPagar> BuscarLancamentosPorFornecedor(int idFornecedor);
        List<ContasAPagar> BuscarPeloFiltro(GeracaoCNABContaPagarViewModel filtro);
        GeracaoCNABContaPagarViewModel GerarBoletosBancariosHtml(List<ContasAPagar> listaContaPagar, int usuarioId, GeracaoCNABContaPagarViewModel filtro);

        void Salvar(ContasAPagar contasAPagar, Usuario usuario);

        void Workflow(ContasAPagar contasAPagar, Usuario usuario);
    }

    public class ContaPagarAplicacao : BaseAplicacao<ContasAPagar, IContaPagarServico>, IContaPagarAplicacao
    {
        private readonly IContaPagarServico _contaPagarServico;
        private readonly IContaFinanceiraServico _contaFinanceiraServico;
        private readonly IUsuarioServico _usuarioServico;

        public ContaPagarAplicacao(
            IContaPagarServico contaPagarServico
            , IContaFinanceiraServico contaFinanceiraServico
            , IUsuarioServico usuarioServico
            )
        {
            _contaPagarServico = contaPagarServico;
            _contaFinanceiraServico = contaFinanceiraServico;
            _usuarioServico = usuarioServico;
        }

        public IList<ContasAPagar> ListarContasPagar(int mes, int idUnidade)
        {
            return _contaPagarServico.ListarContasPagar(mes, idUnidade);
        }

        public IList<ContasAPagar> ListarContasPagar(ContasAPagarViewModel filtro)
        {
            return _contaPagarServico.ListarContasPagar(filtro.ContaFinanceira?.Id, filtro?.DataVencimento, filtro.Departamento?.Id, filtro.TipoPagamento, filtro.Fornecedor?.Id, filtro.Unidade?.Id);
        }

        public void ExecutarPagamento(int Id, int idUsuario)
        {
            _contaPagarServico.ExecutarPagamento(Id, idUsuario);
        }

        public void ExcluirLogicamentePorId(int id)
        {
            _contaPagarServico.ExcluirLogicamentePorId(id);
        }

        public void SalvarRecibos(List<ContasAPagarViewModel> contasAPagar)
        {
            var contasAPagarIds = contasAPagar.Select(x => x.Id);
            var contas = _contaPagarServico.BuscarPor(x => contasAPagarIds.Contains(x.Id));

            foreach (var conta in contas)
            {
                var numeroRecibo = contasAPagar.Find(x => x.Id == conta.Id)?.NumeroRecibo;

                conta.NumeroRecibo = numeroRecibo;

                _contaPagarServico.DeletarNotificacao(conta, "A retirada de cofre para a conta de Id");
            }

            _contaPagarServico.Salvar(contas);
        }

        public decimal CalcularDespesaTotal(ConsolidaAjusteFaturamentoViewModel filtro)
        {
            if (filtro.Unidade != null)
                return _contaPagarServico.CalcularDespesaTotal(filtro.Empresa.Id, filtro.Unidade.Id, (int)filtro.Mes, filtro.Ano);
            else
                return _contaPagarServico.CalcularDespesaTotal(filtro.Empresa.Id, null, (int)filtro.Mes, filtro.Ano);
        }

        public decimal CalcularDespesaFixa(ConsolidaAjusteFaturamentoViewModel filtro)
        {
            if (filtro.Unidade != null)
                return _contaPagarServico.CalcularDespesaFixa(filtro.Empresa.Id, filtro.Unidade.Id, (int)filtro.Mes, filtro.Ano);
            else
                return _contaPagarServico.CalcularDespesaFixa(filtro.Empresa.Id, null, (int)filtro.Mes, filtro.Ano);
        }

        public decimal CalcularDespesaEscolhida(ConsolidaAjusteFaturamentoViewModel filtro)
        {
            if (filtro.Unidade != null)
                return _contaPagarServico.CalcularDespesaEscolhida(filtro.Empresa.Id, filtro.Unidade.Id, (int)filtro.Mes, filtro.Ano);
            else
                return _contaPagarServico.CalcularDespesaEscolhida(filtro.Empresa.Id, null, (int)filtro.Mes, filtro.Ano);
        }

        public IList<ContasAPagar> BuscarLancamentosPorFornecedor(int idFornecedor)
        {
            return _contaPagarServico.BuscarLancamentosPorFornecedor(idFornecedor);
        }

        public void NegarConta(int id, string observacao, int usuarioId)
        {
            var usuario = _usuarioServico.BuscarPorId(usuarioId);
            _contaPagarServico.NegarConta(id, observacao, usuario);
        }

        public List<ContasAPagar> BuscarPeloFiltro(GeracaoCNABContaPagarViewModel filtro)
        {
            Expression<Func<ContasAPagar, bool>> predicate = x => x.ContaFinanceira.Id == filtro.ContaFinanceira.Id;

            var somenteGerados = filtro.TipoFiltro == TipoFiltroGeracaoCNAB.Gerados;

            if (filtro.FormaPagamento > 0)
            {
                predicate = predicate.And(x => x.FormaPagamento == filtro.FormaPagamento);
            }
            else
            {
                predicate = predicate.And(x => x.FormaPagamento == FormaPagamento.Boleto || x.FormaPagamento == FormaPagamento.Doc || x.FormaPagamento == FormaPagamento.Ted || x.FormaPagamento == FormaPagamento.DDA || x.FormaPagamento == FormaPagamento.ImpostoComCodigo || x.FormaPagamento == FormaPagamento.BoletoConcessionaria);
            }

            if (filtro.Unidade != null && filtro.Unidade.Id > 0)
            {
                predicate = predicate.And(x => x.Unidade.Id == filtro.Unidade.Id);
            }
            if (filtro.ContaContabil != null && filtro.ContaContabil.Id > 0)
            {
                predicate = predicate.And(x => x.ContaContabil.Id == filtro.ContaContabil.Id);
            }
            if (filtro.Fornecedor != null && filtro.Fornecedor.Id > 0)
            {
                predicate = predicate.And(x => x.Fornecedor.Id == filtro.Fornecedor.Id);
            }

            if (filtro.DataVencimento.HasValue)
            {
                predicate = predicate.And(x => x.DataVencimento.Date == filtro.DataVencimento.Value.Date);
            }

            if (somenteGerados)
            {
                predicate = predicate.And(x => x.PossueCnab == true);
            }
            else
            {
                predicate = predicate.And(x => x.PossueCnab == false);
            }

            return _contaPagarServico.BuscarPor(predicate).ToList();
        }

        public GeracaoCNABContaPagarViewModel GerarBoletosBancariosHtml(List<ContasAPagar> listaContaPagar, int usuarioId, GeracaoCNABContaPagarViewModel filtro)
        {
            var listaBoletoBancario = new List<BoletoBancario>();
            var listaBoletos = new Boletos();

            var contaFinanceira = _contaFinanceiraServico.BuscarPorId(filtro.ContaFinanceira.Id);

            foreach (var item in listaContaPagar)
            {
                if ((item.FormaPagamento == FormaPagamento.Boleto || item.FormaPagamento == FormaPagamento.BoletoConcessionaria) && string.IsNullOrEmpty(item.CodigoDeBarras))
                    throw new BusinessRuleException($"O Boleto de Id [" + item.Id + "], não possuí código de barras!");

                var boleto = ImprimirBoletoBancario(item, contaFinanceira);
                var boletoBancario = new BoletoBancario()
                {
                    CodigoBanco = Convert.ToInt16(contaFinanceira.Banco.CodigoBanco),
                    Boleto = boleto,
                    MostrarCodigoCarteira = true
                };

                boletoBancario.MostrarComprovanteEntrega = false;
                boletoBancario.FormatoCarne = true;
                boletoBancario.GeraImagemCodigoBarras(boleto);

                listaBoletos.Add(boleto);
                listaBoletoBancario.Add(boletoBancario);

                item.PossueCnab = true;
                _contaPagarServico.Workflow(item, new Usuario { Id = usuarioId });
            }

            var objCedente = new Cedente(
                contaFinanceira.Cnpj,
                contaFinanceira.Descricao,
                contaFinanceira.Agencia,
                contaFinanceira.Conta,
                contaFinanceira.DigitoConta
            )
            {
                CodigoTransmissao = $"{contaFinanceira.Agencia}0{contaFinanceira.ConvenioPagamento.Truncate(7).PadLeft(7, '0')}0{contaFinanceira.Conta.Truncate(7).PadLeft(7, '0')}",
                Convenio = Convert.ToInt64(contaFinanceira.ConvenioPagamento),
                ContaBancaria = new ContaBancaria(contaFinanceira.Agencia, contaFinanceira.DigitoAgencia ?? string.Empty, contaFinanceira.Conta, contaFinanceira.DigitoConta),
                Nome = contaFinanceira.Empresa?.RazaoSocial
            };

            var aqvRemessa = new ArquivoRemessa(PagamentoNet.TipoArquivo.CNAB240);
            var banco = new PagamentoNet.Banco(Convert.ToInt32(contaFinanceira.Banco.CodigoBanco));

            using (var mem = new MemoryStream())
            {
                if (listaBoletos.Any())
                    aqvRemessa.GerarArquivoRemessa(contaFinanceira.ConvenioPagamento, banco, objCedente, listaBoletos, mem, Convert.ToInt32(contaFinanceira.CodigoTransmissao));

                return new GeracaoCNABContaPagarViewModel
                {
                    BoletosHtml = new List<string>(),
                    ArquivoRemessaMemoryStream = mem
                };
            }
        }

        private PagamentoNet.Boleto ImprimirBoletoBancario(ContasAPagar contaPagar, ContaFinanceira contaFinanceira)
        {
            return _contaPagarServico.ImprimirBoletoBancario(contaPagar, contaFinanceira);
        }

        public void Salvar(ContasAPagar contasAPagar, Usuario usuario)
        {
            _contaPagarServico.Salvar(contasAPagar, usuario);
        }

        public void Workflow(ContasAPagar contasAPagar, Usuario usuario)
        {
            _contaPagarServico.Workflow(contasAPagar, usuario);
        }
    }
}