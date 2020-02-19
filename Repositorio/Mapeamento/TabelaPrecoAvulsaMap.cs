using Entidade;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Mapeamento
{
    public class TabelaPrecoAvulsaMap : ClassMap<TabelaPrecoAvulsa>
    {
        public TabelaPrecoAvulsaMap()
        {
            Table("TabelaPrecoAvulsa");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.Nome).Column("Nome");
            Map(x => x.ToleranciaDesistencia).Column("ToleranciaDesistencia");
            Map(x => x.ToleranciaPagamento).Column("ToleranciaPagamento");
            Map(x => x.ValorDiaria).Column("ValorDiaria");
            Map(x => x.InicioDiaria).Column("InicioDiaria");
            Map(x => x.FimDiaria).Column("FimDiaria");
        }
    }
}
