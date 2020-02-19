//using Entidade;
//using FluentNHibernate.Mapping;

//namespace Repositorio.Mapeamento
//{
//    public class ArquivoBaseMap : ClassMap<ArquivoBase>
//    {
//        public ArquivoBaseMap()
//        {
//            Table("ArquivoBase");
//            LazyLoad();

//            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
//            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable().Not.Update();
//        }
//    }
//}