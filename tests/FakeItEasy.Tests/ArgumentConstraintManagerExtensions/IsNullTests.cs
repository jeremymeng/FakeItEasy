namespace FakeItEasy.Tests.ArgumentConstraintManagerExtensions
{
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    internal class IsNullTests
        : ArgumentConstraintTestBase<object>
    {
        protected static new IEnumerable<object> ValidValues
        {
            get { return new object[] { null }; }
        }

        protected static new IEnumerable<object> InvalidValues
        {
            get { return new object[] { string.Empty, "foo", "bar" }; }
        }

        protected override string ExpectedDescription
        {
            get { return "NULL"; }
        }

        protected override void CreateConstraint(IArgumentConstraintManager<object> scope)
        {
            scope.IsNull();
        }
    }
}
