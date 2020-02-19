using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class PagamentoReembolsoMap : ClassMap<PagamentoReembolso>
    {
        public PagamentoReembolsoMap()
        {
            Table("PagamentoReembolso");

            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.Status).Column("Status").Not.Nullable();
            Map(x => x.NumeroRecibo).Column("NumeroRecibo").Nullable().Length(40);

            References(x => x.ContaAPagar).Column("ContaAPagar");
        }
    }
}