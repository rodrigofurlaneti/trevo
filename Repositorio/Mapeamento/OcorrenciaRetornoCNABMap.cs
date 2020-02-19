using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class OcorrenciaRetornoCNABMap : ClassMap<OcorrenciaRetornoCNAB>
    {
        public OcorrenciaRetornoCNABMap()
        {
            Table("OcorrenciaRetornoCNAB");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();

            Map(x => x.Codigo);
            Map(x => x.Descricao);
        }
    }
}