namespace FakeItEasy.Tests
{
    using System;
#if FEATURE_SERIALIZATION
    using System.Runtime.Serialization;
#endif

#if FEATURE_SERIALIZATION
    [Serializable]
#endif
    public class DummyException
        : Exception
    {
        public DummyException()
        {
        }

        public DummyException(string message)
            : base(message)
        {
        }

        public DummyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

#if FEATURE_SERIALIZATION
        protected DummyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}
