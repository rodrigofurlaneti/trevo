using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class TipoEquipeMap : ClassMap<TipoEquipe>
    {
        public TipoEquipeMap()
        {
            Table("TipoEquipe");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");

            Map(x => x.Descricao).Column("Descricao");
            Map(x => x.DataInsercao).Column("DataInsercao");
            Map(x => x.Ativo).Column("Ativo").Not.Nullable().Default("1");
            
        }
    }
}