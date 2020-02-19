using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class DepartamentoMap : ClassMap<Departamento>
    {
        public DepartamentoMap()
        {
            Table("Departamento");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Nome).Column("Nome");
            Map(x => x.Sigla).Column("Sigla");

            HasMany(x => x.DepartamentoResponsaveis)
                .Table("DepartamentoResponsavel")
                .KeyColumn("Departamento")
                .ForeignKeyConstraintName("FK_Departamento_To_DepartamentoResponsavel")
                .Component(c =>
                {
                    c.References(r => r.Funcionario).Column("Responsavel").Cascade.None();
                }).Cascade.All();
        }
    }
}
