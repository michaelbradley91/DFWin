using System;
using System.Reflection;
using Bogus;

namespace DFWin.Core.Tests.TestSetup
{
    public static class BogusHelpers
    {
        public static Func<object> Fake(Type concreteTypeToFake)
        {
            var method = typeof(BogusHelpers).GetMethod(nameof(FakeGeneric), BindingFlags.NonPublic | BindingFlags.Static);
            var genericMethod = method.MakeGenericMethod(concreteTypeToFake);
            return (Func<object>)genericMethod.Invoke(null, null);
        }

        private static Func<object> FakeGeneric<T>()
            where T : class
        {
            var faker = new Faker<T>();
            return () => faker.Generate();
        }
    }
}
