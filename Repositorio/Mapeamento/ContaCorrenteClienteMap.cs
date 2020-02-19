using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class ContaCorrenteClienteMap : ClassMap<ContaCorrenteCliente>
    {
        public ContaCorrenteClienteMap()
        {
            Table("ContaCorrenteCliente");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Update();

            References(x => x.Cliente).Column("Cliente").Unique().Cascade.None();

            HasMany(x => x.ContaCorrenteClienteDetalhes)
                .Table("ContaCorrenteClienteDetalhe")
                .KeyColumn("ContaCorrenteCliente_id")
                .Component(c =>
                {
                    c.Map(x => x.DataInsercao).Column("DataInsercao").Not.Update();
                    c.Map(x => x.DataCompetencia).Column("DataCompetencia").Not.Nullable();
                    c.Map(x => x.Valor).Column("Valor").Not.Nullable().Scale(2);
                    c.Map(x => x.TipoOperacaoContaCorrente).Column("TipoOperacaoContaCorrente").Not.Nullable();
                    c.References(x => x.ContratoMensalista).Column("ContratoMensalistaId").Cascade.None().Nullable();
                }).Cascade.All();
        }
    }
}