using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class MaterialNotificacaoMap : ClassMap<MaterialNotificacao>
    {
        public MaterialNotificacaoMap()
        {
            Table("MaterialNotificacao");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");

            References(x => x.Notificacao).Column("Notificacao").Cascade.All();
            References(x => x.Material).Column("Material");
        }
    }
}