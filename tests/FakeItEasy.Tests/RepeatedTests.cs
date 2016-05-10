namespace FakeItEasy.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
    using FluentAssertions;
    using NUnit.Framework;
    using Guard = FakeItEasy.Guard;

    [TestFixture]
    public class RepeatedTests
    {
        [SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Justification = "Used reflectively.")]
        private static object[] descriptionTestCases = TestCases.Create(
            new RepeatDescriptionTestCase()
            {
                Repeat = () => Repeated.AtLeast.Once,
                ExpectedDescription = "at least once"
            },
            new RepeatDescriptionTestCase()
            {
                Repeat = () => Repeated.AtLeast.Twice,
                ExpectedDescription = "at least twice"
            },
            new RepeatDescriptionTestCase()
            {
                Repeat = () => Repeated.AtLeast.Times(3),
                ExpectedDescription = "at least 3 times"
            },
            new RepeatDescriptionTestCase()
            {
                Repeat = () => Repeated.AtLeast.Times(10),
                ExpectedDescription = "at least 10 times"
            },
            new RepeatDescriptionTestCase()
            {
                Repeat = () => Repeated.AtLeast.Times(10),
                ExpectedDescription = "at least 10 times"
            },
            new RepeatDescriptionTestCase()
            {
                Repeat = () => Repeated.NoMoreThan.Once,
                ExpectedDescription = "no more than once"
            },
            new RepeatDescriptionTestCase()
            {
                Repeat = () => Repeated.NoMoreThan.Twice,
                ExpectedDescription = "no more than twice"
            },
            new RepeatDescriptionTestCase()
            {
                Repeat = () => Repeated.NoMoreThan.Times(10),
                ExpectedDescription = "no more than 10 times"
            },
            new RepeatDescriptionTestCase()
            {
                Repeat = () => Repeated.Exactly.Once,
                ExpectedDescription = "exactly once"
            },
            new RepeatDescriptionTestCase()
            {
                Repeat = () => Repeated.Exactly.Twice,
                ExpectedDescription = "exactly twice"
            },
            new RepeatDescriptionTestCase()
            {
                Repeat = () => Repeated.Exactly.Times(99),
                ExpectedDescription = "exactly 99 times"
            }).AsTestCaseSource();

        [TestCase(1, 1, ExpectedResult = true)]
        [TestCase(1, 2, ExpectedResult = false)]
        public bool Like_should_return_instance_that_delegates_to_expression(int expected, int actual)
        {
            // Arrange
            Expression<Func<int, bool>> repeatPredicate = repeat => repeat == expected;

            // Act
            var happened = Repeated.Like(repeatPredicate);

            // Assert
            return happened.Matches(actual);
        }

        [Test]
        public void Like_should_return_instance_that_has_correct_description()
        {
            // Arrange
            Expression<Func<int, bool>> repeatPredicate = repeat => repeat == 1;

            // Act
            var happened = Repeated.Like(repeatPredicate);

            // Assert
            happened.ToString().Should().Be("the number of times specified by the predicate 'repeat => (repeat == 1)'");
        }

        [TestCase(1, ExpectedResult = true)]
        [TestCase(2, ExpectedResult = false)]
        public bool Exactly_once_should_only_match_one(int actualRepeat)
        {
            // Arrange

            // Act
            var repeated = Repeated.Exactly.Once;

            // Assert
            return repeated.Matches(actualRepeat);
        }

        [TestCase(2, ExpectedResult = true)]
        [TestCase(0, ExpectedResult = false)]
        public bool Exactly_twice_should_only_match_two(int actualRepeat)
        {
            // Arrange

            // Act
            var repeated = Repeated.Exactly.Twice;

            // Assert
            return repeated.Matches(actualRepeat);
        }

        [TestCase(0, 0, ExpectedResult = true)]
        [TestCase(0, 1, ExpectedResult = false)]
        public bool Exactly_number_of_times_should_match_as_expected(int actualRepeat, int expectedNumberOfTimes)
        {
            // Arrange

            // Act
            var repeated = Repeated.Exactly.Times(expectedNumberOfTimes);

            // Assert
            return repeated.Matches(actualRepeat);
        }

        [TestCase(1, ExpectedResult = true)]
        [TestCase(0, ExpectedResult = false)]
        [TestCase(2, ExpectedResult = true)]
        public bool At_least_once_should_match_one_or_higher(int actualRepeat)
        {
            // Arrange

            // Act
            var repeated = Repeated.AtLeast.Once;

            // Assert
            return repeated.Matches(actualRepeat);
        }

        [TestCase(1, ExpectedResult = false)]
        [TestCase(0, ExpectedResult = false)]
        [TestCase(2, ExpectedResult = true)]
        [TestCase(3, ExpectedResult = true)]
        public bool At_least_twice_should_only_match_two_or_higher(int actualRepeat)
        {
            // Arrange

            // Act
            var repeated = Repeated.AtLeast.Twice;

            // Assert
            return repeated.Matches(actualRepeat);
        }

        [TestCase(0, 0, ExpectedResult = true)]
        [TestCase(1, 0, ExpectedResult = true)]
        [TestCase(1, 1, ExpectedResult = true)]
        [TestCase(0, 1, ExpectedResult = false)]
        [TestCase(2, 1, ExpectedResult = true)]
        public bool At_least_number_of_times_should_match_as_expected(int actualRepeat, int expectedNumberOfTimes)
        {
            // Arrange

            // Act
            var repeated = Repeated.AtLeast.Times(expectedNumberOfTimes);

            // Assert
            return repeated.Matches(actualRepeat);
        }

        [TestCase(0, ExpectedResult = true)]
        [TestCase(1, ExpectedResult = true)]
        [TestCase(2, ExpectedResult = false)]
        public bool No_more_than_once_should_match_zero_and_one_only(int actualRepeat)
        {
            // Arrange

            // Act
            var repeated = Repeated.NoMoreThan.Once;

            // Assert
            return repeated.Matches(actualRepeat);
        }

        [TestCase(0, ExpectedResult = true)]
        [TestCase(1, ExpectedResult = true)]
        [TestCase(2, ExpectedResult = true)]
        [TestCase(3, ExpectedResult = false)]
        public bool No_more_than_twice_should_match_zero_one_and_two_only(int actualRepeat)
        {
            // Arrange

            // Act
            var repeated = Repeated.NoMoreThan.Twice;

            // Assert
            return repeated.Matches(actualRepeat);
        }

        [TestCase(0, 0, ExpectedResult = true)]
        [TestCase(1, 0, ExpectedResult = false)]
        [TestCase(1, 1, ExpectedResult = true)]
        [TestCase(0, 1, ExpectedResult = true)]
        [TestCase(2, 1, ExpectedResult = false)]
        public bool No_more_than_times_should_match_as_expected(int actualRepeat, int expectedNumberOfTimes)
        {
            // Arrange

            // Act
            var repeated = Repeated.NoMoreThan.Times(expectedNumberOfTimes);

            // Assert
            return repeated.Matches(actualRepeat);
        }

        [TestCase(0, ExpectedResult = true)]
        [TestCase(1, ExpectedResult = false)]
        public bool Never_should_match_zero_only(int actualRepeat)
        {
            // Arrange

            // Act

            // Assert
            return Repeated.Never.Matches(actualRepeat);
        }

        [TestCaseSource("descriptionTestCases")]
        public void Should_provide_expected_description(Func<Repeated> repeated, string expectedDescription)
        {
            Guard.AgainstNull(repeated, "repeated");

            // Arrange
            var repeatedInstance = repeated.Invoke();

            // Act
            var description = repeatedInstance.ToString();

            // Assert
            description.Should().Be(expectedDescription);
        }

        private class RepeatDescriptionTestCase
        {
            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used reflectively.")]
            public Func<Repeated> Repeat { get; set; }

            [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Used reflectively.")]
            public string ExpectedDescription { get; set; }
        }
    }
}
