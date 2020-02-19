using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class BloqueioReferenciaMap : ClassMap<BloqueioReferencia>
    {
        public BloqueioReferenciaMap()
        {
            Table("BloqueioReferencia");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.DataMesAnoReferencia).Column("DataMesAnoReferencia");
            Map(x => x.Ativo).Column("Ativo");
        }
    }
}
