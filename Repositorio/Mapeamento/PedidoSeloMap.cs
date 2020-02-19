using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class PedidoSeloMap : ClassMap<PedidoSelo>
    {
        public PedidoSeloMap()
        {
            Table("PedidoSelo");

            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            
            Map(x => x.TiposPagamento).Column("TiposPagamentos");
            Map(x => x.ValidadePedido).Column("ValidadePedido");
            Map(x => x.Quantidade).Column("Quantidade");
            Map(x => x.DiasVencimento).Column("DiasVencimento");
            Map(x => x.DataVencimento).Column("DataVencimento");
            Map(x => x.TipoPedidoSelo).Column("TipoPedidoSelo");

            References(x => x.Cliente).Column("Cliente").Cascade.None();
            References(x => x.Convenio).Column("Convenio").Cascade.None();
            References(x => x.Unidade).Column("Unidade").Cascade.None();
            References(x => x.Desconto).Column("Desconto").Cascade.None();
            References(x => x.TipoSelo).Column("TipoSelo").Cascade.None();
            References(x => x.Usuario).Column("Usuario").Cascade.None();
            References(x => x.Proposta).Column("Proposta").Nullable().Cascade.None();
            References(x => x.EmissaoSelo).Column("EmissaoSelo").Nullable().Cascade.None();

            HasMany(x => x.Notificacoes).Cascade.None();
            HasMany(x => x.PedidoSeloHistorico).Cascade.All();
            HasMany(x => x.PedidoSeloEmails).Cascade.All();
            HasMany(x => x.LancamentoCobranca).Cascade.None();
        }
    }
}