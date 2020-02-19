using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    class FuncionamentoMap : ClassMap<Funcionamento>
    {
        public FuncionamentoMap()
        {
            Table("Funcionamentos");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.CodFuncionamento).Column("CodFuncionamento").Not.Nullable();
            Map(x => x.Nome).Column("Nome").Not.Nullable().Length(100);
            Map(x => x.DataInicio).Column("DataInicio");
            Map(x => x.DataFim).Column("DataFim");
            HasMany(x => x.HorariosPrecos).Cascade.All();
        }
    }
}
