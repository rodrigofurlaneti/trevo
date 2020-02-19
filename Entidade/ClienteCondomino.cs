using Entidade.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entidade
{
    public class ClienteCondomino : BaseEntity
    {
        public virtual Cliente Cliente { get; set; }
        public virtual UnidadeCondomino Unidade { get; set; }
        public virtual int NumeroVagas { get; set; }
        public virtual IList<CondominoVeiculo> CondominoVeiculos { get; set; }
        public virtual Boolean Frota { get; set; }
    }
}