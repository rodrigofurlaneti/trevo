using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class MaquinaCartaoMap : ClassMap<MaquinaCartao>
    {
        public MaquinaCartaoMap()
        {
            Table("MaquinaCartao");
            LazyLoad();
            Id(maquinaCartao => maquinaCartao.Id).GeneratedBy.Identity().Column("Id");
            Map(maquinaCartao => maquinaCartao.DataInsercao).Column("DataInsercao");
            Map(maquinaCartao => maquinaCartao.NumeroMaquina).Column("NumeroMaquina");
            Map(maquinaCartao => maquinaCartao.MarcaMaquina).Column("MarcaMaquina");
            Map(maquinaCartao => maquinaCartao.Observacao).Column("Observacao").Nullable();
            References(unidade => unidade.Responsavel).Column("Funcionario");
            References(maquinaCartao => maquinaCartao.CNPJ).Column("CNPJ").Cascade.All();
        }                                                                                    
    }
}
