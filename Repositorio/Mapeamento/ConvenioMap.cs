using Entidade;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Mapeamento
{
    public class ConvenioMap : ClassMap<Convenio>
    {
        public ConvenioMap()
        {
            Table("Convenios");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.Descricao).Column("Descricao").Not.Nullable();
            Map(x => x.Status).Column("Status").Not.Nullable();

            HasMany(x => x.ConvenioUnidades)
                .Table("ConvenioUnidades")
                .KeyColumn("Convenio")
                .Component(m =>
                {
                    m.References(x => x.ConvenioUnidade).Cascade.All();
                }).Cascade.All();

            HasManyToMany(x => x.Clientes)
                .Table("ConvenioCliente")
                .ParentKeyColumn("Convenios_id")
                .ChildKeyColumn("Cliente_id")
                .Cascade.SaveUpdate();
        }
    }
}
