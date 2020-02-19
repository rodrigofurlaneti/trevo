using Entidade.Base;
using System;
namespace Entidade
{
    public class EstoqueManualItem : BaseEntity
    {
        public virtual int CodigoMaterial { get; set; }
        public virtual string NumeroMaterial { get; set; }
        public virtual bool Inventariado { get; set; }

        public virtual Estoque Estoque { get; set; }
        public virtual Unidade Unidade { get; set; }
        public virtual Material Material { get; set; }
    }
}