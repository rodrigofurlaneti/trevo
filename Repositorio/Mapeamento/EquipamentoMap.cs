using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class EquipamentoMap : ClassMap<Equipamento>
    {
        public EquipamentoMap()
        {
            Table("Equipamento");
            LazyLoad();
            Id(periodo => periodo.Id).GeneratedBy.Identity().Column("Id");
            Map(periodo => periodo.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(periodo => periodo.Codigo).Column("Codigo").Nullable();
            Map(periodo => periodo.Descricao).Column("Descricao").Not.Nullable();
        }
    }
}
