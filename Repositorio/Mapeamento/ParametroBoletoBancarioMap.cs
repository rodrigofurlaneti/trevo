using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class ParametroBoletoBancarioMap : ClassMap<ParametroBoletoBancario>
    {
        public ParametroBoletoBancarioMap()
        {
            Table("ParametroBoletoBancario");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Update();

            Map(x => x.TipoServico);
            Map(x => x.DiasAntesVencimento);
            Map(x => x.ValorDesconto).Length(10).Precision(10).Scale(2).Not.Nullable();

            References(x => x.Unidade).Column("Unidade").Cascade.None();
            HasMany(x => x.ParametroBoletoBancarioDescritivos).Cascade.All();

        }
    }
}