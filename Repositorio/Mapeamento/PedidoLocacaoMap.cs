using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class PedidoLocacaoMap : ClassMap<PedidoLocacao>
    {
        public PedidoLocacaoMap()
        {
            Table("PedidoLocacao");

            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao");

            References(x => x.Unidade).Column("Unidade");
            References(x => x.Cliente).Column("Cliente");
            References(x => x.TipoLocacao).Column("TipoLocacao");
            References(x => x.Desconto).Column("Desconto").Nullable();

            Map(x => x.Valor).Column("Valor").Length(10).Precision(10).Scale(2);
            Map(x => x.ValorTotal).Column("ValorTotal");
            Map(x => x.PossuiFiador).Column("PossuiFiador");
            Map(x => x.NomeFiador).Column("NomeFiador");
            Map(x => x.FormaGarantia).Column("FormaGarantia").Length(100);
            Map(x => x.DataReajuste).Column("DataReajuste");
            Map(x => x.TipoReajuste).Column("TipoReajuste");
            Map(x => x.ValorReajuste).Column("ValorReajuste");
            Map(x => x.PrazoReajuste).Column("PrazoReajuste");
            Map(x => x.FormaPagamento).Column("FormaPagamento");
            Map(x => x.DataPrimeiroPagamento).Column("DataPrimeiroPagamento");
            Map(x => x.ValorPrimeiroPagamento).Column("ValorPrimeiroPagamento");
            Map(x => x.DataDemaisPagamentos).Column("DataDemaisPagamentos");
            Map(x => x.CicloPagamentos).Column("CicloPagamentos");
            Map(x => x.DataVigenciaInicio).Column("DataVigenciaInicio");
            Map(x => x.DataVigenciaFim).Column("DataVigenciaFim");
            Map(x => x.Ativo).Column("Ativo").Default("1");
            Map(x => x.PossuiCicloMensal).Column("PossuiCicloMensal");
            Map(x => x.Status).Column("Status");

            Map(x => x.Antecipado).Column("Antecipado");
            Map(x => x.RamoAtividade).Column("RamoAtividade");
            Map(x => x.ValorDeposito).Column("ValorDeposito");
            Map(x => x.PrazoContratoDeterminado).Column("PrazoContratoDeterminado");

            HasMany(x => x.PedidoLocacaoLancamentosAdicionais).Cascade.All();
            HasMany(x => x.PedidoLocacaoNotificacoes).Cascade.AllDeleteOrphan();
        }
    }
}