using Entidade;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Mapeamento
{
    public class RemanejamentoTransferenciaMap : ClassMap<RemanejamentoTransferencia>
    {
        public RemanejamentoTransferenciaMap()
        {
            Table("RemanejamentosTransferencia");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            References(x => x.Equipe).Column("Equipe").Cascade.All();
            References(x => x.Horario).Column("Horario").Cascade.None();
            References(x => x.TipoEquipe).Column("TipoEquipe");
            References(x => x.Unidade).Column("Unidade");
        }
    }
}
