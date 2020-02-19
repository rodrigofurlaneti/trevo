using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class TabelaPrecoMensalistaMap : ClassMap<TabelaPrecoMensalista>
    {
        public TabelaPrecoMensalistaMap()
        {
            Table("TabelaPrecoMensalista");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.Status).Column("Status").Not.Nullable();
            Map(x => x.Nome).Column("Nome").Not.Nullable();
            Map(x => x.Valor).Column("Valor").Length(10).Precision(10).Scale(2).Not.Nullable();
            Map(x => x.DiasCalculo).Column("DiasCalculo").Nullable();
            
            HasMany(x => x.TabelaPrecoUnidade).Cascade.All();

            HasMany(x => x.Notificacoes).Cascade.AllDeleteOrphan();
        }
    }
}
