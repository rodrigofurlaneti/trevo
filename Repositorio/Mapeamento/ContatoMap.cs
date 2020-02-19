using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class ContatoMap : ClassMap<Contato>
    {
        public ContatoMap()
        {
            Table("Contato");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Tipo).Column("Tipo").Not.Nullable();
            Map(x => x.Email).Column("Email");
            Map(x => x.Numero).Column("Numero").Length(100);
            //Map(x => x.Celular).Column("Celular").Length(100);
            Map(x => x.NomeRecado).Column("NomeRecado");
            Map(x => x.Ordem).Column("Ordem");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            //
            //References(x => x.Pessoa).Column("Pessoa");
        }
    }
}