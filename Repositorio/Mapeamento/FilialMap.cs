using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class FilialMap : SubclassMap<Filial>
    {
        public FilialMap()
        {
            Table("Filial");
            LazyLoad();

            References(x => x.TipoFilial).Column("TipoFilial");

            References(x => x.Empresa).Column("Empresa");

            HasMany(x => x.Contatos)
                .Table("FilialContato")
                .KeyColumn("Filial")
                .Component(m =>
                {
                    m.References(x => x.Contato).Cascade.All();
                }).Cascade.All();
        }
    }
}
