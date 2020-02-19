using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class UnidadeCondominoMap : ClassMap<UnidadeCondomino>
    {
        public UnidadeCondominoMap()
        {
            Table("UnidadeCondomino");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");

            References(x => x.Unidade).Column("Unidade").Nullable();

            Map(x => x.NumeroVagas).Column("NumeroVagas").Default("0");
            Map(x => x.NumeroVagasRestantes).Column("NumeroVagasRestantes").Default("0");
            Map(x => x.DataInsercao).Column("DataInsercao");

            HasMany(x => x.ClienteCondomino).Cascade.All();
        }
    }
}