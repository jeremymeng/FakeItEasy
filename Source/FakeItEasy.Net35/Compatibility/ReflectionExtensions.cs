using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FakeItEasy
{
    internal static class ReflectionExtensions
    {
        /// <summary>
        /// A passthrough extension for legacy platforms that lacks GetTypeInfo() extension method.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetTypeInfo(this Type type)
        {
            return type;
        }
    }
}
