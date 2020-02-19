using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao.ViewModels
{
    public class PedidoLocacaoViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }

        public UnidadeViewModel Unidade { get; set; }
        public ClienteViewModel Cliente { get; set; }
        public TipoLocacaoViewModel TipoLocacao { get; set; }
        public string Valor { get; set; }
        public string ValorTotal { get; set; }
        public DescontoViewModel Desconto { get; set; }
        public int IdDesconto { get; set; }
        public bool PossuiFiador { get; set; }
        public string NomeFiador { get; set; }
        public string FormaGarantia { get; set; }
        public DateTime DataReajuste { get; set; }
        public TipoReajuste TipoReajuste { get; set; }
        public string ValorReajuste { get; set; }
        public PrazoReajuste PrazoReajuste { get; set; }
        public FormaPagamento FormaPagamento { get; set; }
        public DateTime DataPrimeiroPagamento { get; set; }
        public string ValorPrimeiroPagamento { get; set; }
        public DateTime DataDemaisPagamentos { get; set; }
        public int CicloPagamentos { get; set; }
        public DateTime DataVigenciaInicio { get; set; }
        public DateTime DataVigenciaFim { get; set; }
        public bool Ativo { get; set; }
        public bool PossuiCicloMensal { get; set; }

        public bool Antecipado { get; set; }
        public string RamoAtividade { get; set; }
        public bool PrazoContratoDeterminado { get; set; }
        public decimal ValorDeposito { get; set; }

        public IList<PedidoLocacaoLancamentoAdicionalViewModel> PedidoLocacaoLancamentosAdicionais { get; set; }
        public StatusSolicitacao Status { get; set; }

        public PedidoLocacaoViewModel()
        {

        }

        public PedidoLocacaoViewModel(PedidoLocacao PedidoLocacao)
        {
            Id = PedidoLocacao.Id;
            Unidade = new UnidadeViewModel(PedidoLocacao.Unidade);
            Cliente = new ClienteViewModel(PedidoLocacao.Cliente);
            TipoLocacao = new TipoLocacaoViewModel(PedidoLocacao.TipoLocacao);
            Valor = PedidoLocacao.Valor.ToString("N2");
            ValorTotal = PedidoLocacao.ValorTotal.ToString("N2");
            Desconto = PedidoLocacao.Desconto == null || PedidoLocacao.Desconto.Id <= 0 ? null : new DescontoViewModel(PedidoLocacao.Desconto);
            IdDesconto = PedidoLocacao.Desconto == null || PedidoLocacao.Desconto.Id <= 0 ? 0 : PedidoLocacao.Desconto.Id;
            PossuiFiador = PedidoLocacao.PossuiFiador;
            NomeFiador = PedidoLocacao.NomeFiador;
            DataReajuste = PedidoLocacao.DataReajuste;
            FormaGarantia = PedidoLocacao.FormaGarantia;
            TipoReajuste = PedidoLocacao.TipoReajuste;
            ValorReajuste = PedidoLocacao.ValorReajuste.ToString("N2");
            PrazoReajuste = PedidoLocacao.PrazoReajuste;
            FormaPagamento = PedidoLocacao.FormaPagamento;
            DataPrimeiroPagamento = PedidoLocacao.DataPrimeiroPagamento;
            ValorPrimeiroPagamento = PedidoLocacao.ValorPrimeiroPagamento.ToString("N2");
            DataDemaisPagamentos = PedidoLocacao.DataDemaisPagamentos;
            CicloPagamentos = PedidoLocacao.CicloPagamentos;
            DataVigenciaInicio = PedidoLocacao.DataVigenciaInicio;
            DataVigenciaFim = PedidoLocacao.DataVigenciaFim;
            Status = PedidoLocacao.Status;
            Ativo = PedidoLocacao.Ativo;
            PossuiCicloMensal = PedidoLocacao.PossuiCicloMensal;

            Antecipado = PedidoLocacao.Antecipado;
            RamoAtividade = PedidoLocacao.RamoAtividade;
            PrazoContratoDeterminado = PedidoLocacao.PrazoContratoDeterminado;
            ValorDeposito = PedidoLocacao.ValorDeposito;

            PedidoLocacaoLancamentosAdicionais = PedidoLocacao?.PedidoLocacaoLancamentosAdicionais?.Select(x => new PedidoLocacaoLancamentoAdicionalViewModel(x))?.ToList() ?? new List<PedidoLocacaoLancamentoAdicionalViewModel>();
        }

        public PedidoLocacao ToEntity()
        {
            var entidade = new PedidoLocacao
            {
                Id = Id,

                Unidade = Unidade.ToEntity(),
                Cliente = Cliente.ToEntity(),
                TipoLocacao = TipoLocacao.ToEntity(),
                Valor = Convert.ToDecimal(Valor),
                ValorTotal = Convert.ToDecimal(ValorTotal),
                Desconto = IdDesconto > 0 ? new Desconto { Id = IdDesconto } : null,
                PossuiFiador = PossuiFiador,
                NomeFiador = NomeFiador,
                FormaGarantia = FormaGarantia,
                DataReajuste = DataReajuste,
                TipoReajuste = TipoReajuste,
                ValorReajuste = Convert.ToDecimal(ValorReajuste),
                PrazoReajuste = PrazoReajuste,
                FormaPagamento = FormaPagamento,
                DataPrimeiroPagamento = DataPrimeiroPagamento,
                ValorPrimeiroPagamento = Convert.ToDecimal(ValorPrimeiroPagamento),
                DataDemaisPagamentos = DataDemaisPagamentos,
                CicloPagamentos = CicloPagamentos,
                DataVigenciaInicio = DataVigenciaInicio,
                DataVigenciaFim = DataVigenciaFim,
                Status = Status,
                Ativo = Ativo,
                PossuiCicloMensal = PossuiCicloMensal,

                Antecipado = Antecipado,
                RamoAtividade = RamoAtividade,
                PrazoContratoDeterminado = PrazoContratoDeterminado,
                ValorDeposito = ValorDeposito,               

                PedidoLocacaoLancamentosAdicionais = PedidoLocacaoLancamentosAdicionais?.Select(x => x.ToEntity())?.ToList() ?? new List<PedidoLocacaoLancamentoAdicional>(),

            };

            return entidade;
        }
    }
}