using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class ParametrizacaoLocacaoMap : ClassMap<ParametrizacaoLocacao>
    {
        public ParametrizacaoLocacaoMap()
        {
            Table("ParametrizacaoLocacao");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");

            Map(x => x.DataInsercao).Column("DataInsercao");

            References(x => x.TipoLocacao).Column("TipoLocacao").Cascade.None();
            References(x => x.Unidade).Column("Unidade").Cascade.None();
        }
    }
}