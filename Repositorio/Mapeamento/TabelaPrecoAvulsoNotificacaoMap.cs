using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class TabelaPrecoAvulsoNotificacaoMap : ClassMap<TabelaPrecoAvulsoNotificacao>
    {
        public TabelaPrecoAvulsoNotificacaoMap()
        {
            Table("TabelaPrecoAvulsoNotificacao");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");

            References(x => x.Notificacao).Column("Notificacao").Cascade.All();
            References(x => x.TabelaPrecoAvulso).Column("TabelaPrecoAvulso").Cascade.None();
        }
    }
}