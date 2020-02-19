using Entidade.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entidade
{
    public class UnidadeCondomino : BaseEntity
    {
        [Required]
        public virtual Unidade Unidade { get; set; }
        public virtual int NumeroVagas { get; set; }
        public virtual int NumeroVagasRestantes { get; set; }
        public virtual IList<ClienteCondomino> ClienteCondomino { get; set; }
    }
}