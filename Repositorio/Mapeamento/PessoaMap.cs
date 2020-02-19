using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class PessoaMap : ClassMap<Pessoa>
    {
        public PessoaMap()
        {
            Table("Pessoa");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.Nome).Column("Nome");
            Map(x => x.Sexo).Column("Sexo").Nullable();
            Map(x => x.DataNascimento).Column("DataNascimento");
            Map(x => x.Ativo).Column("Ativo").Default("1");

            References(x => x.Trabalho).Cascade.None();

            HasMany(x => x.Enderecos)
                .Table("PessoaEndereco")
                .KeyColumn("Pessoa")
                .Component(m =>
                {
                    m.References(x => x.Endereco).Cascade.All();
                }).Cascade.All();

            HasMany(x => x.Documentos)
                .Table("PessoaDocumento")
                .KeyColumn("Pessoa")
                .Component(m =>
                {
                    m.References(x => x.Documento).Cascade.All();
                }).Cascade.All();

            HasMany(x => x.Contatos)
                .Table("PessoaContato")
                .KeyColumn("Pessoa")
                .Component(m =>
                {
                    m.References(x => x.Contato).Cascade.All();
                }).Cascade.All();
                
            HasMany(x => x.Lojas)
                .Table("PessoaLoja")
                .KeyColumn("Pessoa")
                .Component(m =>
                {
                    m.References(x => x.Loja).Column("Loja_id").Cascade.None();
                }).Cascade.None();
        }
    }
}