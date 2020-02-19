using Entidade.Uteis;
using System;

namespace Entidade
{
    public class DadosPagamentoVO
    {
        public virtual int CobrancaId { get; set; }
        public virtual string Unidade { get; set; }
        public virtual string Cliente { get; set; }
        public virtual string Placa { get; set; }
        public virtual string Contrato { get; set; }
        public virtual decimal VlrContrato { get; set; }
        public virtual decimal VlrBoleto { get; set; }
        public virtual decimal VlrPedido { get; set; }
        public virtual decimal VlrPago { get; set; }
        public virtual decimal VlrDesconto { get; set; }

        public virtual decimal VlrDiferenca
        {
            get
            {
                return (TipoServico == TipoServico.Convenio || TipoServico == TipoServico.Mensalista || TipoServico == TipoServico.Locacao ? VlrContrato : VlrBoleto) - (VlrPago - VlrDesconto);
            }
        }

        public virtual decimal VlrReajuste { get; set; }
        public virtual decimal VlrTotal { get; set; }
        public virtual decimal VlrMulta { get; set; }
        public virtual decimal VlrJuros { get; set; }
        public virtual DateTime DataEmissao { get; set; }
        public virtual DateTime DataGeracao { get; set; }
        public virtual DateTime DataVencimento { get; set; }
        public virtual DateTime DataCompetencia { get; set; }
        public virtual DateTime DataPagamento { get; set; }
        public virtual DateTime DataInicioVaga { get; set; }
        public virtual DateTime DataFimVaga { get; set; }
        public virtual DateTime DataReajuste { get; set; }
        public virtual DateTime? ValidadePedido { get; set; }
        public virtual string TipoSelo { get; set; }
        public virtual string TipoPedidoSelo { get; set; }
        public virtual string TipoReajuste { get; set; }
        public virtual int Quantidade { get; set; }
        public virtual int TotalVagasUnidade { get; set; }
        public virtual bool Pago { get; set; }
        public virtual string Documento { get; set; }
        public virtual string NomeConvenio { get; set; }
        public virtual TipoServico TipoServico { get; set; }
    }
}