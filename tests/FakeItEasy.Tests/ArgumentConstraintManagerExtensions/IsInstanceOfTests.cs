namespace FakeItEasy.Tests.ArgumentConstraintManagerExtensions
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    internal class IsInstanceOfTests
        : ArgumentConstraintTestBase<object>
    {
        protected static new IEnumerable<object> InvalidValues
        {
            get { return new object[] { new object(), 1, "foo", null }; }
        }

        protected static new IEnumerable<object> ValidValues
        {
            get { return new object[] { new DateTime(2000, 1, 1) }; }
        }

        protected override string ExpectedDescription
        {
            get { return "Instance of System.DateTime"; }
        }

        protected override void CreateConstraint(IArgumentConstraintManager<object> scope)
        {
            scope.IsInstanceOf(typeof(DateTime));
        }
    }
}
