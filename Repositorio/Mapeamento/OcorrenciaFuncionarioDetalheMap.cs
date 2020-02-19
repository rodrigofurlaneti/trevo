using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class OcorrenciaFuncionarioDetalheMap : ClassMap<OcorrenciaFuncionarioDetalhe>
    {
        public OcorrenciaFuncionarioDetalheMap()
        {
            Table("OcorrenciaFuncionarioDetalhe");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable().Not.Update();
            Map(x => x.DataOcorrencia).Column("DataOcorrencia").Not.Nullable();
            Map(x => x.Justificativa).Column("Justificativa").Not.Nullable();
            References(x => x.TipoOcorrencia).Column("TipoOcorrencia").Not.Nullable().Cascade.None();
            References(x => x.Unidade).Column("Unidade").Not.Nullable().Cascade.None();
            References(x => x.UsuarioResponsavel).Column("UsuarioResponsavel").Not.Nullable().Cascade.None();
        }
    }
}