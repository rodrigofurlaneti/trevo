using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class ClienteMap : ClassMap<Cliente>
    {
        public ClienteMap()
        {
            Table("Cliente");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.IdSoftpark).Column("IdSoftpark").Nullable();

            References(x => x.Pessoa).Column("Pessoa").Cascade.All();
            HasMany(x => x.Veiculos)
              .Table("ClienteVeiculo")
              .KeyColumn("Cliente_Id")
              .Component(m =>
              {
                  m.References(x => x.Veiculo, "Veiculo_Id").Cascade.All();
              }).Cascade.All();

            HasMany(x => x.Unidades)
              .Table("ClienteUnidades")
              .KeyColumn("Cliente")
              .Component(m =>
              {
                  m.References(x => x.Unidade).Cascade.None();
              }).Cascade.All();

            Map(x => x.NomeFantasia).Column("NomeFantasia");
            Map(x => x.RazaoSocial).Column("RazaoSocial");
            Map(x => x.TipoPessoa).Column("TipoPessoa").Not.Nullable();

            Map(x => x.ExigeNotaFiscal).Column("ExigeNotaFiscal").Default("0");
            Map(x => x.NotaFiscalSemDesconto).Column("NotaFiscalSemDesconto").Default("0");

            Map(x => x.NomeConvenio).Column("NomeConvenio").Nullable();
            Map(x => x.Observacao).Column("Observacao").Nullable().CustomSqlType("varchar(max)");

            //HasMany(x => x.SeloCliente)
            //  .Table("SeloCliente")
            //  .KeyColumn("Cliente_Id")
            //  .Component(m =>
            //  {
            //      m.References(x => x.SeloCliente).Cascade.None();
            //  }).Cascade.All();
        }
    }
}