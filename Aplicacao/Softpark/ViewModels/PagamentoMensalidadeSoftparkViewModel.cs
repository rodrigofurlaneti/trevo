using Entidade;
using Entidade.Uteis;
using System;
using System.ComponentModel.DataAnnotations;

namespace Aplicacao.ViewModels
{
    public class PagamentoMensalidadeSoftparkViewModel : BaseSoftparkViewModel
    {
        public string Contrato { get; set; }

        public string Ticket { get; set; }

        public DateTime DataPagamento { get; set; }

        public decimal Valor { get; set; }

        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        public DateTime DataValidade { get; set; }

        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        public FormaPagamento FormaPagamento { get; set; }

        public int FaturamentoId { get; set; }

        public string Observacao { get; set; }

        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        public int CodigoBaixa { get; set; }
        
        public virtual EstacionamentoSoftparkViewModel Estacionamento { get; set; }
        
        public virtual CondutorSoftparkViewModel Condutor { get; set; }

        public PagamentoMensalidadeSoftparkViewModel()
        {
        }

        public PagamentoMensalidadeSoftparkViewModel(Pagamento pagamento)
        {
            Id = pagamento.PagamentoMensalistaId.HasValue && pagamento.PagamentoMensalistaId.Value > 0 ? pagamento.PagamentoMensalistaId.Value : pagamento.Id;
            DataInsercao = pagamento.DataInsercao;
            Valor = pagamento.ValorPago;
            DataPagamento = pagamento.DataPagamento;
            FormaPagamento = pagamento.FormaPagamento;
        }

        public Pagamento ToPagamento(Recebimento recebimento)
        {   
            var pagamento = new Pagamento
            {
                Id = 0,
                PagamentoMensalistaId = Id > 0 ? Id : default(int?),
                DataInsercao = DataInsercao,
                ValorPago = Valor,
                DataPagamento = DataPagamento,
                NumeroRecibo = CodigoBaixa.ToString(),
                Recebimento = recebimento,
                FormaPagamento = FormaPagamento,
                ContaContabil = null,
                DataEnvio = DateTime.Now,
                StatusPagamento = true
            };

            return pagamento;
        }
    }
}
