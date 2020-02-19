using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class LancamentoCobrancaContratoMensalistaMap : ClassMap<LancamentoCobrancaContratoMensalista>
    {
        public LancamentoCobrancaContratoMensalistaMap()
        {
            Table("LancamentoCobrancaContratoMensalista");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");

            References(x => x.LancamentoCobranca).Column("LancamentoCobranca");
            References(x => x.ContratoMensalista).Column("ContratoMensalista");
        }
    }
}