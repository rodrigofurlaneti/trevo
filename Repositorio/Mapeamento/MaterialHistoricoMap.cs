using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class MaterialHistoricoMap : ClassMap<MaterialHistorico>
    {
        public MaterialHistoricoMap()
        {
            Table("MaterialHistorico");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.Quantidade);
            Map(x => x.EhUmAtivo).Not.Nullable();
            Map(x => x.AcaoEstoqueManual);
            Map(x => x.NumeroNota);

            References(x => x.Material).Column("Material_Id");
            References(x => x.Estoque).Column("Estoque_Id");
            References(x => x.Unidade).Column("Unidade_Id");
            References(x => x.Usuario).Column("Usuario_Id");
        }
    }
}