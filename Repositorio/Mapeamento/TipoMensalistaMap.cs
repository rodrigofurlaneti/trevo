using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class TipoMensalistaMap : ClassMap<TipoMensalista>
    {
        public TipoMensalistaMap()
        {
            Table("TipoMensalista");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Descricao).Column("Descricao");
            Map(x => x.DataInsercao).Column("DataInsercao");
            Map(x => x.Ativo).Column("Ativo").Not.Nullable().Default("1");
        }
    }
}