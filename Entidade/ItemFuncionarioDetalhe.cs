using Entidade.Base;
using Entidade.Uteis;
using System;

namespace Entidade
{
    public class ItemFuncionarioDetalhe : BaseEntity
    {
        public virtual Material Material { get; set; }
        public virtual EstoqueMaterial EstoqueMaterial { get; set; }
        public virtual decimal Valor { get; set; }
        public virtual int Quantidade { get; set; }
        public virtual decimal ValorTotal { get; set; }
    }
}