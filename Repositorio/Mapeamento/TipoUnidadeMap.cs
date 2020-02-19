using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class TipoUnidadeMap : ClassMap<TipoUnidade>
    {
        public TipoUnidadeMap()
        {
            Table("TipoUnidade");
            LazyLoad();
            Id(tipoUnidade => tipoUnidade.Id).GeneratedBy.Identity().Column("Id");
            Map(tipoUnidade => tipoUnidade.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(tipoUnidade => tipoUnidade.Codigo).Column("Codigo").Nullable();
            Map(tipoUnidade => tipoUnidade.Descricao).Column("Descricao").Not.Nullable();
        }
    }
}
