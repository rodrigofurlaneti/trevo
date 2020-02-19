using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class OcorrenciaFuncionarioMap : ClassMap<OcorrenciaFuncionario>
    {
        public OcorrenciaFuncionarioMap()
        {
            Table("OcorrenciaFuncionario");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable().Not.Update();

            References(x => x.Funcionario).Column("Funcionario").Unique();
            References(x => x.UsuarioResponsavel).Column("UsuarioResponsavel").Not.Nullable().Cascade.None();
            HasMany(x => x.OcorrenciaFuncionarioDetalhes).Cascade.AllDeleteOrphan();
        }
    }
}