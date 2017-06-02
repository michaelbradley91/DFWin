using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DFWin.Core.States;
using DFWin.Core.Updaters;
using FluentAssertions;
using NUnit.Framework;

namespace DFWin.Core.Tests
{
    [TestFixture]
    public class ConventionTests
    {
        private Assembly coreAssembly;

        [SetUp]
        public void SetUp()
        {
            coreAssembly = typeof(CoreModule).Assembly;
        }

        [Test]
        public void AllScreenStates_HaveCorrespondingUpdaters()
        {
            var screens = GetUpdaters().ToList();
            foreach (var state in GetStates())
            {
                var nameOfUpdater = state.Name.Substring(0, state.Name.Length - "State".Length) + "Updater";
                screens.Should().Contain(s => s.Name == nameOfUpdater, $"The state {state.Name} should have a corresponding updater.");
            }
        }

        private IEnumerable<Type> GetUpdaters()
        {
            return coreAssembly.ExportedTypes.Where(t => typeof(IUpdater).IsAssignableFrom(t) && !t.IsAbstract);
        }

        private IEnumerable<Type> GetStates()
        {
            return coreAssembly.ExportedTypes.Where(t => typeof(IScreenState).IsAssignableFrom(t) && !t.IsAbstract);
        }
    }
}
