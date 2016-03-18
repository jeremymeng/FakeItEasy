namespace FakeItEasy.Tests.ArgumentConstraintManagerExtensions
{
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    internal class CollectionContainsTests
        : ArgumentConstraintTestBase<IEnumerable<object>>
    {
        protected static new IEnumerable<object> InvalidValues
        {
            get
            {
                yield return null;
                yield return new object[] { };
                yield return new object[] { null };
                yield return new object[] { 1, 2, 3, "foo", "bar" };
            }
        }

        protected static new IEnumerable<object> ValidValues
        {
            get
            {
                yield return new object[] { 10 };
                yield return new object[] { 10, 11 };
                yield return new object[] { "foo", 10 };
            }
        }

        protected override string ExpectedDescription
        {
            get
            {
                return "sequence that contains the value 10";
            }
        }

        protected override void CreateConstraint(IArgumentConstraintManager<IEnumerable<object>> scope)
        {
            scope.Contains(10);
        }
    }
}
