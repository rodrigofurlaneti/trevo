using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class BancoMap : ClassMap<Banco>
    {
        public BancoMap()
        {
            Table("Banco");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");

            Map(x => x.Descricao).Column("Descricao");
            Map(x => x.CodigoBanco).Column("CodigoBanco");
            Map(x => x.DataInsercao).Column("DataInsercao");
            
        }
    }
}