using Entidade;
using System;
using System.Data.SqlTypes;

namespace Aplicacao.ViewModels
{
    public class TabelaPrecoAvulsaViewModel
    {
        public TabelaPrecoAvulsaViewModel(TabelaPrecoAvulso tabelaPrecoAvulso)
        {
            InicioDiaria = SqlDateTime.MinValue.Value;
            FimDiaria = SqlDateTime.MinValue.Value;
        }

        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public virtual string Nome { get; set; }
        public virtual int ToleranciaDesistencia { get; set; }
        public virtual int ToleranciaPagamento { get; set; }
        public virtual decimal ValorDiaria { get; set; }
        public virtual DateTime InicioDiaria { get; set; }
        public virtual DateTime FimDiaria { get; set; }
    }
}
