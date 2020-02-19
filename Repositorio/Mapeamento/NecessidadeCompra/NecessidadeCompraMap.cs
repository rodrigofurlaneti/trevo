using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class NecessidadeCompraMap : ClassMap<NecessidadeCompra>
    {
        public NecessidadeCompraMap()
        {
            Table("NecessidadeCompra");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.DataNotificacaoValidade).Not.Nullable();
            Map(x => x.StatusNecessidadeCompra).Not.Nullable();

            References(x => x.Cotacao).Cascade.All();

            HasMany(x => x.MaterialFornecedores).Cascade.All();
        }
    }
}