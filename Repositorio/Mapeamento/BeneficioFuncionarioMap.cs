using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class BeneficioFuncionarioMap : ClassMap<BeneficioFuncionario>
    {
        public BeneficioFuncionarioMap()
        {
            Table("BeneficioFuncionario");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Update();

            References(x => x.Funcionario).Column("Funcionario").Unique();
            HasMany(x => x.BeneficioFuncionarioDetalhes).Cascade.AllDeleteOrphan();
        }
    }
}