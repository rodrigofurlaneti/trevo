using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class ParametroNegociacaoMap : ClassMap<ParametroNegociacao>
    {
        public ParametroNegociacaoMap()
        {
            Table("ParametroNegociacao");
            LazyLoad();
            Id(parametroNegociacao => parametroNegociacao.Id).GeneratedBy.Identity().Column("Id");
            Map(parametroNegociacao => parametroNegociacao.DataInsercao).Column("DataInsercao").Not.Nullable();
            References(parametroNegociacao => parametroNegociacao.Unidade).Column("Unidade").Not.Nullable().Cascade.None();
            References(parametroNegociacao => parametroNegociacao.Perfil).Column("Perfil").Not.Nullable().Cascade.None();
            //References(parametroNegociacao => parametroNegociacao.Usuario).Column("Usuario").Not.Nullable().Cascade.None();
            HasMany(parametroNegociacao => parametroNegociacao.LimitesDesconto)
             .Table("LimiteDesconto")
             .KeyColumn("ParametroNegociacao")
             .Component(m =>
             {
                 m.Map(limiteDesconto => limiteDesconto.DataInsercao).Column("DataInsercao").Not.Nullable();
                 m.Map(limiteDesconto => limiteDesconto.TipoServico).Column("TipoServico").Not.Nullable();
                 m.Map(limiteDesconto => limiteDesconto.TipoValor).Column("TipoValor").Not.Nullable();
                 m.Map(limiteDesconto => limiteDesconto.Valor).Column("Valor").Length(10).Precision(10).Scale(2).Not.Nullable();                                      
             }).Cascade.All();
        }
    }
}
