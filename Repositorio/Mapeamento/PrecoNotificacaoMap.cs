using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class PrecoNotificacaoMap : ClassMap<PrecoNotificacao>
    {
        public PrecoNotificacaoMap()
        {
            Table("PrecoNotificacao");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");

            References(x => x.Notificacao).Column("Notificacao");
            References(x => x.Preco).Column("Preco");
        }
    }
}