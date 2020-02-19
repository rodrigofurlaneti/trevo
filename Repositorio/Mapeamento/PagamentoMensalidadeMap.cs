using Entidade;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Mapeamento
{
    public class PagamentoMensalidadeMap : ClassMap<PagamentoMensalidade>
    {
        public PagamentoMensalidadeMap()
        {
            Table("PagamentoMensalidade");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao");
            Map(x => x.NumFechamento).Column("NumFechamento");
            Map(x => x.NumTerminal).Column("NumTerminal");
            Map(x => x.DataAbertura).Column("DataAbertura");
            Map(x => x.DataFechamento).Column("DataFechamento");
            Map(x => x.DataRecebimento).Column("DataRecebimento");
            Map(x => x.NumContratoMensalista).Column("NumContratoMensalista");
            Map(x => x.ValorRecebido).Column("ValorRecebido");
            Map(x => x.NumCobranca).Column("NumCobranca");
            References(x => x.Unidade).Column("Unidade");
            References(x => x.LancamentoCobranca).Column("LancamentoCobranca");
        }
    }
}
