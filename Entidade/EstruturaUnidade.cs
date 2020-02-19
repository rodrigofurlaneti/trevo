using Entidade.Base;

namespace Entidade
{
    public class EstruturaUnidade : BaseEntity
    {
        public virtual EstruturaGaragem EstruturaGaragem { get;set;}
        public virtual int Quantidade { get; set; }
        public virtual bool SolicitarCompra { get; set; }
        public virtual int Unidade { get; set; }
    }
}
