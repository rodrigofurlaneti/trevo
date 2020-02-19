using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class PessoaJuridicaMap : ClassMap<PessoaJuridica>
    {
        public PessoaJuridicaMap()
        {
            Table("PessoaJuridica");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Descricao).Column("Nome").Not.Nullable();
            Map(x => x.CNPJ).Column("CNPJ").Not.Nullable();
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.InscricaoEstadual).Column("InscricaoEstadual");
            Map(x => x.InscricaoMunicipal).Column("InscricaoMunicipal");
            Map(x => x.RazaoSocial).Column("RazaoSocial");

            References(x => x.Endereco).Cascade.All();
        }
    }
}
