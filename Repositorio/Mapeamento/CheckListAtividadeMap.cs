using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class CheckListAtividadeMap : ClassMap<CheckListAtividade>
    {
        public CheckListAtividadeMap()
        {
            Table("CheckListAtividade");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");

            Map(x => x.Descricao).Column("Descricao");
            Map(x => x.DataInsercao).Column("DataInsercao");
            Map(x => x.Usuario).Column("Usuario");
            Map(x => x.Ativo).Column("Ativo").Not.Nullable().Default("1");
            References(x => x.Responsavel).Column("Funcionario");
            HasMany(x => x.TiposAtividade)
              .Table("CheckListAtividadeTipoAtividade")
              .KeyColumn("CheckListAtividade_Id")
              .Component(m =>
              {
                  m.References(x => x.TipoAtividade, "TipoAtividade_Id").Cascade.None();
              }).Cascade.None();
        }
    }
}