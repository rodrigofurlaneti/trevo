using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class HorarioUnidadeNotificacaoMap : ClassMap<HorarioUnidadeNotificacao>
    {
        public HorarioUnidadeNotificacaoMap()
        {
            Table("HorarioUnidadeNotificacao");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");

            References(x => x.Notificacao).Column("Notificacao");
            References(x => x.HorarioUnidade).Column("HorarioUnidade");
        }
    }
}