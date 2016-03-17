Imports NUnit.Framework
Imports FakeItEasy.Tests

<TestFixture()> _
Public Class NextCallTests

    <Test()>
    Public Overridable Sub AssertWasCalled_should_fail_when_call_has_not_been_made()
        Assert.Throws(Of ExpectationException)(Sub()
                                                   Dim foo = A.Fake(Of IFoo)()

                                                   NextCall.To(foo).MustHaveHappened()
                                                   foo.Bar()
                                               End Sub)
    End Sub

    <Test()> _
    Public Overridable Sub AssertWasCalled_should_succeed_when_call_has_been_made()
        Dim foo = A.Fake(Of IFoo)()

        foo.Bar()

        NextCall.To(foo).MustHaveHappened()
        foo.Bar()
    End Sub

    <Test()>
    Public Overridable Sub AssertWasCalled_with_arguments_specified_should_fail_if_not_argument_predicate_passes()
        Assert.Throws(Of ExpectationException)(Sub()
                                                   Dim foo = A.Fake(Of IFoo)()

                                                   foo.Bar("something", "")

        NextCall.To(foo).WhenArgumentsMatch(Function(a) a.Get(Of String)(0) = "something else").MustHaveHappened(Repeated.Exactly.Once)
                                                   foo.Bar(Nothing, Nothing)
                                               End Sub)
    End Sub

    <Test()> _
    Public Sub AssertWasCalled_should_succeed_when_arguments_matches_argument_predicate()
        Dim foo = A.Fake(Of IFoo)()

        foo.Bar("something", "")

        NextCall.To(foo).WhenArgumentsMatch(Function(a) a.Get(Of String)(0) = "something").MustHaveHappened()
        foo.Bar(Nothing, Nothing)
    End Sub

End Class
