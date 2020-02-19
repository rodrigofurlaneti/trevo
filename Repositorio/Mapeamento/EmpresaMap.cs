using Entidade;
using FluentNHibernate.Mapping;


namespace Repositorio.Mapeamento
{
    public class EmpresaMap : SubclassMap<Empresa>
    {
        public EmpresaMap()
        {
            Table("Empresa");
            LazyLoad();

            References(x => x.Grupo).Not.LazyLoad();

			HasMany(x => x.Contatos)
				.Table("EmpresaContato")
				.KeyColumn("Empresa")
				.Component(m =>
				{
					m.References(x => x.Contato).Cascade.All();
				}).Cascade.All();
		}
	}
}
