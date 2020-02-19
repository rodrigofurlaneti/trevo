using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class EmissaoSeloMap : ClassMap<EmissaoSelo>
    {
        public EmissaoSeloMap()
        {
            Table("EmissaoSelo");

            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");

            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.Validade).Column("Validade").Nullable();
            Map(x => x.StatusSelo).Column("StatusSelo").Nullable();
            Map(x => x.ClienteRemetente).Column("ClienteRemetente").Nullable();
            Map(x => x.EntregaRealizada).Column("EntregaRealizada").Not.Nullable().Default("0");
            Map(x => x.DataEntrega).Column("DataEntrega").Nullable();
            Map(x => x.Responsavel).Column("Responsavel").Nullable();
            Map(x => x.NumeroLote).Column("NumeroLote");
            Map(x => x.NomeImpressaoSelo).Column("NomeImpressaoSelo").Nullable();

            References(x => x.UsuarioAlteracaoStatus).Column("UsuarioAlteracaoStatus").Cascade.None();
            References(x => x.PedidoSelo).Column("PedidoSelo").Cascade.None();

            HasMany(x => x.Selo).Cascade.All();
        }
    }
}