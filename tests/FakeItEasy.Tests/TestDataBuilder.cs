namespace FakeItEasy.Tests
{
    using System;
#if FEATURE_NETCORE_REFLECTION_API
    using System.Reflection;
#endif
    using System.Diagnostics.CodeAnalysis;

    public abstract class TestDataBuilder<TSubject, TBuilder> where TBuilder : TestDataBuilder<TSubject, TBuilder>
    {
        protected TestDataBuilder()
        {
        }

        public static TSubject Build(Action<TBuilder> buildAction)
        {
            Guard.AgainstNull(buildAction, "buildAction");

            var builder = CreateBuilderInstance();
            buildAction.Invoke(builder);
            return builder.Build();
        }

        public static TSubject BuildWithDefaults()
        {
            return Build(x => { });
        }

        [SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates", Justification = "Not required.")]
        public static implicit operator TSubject(TestDataBuilder<TSubject, TBuilder> builder)
        {
            Guard.AgainstNull(builder, "builder");

            return builder.Build();
        }

        protected abstract TSubject Build();

        protected TBuilder Do(Action<TBuilder> action)
        {
            Guard.AgainstNull(action, "action");

            action((TBuilder)this);
            return (TBuilder)this;
        }

        private static TBuilder CreateBuilderInstance()
        {
#if !FEATURE_NETCORE_REFLECTION_API
            return (TBuilder)Activator.CreateInstance(typeof(TBuilder), nonPublic: true);
#else
            var constructor = typeof(TBuilder).GetConstructor(new Type[0]);
            return (TBuilder)constructor.Invoke(new object[0]);
#endif
        }
    }
}
