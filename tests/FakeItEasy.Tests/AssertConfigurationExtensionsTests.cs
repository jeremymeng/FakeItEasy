namespace FakeItEasy.Tests
{
    using System.Linq;
    using System.Linq.Expressions;
    using FakeItEasy.Configuration;
    using NUnit.Framework;

    [TestFixture]
    public class MustHaveHappenedExtensionsTests : ConfigurableServiceLocatorTestBase
    {
        [Test]
        public void MustHaveHappened_should_call_configuration_with_repeat_once()
        {
            // Arrange
            var configuration = A.Fake<IAssertConfiguration>();

            // Act
            configuration.MustHaveHappened();

            // Assert
            A.CallTo(() => configuration.MustHaveHappened(A<Repeated>.That.Matches(x => x.Matches(1))))
                .MustHaveHappened(Repeated.Exactly.Once); // avoid .MustHaveHappened(), since we're testing it
        }

        [Test]
        public void MustHaveHappened_should_be_null_guarded()
        {
            // Arrange

            // Act

            // Assert
            Expression<System.Action> call = () => A.Fake<IAssertConfiguration>().MustHaveHappened();
            call.Should().BeNullGuarded();
        }

        [TestCase(0, ExpectedResult = true)]
        [TestCase(1, ExpectedResult = false)]
        [TestCase(3, ExpectedResult = false)]
        public bool MustNotHaveHappened_should_call_configuration_with_repeat_that_validates_correctly(int repeat)
        {
            // Arrange
            var configuration = A.Fake<IAssertConfiguration>();

            // Act
            configuration.MustNotHaveHappened();

            // Assert
            var specifiedRepeat = Fake.GetCalls(configuration).Single().Arguments.Get<Repeated>(0);
            return specifiedRepeat.Matches(repeat);
        }

        [Test]
        public void MustNotHaveHappened_should_be_null_guarded()
        {
            // Arrange

            // Act

            // Assert
            Expression<System.Action> call = () => A.Fake<IAssertConfiguration>().MustNotHaveHappened();
            call.Should().BeNullGuarded();
        }
    }
}
