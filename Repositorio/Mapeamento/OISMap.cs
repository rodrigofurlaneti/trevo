using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class OISMap : ClassMap<OIS>
    {
        public OISMap()
        {
            Table("OIS");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.DataAtualizacao).Column("DataAtualizacao").Not.Nullable();
            Map(x => x.NomeCliente);
            Map(x => x.TipoVeiculo);
            Map(x => x.Placa);
            Map(x => x.Cor);
            Map(x => x.Ano);
            Map(x => x.StatusSinistro);
            Map(x => x.Observacao);

            References(x => x.Marca, "Marca_Id");
            References(x => x.Modelo, "Modelo_Id");
            References(x => x.Unidade, "Unidade_Id");
            References(x => x.Usuario, "Usuario_Id");

            HasMany(x => x.OISCategorias)
                .Table("OISCategoria")
                    .KeyColumn("OIS_Id")
                    .Component(m => {
                        m.Map(r => r.TipoCategoria, "TipoCategoria");
                        m.Map(r => r.OutraCategoria, "OutraCategoria").Length(50);
                    }).Cascade.All();

            HasMany(x => x.OISFuncionarios)
                .Table("OISFuncionario")
                .KeyColumn("OIS_Id")
                .Component(m => {
                    m.References(r => r.Funcionario, "Funcionario_Id").Cascade.None();
                }).Cascade.All();

            HasMany(x => x.OISContatos)
                .Table("OISContato")
                .KeyColumn("OIS_Id")
                .Component(m => {
                    m.References(r => r.Contato, "Contato_Id").Cascade.SaveUpdate();
                }).Cascade.All();

            HasMany(x => x.OISImagens)
                .Table("OISImagem")
                .KeyColumn("OIS_Id")
                .Component(m => {
                    m.Map(r => r.ImagemUpload, "Imagem").CustomSqlType("varbinary(max)").Length(int.MaxValue);
                }).Cascade.All();

            HasMany(x => x.OISNotificacoes)
                .Table("OISNotificacao")
                .KeyColumn("OIS_Id")
                .Component(m => {
                    m.References(r => r.Notificacao, "Notificacao_Id").Cascade.All();
                }).Cascade.All();
        }
    }
}