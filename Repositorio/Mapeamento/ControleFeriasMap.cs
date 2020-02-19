using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class ControleFeriasMap : ClassMap<ControleFerias>
    {
        public ControleFeriasMap()
        {
            Table("ControleFerias");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable().Not.Update();

            Map(x => x.AutorizadoTrabalhar).Column("AutorizadoTrabalhar");
            Map(x => x.DataInicial).Column("DataInicial");
            Map(x => x.DataFinal).Column("DataFinal");

            References(x => x.Funcionario).Column("Funcionario").Cascade.None();

            HasMany(x => x.ListaPeriodoPermitido)
                .Table("ControleFeriasPeriodoPermitido")
                .KeyColumn("ControleFerias")
                .Component(c =>
                {
                    c.Map(x => x.DataDe, "DataDe").Not.Nullable();
                    c.Map(x => x.DataAte, "DataAte").Not.Nullable();
                }).Cascade.AllDeleteOrphan();
        }
    }
}