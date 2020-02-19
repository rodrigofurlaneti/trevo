using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class EstruturaUnidadeMap : ClassMap<EstruturaUnidade>
    {
        public EstruturaUnidadeMap()
        {
            Table("EstruturaUnidade");
            LazyLoad();
            Id(estruturaUnidade => estruturaUnidade.Id).GeneratedBy.Identity().Column("Id");
            Map(estruturaUnidade => estruturaUnidade.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(estruturaUnidade => estruturaUnidade.Quantidade).Column("Quantidade").Nullable();
            Map(estruturaUnidade => estruturaUnidade.SolicitarCompra).Column("SolicitarCompra").Nullable();
            References(estruturaUnidade => estruturaUnidade.EstruturaGaragem).Column("EstruturaGaragem").Cascade.None();
        }
    }
}
                                                                   