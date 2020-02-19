using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class BeneficioFuncionarioDetalheMap : ClassMap<BeneficioFuncionarioDetalhe>
    {
        public BeneficioFuncionarioDetalheMap()
        {
            Table("BeneficioFuncionarioDetalhe");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Update();

            Map(x => x.Valor).Column("Valor").Not.Nullable().Scale(2);
            References(x => x.TipoBeneficio).Column("TipoBeneficio").Not.Nullable().Cascade.None();
        }
    }
}