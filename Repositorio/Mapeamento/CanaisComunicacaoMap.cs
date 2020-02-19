using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class CanaisComunicacaoMap : ClassMap<CanaisComunicacao>
    {
        public CanaisComunicacaoMap()
        {
            Table("CanaisComunicacao");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.TipoComunicacao).Column("TipoComunicacao");
            Map(x => x.CanalComunicacao).Column("CanalComunicacao");
        }
    }
}
