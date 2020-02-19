using Entidade;
using Entidade.Uteis;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class OcorrenciaClienteMap : ClassMap<OcorrenciaCliente>
    {
        public OcorrenciaClienteMap()
        {
            Table("OcorrenciaCliente");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");

            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.DataOcorrencia).Column("DataOcorrencia").Not.Nullable();
            Map(x => x.NumeroProtocolo).Column("NumeroProtocolo").Not.Nullable();
            Map(x => x.DataCompetencia).Column("DataCompetencia").Not.Nullable();
            Map(x => x.Natureza).Column("Natureza").CustomType(typeof(TipoNatureza)).Not.Nullable();
            Map(x => x.Origem).Column("Origem").CustomType(typeof(TipoOrigem)).Not.Nullable();
            Map(x => x.Prioridade).Column("Prioridade").CustomType(typeof(TipoPrioridade)).Not.Nullable();
            Map(x => x.StatusOcorrencia).Column("StatusOcorrencia").CustomType(typeof(StatusOcorrencia)).Not.Nullable();
            Map(x => x.Descricao).Column("Descricao").CustomSqlType("varchar(max)").Not.Nullable();
            Map(x => x.Solucao).Column("Solucao").CustomSqlType("varchar(max)").Nullable();

            References(x => x.Unidade).Column("Unidade").Cascade.None().Nullable();
            References(x => x.Veiculo).Column("Veiculo").Cascade.None().Nullable();
            References(x => x.FuncionarioAtribuido).Column("FuncionarioAtribuido").Cascade.None().Nullable();
            References(x => x.Cliente).Column("Cliente_Id").Cascade.None().Not.Nullable();


            

        }
    }
}
