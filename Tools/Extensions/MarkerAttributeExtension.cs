using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Core.Extensions
{
    public static class MarkerAttributeExtension
    {
        public static bool HasMarkerAttribute<T>(this AuthorizationContext that)
        {
            return that.Controller.HasMarkerAttribute<T>()
                || that.ActionDescriptor.HasMarkerAttribute<T>();
        }

        public static bool HasMarkerAttribute<T>(this ActionExecutingContext that)
        {
            return that.Controller.HasMarkerAttribute<T>()
                || that.ActionDescriptor.HasMarkerAttribute<T>();
        }

        public static bool HasMarkerAttribute<T>(this ControllerBase that)
        {
            return that.GetType().HasMarkerAttribute<T>();
        }

        public static bool HasMarkerAttribute<T>(this Type that)
        {
            return that.IsDefined(typeof(T), false);
        }

        public static IEnumerable<T> GetCustomAttributes<T>(this Type that)
        {
            return that.GetCustomAttributes(typeof(T), false).Cast<T>();
        }

        public static bool HasMarkerAttribute<T>(this ActionDescriptor that)
        {
            return that.IsDefined(typeof(T), false);
        }

        public static IEnumerable<T> GetCustomAttributes<T>(this ActionDescriptor that)
        {
            return that.GetCustomAttributes(typeof(T), false).Cast<T>();
        }
    }
}