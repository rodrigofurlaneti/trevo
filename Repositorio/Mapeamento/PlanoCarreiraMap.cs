using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class PlanoCarreiraMap : ClassMap<PlanoCarreira>
    {
        public PlanoCarreiraMap()
        {
            Table("PlanoCarreira");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable().Not.Update();
            Map(x => x.Valor).Scale(2).Not.Nullable();
            Map(x => x.Descricao).Not.Nullable();
            Map(x => x.AnoDe).Scale(2).Not.Nullable();
            Map(x => x.AnoAte).Scale(2).Not.Nullable();
        }
    }
}