using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class PecaServicoMap : ClassMap<PecaServico>
    {
        public PecaServicoMap()
        {
            Table("PecaServico");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.Nome).Column("Nome").Not.Nullable();
        }
    }
}