using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class ParametroEquipeNotificacaoMap : ClassMap<ParametroEquipeNotificacao>
    {
        public ParametroEquipeNotificacaoMap()
        {
            Table("ParametroEquipeNotificacao");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");

            References(x => x.Notificacao).Column("Notificacao").Cascade.All();
            References(x => x.ParametroEquipe).Column("ParametroEquipe");
        }
    }
}