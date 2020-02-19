using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class PedidoSeloHistoricoMap : ClassMap<PedidoSeloHistorico>
    {
        public PedidoSeloHistoricoMap()
        {
            Table("PedidoSeloHistorico");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao");
            
            Map(x => x.StatusPedidoSelo).Column("StatusPedidoSelo");
            Map(x => x.Descricao).Column("Descricao");

            References(x => x.Usuario).Column("Usuario").Cascade.None();
            References(x => x.PedidoSelo).Column("PedidoSelo").Cascade.None();
            References(x => x.Proposta).Column("Proposta").Cascade.None();
        }
    }
}