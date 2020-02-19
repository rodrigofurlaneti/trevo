using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class ArquivoMap : ClassMap<Arquivo>
    {
        public ArquivoMap()
        {
            Table("Arquivos");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");

            Map(x => x.Nome).Column("Nome");
            Map(x => x.Endereco).Column("Endereco");
            Map(x => x.Tipo).Column("Tipo");
            Map(x => x.DataInsercao).Column("DataInsercao");
            Map(x => x.Ativo).Column("Ativo").Not.Nullable().Default("1");

            References(x => x.Usuario).Cascade.None();
        }
    }
}