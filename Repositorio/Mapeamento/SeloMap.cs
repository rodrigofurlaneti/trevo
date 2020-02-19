using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class SeloMap : ClassMap<Selo>
    {
        public SeloMap()
        {
            Table("Selo");

            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");

            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.Sequencial).Column("Sequencial").Nullable();
            Map(x => x.Validade).Column("Validade").Nullable();
            Map(x => x.Valor).Column("Valor").Length(10).Precision(10).Scale(2).Nullable();

            References(x => x.EmissaoSelo).Column("EmissaoSelo").Cascade.None();
        }
    }
}