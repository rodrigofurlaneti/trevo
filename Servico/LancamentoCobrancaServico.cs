using BoletoNet;
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
    public interface ILancamentoCobrancaServico : IBaseServico<LancamentoCobranca>
    {
        IList<LancamentoCobranca> ListarLancamentosCobranca(int? idContaFinanceira, TipoServico? tipoServico,
            DateTime? dataVencimentoIni, DateTime? dataVencimentoFim, int? idCliente, string documento);
        IList<LancamentoCobranca> ListarLancamentosCobranca(int? idContaFinanceira, TipoServico? tipoServico, int? idUnidade,
            StatusLancamentoCobranca? statusLancamentoCobranca, TipoFiltroGeracaoCNAB? tipoFiltroGeracaoCNAB, int? supervisor, int? cliente,
            string dataDe, string dataAte);
        IList<LancamentoCobranca> BuscarLancamentosPorCliente(int idCliente);
        BoletoNet.Boleto ImprimirBoletoBancario(LancamentoCobranca cobranca, ContaFinanceira contaFinanceira, DateTime? dtVencimento, 
                                                TipoValor tipoValorJuros, decimal? juros, TipoValor tipoValorMulta, decimal? multa, 
                                                List<ParametroBoletoBancario> listaParametroBoletoBancario, TipoOcorrenciaCNAB tipoOcorrenciaCNAB);
        LancamentoCobranca BuscarPelaNotificacaoId(int notificacaoId);
        void GerarNotificacaoPagamento(LancamentoCobranca lance, Usuario usuarioLogado);
        void SalvarNotificacaoDeAviso(LancamentoCobranca lancamentoCobranca, Usuario usuario, string descricao, string urlPersonalizada = "");
        IList<LancamentoCobranca> BuscarLancamentosCobranca(StatusLancamentoCobranca? status, int unidade, int cliente, string dataVencimento, int numeroContrato, List<int> listaIds = null);
        void UpdateDetalhesCNAB(List<LancamentoCobranca> listaLancamento);
        IList<DadosLancamentosVO> BuscarLancamentosPagosRelatorio(DateTime dataInicio, DateTime dataFim, TipoServico? tipoServico, int unidade);
        IList<DadosLancamentosVO> BuscarLancamentosEmAbertoRelatorio(DateTime dataInicio, DateTime dataFim, TipoServico? tipoServico, int unidade);
        IList<DadosPagamentoVO> BuscarPagamentosPagosRelatorio(DateTime dataInicio, DateTime dataFim, TipoServico? tipoServico, int unidade);
        IList<DadosPagamentoVO> BuscarPagamentosEmAbertoRelatorio(DateTime dataInicio, DateTime dataFim, TipoServico? tipoServico, int unidade, bool acrescentarCancelados = false);
        IList<LancamentoCobranca> BuscarLancamentosCobrancaLinq(StatusLancamentoCobranca? status, int unidade, int cliente, string dataVencimento, List<int> listaIds);
        IList<DadosPagamentoVO> BuscarPagamentosEfetuadosConferenciaRelatorio(DateTime dataInicio, DateTime dataFim, TipoServico? tipoServico, int unidade);
    }

    public class LancamentoCobrancaServico : BaseServico<LancamentoCobranca, ILancamentoCobrancaRepositorio>, ILancamentoCobrancaServico
    {
        private readonly ILancamentoCobrancaRepositorio _lancamentoCobrancaRepositorio;
        private readonly ILancamentoCobrancaPedidoSeloRepositorio lancamentoCobrancaPedidoSeloRepositorio;
        private readonly IEmpresaRepositorio _empresaRepositorio;
        private readonly INotificacaoRepositorio _notificacaoRepositorio;
        private readonly IParametroBoletoBancarioRepositorio _parametroBoletoBancarioRepositorio;

        public LancamentoCobrancaServico(
            ILancamentoCobrancaRepositorio lancamentoCobrancaRepositorio
            , ILancamentoCobrancaPedidoSeloRepositorio lancamentoCobrancaPedidoSeloRepositorio
            , IEmpresaRepositorio empresaRepositorio
            , INotificacaoRepositorio notificacaoRepositorio
            , IParametroBoletoBancarioRepositorio parametroBoletoBancarioRepositorio)
        {
            _lancamentoCobrancaRepositorio = lancamentoCobrancaRepositorio;
            this.lancamentoCobrancaPedidoSeloRepositorio = lancamentoCobrancaPedidoSeloRepositorio;
            _empresaRepositorio = empresaRepositorio;
            _notificacaoRepositorio = notificacaoRepositorio;
            _parametroBoletoBancarioRepositorio = parametroBoletoBancarioRepositorio;
        }

        public IList<LancamentoCobranca> ListarLancamentosCobranca(int? idContaFinanceira, TipoServico? tipoServico,
            DateTime? dataVencimentoIni, DateTime? dataVencimentoFim, int? idCliente, string documento)
        {
            return _lancamentoCobrancaRepositorio.ListarLancamentosCobranca(idContaFinanceira, tipoServico, dataVencimentoIni, dataVencimentoFim, idCliente, documento)?.ToList() ?? new List<LancamentoCobranca>();
        }

        public IList<LancamentoCobranca> ListarLancamentosCobranca(int? idContaFinanceira, TipoServico? tipoServico, int? idUnidade,
            StatusLancamentoCobranca? statusLancamentoCobranca, TipoFiltroGeracaoCNAB? tipoFiltroGeracaoCNAB, int? supervisor, int? cliente,
            string dataDe, string dataAte)
        {
            return _lancamentoCobrancaRepositorio.ListarLancamentosCobranca(idContaFinanceira, tipoServico, idUnidade, statusLancamentoCobranca, 
                tipoFiltroGeracaoCNAB, supervisor, cliente, dataDe, dataAte)?.ToList() ?? new List<LancamentoCobranca>();
        }

        public IList<LancamentoCobranca> BuscarLancamentosPorCliente(int idCliente)
        {
            return BuscarPor(x => x.Cliente.Id == idCliente);
        }

        public BoletoNet.Boleto ImprimirBoletoBancario(LancamentoCobranca cobranca, ContaFinanceira contaFinanceira, DateTime? dtVencimento, TipoValor tipoValorJuros, decimal? juros, TipoValor tipoValorMulta, decimal? multa, List<ParametroBoletoBancario> listaParametroBoletoBancario, TipoOcorrenciaCNAB tipoOcorrenciaCNAB)
        {
            if (dtVencimento < DateTime.Now)
                throw new Exception("Data vencimento deve ser maior que a data atual!");

            var vencimento = dtVencimento.HasValue ? dtVencimento.Value.ToShortDateString() : cobranca.DataVencimento.ToShortDateString();
            var valorBoleto = cobranca.ValorContrato;

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

            objCedente.CodigoTransmissao = $"{contaFinanceira.Agencia}0{contaFinanceira.Convenio.Truncate(7).PadLeft(7, '0')}0{contaFinanceira.Conta.Truncate(7).PadLeft(7, '0')}";
            objCedente.Convenio = Convert.ToInt64(contaFinanceira.Convenio);
            objCedente.ContaBancaria = new ContaBancaria(contaFinanceira.Agencia, contaFinanceira.DigitoAgencia ?? string.Empty, contaFinanceira.Conta, contaFinanceira.DigitoConta);
            objCedente.Nome = contaFinanceira.Empresa.RazaoSocial;
            if (!contaFinanceira.Empresa.Endereco.Valido)
                throw new BusinessRuleException($"O Endereço da Empresa ${contaFinanceira.Empresa.Descricao} está incompleto.");

            objCedente.Endereco = new BoletoNet.Endereco
            {
                End = contaFinanceira.Empresa.Endereco.Logradouro,
                CEP = contaFinanceira.Empresa.Endereco.Cep,
                Numero = contaFinanceira.Empresa.Endereco?.Numero,
                Cidade = contaFinanceira.Empresa.Endereco?.Cidade?.Descricao,
                Bairro = contaFinanceira.Empresa.Endereco?.Bairro
            };

            cobranca.ContaFinanceira = contaFinanceira;

            //cedente         
            var cedenteNossoNumeroBoleto = Constantes.FormataNossoNumeroBanco(cobranca.Id, cobranca.ContaFinanceira.Banco.CodigoBanco);
            if (CodigoBanco == "033")
                cedenteNossoNumeroBoleto = cedenteNossoNumeroBoleto.ToString().PadLeftTrunc(7, '0');

            //sacado         
            var sacado_cpfCnpj = cobranca.Cliente.Pessoa.DocumentoCnpjOuCpf;
            var sacado_nome = string.IsNullOrEmpty(cobranca.Cliente.RazaoSocial) ? cobranca.Cliente.Pessoa.Nome : cobranca.Cliente.RazaoSocial;

            var endereco = cobranca.Cliente.Pessoa.Enderecos.FirstOrDefault()?.Endereco ?? new Entidade.Endereco();
            var sacado_endereco = endereco.Logradouro;
            var sacado_bairro = endereco.Bairro;

            if (endereco.Cidade?.Descricao == null)
                throw new BusinessRuleException($"<br/>Verifique o Endereço completo do cliente abaixo ref. a Cobrança [{cobranca.Id}].<br/>Cliente: {cobranca.Cliente?.Pessoa?.Nome}<br/>Documento: {cobranca.Cliente.Pessoa.DocumentoCnpjOuCpf.FormatCpfCnpj()}");

            var sacado_cidade = endereco.Cidade?.Descricao;
            var sacado_cep = endereco.Cep;
            var sacado_uf = endereco?.Cidade?.Estado?.Sigla;

            var cedente = objCedente;
            cedente.Codigo = contaFinanceira.Convenio;

            if (cedente.Codigo == null)
                cedente.Codigo = string.Empty;

            var boleto = new BoletoNet.Boleto();
            boleto.NossoNumero = cedenteNossoNumeroBoleto;
            boleto.DataVencimento = Convert.ToDateTime(vencimento);
            boleto.ValorBoleto = valorBoleto;

            var parametroBoleto = listaParametroBoletoBancario.FirstOrDefault(x => x.TipoServico == cobranca.TipoServico && (x.Unidade != null && x.Unidade.Id == cobranca.Unidade.Id));
            if (parametroBoleto == null)
                parametroBoleto = listaParametroBoletoBancario.FirstOrDefault(x => x.TipoServico == cobranca.TipoServico);
            if (parametroBoleto != null)
            {
                boleto.Instrucoes = new Instrucoes();
                foreach (var instrucao in parametroBoleto.ParametroBoletoBancarioDescritivos)
                {
                    boleto.Instrucoes.Add(new Instrucao(int.Parse(CodigoBanco)) { Descricao = instrucao.Descritivo });
                }

                boleto.DataDesconto = cobranca.DataVencimento.AddDays(-parametroBoleto.DiasAntesVencimento);
                boleto.ValorDescontoAntecipacao = boleto.ValorBoleto * parametroBoleto.ValorDesconto;
            }

            var multaValorBoleto = new decimal?();
            var jurosValorBoleto = new decimal?();

            //MULTA
            if (multa.HasValue)
            {
                boleto.DataMulta = boleto.DataVencimento.AddDays(1);
                boleto.PercMulta = multa.Value;
                multaValorBoleto = multa.Value;

                if (tipoValorMulta == TipoValor.Monetario)
                    boleto.ValorMulta = multa.Value;
                else
                    boleto.ValorMulta = cobranca.ValorAReceber * boleto.PercMulta / 100;
            }
            else
            {
                tipoValorMulta = cobranca.TipoValorMulta;
                boleto.DataMulta = boleto.DataVencimento.AddDays(1);
                boleto.PercMulta = cobranca.ValorMulta;
                multaValorBoleto = cobranca.ValorMulta;

                if (cobranca.TipoValorMulta == TipoValor.Monetario)
                    boleto.ValorMulta = cobranca.ValorMulta;
                else
                    boleto.ValorMulta = cobranca.ValorAReceber * boleto.PercMulta / 100;
            }

            //JUROS
            if (juros.HasValue)
            {
                boleto.DataJurosMora = boleto.DataVencimento.AddDays(1);
                boleto.PercJurosMora = juros.Value;
                jurosValorBoleto = juros.Value;
                boleto.JurosMora = juros.Value;
            }
            else
            {
                tipoValorJuros = cobranca.TipoValorJuros;
                boleto.DataJurosMora = boleto.DataVencimento.AddDays(1);
                boleto.PercJurosMora = cobranca.ValorJuros;
                jurosValorBoleto = cobranca.ValorJuros;
                boleto.JurosMora = cobranca.ValorJuros;
            }

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

            if (multaValorBoleto.HasValue && multaValorBoleto.Value > 0)
            {
                InstrucoesBoleto(boleto, Convert.ToDouble(multaValorBoleto.Value), tipoValorMulta, TipoCobrancaPagamentoExtra.Multa, CodigoBanco);
            }
            if (jurosValorBoleto.HasValue && jurosValorBoleto.Value > 0)
            {
                InstrucoesBoleto(boleto, Convert.ToDouble(jurosValorBoleto.Value), tipoValorJuros, TipoCobrancaPagamentoExtra.Juros, CodigoBanco);
            }

            boleto.Remessa = new Remessa
            {
                TipoDocumento = "2",
                CodigoOcorrencia = ((int)tipoOcorrenciaCNAB).ToString()
            };

            return boleto;
        }

        private void InstrucoesBoleto(BoletoNet.Boleto boleto, double valor, TipoValor tipoValor, TipoCobrancaPagamentoExtra tipoExtra, string CodigoBanco)
        {
            var banco = (CodigoBancos)Enum.Parse(typeof(CodigoBancos), CodigoBanco);
            IInstrucao instrucao;
            IEspecieDocumento especie;

            switch (banco)
            {
                case CodigoBancos.BancodoBrasil:
                    instrucao = new Instrucao_BancoBrasil((int)(tipoExtra == TipoCobrancaPagamentoExtra.Juros
                                                                ? EnumInstrucoes_BancoBrasil.JurosdeMora
                                                                : EnumInstrucoes_BancoBrasil.Multa),
                                                            valor,
                                                            tipoValor == TipoValor.Monetario
                                                                ? AbstractInstrucao.EnumTipoValor.Reais
                                                                : AbstractInstrucao.EnumTipoValor.Percentual);
                    especie = new EspecieDocumento_BancoBrasil(boleto.Cedente.Codigo);
                    break;
                case CodigoBancos.Bradesco:
                    instrucao = new Instrucao_Bradesco((int)(tipoExtra == TipoCobrancaPagamentoExtra.Juros
                                                                ? EnumInstrucoes_Bradesco.OutrasInstrucoes_ExibeMensagem_MoraDiaria
                                                                : EnumInstrucoes_Bradesco.OutrasInstrucoes_ExibeMensagem_MultaVencimento),
                                                            valor,
                                                            tipoValor == TipoValor.Monetario
                                                                ? AbstractInstrucao.EnumTipoValor.Reais
                                                                : AbstractInstrucao.EnumTipoValor.Percentual);
                    especie = new EspecieDocumento_Bradesco(boleto.Cedente.Codigo);
                    break;
                case CodigoBancos.Caixa:
                    instrucao = new Instrucao_Caixa((int)(tipoExtra == TipoCobrancaPagamentoExtra.Juros
                                                                ? EnumInstrucoes_Caixa.JurosdeMora
                                                                : EnumInstrucoes_Caixa.Multa),
                                                            Convert.ToDecimal(valor),
                                                            tipoValor == TipoValor.Monetario
                                                                ? AbstractInstrucao.EnumTipoValor.Reais
                                                                : AbstractInstrucao.EnumTipoValor.Percentual);
                    especie = new EspecieDocumento_Caixa(boleto.Cedente.Codigo);
                    break;
                case CodigoBancos.Itau:
                    instrucao = new Instrucao_Itau((int)(tipoExtra == TipoCobrancaPagamentoExtra.Juros
                                                                ? EnumInstrucoes_Itau.JurosdeMora
                                                                : EnumInstrucoes_Itau.MultaVencimento),
                                                            valor,
                                                            tipoValor == TipoValor.Monetario
                                                                ? AbstractInstrucao.EnumTipoValor.Reais
                                                                : AbstractInstrucao.EnumTipoValor.Percentual);
                    especie = new EspecieDocumento_Itau(boleto.Cedente.Codigo);
                    break;
                case CodigoBancos.Santander:
                    instrucao = new Instrucao_Santander((int)(tipoExtra == TipoCobrancaPagamentoExtra.Juros
                                                                ? EnumInstrucoes_Santander.JurosAoDia
                                                                : EnumInstrucoes_Santander.MultaVencimento),
                                                            valor,
                                                            tipoValor == TipoValor.Monetario
                                                                ? AbstractInstrucao.EnumTipoValor.Reais
                                                                : AbstractInstrucao.EnumTipoValor.Percentual);

                    especie = new EspecieDocumento_Santander(boleto.Cedente.Codigo);
                    break;
                case CodigoBancos.Banrisul:
                case CodigoBancos.Basa:
                case CodigoBancos.BRB:
                case CodigoBancos.HSBC:
                case CodigoBancos.Real:
                case CodigoBancos.Safra:
                case CodigoBancos.Sicoob:
                case CodigoBancos.Sicred:
                case CodigoBancos.Sudameris:
                case CodigoBancos.Semear:
                default:
                    throw new BusinessRuleException("Banco não implementado para geração de Boleto/CNAB!");
            }

            boleto.Instrucoes.Add(instrucao);
            boleto.EspecieDocumento = especie;
        }

        public LancamentoCobranca BuscarPelaNotificacaoId(int notificacaoId)
        {
            return _lancamentoCobrancaRepositorio.FirstBy(x => x.LancamentoCobrancaNotificacoes.Any(on => on.Notificacao.Id == notificacaoId));
        }

        public void GerarNotificacaoPagamento(LancamentoCobranca lance, Usuario usuarioLogado)
        {
            var notificacao = string.Empty; var urlPersonalizada = string.Empty;

            var cobranca = BuscarPorId(lance.Id);
            if (cobranca is LancamentoCobrancaPedidoSelo
                || cobranca.TipoServico == TipoServico.Convenio)
            {
                var lancamentoCobrancaPedidoSelo = lancamentoCobrancaPedidoSeloRepositorio.GetById(cobranca.Id);
                var pedidoSelo = lancamentoCobrancaPedidoSelo?.PedidoSelo;
                if (pedidoSelo != null)
                {
                    urlPersonalizada = $"PedidoSelo/Edit/{pedidoSelo?.Id}";

                    notificacao = "Um Pedido Selo foi pago!<br/>" +
                        "<ul>" +
                        $"<li><b>Cliente: </b>{pedidoSelo?.Cliente?.NomeExibicao ?? cobranca?.Cliente?.NomeExibicao}</li>" +
                        $"<li><b>Unidade: </b>{pedidoSelo?.Unidade?.Nome ?? cobranca?.Unidade?.Nome}</li>" +
                        $"<li><b>Convênio: </b>{pedidoSelo?.Convenio?.Descricao ?? "Não Informado"}</li>" +
                        "</ul>";
                }
            }

            if (!string.IsNullOrEmpty(notificacao))
                SalvarNotificacaoDeAviso(cobranca, usuarioLogado, notificacao, urlPersonalizada);
        }

        public void SalvarNotificacaoDeAviso(LancamentoCobranca lancamentoCobranca, Usuario usuario, string descricao, string urlPersonalizada = "")
        {
            var oldNotificationssssss = lancamentoCobranca.LancamentoCobrancaNotificacoes.ToList(); // Não retire essa linha! Sério!
            var notificacao = _notificacaoRepositorio
                    .SalvarNotificacaoComRetorno(lancamentoCobranca,
                                                    Entidades.LancamentoCobranca,
                                                    usuario,
                                                    DateTime.Now.Date.AddDays(2).AddHours(23).AddMinutes(59).AddSeconds(59),
                                                    descricao,
                                                    urlPersonalizada,
                                                    TipoAcaoNotificacao.Aviso);

            if (lancamentoCobranca.LancamentoCobrancaNotificacoes == null)
                lancamentoCobranca.LancamentoCobrancaNotificacoes = new List<LancamentoCobrancaNotificacao>();

            lancamentoCobranca.LancamentoCobrancaNotificacoes.Add(new LancamentoCobrancaNotificacao
            {
                Notificacao = notificacao
            });

            _lancamentoCobrancaRepositorio.Save(lancamentoCobranca);
        }

        public IList<LancamentoCobranca> BuscarLancamentosCobranca(StatusLancamentoCobranca? status, int unidade, int cliente, string dataVencimento, int numeroContrato, List<int> listaIds = null)
        {
            return _lancamentoCobrancaRepositorio.BuscarLancamentosCobranca(status, unidade, cliente, dataVencimento, numeroContrato, listaIds);
        }

        public void UpdateDetalhesCNAB(List<LancamentoCobranca> listaLancamento)
        {
            _lancamentoCobrancaRepositorio.UpdateDetalhesCNAB(listaLancamento);
        }

        public IList<DadosLancamentosVO> BuscarLancamentosPagosRelatorio(DateTime dataInicio, DateTime dataFim, TipoServico? tipoServico, int unidade)
        {
            return _lancamentoCobrancaRepositorio.BuscarLancamentosPagosRelatorio(dataInicio, dataFim, tipoServico, unidade);
        }
        public IList<DadosLancamentosVO> BuscarLancamentosEmAbertoRelatorio(DateTime dataInicio, DateTime dataFim, TipoServico? tipoServico, int unidade)
        {
            return _lancamentoCobrancaRepositorio.BuscarLancamentosEmAbertoRelatorio(dataInicio, dataFim, tipoServico, unidade);
        }

        public IList<DadosPagamentoVO> BuscarPagamentosPagosRelatorio(DateTime dataInicio, DateTime dataFim, TipoServico? tipoServico, int unidade)
        {
            var listaRetorno = new List<DadosPagamentoVO>();

            switch (tipoServico.Value)
            {
                case TipoServico.Mensalista:
                case TipoServico.CartaoAcesso:
                    listaRetorno = _lancamentoCobrancaRepositorio.BuscarPagamentosContratoMensalistasPagosRelatorio(dataInicio, dataFim, unidade).ToList();
                    break;
                case TipoServico.Convenio:
                    listaRetorno = _lancamentoCobrancaRepositorio.BuscarPagamentosPedidoSeloPagosRelatorio(dataInicio, dataFim, unidade).ToList();
                    break;
                case TipoServico.Locacao:
                    listaRetorno = _lancamentoCobrancaRepositorio.BuscarPagamentosPedidoLocacaoPagosRelatorio(dataInicio, dataFim, unidade).ToList();
                    break;
                case TipoServico.Evento:
                case TipoServico.Avulso:
                case TipoServico.SeguroReembolso:
                case TipoServico.Outros:
                    listaRetorno = _lancamentoCobrancaRepositorio.BuscarPagamentosPagosRelatorio(dataInicio, dataFim, tipoServico.Value, unidade).ToList();
                    break;
                default:
                    throw new BusinessRuleException($"Tipo de Serviço [{tipoServico.Value.ToDescription()}] não implementado!");
            }
            return listaRetorno;
        }
        public IList<DadosPagamentoVO> BuscarPagamentosEmAbertoRelatorio(DateTime dataInicio, DateTime dataFim, TipoServico? tipoServico, int unidade, bool acrescentarCancelados = false)
        {
            var listaRetorno = new List<DadosPagamentoVO>();

            switch (tipoServico.Value)
            {
                case TipoServico.Mensalista:
                case TipoServico.CartaoAcesso:
                    listaRetorno = _lancamentoCobrancaRepositorio.BuscarPagamentosContratoMensalistasEmAbertoRelatorio(dataInicio, dataFim, unidade, acrescentarCancelados).ToList();
                    break;
                case TipoServico.Convenio:
                    listaRetorno = _lancamentoCobrancaRepositorio.BuscarPagamentosPedidoSeloEmAbertoRelatorio(dataInicio, dataFim, unidade, acrescentarCancelados).ToList();
                    break;
                case TipoServico.Locacao:
                    listaRetorno = _lancamentoCobrancaRepositorio.BuscarPagamentosPedidoLocacaoEmAbertoRelatorio(dataInicio, dataFim, unidade, acrescentarCancelados).ToList();
                    break;
                case TipoServico.Evento:
                case TipoServico.Avulso:
                case TipoServico.SeguroReembolso:
                case TipoServico.Outros:
                    listaRetorno = _lancamentoCobrancaRepositorio.BuscarPagamentosEmAbertoRelatorio(dataInicio, dataFim, tipoServico.Value, unidade, acrescentarCancelados).ToList();
                    break;
                default:
                    throw new BusinessRuleException($"Tipo de Serviço [{tipoServico.Value.ToDescription()}] não implementado!");
            }
            return listaRetorno;
        }

        public IList<DadosPagamentoVO> BuscarPagamentosEfetuadosConferenciaRelatorio(DateTime dataInicio, DateTime dataFim, TipoServico? tipoServico, int unidade)
        {
            var listaRetorno = new List<DadosPagamentoVO>();

            switch (tipoServico.Value)
            {
                case TipoServico.Mensalista:
                    listaRetorno = _lancamentoCobrancaRepositorio.BuscarPagamentosEfetuadosConferenciaContratoMensalistasRelatorio(dataInicio, dataFim, unidade).ToList();
                    break;
                case TipoServico.Convenio:
                    listaRetorno = _lancamentoCobrancaRepositorio.BuscarPagamentosPedidoSeloPagosRelatorio(dataInicio, dataFim, unidade).ToList();
                    break;
                case TipoServico.Locacao:
                    listaRetorno = _lancamentoCobrancaRepositorio.BuscarPagamentosPedidoLocacaoPagosRelatorio(dataInicio, dataFim, unidade).ToList();
                    break;
                case TipoServico.CartaoAcesso:
                case TipoServico.Evento:
                case TipoServico.Avulso:
                case TipoServico.SeguroReembolso:
                case TipoServico.Outros:
                    listaRetorno = _lancamentoCobrancaRepositorio.BuscarPagamentosPagosRelatorio(dataInicio, dataFim, tipoServico.Value, unidade).ToList();
                    break;
                default:
                    throw new BusinessRuleException($"Tipo de Serviço [{tipoServico.Value.ToDescription()}] não implementado!");
            }
            return listaRetorno;
        }

        public IList<LancamentoCobranca> BuscarLancamentosCobrancaLinq(StatusLancamentoCobranca? status, int unidade, int cliente, string dataVencimento, List<int> listaIds)
        {
            return _lancamentoCobrancaRepositorio.BuscarLancamentosCobrancaLinq(status, unidade, cliente, dataVencimento, listaIds);
        }
    }
}