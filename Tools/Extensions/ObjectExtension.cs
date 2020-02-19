using System;

namespace Core.Extensions
{
    public static class ObjectExtension
    {
        public static T ToCast<T>(this object value)
        {
            var cast = (T)value;
            if (cast == null)
                throw new Exception($"Não foi possível converter o tipo {value?.GetType().Name} para o tipo {typeof(T).Name}");
            return cast;
        }
    }
}
