namespace FakeItEasy.Tests.FakeConstraints
{
    using System;
    using System.Linq;
    using FakeItEasy.Core;
    using NUnit.Framework.Constraints;

    internal class WrappingFakeConstraint
        : Constraint
    {
        public WrappingFakeConstraint()
        {
            this.Description = "A fake object wrapper.";
        }

        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            var fake = Fake.GetFakeManager(actual);

            return new ConstraintResult(this, actual, fake.Rules.Any(x => x is WrappedObjectRule));
        }
    }
}
