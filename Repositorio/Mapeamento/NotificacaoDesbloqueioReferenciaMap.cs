using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class NotificacaoDesbloqueioReferenciaMap : ClassMap<NotificacaoDesbloqueioReferencia>
    {
        public NotificacaoDesbloqueioReferenciaMap()
        {
            Table("NotificacaoDesbloqueioReferencia");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");

            Map(x => x.IdRegistro).Column("IdRegistro").Not.Nullable();
            Map(x => x.EntidadeRegistro).Column("EntidadeRegistro").Not.Nullable();
            Map(x => x.DataReferencia).Column("DataReferencia").Not.Nullable();
            Map(x => x.LiberacaoUtilizada).Column("LiberacaoUtilizada").Not.Nullable();
            Map(x => x.StatusDesbloqueioLiberacao).Column("StatusDesbloqueioLiberacao").Not.Nullable();
            Map(x => x.NomeArquivoCNABAssociado).Column("NomeArquivoCNABAssociado").Nullable();

            References(x => x.Notificacao).Column("Notificacao").Cascade.None();
        }
    }
}