using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class UsuarioMap : ClassMap<Usuario>
    {
        public UsuarioMap()
        {
            Table("Usuario");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Login).Column("Login");
            Map(x => x.Senha).Column("Senha");
            Map(x => x.ImagemUpload).Column("ImagemUpload").CustomSqlType("varbinary(max)").Length(int.MaxValue);
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.PrimeiroLogin).Column("PrimeiroLogin").Not.Nullable().Default("1");
            Map(x => x.Ativo).Column("Ativo").Not.Nullable().Default("1");
            Map(x => x.OperadorPerfil).Nullable();
            Map(x => x.TemAcessoAoPDV).Nullable();
            Map(x => x.NomeCompleto, "NomeCompleto").Nullable();
            Map(x => x.EhFuncionario, "EhFuncionario").Default("1").Not.Nullable();
            Map(x => x.Email, "Email").Nullable();

            References(x => x.Unidade).Column("Unidade_Id").Nullable();
            References(x => x.Funcionario).Column("Funcionario").Nullable();

            HasMany(x => x.Perfils)
                .Table("UsuarioPerfil")
                .KeyColumn("Usuario")
                .Component(m =>
                {
                    m.References(x => x.Perfil).Column("Perfil_id").Cascade.None();
                });
        }
    }
}
