using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DFWin.Core;
using DFWin.Core.Models;
using DFWin.Core.States;
using FluentAssertions;
using NUnit.Framework;

namespace DFWin.Tests
{
    [TestFixture]
    public class ConventionTests
    {
        private Assembly winAssembly;
        private Assembly coreAssembly;

        [SetUp]
        public void SetUp()
        {
            winAssembly = typeof(GameModule).Assembly;
            coreAssembly = typeof(CoreModule).Assembly;
        }

        [Test]
        public void AllScreenStates_HaveCorrespondingScreens()
        {
            var screens = GetScreens().ToList();
            foreach (var state in GetStates())
            {
                var nameOfScreen = state.Name.Substring(0, state.Name.Length - "State".Length) + "Screen";
                screens.Should().Contain(s => s.Name == nameOfScreen, $"The state {state.Name} should have a corresponding screen.");
            }
        }

        private IEnumerable<Type> GetScreens()
        {
            return winAssembly.ExportedTypes.Where(t => typeof(IScreen).IsAssignableFrom(t) && !t.IsAbstract);
        }

        private IEnumerable<Type> GetStates()
        {
            return coreAssembly.ExportedTypes.Where(t => typeof(IScreenState).IsAssignableFrom(t) && !t.IsAbstract);
        }
    }
}
