using System.Collections.Generic;
using System.Linq;

namespace Core.Extensions
{
    public static class ListExtension
    {
        public static IEnumerable<T> ItensDaPagina<T>(this IEnumerable<T> value, int registroInicial, int registrosPorPagina)
        {
            return value.Skip(registroInicial).Take(registrosPorPagina).ToList();
        }
    }
}