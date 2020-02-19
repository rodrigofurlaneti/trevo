using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class PropostaMap : ClassMap<Proposta>
    {
        public PropostaMap()
        {
            Table("Proposta");
            LazyLoad();
            
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();

            Map(x => x.Email).Column("Email").Not.Nullable();

            References(x => x.Cliente).Column("Cliente").Cascade.None();
            References(x => x.Unidade).Column("Unidade").Cascade.None();
        }
    }
}