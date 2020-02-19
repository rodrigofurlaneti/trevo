using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class TabelaPrecoAvulsoMap : ClassMap<TabelaPrecoAvulso>
    {
        public TabelaPrecoAvulsoMap()
        {
            Table("TabelaPrecoAvulso");

            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.Nome).Column("Nome").Not.Nullable();
            Map(x => x.Numero).Column("Numero").Not.Nullable();
            Map(x => x.TempoToleranciaDesistencia).Column("TempoToleranciaDesistencia").Not.Nullable();
            Map(x => x.TempoToleranciaPagamento).Column("TempoToleranciaPagamento").Not.Nullable();
            Map(x => x.HoraAdicional).Column("HoraAdicional").Not.Nullable();
            Map(x => x.Padrao).Column("Padrao").Not.Nullable();
            Map(x => x.QuantidadeHoraAdicional).Column("QuantidadeHoraAdicional").Not.Nullable();
            Map(x => x.ValorHoraAdicional).Column("ValorHoraAdicional").Not.Nullable();
            Map(x => x.DescricaoHoraValor).Column("DescricaoHoraValor").Nullable();
            Map(x => x.Status).Column("Status").Not.Nullable();
            Map(x => x.HoraInicioVigencia).Column("HoraInicioVigencia").Not.Nullable();
            Map(x => x.HoraFimVigencia).Column("HoraFimVigencia").Not.Nullable();

            References(x => x.Usuario).Column("Usuario");

            HasMany(x => x.ListaPeriodo).Cascade.AllDeleteOrphan();
            HasMany(x => x.ListaHoraValor).Cascade.AllDeleteOrphan();
            HasMany(x => x.ListaUnidade).Cascade.All();

            HasMany(x => x.Notificacoes).Cascade.AllDeleteOrphan();
        }
    }
}