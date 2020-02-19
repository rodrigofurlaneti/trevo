using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class DocumentoMap : ClassMap<Documento>
    {
        public DocumentoMap()
        {
            Table("Documento");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Tipo).Column("Tipo").Not.Nullable();
            Map(x => x.Numero).Column("Numero").Nullable().Length(20);
            Map(x => x.OrgaoExpedidor).Column("OrgaoExpedidor").Nullable().Length(10);
            Map(x => x.DataExpedicao).Column("DataExpedicao").Nullable();
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            //
            //HasManyToMany(x => x.Pessoas).Table("DocumentoPessoa");
            //References(x => x.Pessoa).Column("Pessoa");//.Not.Nullable();
        }
    }
}