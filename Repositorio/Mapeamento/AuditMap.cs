using Entidade.Base;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class AuditMap : ClassMap<Audit>
    {
        public AuditMap()
        {
            Table("Audit");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Data).Column("Data").Not.Nullable();
            Map(x => x.Entidade).Column("Entidade").Not.Nullable();
            Map(x => x.Atributo).Column("Atributo").Not.Nullable();
            Map(x => x.CodigoEntidade).Column("CodigoEntidade").Not.Nullable();
            Map(x => x.ValorAntigo).Column("ValorAntigo").Not.Nullable();
            Map(x => x.ValorNovo).Column("ValorNovo").Not.Nullable();
            Map(x => x.Usuario).Column("UsuarioId").Not.Nullable();
            Map(x => x.UsuarioNome).Column("UsuarioNome").Not.Nullable();
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
        }
    }
}
