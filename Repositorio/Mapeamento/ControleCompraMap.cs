using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class ControleCompraMap : ClassMap<ControleCompra>
    {
        public ControleCompraMap()
        {
            Table("ControleCompra");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();

            Map(x => x.Observacao, "Observacao");

            References(x => x.OrcamentoSinistroCotacao, "OrcamentoSinistroCotacao_Id").Cascade.SaveUpdate();
            References(x => x.PecaServico, "PecaServico_Id").Cascade.None();
        }
    }
}