﻿using System;
using System.Reflection;

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

        public static MethodInfo GetMethodInfo(this Delegate @delegate)
        {
            return @delegate.Method;
        }
    }
}
