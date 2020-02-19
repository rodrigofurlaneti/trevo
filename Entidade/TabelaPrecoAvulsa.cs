using Entidade.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidade
{
    public class TabelaPrecoAvulsa : BaseEntity
    {
        [Required]
        public virtual string Nome { get; set; }
        [Required]
        public virtual int ToleranciaDesistencia { get; set; }
        [Required]
        public virtual int ToleranciaPagamento { get; set; }
        [Required]
        public virtual decimal ValorDiaria { get; set; }
        [Required]
        public virtual DateTime InicioDiaria { get; set; }
        [Required]
        public virtual DateTime FimDiaria { get; set; }

        public TabelaPrecoAvulsa()
        {
            InicioDiaria = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            FimDiaria = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
        }
    }
}
