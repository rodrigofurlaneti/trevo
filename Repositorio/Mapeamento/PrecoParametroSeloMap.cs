using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class PrecoParametroSeloMap : ClassMap<PrecoParametroSelo>
    {
        public PrecoParametroSeloMap()
        {
            Table("PrecoParametroSelo");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DescontoTabelaPreco).Column("DescontoTabelaPreco").Not.Nullable();

            References(x => x.TipoPreco).Column("TipoPreco").Cascade.None();
            
            Map(x => x.DescontoMaximoValor).Column("DescontoMaximoValor").Not.Nullable();

            Map(x => x.DescontoCustoTabelaPreco).Column("DescontoCustoTabelaPreco").Nullable();

            References(x => x.Unidade).Column("Unidade");
            References(x => x.Perfil).Column("Perfil");
        }
    }
}