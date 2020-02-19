using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class LogMap : ClassMap<Log>
    {
        public LogMap()
        {
            Table("Log");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Date).CustomSqlType("datetime").Not.Nullable();
            Map(x => x.Thread).CustomSqlType("varchar(255)").Length(255).Not.Nullable();
            Map(x => x.Level).CustomSqlType("varchar(50)").Length(50).Not.Nullable();
            Map(x => x.Logger).CustomSqlType("varchar(255)").Length(255).Not.Nullable();
            Map(x => x.Message).CustomSqlType("varchar(4000)").Length(4000).Not.Nullable();
            Map(x => x.Exception).CustomSqlType("varchar(2000)").Length(2000).Nullable();
        }
    }
}
