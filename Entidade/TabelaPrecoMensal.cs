using Entidade.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidade
{
    public class TabelaPrecoMensal : BaseEntity
    {
        [Required]
        public virtual string Nome { get; set; }
        [Required]
        public virtual decimal Valor { get; set; }
    }
}
