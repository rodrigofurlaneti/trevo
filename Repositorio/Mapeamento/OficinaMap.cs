using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class OficinaMap : ClassMap<Oficina>
    {
        public OficinaMap()
        {
            Table("Oficina");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();

            Map(x => x.NomeFantasia).Column("NomeFantasia");
            Map(x => x.RazaoSocial).Column("RazaoSocial");
            Map(x => x.TipoPessoa).Column("TipoPessoa").Not.Nullable();
            Map(x => x.IndicadaPeloCliente).Column("IndicadaPeloCliente");
            Map(x => x.NomeCliente).Column("NomeCliente");

            References(x => x.Pessoa).Column("Pessoa_Id").Cascade.SaveUpdate();
            References(x => x.CelularCliente).Column("Contato_Id").Cascade.SaveUpdate();
        }
    }
}