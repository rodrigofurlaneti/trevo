using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class EstoqueMap : ClassMap<Estoque>
    {
        public EstoqueMap()
        {
            Table("Estoque");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");

            Map(x => x.DataInsercao).Column("DataInsercao");
            Map(x => x.Nome).Column("Nome").Nullable();
            Map(x => x.Cep).Column("Cep").Nullable();
            Map(x => x.Logradouro).Column("Logradouro").Nullable();
            Map(x => x.Numero).Column("Numero").Nullable();
            Map(x => x.Complemento).Column("Complemento").Nullable();
            Map(x => x.Bairro).Column("Bairro").Nullable();
            Map(x => x.Tipo).Column("Tipo").Nullable();
            Map(x => x.CidadeNome).Column("CidadeNome").Nullable();
            Map(x => x.UF).Column("UF").Nullable();
            Map(x => x.EstoquePrincipal).Column("EstoquePrincipal");

            References(x => x.Unidade).Column("Unidade").Cascade.None();

        }
    }
}