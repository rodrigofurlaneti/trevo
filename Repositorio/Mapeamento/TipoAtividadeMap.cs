using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class TipoAtividadeMap : ClassMap<TipoAtividade>
    {
        public TipoAtividadeMap()
        {
            Table("TipoAtividade");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");

            Map(x => x.Descricao).Column("Descricao");
            Map(x => x.DataInsercao).Column("DataInsercao");
            Map(x => x.Usuario).Column("Usuario");
            Map(x => x.Ativo).Column("Ativo").Not.Nullable().Default("1");

        }
    }
}