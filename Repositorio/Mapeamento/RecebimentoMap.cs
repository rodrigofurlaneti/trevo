using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class RecebimentoMap : ClassMap<Recebimento>
    {
        public RecebimentoMap()
        {
            Table("Recebimento");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();

            Map(x => x.StatusRecebimento).Column("StatusRecebimento").Nullable();
            
            HasMany(x => x.Pagamentos).KeyColumn("Recebimento").Inverse().Cascade.All();
            HasMany(x => x.LancamentosCobranca).KeyColumn("Recebimento").Inverse().Cascade.None();

            //HasMany(x => x.DescontosPagamento).KeyColumn("Recebimento").Inverse().Cascade.AllDeleteOrphan();
        }
    }
}