using Entidade.Base;

namespace Entidade
{
    public class MaterialFornecedor : BaseEntity
    {
        public virtual Material Material { get; set; }
        public virtual Fornecedor Fornecedor { get; set; }
        public virtual bool EhPersonalizado { get; set; }
        public virtual int QuantidadeParaPedidoAutomatico { get; set; }
    }
}