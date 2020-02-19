using PagamentoNet;
using Core.Exceptions;
using Core.Extensions;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dominio
{
    public interface IContaPagarServico : IBaseServico<ContasAPagar>
    {
        IList<ContasAPagar> ListarContasPagar(int mes, int idUnidade);
        IList<ContasAPagar> ListarContasPagar(int? idContaPagar);
        IList<ContasAPagar> ListarContasPagar(int? idContaFinanceira, DateTime? data, int? departamento, TipoContaPagamento tipopagamento, int? fornecedor, int? unidade);
        void ExecutarPagamento(int Id, int idUsuario);
        void NegarConta(int id, string observacao, Usuario usuario);
        void ExcluirLogicamentePorId(int id);
        decimal CalcularDespesaTotal(int empresa, int? unidade, int mes, int ano);
        decimal CalcularDespesaFixa(int empresa, int? unidade, int mes, int ano);
        decimal CalcularDespesaEscolhida(int empresa, int? unidade, int mes, int ano);
        IList<ContasAPagar> BuscarLancamentosPorFornecedor(int idFornecedor);
        ContasAPagar BuscarPelaNotificacaoId(int notificacaoId);
        void DeletarNotificacao(ContasAPagar contasAPagar, string descricao = "");
        PagamentoNet.Boleto ImprimirBoletoBancario(ContasAPagar contaPagar, ContaFinanceira contaFinanceira);
        void Salvar(ContasAPagar contasAPagar, Usuario usuario);
        void AtualizarStatus(int idNotificacao, Usuario usuario, AcaoNotificacao acao);
        void Workflow(ContasAPagar contasAPagar, Usuario usuario);
    }

    public class ContaPagarServico : BaseServico<ContasAPagar, IContaPagarRepositorio>, IContaPagarServico
    {
        private readonly IContaPagarRepositorio _contasPagarRepositorio;
        private readonly IContaContabilRepositorio _contasContabilRepositorio;
        private readonly IUnidadeRepositorio _unidadeRepositorio;
        private readonly INotificacaoRepositorio _notificacaoRepositorio;
        private readonly IOrcamentoSinistroRepositorio _orcamentoSinistroRepositorio;
        private readonly IParametroBoletoBancarioRepositorio _parametroBoletoBancarioRepositorio;

        public ContaPagarServico(
            IContaPagarRepositorio contasPagarRepositorio
            , IContaContabilRepositorio contasContabilRepositorio
            , IUnidadeRepositorio unidadeRepositorio
            , INotificacaoRepositorio notificacaoRepositorio
            , IOrcamentoSinistroRepositorio orcamentoSinistroRepositorio
            , IParametroBoletoBancarioRepositorio parametroBoletoBancarioRepositorio
            )
        {
            _contasPagarRepositorio = contasPagarRepositorio;
            _contasContabilRepositorio = contasContabilRepositorio;
            _unidadeRepositorio = unidadeRepositorio;
            _notificacaoRepositorio = notificacaoRepositorio;
            _orcamentoSinistroRepositorio = orcamentoSinistroRepositorio;
            _parametroBoletoBancarioRepositorio = parametroBoletoBancarioRepositorio;
        }

        public void DeletarNotificacao(ContasAPagar contaPagar, string descricao = "")
        {
            var notificacoes = _contasPagarRepositorio.GetById(contaPagar.Id)?.ContaPagarNotificacoes?.Select(x => x.Notificacao)?.ToList() ?? new List<Notificacao>();
            contaPagar.ContaPagarNotificacoes = new List<ContaPagarNotificacao>();
            _contasPagarRepositorio.Save(contaPagar);

            if (!string.IsNullOrEmpty(descricao))
                notificacoes.Where(x => x.Descricao.Contains(descricao));

            foreach (var notificacao in notificacoes)
            {
                _notificacaoRepositorio.Delete(notificacao);
            }
        }

        public void Salvar(ContasAPagar contasAPagar, Usuario usuario)
        {
            contasAPagar.DataCompetencia = contasAPagar.DataCompetencia.HasValue ? contasAPagar.DataCompetencia : new DateTime(contasAPagar.DataVencimento.Year, contasAPagar.DataVencimento.Month, 1);

            DeletarNotificacao(contasAPagar);

            Workflow(contasAPagar, usuario);
        }

        public void Workflow(ContasAPagar contasAPagar, Usuario usuario)
        {
            if (contasAPagar.Id <= 0)
                contasAPagar.StatusConta = StatusContasAPagar.PendenteAprovacao;

            switch (contasAPagar.StatusConta)
            {
                case StatusContasAPagar.EmAberto:
                    contasAPagar.StatusConta = contasAPagar.DataVencimento.Date < DateTime.Now.Date
                                                ? StatusContasAPagar.Vencida
                                                : StatusContasAPagar.EmAberto;
                    break;
                case StatusContasAPagar.Paga:
                    break;
                case StatusContasAPagar.Vencida:
                    break;
                case StatusContasAPagar.RetiradaCofre:
                    break;
                case StatusContasAPagar.PendentePagamento:
                    break;
                case StatusContasAPagar.Cancelado:
                    break;
                case StatusContasAPagar.Negada:
                    break;
                case StatusContasAPagar.PendenteAprovacao:
                    //Gera apenas UMA notificação para aprovação da "conta a pagar".
                    if (contasAPagar.ContaPagarNotificacoes != null
                        && !contasAPagar.ContaPagarNotificacoes.Any())
                    {
                        GerarNotificacaoPadrao(contasAPagar,
                                                usuario,
                                                TipoAcaoNotificacao.AprovarReprovar,
                                                $"Ct. Financeira: [{contasAPagar.ContaFinanceira.Descricao}] Fornecedor: [{contasAPagar.Fornecedor.Descricao}] Forma de Pag. [{contasAPagar.FormaPagamento.ToDescription()}]<br>Cod. Agrupador: [{contasAPagar.CodigoAgrupadorParcela}] Parcela: [{contasAPagar.NumeroParcela}] Vencimento: [{contasAPagar.DataVencimento.ToShortDateString()}] Valor: [{contasAPagar.ValorTotal.ToString("N2")}].",
                                                "ContaPagar/Edit");
                    }
                    break;
                case StatusContasAPagar.EnviadoAoBanco:
                    break;
                default:
                    break;
            }

            if (contasAPagar.FormaPagamento == FormaPagamento.Ted
                || contasAPagar.FormaPagamento == FormaPagamento.Doc)
                if (string.IsNullOrEmpty(contasAPagar.Fornecedor.Agencia) || string.IsNullOrEmpty(contasAPagar.Fornecedor.Conta))
                    throw new BusinessRuleException($"Complete os dados da conta do Fornecedor na tela de <a target='_blank' href='/Fornecedor/edit/{contasAPagar.Fornecedor.Id}'>Cadastro de Fornecedor</a>");

            base.Salvar(contasAPagar);
        }

        public IList<ContasAPagar> ListarContasPagar(int mes, int idUnidade)
        {
            var list = _contasPagarRepositorio.ListarContasPagar(mes, idUnidade);
            return list;
        }

        public IList<ContasAPagar> ListarContasPagar(int? idContaFinanceira)
        {
            var valor = _contasPagarRepositorio.List().ToList();

            return valor.Where(x => x.ContaFinanceira.Id == idContaFinanceira).ToList();
        }

        public IList<ContasAPagar> ListarContasPagar(int? idContaFinanceira, DateTime? data, int? departamento, TipoContaPagamento tipopagamento, int? fornecedor, int? unidade)
        {
            var predicate = PredicateBuilder.True<ContasAPagar>().And(x => x.ContaFinanceira != null);

            if (idContaFinanceira.HasValue && idContaFinanceira.Value > 0)
                predicate = predicate.And(x => x.ContaFinanceira.Id == idContaFinanceira.Value);

            if (data.HasValue && data.Value > DateTime.MinValue)
                predicate = predicate.And(x => x.DataVencimento.Date == data.Value.Date);

            if (departamento.HasValue && departamento.Value > 0)
                predicate = predicate.And(x => x.Departamento.Id == departamento.Value);

            if (tipopagamento != 0)
                predicate = predicate.And(x => x.TipoPagamento == tipopagamento);

            if (fornecedor.HasValue && fornecedor.Value > 0)
                predicate = predicate.And(x => x.Fornecedor.Id == fornecedor.Value);

            if (unidade.HasValue && unidade.Value > 0)
                predicate = predicate.And(x => x.Unidade.Id == unidade.Value);

            return _contasPagarRepositorio.ListBy(predicate);
        }

        public void ExecutarPagamento(int Id, int idUsuario)
        {
            var contaspagar = this.BuscarPorId(Id);

            contaspagar.DataPagamento = DateTime.Now;

            contaspagar.StatusConta = StatusContasAPagar.Paga;

            Workflow(contaspagar, new Usuario { Id = idUsuario });
        }

        public void ExcluirLogicamentePorId(int id)
        {
            var entidade = _contasPagarRepositorio.GetById(id);
            entidade.Ativo = false;

            _contasPagarRepositorio.Save(entidade);
        }

        public decimal CalcularDespesaTotal(int empresa, int? unidade, int mes, int ano)
        {

            decimal ret = 0;

            DateTime datainicio = new DateTime(ano, mes, 1);

            DateTime datafim = new DateTime(ano, mes, DateTime.DaysInMonth(ano, mes));

            var unidades = _unidadeRepositorio.List().Where(x => x.Empresa.Id == empresa).ToList();

            List<ContasAPagar> contasPagar = new List<ContasAPagar>();

            if (unidade != null)
                contasPagar = _contasPagarRepositorio.List().Where(x => x.Unidade != null && x.DataPagamento != null).Where(x => x.Unidade.Id == unidade).ToList();
            else
            {
                foreach (var uni in unidades)
                {
                    var contaPagar = _contasPagarRepositorio.List().Where(x => x.Unidade != null && x.DataPagamento != null).Where(x => x.Unidade.Id == uni.Id).ToList();
                    contasPagar.AddRange(contaPagar);
                }
            }
            if (contasPagar.Count() > 0)
            {

                return contasPagar.Where(x => x.DataPagamento >= datainicio && x.DataPagamento <= datafim).Select(a => a.ValorTotal).Sum();

            }

            return ret;
        }

        public decimal CalcularDespesaFixa(int empresa, int? unidade, int mes, int ano)
        {
            decimal ret = 0;

            DateTime datainicio = new DateTime(ano, mes, 1);

            DateTime datafim = new DateTime(ano, mes, DateTime.DaysInMonth(ano, mes));

            var unidades = _unidadeRepositorio.List().Where(x => x.Empresa.Id == empresa).ToList();

            var contasContabil = _contasContabilRepositorio.List().Where(c => c.Fixa == true);

            //var contratos = _contratoAplicacao.BuscarPor(x => idsContratos.Contains(x.Id));


            var contasPagar = new List<ContasAPagar>();


            if (unidade != null)
                contasPagar = _contasPagarRepositorio.List().Where(x => x.Unidade != null && x.DataPagamento != null).Where(x => x.Unidade.Id == unidade && contasContabil.Contains(x.ContaContabil)).ToList();
            else
            {
                foreach (var uni in unidades)
                {
                    var contaPagar = _contasPagarRepositorio.List().Where(x => x.Unidade != null && x.DataPagamento != null).Where(x => x.Unidade.Id == uni.Id && contasContabil.Contains(x.ContaContabil)).ToList();
                    contasPagar.AddRange(contaPagar);
                }
            }

            //var contasPagar = _contasPagarRepositorio.List().Where(x => x.Unidade != null && x.DataPagamento != null).
            //                        Where(x => x.Unidade.Id == unidade && contasContabil.Contains(x.ContaContabil));



            if (contasPagar.Count() > 0)
            {

                return contasPagar.Where(x => x.DataPagamento >= datainicio && x.DataPagamento <= datafim).Select(a => a.ValorTotal).Sum();

            }

            return ret;
        }

        public decimal CalcularDespesaEscolhida(int empresa, int? unidade, int mes, int ano)
        {
            decimal ret = 0;

            DateTime datainicio = new DateTime(ano, mes, 1);

            DateTime datafim = new DateTime(ano, mes, DateTime.DaysInMonth(ano, mes));

            var unidades = _unidadeRepositorio.List().Where(x => x.Empresa.Id == empresa).ToList();


            List<ContasAPagar> contasPagar = new List<ContasAPagar>();

            if (unidade != null)
            {
                contasPagar = _contasPagarRepositorio.List().Where(x => x.Unidade != null && x.DataPagamento != null)
                                                                    .Where(x => x.Unidade.Id == unidade).ToList();
            }
            else
            {
                foreach (var uni in unidades)
                {
                    var contaPagar = _contasPagarRepositorio.List().Where(x => x.Unidade != null && x.DataPagamento != null)
                                                                    .Where(x => x.Unidade.Id == uni.Id).ToList();
                    contasPagar.AddRange(contaPagar);
                }
            }

            if (contasPagar.Count() > 0)
            {

                return contasPagar.Where(x => x.DataPagamento >= datainicio && x.DataPagamento <= datafim && x.Ignorado == false).Select(a => a.ValorTotal).Sum();

            }

            return ret;
        }

        public IList<ContasAPagar> BuscarLancamentosPorFornecedor(int idFornecedor)
        {
            return BuscarPor(x => x.Fornecedor.Id == idFornecedor);
        }

        public ContasAPagar BuscarPelaNotificacaoId(int notificacaoId)
        {
            return Repositorio.FirstBy(x => x.ContaPagarNotificacoes.Any(on => on.Notificacao.Id == notificacaoId));
        }

        public void NegarConta(int id, string observacao, Usuario usuario)
        {
            var contaspagar = this.BuscarPorId(id);

            contaspagar.Observacoes = observacao;
            contaspagar.StatusConta = StatusContasAPagar.Negada;
            this.Salvar(contaspagar);
            GerarNotificacao(contaspagar, usuario);
        }

        private void GerarNotificacao(ContasAPagar contasAPagar, Usuario usuario)
        {
            var orcamentoSinistro = _orcamentoSinistroRepositorio.FirstBy(x => x.OrcamentoSinistroCotacao.OrcamentoSinistroCotacaoItens
                                                                                .Any(oc => oc.ContasAPagar.Id == contasAPagar.Id));

            if (orcamentoSinistro != null)
            {
                var orcamentoSinistroCotacao = orcamentoSinistro.OrcamentoSinistroCotacao;

                orcamentoSinistroCotacao.OrcamentoSinistroCotacaoNotificacoes = orcamentoSinistroCotacao.OrcamentoSinistroCotacaoNotificacoes ?? new List<OrcamentoSinistroCotacaoNotificacao>();

                var descrição = $"As contas do orçamento de ID: {orcamentoSinistro.Id} foram negadas.";

                var notificacao = _notificacaoRepositorio.SalvarNotificacaoComRetorno(orcamentoSinistroCotacao, Entidades.ContasAPagar, usuario, DateTime.Now.AddDays(7).AddHours(23).AddMinutes(59).AddSeconds(59), descrição, "OrcamentoSinistro/Cotacao", TipoAcaoNotificacao.Aviso);

                orcamentoSinistroCotacao.OrcamentoSinistroCotacaoNotificacoes.Add(new OrcamentoSinistroCotacaoNotificacao
                {
                    OrcamentoSinistroCotacao = orcamentoSinistroCotacao,
                    Notificacao = notificacao
                });

                _orcamentoSinistroRepositorio.Save(orcamentoSinistro);
            }
            else
            {
                GerarNotificacaoPadrao(contasAPagar, usuario, TipoAcaoNotificacao.Aviso, $"A conta de ID: {contasAPagar.Id} foi negada.", "contapagar/edit");
            }
        }

        private void GerarNotificacaoPadrao(ContasAPagar contasAPagar, Usuario usuario, TipoAcaoNotificacao acao, string descricao = null, string url = null)
        {
            var conta = _contasPagarRepositorio.GetById(contasAPagar.Id);
            contasAPagar.ContaPagarNotificacoes = conta.ContaPagarNotificacoes;
            if (contasAPagar.ContaPagarNotificacoes == null)
                contasAPagar.ContaPagarNotificacoes = new List<ContaPagarNotificacao>();

            var notificacao = _notificacaoRepositorio.SalvarNotificacaoComRetorno(contasAPagar, Entidades.ContasAPagar, usuario, contasAPagar.DataVencimento.AddDays(5).AddHours(23).AddMinutes(59).AddSeconds(59), descricao, url, acao);

            contasAPagar.ContaPagarNotificacoes.Add(new ContaPagarNotificacao
            {
                Notificacao = notificacao
            });

            _contasPagarRepositorio.Save(contasAPagar);
        }

        public void AtualizarStatus(int idNotificacao, Usuario usuario, AcaoNotificacao acao)
        {
            var objeto = _contasPagarRepositorio.FirstBy(x => x.ContaPagarNotificacoes.Any(y => y.Notificacao.Id == idNotificacao));
            var notificacao = _notificacaoRepositorio.GetById(idNotificacao);

            switch (acao)
            {
                case AcaoNotificacao.Aprovado:
                    notificacao.Status = StatusNotificacao.Aprovado;
                    notificacao.DataAprovacao = DateTime.Now;
                    notificacao.Aprovar(usuario);

                    objeto.StatusConta = StatusContasAPagar.EmAberto;
                    break;
                case AcaoNotificacao.Reprovado:
                    notificacao.Status = StatusNotificacao.Reprovado;
                    notificacao.DataAprovacao = DateTime.Now;
                    notificacao.Reprovar(usuario);

                    objeto.StatusConta = StatusContasAPagar.Cancelado;
                    break;
                default:
                    break;
            }

            _notificacaoRepositorio.Save(notificacao);

            Workflow(objeto, usuario);
        }

        public PagamentoNet.Boleto ImprimirBoletoBancario(ContasAPagar contaPagar, ContaFinanceira contaFinanceira)
        {
            var vencimento = contaPagar.DataVencimento.ToShortDateString();
            var valorBoleto = contaPagar.ValorTotal;

            if (contaFinanceira == null)
            {
                throw new Exception("Nao encontrado uma conta financeira!");
            }

            var CodigoBanco = contaFinanceira.Banco.CodigoBanco;

            if (contaFinanceira.Cpf == null)
                contaFinanceira.Cpf = contaFinanceira.Cnpj.Replace(".", string.Empty).Replace(",", string.Empty).Replace("/", string.Empty);

            var objCedente = new Cedente(
                contaFinanceira.Cpf,
                contaFinanceira.Descricao,
                contaFinanceira.Agencia,
                contaFinanceira.Conta,
                contaFinanceira.DigitoConta
            );

            objCedente.CodigoTransmissao = $"{contaFinanceira.Agencia}{contaFinanceira.ConvenioPagamento.PadLeft(8, '0')}{contaFinanceira.Conta}";
            objCedente.Convenio = Convert.ToInt64(contaFinanceira.ConvenioPagamento);
            objCedente.ContaBancaria = new ContaBancaria(contaFinanceira.Agencia, contaFinanceira.DigitoAgencia ?? string.Empty, contaFinanceira.Conta, contaFinanceira.DigitoConta);
            objCedente.Nome = contaFinanceira.Empresa.RazaoSocial;
            objCedente.Endereco = new PagamentoNet.Endereco
            {
                End = contaFinanceira.Empresa.Endereco.Logradouro,
                CEP = contaFinanceira.Empresa.Endereco.Cep,
                Numero = contaFinanceira.Empresa.Endereco?.Numero,
                Cidade = contaFinanceira.Empresa.Endereco?.Cidade?.Descricao,
                Bairro = contaFinanceira.Empresa.Endereco?.Bairro
            };


            //cedente         
            var cedenteNossoNumeroBoleto = Constantes.FormataNossoNumeroBanco(contaPagar.Id, contaPagar.ContaFinanceira.Banco.CodigoBanco);
            if (CodigoBanco == "033")
                cedenteNossoNumeroBoleto = cedenteNossoNumeroBoleto.ToString().PadLeftTrunc(7, '0');

            //sacado         
            var sacado_cpfCnpj = contaPagar.Fornecedor.DocumentoCnpjOuCpf;
            var sacado_nome = contaPagar.Fornecedor.TipoPessoa == TipoPessoa.Fisica ? contaPagar.Fornecedor.Nome : contaPagar.Fornecedor.RazaoSocial;

            var endereco = contaPagar.Fornecedor.Enderecos.FirstOrDefault()?.Endereco ?? new Entidade.Endereco();
            var sacado_endereco = endereco.Logradouro;
            var sacado_bairro = endereco.Bairro;


            var sacado_cidade = endereco.Cidade?.Descricao;
            if (string.IsNullOrEmpty(sacado_cidade))
                throw new BusinessRuleException($"\nVerifique o Endereço completo do fornecedor abaixo ref. a Conta a Pagar [{contaPagar.Id}].\nFornecedor: {contaPagar.Fornecedor?.Nome}\nDocumento: {contaPagar.Fornecedor.DocumentoCnpjOuCpf.FormatCpfCnpj()}");

            var sacado_cep = endereco.Cep;
            var sacado_uf = endereco?.Cidade?.Estado?.Sigla;

            var cedente = objCedente;
            cedente.Codigo = contaFinanceira.ConvenioPagamento;

            if (cedente.Codigo == null)
                cedente.Codigo = string.Empty;

            var boleto = new PagamentoNet.Boleto();
            boleto.NossoNumero = cedenteNossoNumeroBoleto;
            boleto.DataVencimento = Convert.ToDateTime(vencimento);
            boleto.ValorBoleto = valorBoleto;

            boleto.Carteira = contaFinanceira.Carteira;
            boleto.Cedente = cedente;
            boleto.Cedente.MostrarCNPJnoBoleto = true;
            boleto.LocalPagamento = "Qualquer agência da rede bancária, internet banking ou lotéricas";
            var sacado = new Sacado(sacado_cpfCnpj, sacado_nome);
            boleto.Sacado = sacado;
            boleto.Sacado.Endereco.End = sacado_endereco;
            boleto.Sacado.Endereco.Numero = endereco.Numero;
            boleto.Sacado.Endereco.Bairro = sacado_bairro;
            boleto.Sacado.Endereco.Cidade = sacado_cidade;
            boleto.Sacado.Endereco.CEP = sacado_cep;
            boleto.Sacado.Endereco.UF = sacado_uf;
            boleto.Sacado.ContaBancaria = new ContaBancaria(contaPagar.Fornecedor.Agencia, contaPagar.Fornecedor.DigitoAgencia, contaPagar.Fornecedor.Conta, contaPagar.Fornecedor.DigitoConta);
            boleto.Sacado.CodigoBanco = contaPagar.Fornecedor.Banco.CodigoBanco;

            //MULTA
            if (contaPagar.ValorMulta > 0)
            {
                boleto.DataMulta = boleto.DataVencimento.AddDays(1);
                boleto.PercMulta = contaPagar.ValorMulta;

                if (contaPagar.TipoMulta == TipoMultaContaPagar.Monetario)
                    boleto.ValorMulta = contaPagar.ValorMulta;
                else
                    boleto.ValorMulta = (contaPagar.ValorMulta * boleto.PercMulta) / 100;

                InstrucoesBoletoMulta(boleto, contaPagar.ValorMulta, contaPagar.TipoMulta, CodigoBanco);
            }

            //JUROS
            if (contaPagar.ValorJuros > 0)
            {
                boleto.DataJurosMora = boleto.DataVencimento.AddDays(1);
                boleto.PercJurosMora = contaPagar.ValorJuros;
                boleto.JurosMora = contaPagar.ValorJuros;

                InstrucoesBoletoJuros(boleto, contaPagar.ValorJuros, contaPagar.TipoJuros, CodigoBanco);
            }

            boleto.Remessa = new Remessa
            {
                FormaPagamento = contaPagar.FormaPagamento == FormaPagamento.Doc
                                    ? Remessa.FormasPagamento.DOC
                                    : contaPagar.FormaPagamento == FormaPagamento.Ted
                                        ? Remessa.FormasPagamento.TED
                                        : Remessa.FormasPagamento.ContaCorrente,
                CodigoOcorrencia = contaPagar.FormaPagamento == FormaPagamento.Doc
                                    || contaPagar.FormaPagamento == FormaPagamento.Ted
                                        ? "DOC/TED"
                                        : contaPagar.FormaPagamento == FormaPagamento.Boleto
                                            ? "TITULO_BANCARIO"
                                                : contaPagar.FormaPagamento == FormaPagamento.BoletoConcessionaria
                                                ? "TITULO_CONCESSIONARIA"
                                                    : contaPagar.FormaPagamento == FormaPagamento.ImpostoComCodigo ? "TRIBUTOS" : ""
            };

            boleto.NumeroCodigoBarra = contaPagar.CodigoDeBarras;

            return boleto;
        }

        private void InstrucoesBoletoMulta(PagamentoNet.Boleto boleto, decimal valorMulta, TipoMultaContaPagar tipoMulta, string codigoBanco)
        {
            var banco = (CodigoBancos)Enum.Parse(typeof(CodigoBancos), codigoBanco);
            IInstrucao instrucao;
            IEspecieDocumento especie;

            var tipoValor = tipoMulta == TipoMultaContaPagar.Monetario
                                                                ? AbstractInstrucao.EnumTipoValor.Reais
                                                                : AbstractInstrucao.EnumTipoValor.Percentual;

            switch (banco)
            {
                case CodigoBancos.Santander:
                    instrucao = new Instrucao_Santander((int)(EnumInstrucoes_Santander.MultaVencimento), Convert.ToDouble(valorMulta), tipoValor);

                    especie = new EspecieDocumento_Santander(boleto.Cedente.Codigo);
                    break;
                default:
                    throw new BusinessRuleException("Banco não implementado para geração de Boleto/CNAB!");
            }

            boleto.Instrucoes.Add(instrucao);
            boleto.EspecieDocumento = especie;
        }

        private void InstrucoesBoletoJuros(PagamentoNet.Boleto boleto, decimal valorJuros, TipoJurosContaPagar tipoJuros, string codigoBanco)
        {
            var banco = (CodigoBancos)Enum.Parse(typeof(CodigoBancos), codigoBanco);
            IInstrucao instrucao;
            IEspecieDocumento especie;

            var tipoValor = tipoJuros == TipoJurosContaPagar.Monetario
                                                                ? AbstractInstrucao.EnumTipoValor.Reais
                                                                : AbstractInstrucao.EnumTipoValor.Percentual;

            switch (banco)
            {
                case CodigoBancos.Santander:
                    instrucao = new Instrucao_Santander((int)(EnumInstrucoes_Santander.MultaVencimento), Convert.ToDouble(valorJuros), tipoValor);

                    especie = new EspecieDocumento_Santander(boleto.Cedente.Codigo);
                    break;
                default:
                    throw new BusinessRuleException("Banco não implementado para geração de Boleto/CNAB!");
            }

            boleto.Instrucoes.Add(instrucao);
            boleto.EspecieDocumento = especie;
        }
    }
}