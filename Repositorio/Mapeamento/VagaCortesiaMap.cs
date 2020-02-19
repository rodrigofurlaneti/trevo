using Entidade;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Mapeamento
{
    public class VagaCortesiaMap : ClassMap<VagaCortesia>
    {
        public VagaCortesiaMap()
        {
            Table("VagaCortesia");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();

            References(x => x.Cliente).Column("Cliente").Not.Nullable();

            HasMany(x => x.VagaCortesiaVigencia)
                .KeyColumn("VagaCortesia")
                .Cascade.All();
        }
    }
}
