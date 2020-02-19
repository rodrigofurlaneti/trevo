using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Repositorio.Base
{
    public class CascadeConvention : IReferenceConvention, IHasManyConvention, IHasManyToManyConvention
    {
        public void Apply(IOneToOneInstance instance)
        {

        }

        public void Apply(IManyToOneInstance instance)
        {

        }

        public void Apply(IOneToManyCollectionInstance instance)
        {
            instance.Cascade.All();
        }

        public void Apply(IManyToManyCollectionInstance instance)
        {

        }
    }
}