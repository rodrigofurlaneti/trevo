using Entidade;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Mapeamento
{
    public class ConsolidaFaturamentoMap : ClassMap<ConsolidaFaturamento>
    {
        public ConsolidaFaturamentoMap() 
        {
            Table("ConsolidaFaturamento");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao");
            Map(x => x.FaturamentoCartao).Column("FaturamentoCartao");
            Map(x => x.FaturamentoFinal).Column("FaturamentoFinal");
            Map(x => x.FaturamentoMes).Column("FaturamentoMes");
            Map(x => x.Diferenca).Column("Diferenca");
        }
    }
}
