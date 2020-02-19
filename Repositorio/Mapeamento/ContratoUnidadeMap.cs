using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class ContratoUnidadeMap : ClassMap<ContratoUnidade>
    {
        public ContratoUnidadeMap()
        {

            Table("ContratoUnidade");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");

            Map(x => x.TipoContrato).Column("TipoContrato");
            Map(x => x.NumeroContrato).Column("NumeroContrato");
            Map(x => x.DiaVencimento).Column("DiaVencimento").Not.Nullable();
            Map(x => x.InformarVencimentoDias).Column("InformarVencimentoDias");
            Map(x => x.Valor).Column("Valor").Length(10).Precision(10).Scale(2).Not.Nullable();
            Map(x => x.TipoValor).Column("TipoValor");
            Map(x => x.InicioContrato).Column("InicioContrato").Not.Nullable();
            Map(x => x.FinalContrato).Column("FinalContrato").Not.Nullable();
            Map(x => x.ExistiraReajuste).Column("ExistiraReajuste");
            Map(x => x.IndiceReajuste).Column("IndiceReajuste");

            Map(x => x.Ativo).Column("Ativo").Not.Nullable().Default("1");

            References(x => x.Unidade).Column("Unidade").Cascade.None();

            HasMany(x => x.ContratoUnidadeContasAPagar)
              .Table("ContratoUnidadeContasAPagar")
              .KeyColumn("Id")
              .Component(m =>
              {
                  m.References(x => x.ContaAPagar).Cascade.All();
                  m.References(x => x.ContratoUnidade).Cascade.All();
              }).Cascade.All();

        }
    }
}