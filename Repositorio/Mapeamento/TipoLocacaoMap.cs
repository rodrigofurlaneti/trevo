using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class TipoLocacaoMap : ClassMap<TipoLocacao>
    {
        public TipoLocacaoMap()
        {
            Table("TipoLocacao");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");

            Map(x => x.Descricao).Column("Descricao");
            Map(x => x.DataInsercao).Column("DataInsercao");
        }
    }
}