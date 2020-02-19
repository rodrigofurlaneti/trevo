using Entidade.Base;

namespace Entidade
{
    public class EquipamentoUnidadeEquipamento : BaseEntity
    {
        public virtual bool Ativo { get; set; }
        public virtual int Quantidade { get; set; }
        public virtual int EquipamentoUnidade { get; set; }
        public virtual Equipamento Equipamento { get; set; }
    }
}
