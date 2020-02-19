using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class NegociacaoSeloDescontoNotificacaoMap : ClassMap<NegociacaoSeloDescontoNotificacao>
    {
        public NegociacaoSeloDescontoNotificacaoMap()
        {
            Table("NegociacaoSeloDescontoNotificacao");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");

            References(x => x.Notificacao).Column("Notificacao");
            References(x => x.PedidoSelo).Column("PedidoSelo");
        }
    }
}