using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class ConvenioUnidadeMap : ClassMap<ConvenioUnidade>
    {
        public ConvenioUnidadeMap()
        {
            Table("ConvenioUnidade");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Valor).Column("Valor").Length(10).Precision(10).Scale(2).Not.Nullable();
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            References(x => x.Unidade).Column("Unidade").Not.Nullable();
            References(x => x.TipoSelo).Column("TipoSelo").Not.Nullable();
        }
    }
}