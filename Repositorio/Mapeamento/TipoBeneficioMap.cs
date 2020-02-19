using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class TipoBeneficioMap : ClassMap<TipoBeneficio>
    {
        public TipoBeneficioMap()
        {
            Table("TipoBeneficio");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Update();

            Map(x => x.Descricao).Column("Descricao").Not.Nullable();
            Map(x => x.Ativo).Column("Ativo").Not.Nullable();
        }
    }
}