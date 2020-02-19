using Entidade.Base;
using Entidade.Uteis;
using System;
using System.Collections.Generic;

namespace Entidade
{
    public class ItemFuncionario : BaseEntity
    {
        public virtual Funcionario Funcionario { get; set; }
        public virtual Funcionario ResponsavelEntrega { get; set; }
        public virtual DateTime? DataEntrega { get; set; }
        public virtual Funcionario ResponsavelDevolucao { get; set; }
        public virtual DateTime? DataDevolucao { get; set; }
        public virtual IList<ItemFuncionarioDetalhe> ItemFuncionariosDetalhes { get; set; }
    }
}