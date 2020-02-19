using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class EquipamentoUnidadeMap : ClassMap<EquipamentoUnidade>
    {
        public EquipamentoUnidadeMap()
        {
            Table("EquipamentoUnidade");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.Codigo).Column("Codigo").Nullable();
            Map(x => x.GerarNotificacao).Column("GerarNotificacao").Not.Nullable();
            Map(x => x.Observacao).Column("Observacao").Nullable();
            Map(x => x.ConferenciaConcluida).Column("ConferenciaConcluida").Not.Nullable();
            References(x => x.Unidade).Column("Unidade").Cascade.None();

            Map(x => x.PeriodoEquipamentoUnidade).Column("PeriodoEquipamentoUnidade").Nullable();
            Map(x => x.UltimaConferencia).Column("UltimaConferencia").Nullable();
            Map(x => x.Usuario).Column("Usuario");
            //HasMany(equipamentoUnidade => equipamentoUnidade.EquipamentosUnidadeEquipamento)
            //.Table("EquipamentoUnidadeEquipamento")
            //.KeyColumn("EquipamentosUnidade")
            //.Component(m =>
            //{
            //    m.Map(equipamentoUnidadeEquipamento => equipamentoUnidadeEquipamento.Ativo).Column("Ativo").Not.Nullable();
            //    m.Map(equipamentoUnidadeEquipamento => equipamentoUnidadeEquipamento.Quantidade).Column("Quantidade").Not.Nullable();
            //    m.References(equipamentoUnidadeEquipamento => equipamentoUnidadeEquipamento.Equipamento).Cascade.All();
            //}).Cascade.All();

        }
    }
}
