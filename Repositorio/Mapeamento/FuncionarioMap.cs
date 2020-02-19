using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class 
	    FuncionarioMap : ClassMap<Funcionario>
    {
        public FuncionarioMap()
        {
            Table("Funcionario");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.Codigo).Column("Codigo").Nullable();
            Map(x => x.Salario).Column("Salario").Nullable();
            Map(x => x.Imagem).Column("Imagem").Nullable().CustomSqlType("varbinary(max)").Length(int.MaxValue);
            Map(x => x.Status).Nullable();
            Map(x => x.DataAdmissao).Column("DataAdmissao").Nullable();
            Map(x => x.TipoEscala).Column("TipoEscala").Default("0").Not.Nullable();

            References(x => x.Pessoa).Column("Pessoa").Cascade.All();

            References(x => x.Supervisor).Column("Supervisor").Cascade.None();
            References(x => x.Unidade).Column("Unidade").Cascade.None();
            References(x => x.Cargo).Column("Cargo").Cascade.None();
            HasOne(x => x.ItemFuncionario).PropertyRef("Funcionario").Cascade.All();
            HasOne(x => x.BeneficioFuncionario).PropertyRef("Funcionario").Cascade.All();
            HasOne(x => x.OcorrenciaFuncionario).PropertyRef("Funcionario").Cascade.All();
            HasMany(x => x.ControlesFerias)
                .KeyColumn("Funcionario")
                .Not.KeyUpdate().Cascade.Delete();

            HasMany(x => x.ListaIntervaloDozeTrintaSeis)
                .Table("FuncionarioIntervaloDozeTrintaSeis")
                .KeyColumn("Funcionario")
                .Component(c =>
                {
                    c.Map(x => x.DataInicial, "DataInicial");
                    c.Map(x => x.DataFinal, "DataFinal");
                }).Cascade.AllDeleteOrphan();

            HasMany(x => x.ListaIntervaloCompensacao)
                .Table("FuncionarioIntervaloCompensacao")
                .KeyColumn("Funcionario")
                .Component(c =>
                {
                    c.Map(x => x.DataInicial, "DataInicial");
                    c.Map(x => x.DataFinal, "DataFinal");
                }).Cascade.AllDeleteOrphan();

            HasMany(x => x.ListaIntervaloNoturno)
                .Table("FuncionarioIntervaloNoturno")
                .KeyColumn("Funcionario")
                .Component(c =>
                {
                    c.Map(x => x.DataInicial, "DataInicial");
                    c.Map(x => x.DataFinal, "DataFinal");
                }).Cascade.AllDeleteOrphan();
        }
    }
}