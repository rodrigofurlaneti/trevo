using Entidade;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Mapeamento
{
    public class MovimentacaoSeloMap : ClassMap<MovimentacaoSelo>
    {
        public MovimentacaoSeloMap()
        {
            Table("MovimentacaoSelo");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao");

            Map(x => x.IdSoftpark).Column("IdSoftpark");
            Map(x => x.MovimentacaoIdSoftpark).Column("MovimentacaoIdSoftpark");
            References(x => x.Selo, "Selo_Id");
            References(x => x.Movimentacao, "Movimentacao_Id");
        }
    }
}
