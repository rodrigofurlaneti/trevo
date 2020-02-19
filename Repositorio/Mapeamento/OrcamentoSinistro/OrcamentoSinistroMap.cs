using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class OrcamentoSinistroMap : ClassMap<OrcamentoSinistro>
    {
        public OrcamentoSinistroMap()
        {
            Table("OrcamentoSinistro");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable().Not.Update();
            Map(x => x.Status).Column("Status").Not.Nullable();

            References(x => x.OIS, "OIS_Id").Cascade.None();
            References(x => x.OrcamentoSinistroCotacao, "OrcamentoSinistroCotacao_Id").Cascade.All();

            HasMany(x => x.OrcamentoSinistroOficinas)
                .Table("OrcamentoSinistroOficina")
                .KeyColumn("OrcamentoSinistro_Id")
                .Component(c =>
                {
                    c.References(r => r.Oficina, "Oficina_Id").Cascade.None();
                }).Cascade.All();

            HasMany(x => x.OrcamentoSinistroOficinaClientes)
                .Table("OrcamentoSinistroOficinaCliente")
                .KeyColumn("OrcamentoSinistro_Id")
                .Component(c =>
                {
                    c.References(r => r.Oficina, "Oficina_Id").Cascade.None();
                }).Cascade.All();

            HasMany(x => x.OrcamentoSinistroPecaServicos)
                .Table("OrcamentoSinistroPecaServico")
                .KeyColumn("OrcamentoSinistro_Id")
                .Component(c =>
                {
                    c.References(r => r.PecaServico, "PecaServico_Id").Cascade.SaveUpdate();
                }).Cascade.All();

            HasMany(x => x.OrcamentoSinistroFornecedores)
                .Table("OrcamentoSinistroFornecedor")
                .KeyColumn("OrcamentoSinistro_Id")
                .Component(c =>
                {
                    c.References(r => r.Fornecedor, "Fornecedor_Id").Cascade.None();
                }).Cascade.All();

            HasMany(x => x.OrcamentoSinistroNotificacoes)
                .Table("OrcamentoSinistroNotificacao")
                .KeyColumn("OrcamentoSinistro_Id")
                .Component(c =>
                {
                    c.References(r => r.Notificacao, "Notificacao_Id").Cascade.All();
                }).Cascade.All();
        }
    }
}