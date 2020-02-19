using Entidade;
using FluentNHibernate.Mapping;


namespace Repositorio.Mapeamento
{
    public class ColaboradorMap : ClassMap<Colaborador>
    {
        public ColaboradorMap()
        {
            Table("Colaboradores");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao");
            References(x => x.NomeColaborador).Column("Colaborador");
            References(x => x.Turno).Column("Turno");
        }
    }
}
