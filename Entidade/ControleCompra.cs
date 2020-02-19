using Entidade.Base;

namespace Entidade
{
    public class ControleCompra : BaseEntity
    {
        public virtual string Observacao { get; set; }
        public virtual OrcamentoSinistroCotacao OrcamentoSinistroCotacao { get; set; }
        public virtual PecaServico PecaServico { get; set; }
    }
}