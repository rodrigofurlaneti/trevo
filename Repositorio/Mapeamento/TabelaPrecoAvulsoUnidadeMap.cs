using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class TabelaPrecoAvulsoUnidadeMap : ClassMap<TabelaPrecoAvulsoUnidade>
    {
        public TabelaPrecoAvulsoUnidadeMap()
        {
            Table("TabelaPrecoAvulsoUnidade");

            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.HoraInicio).Column("HoraInicio").Not.Nullable();
            Map(x => x.HoraFim).Column("HoraFim").Not.Nullable();
            Map(x => x.ValorDiaria).Column("ValorDiaria").Nullable();

            References(x => x.Unidade).Column("Unidade").Cascade.None();
            References(x => x.TabelaPrecoAvulso).Column("TabelaPrecoAvulso").Cascade.None();
        }
    }
}