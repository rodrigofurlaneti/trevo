using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class TabelaPrecoAvulsoPeriodoMap : ClassMap<TabelaPrecoAvulsoPeriodo>
    {
        public TabelaPrecoAvulsoPeriodoMap()
        {
            Table("TabelaPrecoAvulsoPeriodo");

            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();

            Map(x => x.Periodo).Column("Periodo").Not.Nullable();
            References(x => x.TabelaPrecoAvulso).Column("TabelaPrecoAvulso");
        }
    }
}