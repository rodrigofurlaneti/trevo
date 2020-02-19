using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class ParametroBoletoBancarioDescritivoMap : ClassMap<ParametroBoletoBancarioDescritivo>
    {
        public ParametroBoletoBancarioDescritivoMap()
        {
            Table("ParametroBoletoBancarioDescritivo");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Update();

            Map(x => x.Descritivo);
        }
    }
}