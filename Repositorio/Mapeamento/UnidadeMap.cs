using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class UnidadeMap : ClassMap<Unidade>
    {
        public UnidadeMap()
        {
            Table("Unidade");
            LazyLoad();
            Id(unidade => unidade.Id).GeneratedBy.Identity().Column("Id");
            Map(unidade => unidade.DataInsercao).Column("DataInsercao");
            Map(unidade => unidade.Codigo).Column("Codigo");
            Map(unidade => unidade.Nome).Column("Nome");
            Map(unidade => unidade.NumeroVaga).Column("NumeroVaga");
            Map(unidade => unidade.DiaVencimento).Column("DiaVencimento").Default("1").Nullable();
            Map(unidade => unidade.TiposUnidades).Column("TiposUnidades");
            Map(unidade => unidade.Ativa).Column("Ativa");
            Map(x => x.CNPJ).Column("CPFCNPJ");
            Map(x => x.CCM).Column("CCM");
            Map(x => x.HorarioInicial).Column("HorarioInicial");
            Map(x => x.HorarioFinal).Column("HorarioFinal");

            References(unidade => unidade.Endereco).Column("Endereco").Cascade.All();
            References(unidade => unidade.MaquinaCartao).Column("MaquinaCartao").Cascade.All();
            References(unidade => unidade.Responsavel).Column("Funcionario");
            References(unidade => unidade.Empresa).Column("Empresa");
            References(unidade => unidade.CheckListAtividade).Column("CheckListAtividade").Cascade.None();

            HasMany(unidade => unidade.TiposPagamento).Cascade.All();

            HasMany(x => x.UnidadeCheckListTipoAtividades)
                .Table("UnidadeCheckListTipoAtividades")
                .KeyColumn("Unidade_Id")
                .Component(m =>
                {
                    m.Map(r => r.Selecionado, "Selecionado");
                    m.References(r => r.TipoAtividade, "TipoAtividade_Id");
                }).Cascade.All();

            HasMany(unidade => unidade.UnidadeCheckListAtividades)
            .Table("UnidadeCheckListAtividades")
            .KeyColumn("Unidade")
            .Component(m =>
            {
                m.Map(unidadeCheckListAtividades => unidadeCheckListAtividades.StatusCheckList).Column("StatusCheckList").Not.Nullable();
                m.References(unidadeCheckListAtividades => unidadeCheckListAtividades.CheckListAtividade).Cascade.All();
            }).Cascade.All();

            HasMany(unidade => unidade.UnidadeFuncionarios)
           .Table("UnidadeFuncionario")
           .KeyColumn("Unidade")
           .Component(m =>
           {
               m.Map(unidadeFuncionario => unidadeFuncionario.DataInsercao).Column("DataInsercao").Not.Nullable();
               m.Map(unidadeFuncionario => unidadeFuncionario.Funcao).Column("Funcao").Not.Nullable();
               m.References(unidadeFuncionario => unidadeFuncionario.MaquinaCartao).Column("MaquinaCartao").Cascade.All();
               m.References(unidadeFuncionario => unidadeFuncionario.Funcionario).Column("Funcionario").Cascade.All();
           }).Cascade.All();

            HasMany(x => x.CheckListEstruturaUnidade).Cascade.All();


        }
    }
}