using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class TipoOcorrenciaMap : ClassMap<TipoOcorrencia>
    {
        public TipoOcorrenciaMap()
        {
            Table("TipoOcorrencia");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable().Not.Update();
            Map(x => x.Descricao).Not.Nullable();
            Map(x => x.Percentual).Scale(2).Not.Nullable();
        }
    }
}