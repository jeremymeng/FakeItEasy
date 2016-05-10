namespace FakeItEasy.Tests
{
    using System;
    using System.Reflection;
#if FEATURE_SERIALIZATION
    using System.Runtime.Serialization;
#endif
    using FluentAssertions;
    using NUnit.Framework;

    public abstract class ExceptionContractTests<T> where T : Exception
    {
        private T exception;

        [SetUp]
        public void Setup()
        {
            this.exception = this.CreateException();
        }

#if FEATURE_SERIALIZATION
        [Test]
        public void Exception_should_provide_serialization_constructor()
        {
            // Arrange

            // Act
            var constructor = typeof(T).GetConstructor(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, new[] { typeof(SerializationInfo), typeof(StreamingContext) }, null);

            // Assert
            constructor.Should().NotBeNull("exception classes should implement a constructor serialization constructor");
            constructor.IsPublic.Should().BeFalse("serialization constructor should be protected, not public");
            constructor.IsPrivate.Should().BeFalse("serialization constructor should be protected, not private");
        }
#endif

        [Test]
        public void Exception_should_provide_message_only_constructor()
        {
            // Arrange

            // Act
            var constructor = this.GetMessageOnlyConstructor();

            // Assert
            constructor.Should().NotBeNull("Exception classes should provide a public message only constructor.");
        }

        [Test]
        public void Constructor_that_takes_message_should_set_message()
        {
            // Arrange
            var constructor = this.GetMessageOnlyConstructor();

            // Act
            var result = (T)constructor.Invoke(new object[] { "A message" });

            // Assert
            result.Message.Should().StartWith("A message");
        }

#if FEATURE_SERIALIZATION
        [Test]
        public void Exception_should_be_serializable()
        {
            this.exception.Should().BeBinarySerializable();
        }
#endif

        [Test]
        public void Exception_should_provide_message_and_inner_exception_constructor()
        {
            // Arrange
            var constructor = this.GetMessageAndInnerExceptionConstructor();

            // Act

            // Assert
            constructor.Should().NotBeNull("exception classes should provide a public constructor that takes message and inner exception.");
        }

        [Test]
        public void Constructor_that_takes_message_and_inner_exception_should_set_message()
        {
            // Arrange
            var constructor = this.GetMessageAndInnerExceptionConstructor();

            // Act
            var result = (T)constructor.Invoke(new object[] { "A message", new InvalidOperationException() });

            // Assert
            result.Message.Should().Be("A message");
        }

        [Test]
        public void Constructor_that_takes_message_and_inner_exception_should_set_inner_exception()
        {
            // Arrange
            var constructor = this.GetMessageAndInnerExceptionConstructor();
            var innerException = new InvalidOperationException();

            // Act
            var result = (T)constructor.Invoke(new object[] { string.Empty, innerException });

            // Assert
            result.InnerException.Should().Be(innerException);
        }

        [Test]
        public void Exception_should_provide_default_constructor()
        {
            // Arrange
            var constructor = typeof(T).GetConstructor(new Type[] { });

            // Act
            constructor.Invoke(new object[] { });

            // Assert
            constructor.Should().NotBeNull("exception classes should provide a public default constructor.");
        }

        protected abstract T CreateException();

        private ConstructorInfo GetMessageAndInnerExceptionConstructor()
        {
            return typeof(T).GetConstructor(new[] { typeof(string), typeof(Exception) });
        }

        private ConstructorInfo GetMessageOnlyConstructor()
        {
            return typeof(T).GetConstructor(new[] { typeof(string) });
        }
    }
}
