using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class TrabalhoMap : ClassMap<Trabalho>
    {
        public TrabalhoMap()
        {
            Table("Trabalho");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao");
            Map(x => x.Empresa).Column("Empresa");
            Map(x => x.Profissao).Column("Profissao");
            Map(x => x.Cargo).Column("Cargo");
            
            HasMany(x => x.Contatos)
                .Table("TrabalhoContato")
                .KeyColumn("Trabalho")
                .Component(m =>
                {
                    m.References(x => x.Contato).Cascade.All();
                }).Cascade.AllDeleteOrphan();
                
            HasMany(x => x.Enderecos)
                .Table("TrabalhoEndereco")
                .KeyColumn("Trabalho")
                .Component(m =>
                {
                    m.References(x => x.Endereco).Cascade.All();
                }).Cascade.AllDeleteOrphan();
        }
    }
}
