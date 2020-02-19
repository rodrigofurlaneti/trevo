using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class OrcamentoSinistroCotacaoItemMap : ClassMap<OrcamentoSinistroCotacaoItem>
    {
        public OrcamentoSinistroCotacaoItemMap()
        {
            Table("OrcamentoSinistroCotacaoItem");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");

            Map(r => r.FormaPagamento, "FormaPagamento");
            Map(r => r.Quantidade, "Quantidade");
            Map(r => r.ValorUnitario, "ValorUnitario");
            Map(r => r.ValorTotal, "ValorTotal");
            Map(r => r.DataServico, "DataServico");
            Map(r => r.StatusCompraServico, "StatusCompraServico");
            Map(r => r.NovaData, "NovaData");
            Map(r => r.TemSeguroReembolso, "TemSeguroReembolso");
            Map(r => r.CiaSeguro, "CiaSeguro");
            Map(r => r.DataReembolso, "DataReembolso");
            Map(r => r.ValorReembolso, "ValorReembolso");
            References(r => r.Oficina, "Oficina_Id").Cascade.None();
            References(r => r.ContasAPagar, "ContasAPagar_Id").Cascade.All();
            References(r => r.LancamentoCobranca, "LancamentoCobranca_Id").Cascade.All();
            References(r => r.Fornecedor, "Fornecedor_Id").Cascade.None();
            References(r => r.OrcamentoSinistroCotacao, "OrcamentoSinistroCotacao_Id").Cascade.None();
            References(r => r.PecaServico, "PecaServico_Id").Cascade.None();

            HasMany(x => x.OrcamentoSinistroCotacaoHistoricoDataItens)
                .Table("OrcamentoSinistroCotacaoHistoricoDataItem")
                .KeyColumn("OrcamentoSinistroCotacaoHistoricoDataItem_Id")
                .Component(c => {
                    c.Map(r => r.Data, "Data");
                }).Cascade.All();
        }
    }
}