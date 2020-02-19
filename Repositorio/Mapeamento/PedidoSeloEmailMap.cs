using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class PedidoSeloEmailMap : ClassMap<PedidoSeloEmail>
    {
        public PedidoSeloEmailMap()
        {
            Table("PedidoSeloEmail");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao");

            Map(x => x.Email).Column("Email");
            Map(x => x.Enviado).Column("Enviado");
            Map(x => x.Descricao).Column("Descricao");
            Map(x => x.Tipo).Column("Tipo");

            References(x => x.PedidoSelo).Column("PedidoSelo").Cascade.None();
            References(x => x.Proposta).Column("Proposta").Cascade.None();
        }
    }
}