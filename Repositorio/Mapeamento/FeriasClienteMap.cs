using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class FeriasClienteMap : ClassMap<FeriasCliente>
    {
        public FeriasClienteMap()
        {
            Table("FeriasCliente");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");

            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.DataInicio).Column("DataInicio").Not.Nullable();
            Map(x => x.DataFim).Column("DataFim").Not.Nullable();
            Map(x => x.InutilizarTodasVagas).Column("InutilizarTodasVagas").Not.Nullable();
            Map(x => x.TotalVagas).Column("TotalVagas").Not.Nullable();
            Map(x => x.ValorFeriasCalculada).Column("ValorFeriasCalculada").Not.Nullable();

            References(x => x.Cliente).Column("Cliente").Cascade.None();
            References(x => x.ContratoMensalista).Column("ContratoMensalista").Cascade.None();
            References(x => x.UsuarioCadastro).Column("UsuarioCadastro").Cascade.None();

            HasMany(x => x.ListaFeriasClienteDetalhe)
                .Table("FeriasClienteDetalhe")
                .KeyColumn("FeriasCliente")
                .Component(m =>
                {
                    m.Map(x => x.DataInicio).Column("DataInicio").Not.Nullable();
                    m.Map(x => x.DataFim).Column("DataFim").Not.Nullable();
                    m.Map(x => x.ValorFeriasCalculada).Column("ValorFeriasCalculada").Not.Nullable();

                    m.References(x => x.FeriasCliente).Column("FeriasCliente_Id").Cascade.None();
                }).Cascade.All();
        }
    }
}