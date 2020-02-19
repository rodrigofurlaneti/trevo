using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Transform;

namespace Repositorio.Base
{
    public static class NHibTransformers
    {
        public static readonly IResultTransformer ExpandoObject;

        static NHibTransformers()
        {
            ExpandoObject = new ExpandoObjectResultSetTransformer();
        }

        private class ExpandoObjectResultSetTransformer : IResultTransformer
        {
            public IList TransformList(IList collection)
            {
                return collection;
            }

            public object TransformTuple(object[] tuple, string[] aliases)
            {
                var expando = new ExpandoObject();
                var dictionary = (IDictionary<string, object>)expando;
                for (var i = 0; i < tuple.Length; i++)
                {
                    var alias = aliases[i];
                    if (alias != null)
                    {
                        dictionary[alias] = tuple[i];
                    }
                }
                return expando;
            }
        }
    }
}
