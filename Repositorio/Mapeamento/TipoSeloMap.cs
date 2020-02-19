using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class TipoSeloMap : ClassMap<TipoSelo>
    {
        public TipoSeloMap()
        {
            Table("TipoSelo");
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao");
            Map(x => x.Nome).Column("Nome");
            Map(x => x.Valor).Column("Valor").Length(10).Precision(10).Scale(2);
            Map(x => x.ComValidade).Column("ComValidade");
            Map(x => x.PagarHorasAdicionais).Column("PagarHorasAdicionais");
            Map(x => x.Ativo).Column("Ativo");
            Map(x => x.ParametroSelo).Column("ParametroSelo");
        }
    }
}
