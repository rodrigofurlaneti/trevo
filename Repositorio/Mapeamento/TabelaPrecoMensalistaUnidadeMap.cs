using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class TabelaPrecoMensalistaUnidadeMap : ClassMap<TabelaPrecoMensalistaUnidade>
    {
        public TabelaPrecoMensalistaUnidadeMap()
        {
            Table("TabelaPrecoMensalistaUnidade");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");

            References(x => x.Unidade).Column("Unidade").Cascade.None();
            Map(x => x.HorarioInicio).Column("HorarioInicio");
            Map(x => x.HorarioFim).Column("HorarioFim");
            Map(x => x.HoraAdicional).Column("HoraAdicional");
            Map(x => x.QuantidadeHoras).Column("QuantidadeHoras");
            Map(x => x.ValorQuantidade).Column("ValorQuantidade");
            Map(x => x.DiasParaCorte).Column("DiasParaCorte");
        }
    }
}