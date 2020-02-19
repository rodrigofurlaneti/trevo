using Entidade;
using Entidade.Uteis;
using System;

namespace Aplicacao.ViewModels
{
    public class PagamentoReembolsoViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public ContasAPagarViewModel ContasAPagarViewModel { get; set; }
        public string NumeroRecibo { get; set; }
        public StatusPagamentoReembolso StatusPagamentoReembolso { get; set; }
       
        public PagamentoReembolsoViewModel() { }

        public PagamentoReembolsoViewModel(PagamentoReembolso pagamentoReembolso)
        {
            Id = pagamentoReembolso.Id;
            DataInsercao = pagamentoReembolso.DataInsercao;
            ContasAPagarViewModel = new ContasAPagarViewModel(pagamentoReembolso?.ContaAPagar ?? new ContasAPagar());
            NumeroRecibo = pagamentoReembolso.NumeroRecibo;
            StatusPagamentoReembolso = pagamentoReembolso.Status;
        }

        public PagamentoReembolso ToEntity()
        {
            return new PagamentoReembolso
            {
                Id = Id,
                DataInsercao = DataInsercao,
                ContaAPagar = ContasAPagarViewModel?.ToEntity(),
                NumeroRecibo = NumeroRecibo,
                Status = StatusPagamentoReembolso
            };
        }

        public PagamentoReembolsoViewModel ToViewModel(PagamentoReembolso pagamentoReembolso)
        {
            return new PagamentoReembolsoViewModel
            {
                Id = pagamentoReembolso.Id,
                DataInsercao = pagamentoReembolso.DataInsercao,
                ContasAPagarViewModel = new ContasAPagarViewModel(pagamentoReembolso?.ContaAPagar ?? new ContasAPagar()),
                NumeroRecibo = pagamentoReembolso.NumeroRecibo,
                StatusPagamentoReembolso = pagamentoReembolso.Status
            };
        }

    }
}
