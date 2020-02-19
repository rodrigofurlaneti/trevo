using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class TabelaPrecoMensalistaNotificacaoMap : ClassMap<TabelaPrecoMensalistaNotificacao>
    {
        public TabelaPrecoMensalistaNotificacaoMap()
        {
            Table("TabelaPrecoMensalistaNotificacao");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");

            References(x => x.Notificacao).Column("Notificacao").Cascade.All();
            References(x => x.TabelaPrecoMensalista).Column("TabelaPrecoMensalista");
        }
    }
}