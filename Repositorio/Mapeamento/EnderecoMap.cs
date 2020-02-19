using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class EnderecoMap : ClassMap<Endereco>
    {
        public EnderecoMap()
        {
            Table("Endereco");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Cep).Column("Cep").Nullable();
            Map(x => x.Tipo).Column("Tipo").Nullable();
            Map(x => x.Logradouro).Column("Logradouro").Nullable();
            Map(x => x.Numero).Column("Numero").Nullable();
            Map(x => x.Complemento).Column("Complemento").Nullable();
            Map(x => x.Bairro).Column("Bairro").Nullable();
            Map(x => x.Descricao).Column("Descricao").Nullable();
            Map(x => x.Latitude).Column("Latitude").Nullable();
            Map(x => x.Longitude).Column("Longitude").Nullable();
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            
            References(x => x.Cidade).Nullable().Not.LazyLoad().Cascade.None();
        }
    }
}