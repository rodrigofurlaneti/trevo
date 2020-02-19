using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class MaterialMap : ClassMap<Material>
    {
        public MaterialMap()
        {
            Table("Material");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.Nome).Not.Nullable();
            Map(x => x.Descricao);
            Map(x => x.Altura);
            Map(x => x.Imagem).CustomSqlType("varbinary(max)").Length(int.MaxValue);
            Map(x => x.Largura);
            Map(x => x.Profundidade);
            Map(x => x.Comprimento);
            Map(x => x.EAN);
            Map(x => x.EhUmAtivo).Not.Nullable();
            Map(x => x.EstoqueMaximo).Not.Nullable();
            Map(x => x.EstoqueMinimo).Not.Nullable();
            Map(x => x.QuantidadeTotalEstoque);

            References(x => x.TipoMaterial).Column("TipoMaterial_Id").Not.Nullable();

            HasMany(x => x.MaterialFornecedores).Cascade.All();
            HasMany(x => x.MaterialNotificacaos).Cascade.AllDeleteOrphan();
        }
    }
}