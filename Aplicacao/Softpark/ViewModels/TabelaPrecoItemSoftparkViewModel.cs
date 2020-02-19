using Entidade;
using System;
using System.Linq;

namespace Aplicacao.ViewModels
{
    public class TabelaPrecoItemSoftparkViewModel : BaseSoftparkViewModel
    {
        public decimal Hora { get; set; }

        public decimal Minuto { get; set; }

        public decimal Valor { get; set; }

        public bool Ativo { get; set; }

        public virtual int TabelaPrecoId { get; set; }

        public TabelaPrecoItemSoftparkViewModel()
        {
            DataInsercao = DateTime.Now;
        }
    }
}
