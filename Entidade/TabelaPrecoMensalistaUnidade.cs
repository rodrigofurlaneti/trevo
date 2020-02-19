using Entidade.Base;
using System.ComponentModel.DataAnnotations;

namespace Entidade
{
    public class TabelaPrecoMensalistaUnidade : BaseEntity
    {
        [Required]
        public virtual Unidade Unidade { get; set; }

        public virtual string HorarioInicio { get; set; }
        public virtual string HorarioFim { get; set; }

        public virtual bool HoraAdicional { get; set; }
        public virtual int QuantidadeHoras { get; set; }
        public virtual decimal ValorQuantidade { get; set; }

        public virtual int DiasParaCorte { get; set; }

        public TabelaPrecoMensalistaUnidade()
        {
        }
    }
}