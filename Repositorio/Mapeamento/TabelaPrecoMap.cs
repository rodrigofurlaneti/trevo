using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class TabelaPrecoMap : ClassMap<TabelaPreco>
    {
        public TabelaPrecoMap()
        {
            Table("TabelaPreco");
            LazyLoad();
            Id(tabelaPreco => tabelaPreco.Id).GeneratedBy.Identity().Column("Id");
            Map(tabelaPreco => tabelaPreco.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(tabelaPreco => tabelaPreco.Codigo).Column("Codigo").Nullable();
            Map(tabelaPreco => tabelaPreco.Descricao).Column("Descricao").Not.Nullable();
        }
    }
}
