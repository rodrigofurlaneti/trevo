using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class ClienteCondominoMap : ClassMap<ClienteCondomino>
    {
        public ClienteCondominoMap()
        {
            Table("ClienteCondomino");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");

            References(x => x.Cliente).Column("Cliente").Nullable();
            References(x => x.Unidade).Column("Unidade").Nullable();

            Map(x => x.NumeroVagas).Column("NumeroVagas").Default("0");
            Map(x => x.DataInsercao).Column("DataInsercao");
            Map(x => x.Frota).Column("Frota");

            HasMany(x => x.CondominoVeiculos)
              .Table("ClienteCondominoVeiculo")
              .KeyColumn("ClienteCondomino")
              .Component(m =>
              {
                  m.References(x => x.Veiculo).Column("Veiculo").Cascade.None();
              }).Cascade.All();
        }
    }
}