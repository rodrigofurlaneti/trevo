using Entidade;
using Entidade.Uteis;
using System;

namespace Aplicacao.ViewModels
{
    public class ContratoUnidadeViewModel
    {
        public int Id { get; set; }
        public Unidade Unidade { get; set; }
        public TipoContrato TipoContrato { get; set; }
        public string NumeroContrato { get; set; }
        public int DiaVencimento { get; set; }
        public int InformarVencimentoDias { get; set; }
        public string Valor { get; set; }
        public TipoValor TipoValor { get; set; }
        public DateTime InicioContrato { get; set; }
        public DateTime FinalContrato { get; set; }
        public bool ExistiraReajuste { get; set; }
        public IndiceReajuste IndiceReajuste { get; set; }
        public bool Ativo { get; set; }
        //public virtual IList<Parcela> Parcelas { get; set; }

        public String displayValor { get; set; }

        public ContratoUnidadeViewModel()
        {
            InicioContrato = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            FinalContrato = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
        }

        public ContratoUnidadeViewModel(ContratoUnidade contrato)
        {
            Id = contrato?.Id ?? 0;
            Unidade = contrato?.Unidade;
            TipoContrato = contrato.TipoContrato;
            NumeroContrato = contrato.NumeroContrato;
            DiaVencimento = contrato.DiaVencimento;
            InformarVencimentoDias = contrato.InformarVencimentoDias;
            Valor = Convert.ToString(contrato.Valor);
            TipoValor = contrato.TipoValor;
            InicioContrato = contrato.InicioContrato;
            FinalContrato = contrato.FinalContrato;
            ExistiraReajuste = contrato.ExistiraReajuste;
            IndiceReajuste = contrato.IndiceReajuste;
            Ativo = contrato.Ativo;
            //var listaParcelas = contrato?.Parcelas?.Select(x => new ParcelaViewModel(x)).ToList();
            //listaParcelas = listaParcelas?.OrderBy(o => o.NumParcela).ToList();
            //Parcelas = listaParcelas;
        }

        public ContratoUnidade ToEntity() => new ContratoUnidade
        {
            Id = Id,
            Unidade = Unidade,
            TipoContrato = TipoContrato,
            NumeroContrato = NumeroContrato,
            DiaVencimento = DiaVencimento,
            InformarVencimentoDias = InformarVencimentoDias,
            Valor = Convert.ToDecimal(Valor),
            TipoValor = TipoValor,
            InicioContrato = InicioContrato,
            FinalContrato = FinalContrato,
            ExistiraReajuste = ExistiraReajuste,
            IndiceReajuste = IndiceReajuste,
            Ativo = Ativo
        };
    }
}