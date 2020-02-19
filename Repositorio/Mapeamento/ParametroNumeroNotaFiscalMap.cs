using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class ParametroNumeroNotaFiscalMap : ClassMap<ParametroNumeroNotaFiscal>
    {
        public ParametroNumeroNotaFiscalMap()
        {
            Table("ParametroNumeroNotaFiscal");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.ValorMaximoNota).Column("ValorMaximoNota").Not.Nullable();
            Map(x => x.ValorMaximoNotaDia).Column("ValorMaximoNotaDia").Not.Nullable();
            References(x => x.Unidade).Column("Unidade");
        }
    }
}