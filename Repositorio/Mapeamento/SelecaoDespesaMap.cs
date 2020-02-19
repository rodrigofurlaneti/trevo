using Entidade;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositorio.Mapeamento
{
    public class SelecaoDespesaMap : ClassMap<SelecaoDespesa>
    {
        public SelecaoDespesaMap()
        {
            Table("SelecaoDespesa");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao");
            Map(x => x.MesVigente).Column("MesVigente");
            Map(x => x.Ano).Column("Ano");
            References(x => x.Unidade).Column("Unidade");
            References(x => x.Empresa).Column("Empresa");

            HasMany(x => x.Despesas)
                .Table("DespesaContasAPagar")
                .KeyColumn("SelecaoDespesa")
                .Component(m =>
                {
                    m.References(x => x.ContaAPagar).Column("ContaAPagar").Cascade.All();
                }).Cascade.All();
        }
    }
}
