using Core.Exceptions;
using Core.Extensions;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Dominio
{
    public interface IEmissaoSeloServico : IBaseServico<EmissaoSelo>
    {
        IList<EmissaoSelo> ListarEmissaoSeloFiltro(int? idUnidade, int? idCliente, int? idTipoSelo, int? idEmissao);
        IList<EmissaoSelo> ListarEmissaoSeloFiltroSimples(StatusSelo? status, int? idUnidade, int? idCliente, int? idTipoSelo, int? idEmissao);
        EmissaoSelo GerarEmissaoSelos(EmissaoSelo filtro, int idPedido, DateTime? dataValidade);
        void SalvarEmissaoSeloGerada(EmissaoSelo emissaoSelo);
        List<int> RetornaListaIdPedidoSeloDasEmissoesDeSelo(List<int> listaIdPedido);
        decimal RetornaValorSeloComDesconto(PedidoSelo filtro);
    }

    public class EmissaoSeloServico : BaseServico<EmissaoSelo, IEmissaoSeloRepositorio>, IEmissaoSeloServico
    {
        private readonly IEmissaoSeloRepositorio _emissaoSeloRepositorio;
        private readonly IPedidoSeloServico _pedidoSeloServico;
        private readonly ITipoSeloRepositorio _tipoSeloRepositorio;
        private readonly IUnidadeRepositorio _unidadeRepositorio;
        private readonly IPrecoParametroSeloRepositorio _precoParametroRepositorio;
        private readonly ITabelaPrecoAvulsoRepositorio _tabelaPrecoAvulsoRepositorio;

        public EmissaoSeloServico(IEmissaoSeloRepositorio emissaoSeloRepositorio,
                                  IPedidoSeloServico pedidoSeloServico,
                                  ITipoSeloRepositorio tipoSeloRepositorio,
                                  IUnidadeRepositorio unidadeRepositorio,
                                  IPrecoParametroSeloRepositorio precoParametroRepositorio,
                                   ITabelaPrecoAvulsoRepositorio tabelaPrecoAvulsoRepositorio)
        {
            _emissaoSeloRepositorio = emissaoSeloRepositorio;
            _pedidoSeloServico = pedidoSeloServico;
            _tipoSeloRepositorio = tipoSeloRepositorio;
            _unidadeRepositorio = unidadeRepositorio;
            _precoParametroRepositorio = precoParametroRepositorio;
            _tabelaPrecoAvulsoRepositorio = tabelaPrecoAvulsoRepositorio;
        }

        public void Salvar()
        {

        }

        public IList<EmissaoSelo> ListarEmissaoSeloFiltro(int? idUnidade, int? idCliente, int? idTipoSelo, int? idEmissao)
        {
            return _emissaoSeloRepositorio.List()
                .Where(x => (idUnidade != 0 ? x.PedidoSelo.Unidade.Id == idUnidade : x.PedidoSelo.Unidade.Id == x.PedidoSelo.Unidade.Id)
                    && (idCliente != 0 ? x.PedidoSelo.Cliente.Id == idCliente : x.PedidoSelo.Cliente.Id == x.PedidoSelo.Cliente.Id)
                    && (idTipoSelo != 0 ? x.PedidoSelo.TipoSelo.Id == idTipoSelo : x.PedidoSelo.TipoSelo.Id == x.PedidoSelo.TipoSelo.Id)
                    && (idEmissao != 0 ? x.Id == idEmissao : x.Id == x.Id))
                .ToList();
        }

        public IList<EmissaoSelo> ListarEmissaoSeloFiltroSimples(StatusSelo? status, int? idUnidade, int? idCliente, int? idTipoSelo, int? idEmissao)
        {
            return _emissaoSeloRepositorio.ListarEmissaoSeloFiltroSimples(status, idUnidade, idCliente, idTipoSelo, idEmissao);
        }

        /// <summary>
        /// Gera uma nova Emissao e os seus selos
        /// </summary>
        /// <param name="filtro"> Emissão a ser criada</param>
        /// <param name="idPedido">Id do pedido, para criação dos selos</param>
        /// <returns>Id da nova Emissão</returns>
        public EmissaoSelo GerarEmissaoSelos(EmissaoSelo filtro, int idPedido, DateTime? dataValidade)
        {
            decimal valorSeloComDesconto = 0;

            var listSelo = new List<Selo>();
            var _pedido = _pedidoSeloServico.BuscarPorId(idPedido);

            ValidarRegraPedidoTipoPagamento(_pedido);

            valorSeloComDesconto = RetornaValorSeloComDesconto(_pedido);

            var tiposelo = _pedido.TipoSelo;
            var unidade = _pedido.Unidade;
            var emissaoSelo = filtro;
            var dataAtual = DateTime.Now;

            for (int i = 1; i <= _pedido.Quantidade; i++)
            {
                var selo = new Selo
                {
                    Sequencial = i,
                    Validade = dataValidade,
                    Valor = valorSeloComDesconto,
                    DataInsercao = dataAtual,
                    EmissaoSelo = emissaoSelo
                };

                listSelo.Add(selo);
            }

            emissaoSelo.NomeImpressaoSelo = _pedido.Cliente.NomeConvenio;
            emissaoSelo.NumeroLote = _emissaoSeloRepositorio.RetornaProximoNumeroLote().ToString();
            emissaoSelo.PedidoSelo = _pedido;
            emissaoSelo.Validade = dataValidade;
            emissaoSelo.StatusSelo = StatusSelo.Ativo;
            emissaoSelo.Selo = listSelo;

            return emissaoSelo;
        }

        /// <summary>
        /// Retorna o valor do desconto para os selos do pedido
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns>o valor do desconto</returns>
        public decimal RetornaValorSeloComDesconto(PedidoSelo filtro)
        {
            decimal valorSeloComDesconto = 0;

            var tipoSelo = _tipoSeloRepositorio.GetById(filtro.TipoSelo.Id);
            var listPrecosParametro = _precoParametroRepositorio.List().ToList();
            var unidade = _unidadeRepositorio.GetById(filtro.Unidade.Id);
            var precoParametro = listPrecosParametro.Where(x => x.Unidade.Id == filtro.Unidade.Id && x.TipoPreco.Id == filtro.TipoSelo.Id).FirstOrDefault();
            var tabelaPreco = _tabelaPrecoAvulsoRepositorio.FirstBy(x => x.Padrao && x.ListaUnidade.Count(y => y.Unidade.Id == filtro.Unidade.Id) > 0);

            if (tipoSelo.ParametroSelo == ParametroSelo.HoraInicial)
            {
                valorSeloComDesconto = tipoSelo.Valor;
            }
            else if (tipoSelo.ParametroSelo == ParametroSelo.HoraAdicional)
            {
                valorSeloComDesconto = tipoSelo.Valor;
            }
            else if (tipoSelo.ParametroSelo == ParametroSelo.Monetario)
            {
                valorSeloComDesconto = tipoSelo.Valor;
            }
            else if (tipoSelo.ParametroSelo == ParametroSelo.Percentual)
            {
                if (tabelaPreco != null)
                {
                    var diaria = tabelaPreco.ListaUnidade.FirstOrDefault(y => y.Unidade.Id == filtro.Unidade.Id).ValorDiaria;
                    decimal desconto;
                    if (precoParametro != null)
                    {
                        desconto = precoParametro.DescontoCustoTabelaPreco;

                        valorSeloComDesconto = diaria - (diaria * desconto);
                    }
                }
                else
                {
                    valorSeloComDesconto = tipoSelo.Valor;
                }
            }
            else if (tipoSelo.ParametroSelo == ParametroSelo.Personalizado)
            {
                valorSeloComDesconto = filtro.Convenio.ConvenioUnidades.FirstOrDefault(x => x.ConvenioUnidade.Unidade.Id == filtro.Unidade.Id).ConvenioUnidade.Valor;
            }

            if (valorSeloComDesconto < 0)
                valorSeloComDesconto = 0;

            return valorSeloComDesconto;
        }

        private void ValidarRegraPedidoTipoPagamento(PedidoSelo pedido)
        {
            //var pedidoRetorno = _pedidoSeloServico.BuscarPorId(pedido.Id);
            var lancamentoCobranca = pedido.UltimoLancamento;

            var valorLancamentoCobranca = _pedidoSeloServico.CalculaValorLancamentoCobranca(pedido);
            if (lancamentoCobranca == null && valorLancamentoCobranca > 0)
                throw new BusinessRuleException($"Não há lançamento de cobrança para o pedido [Id: {pedido.Id}]");

            switch (pedido.TiposPagamento)
            {
                case TipoPagamentoSelo.Prepago:
                    if (valorLancamentoCobranca > 0
                        && lancamentoCobranca.StatusLancamentoCobranca != StatusLancamentoCobranca.Pago)
                        throw new BusinessRuleException($"Pedidos com o tipo de pagamento \"{TipoPagamentoSelo.Prepago.ToDescription()}\" para poderem emitir selo(s) o lançamento de cobrança deve estar com o status \"{StatusLancamentoCobranca.Pago.ToDescription()}\"");
                    break;
                case TipoPagamentoSelo.Pospago:
                    if (valorLancamentoCobranca > 0
                        && (lancamentoCobranca.StatusLancamentoCobranca == StatusLancamentoCobranca.Vencido
                            || lancamentoCobranca.StatusLancamentoCobranca == StatusLancamentoCobranca.Cancelado))
                        throw new BusinessRuleException($"Pedidos com o tipo de pagamento \"{TipoPagamentoSelo.Pospago.ToDescription()}\" para poderem emitir selo(s) o lançamento de cobrança deve estar com o status diferente de \"{StatusLancamentoCobranca.Cancelado.ToDescription()}\" e \"{StatusLancamentoCobranca.Vencido.ToDescription()}\"");
                    break;
                default:
                    throw new BusinessRuleException("Tipo de pagamento não implementado");
            }
        }

        public void SalvarEmissaoSeloGerada(EmissaoSelo emissaoSelo)
        {
            _emissaoSeloRepositorio.Save(emissaoSelo);

            var pedido = _pedidoSeloServico.BuscarPorId(emissaoSelo.PedidoSelo.Id);
            pedido.EmissaoSelo = new EmissaoSelo { Id = emissaoSelo.Id };
            _pedidoSeloServico.SalvarComRetorno(pedido);
        }

        public List<int> RetornaListaIdPedidoSeloDasEmissoesDeSelo(List<int> listaIdPedido)
        {
            return _emissaoSeloRepositorio.RetornaListaIdPedidoSeloDasEmissoesDeSelo(listaIdPedido);
        }
    }
}