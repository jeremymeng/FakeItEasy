namespace FakeItEasy.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using FakeItEasy.Creation;

    /// <content>Object member rule.</content>
    public partial class FakeManager
    {
#if FEATURE_SERIALIZATION
        [Serializable]
#endif
        private class ObjectMemberRule
            : IFakeObjectCallRule
        {
            private static readonly List<MethodInfo> ObjectMethods =
                new List<MethodInfo>
                    {
                        typeof(object).GetMethod("Equals", new[] { typeof(object) }),
                        typeof(object).GetMethod("ToString", new Type[] { }),
                        typeof(object).GetMethod("GetHashCode", new Type[] { })
                    };

            public FakeManager FakeManager { get; set; }

            public int? NumberOfTimesToCall
            {
                get { return null; }
            }

            public bool IsApplicableTo(IFakeObjectCall fakeObjectCall)
            {
                return IsObjetMethod(fakeObjectCall);
            }

            public void Apply(IInterceptedFakeObjectCall fakeObjectCall)
            {
                if (this.TryHandleToString(fakeObjectCall))
                {
                    return;
                }

                if (this.TryHandleGetHashCode(fakeObjectCall))
                {
                    return;
                }

                if (this.TryHandleEquals(fakeObjectCall))
                {
                    return;
                }
            }

            private static bool IsObjetMethod(IFakeObjectCall fakeObjectCall)
            {
                return ObjectMethods[0].Module.Equals(fakeObjectCall.Method.Module) && ObjectMethods[0].Name.Equals(fakeObjectCall.Method.Name) ||
                       ObjectMethods[1].Module.Equals(fakeObjectCall.Method.Module) && ObjectMethods[1].Name.Equals(fakeObjectCall.Method.Name) ||
                       ObjectMethods[2].Module.Equals(fakeObjectCall.Method.Module) && ObjectMethods[2].Name.Equals(fakeObjectCall.Method.Name);
                //return ObjectMethods.Any(m => m.Module.Equals(fakeObjectCall.Method.Module) && m.Name.Equals(fakeObjectCall.Method.Name));
            }

            private bool TryHandleGetHashCode(IInterceptedFakeObjectCall fakeObjectCall)
            {
                if (!fakeObjectCall.Method.Module.Equals(ObjectMethods[2].Module) ||
                    !fakeObjectCall.Method.Name.Equals(ObjectMethods[2].Name))
                {
                    return false;
                }

                fakeObjectCall.SetReturnValue(this.FakeManager.GetHashCode());

                return true;
            }

            private bool TryHandleToString(IInterceptedFakeObjectCall fakeObjectCall)
            {
                if (!fakeObjectCall.Method.Module.Equals(ObjectMethods[1].Module) ||
                    !fakeObjectCall.Method.Name.Equals(ObjectMethods[1].Name))
                {
                    return false;
                }

                fakeObjectCall.SetReturnValue("Faked {0}".FormatInvariant(this.FakeManager.FakeObjectType.FullName));

                return true;
            }

            private bool TryHandleEquals(IInterceptedFakeObjectCall fakeObjectCall)
            {
                if (!fakeObjectCall.Method.Module.Equals(ObjectMethods[0].Module) ||
                    !fakeObjectCall.Method.Name.Equals(ObjectMethods[0].Name))
                {
                    return false;
                }

                var argument = fakeObjectCall.Arguments[0] as ITaggable;

                if (argument != null)
                {
                    fakeObjectCall.SetReturnValue(argument.Tag.Equals(this.FakeManager));
                }
                else
                {
                    fakeObjectCall.SetReturnValue(false);
                }

                return true;
            }
        }
    }
}
