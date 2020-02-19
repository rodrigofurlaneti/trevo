using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class DescontoMap : ClassMap<Desconto>
    {
        public DescontoMap()
        {
            Table("Desconto");
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao");
            Map(x => x.Descricao).Column("Descricao");
            Map(x => x.Valor).Column("Valor").Length(10).Precision(10).Scale(2);
            Map(x => x.NecessitaAprovacao).Column("NecessitaAprovacao");
            Map(x => x.TipoDesconto).Column("TipoDesconto");

            HasMany(x => x.Notificacoes).Cascade.AllDeleteOrphan();
        }
    }
}

