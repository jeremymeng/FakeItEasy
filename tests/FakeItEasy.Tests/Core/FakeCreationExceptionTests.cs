namespace FakeItEasy.Tests.Core
{
    using FakeItEasy.Core;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class FakeCreationExceptionTests
        : ExceptionContractTests<FakeCreationException>
    {
#if FEATURE_NUNIT_SETCULTURE
        [Test]
        [SetCulture("en-US")]
        public void DefaultConstructor_should_set_correct_error_message()
        {
            // Arrange
            var exception = new FakeCreationException();

            // Act

            // Assert
            exception.Message.Should().Be("Unable to create fake object.");
        }
#endif

        protected override FakeCreationException CreateException()
        {
            return new FakeCreationException();
        }
    }
}
