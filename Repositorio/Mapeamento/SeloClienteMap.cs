using Entidade;
using Entidade.Uteis;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class SeloClienteMap : ClassMap<SeloCliente>
    {
        public SeloClienteMap()
        {
            Table("SeloCliente");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");

            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.TipoPagamentoSelo).Column("TipoPagamentoSelo").CustomType(typeof(TipoPagamentoSelo)).Not.Nullable();
            Map(x => x.ValidadeSelo).Column("ValidadeSelo").Not.Nullable();
            Map(x => x.PrazoPagamentoSelo).Column("PrazoPagamentoSelo").Not.Nullable();
            Map(x => x.EmissaoNF).Column("EmissaoNF").Not.Nullable();

            References(x => x.Cliente).Column("Cliente_Id").Cascade.None().Not.Nullable();
        }
    }
}
