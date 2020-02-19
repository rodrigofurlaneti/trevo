using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class CheckListEstruturaUnidadeMap : ClassMap<CheckListEstruturaUnidade>
    {
        public CheckListEstruturaUnidadeMap()
        {
            Table("CheckListEstruturaUnidade");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");

            Map(x => x.EstruturaGaragem).Column("EstruturaGaragem");
            Map(x => x.Quantidade).Column("Quantidade");
            Map(x => x.Ativo).Column("Ativo").Not.Nullable().Default("1");
            Map(x => x.DataInsercao).Column("DataInsercao");
        }
    }
}