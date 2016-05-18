#if NUNITLITE
using System;
using System.Reflection;
using NUnitLite;

namespace FakeItEasy.IntegrationTests
{
    public class Program
    {
        public static int Main(string[] args)
        {
            return new AutoRun().Execute(typeof(FakeItEasy.IntegrationTests.Program).GetTypeInfo().Assembly, Console.Out, Console.In, args);
        }
    }
}
#endif
