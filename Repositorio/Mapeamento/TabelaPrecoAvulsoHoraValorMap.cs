using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class TabelaPrecoAvulsoHoraValorMap : ClassMap<TabelaPrecoAvulsoHoraValor>
    {
        public TabelaPrecoAvulsoHoraValorMap()
        {
            Table("TabelaPrecoAvulsoHoraValor");

            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();

            Map(x => x.Hora).Column("Hora").Not.Nullable();
            Map(x => x.Valor).Column("Valor").Length(10).Precision(10).Scale(2).Not.Nullable();
            References(x => x.TabelaPrecoAvulso).Column("TabelaPrecoAvulso");
        }
    }
}