using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bogus;
using DFWin.Core.Helpers;
using DFWin.Core.Inputs.DwarfFortress;
using DFWin.Core.States;
using DFWin.Core.States.DwarfFortress;
using DFWin.Core.Tests.TestSetup;
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

        [Test]
        public void AllDwarfFortressStates_HaveCorrespondingDwarfFortressInputs()
        {
            var inputs = GetDwarfFortressInputs().ToList();
            foreach (var state in GetDwarfFortressStates())
            {
                var nameOfInput = state.Name.Substring(0, state.Name.Length - "State".Length) + "Input";
                inputs.Should().Contain(s => s.Name == nameOfInput, $"The DF state {state.Name} should have a corresponding DF input.");
            }
        }

        [Test]
        public void AllDwarfFortressStates_CanBeCreatedDirectlyFromTheirDwarfFortressInput()
        {
            foreach (var state in GetDwarfFortressStates())
            {
                if (state.BaseType == null) throw new InvalidOperationException();

                var typeOfInput = FindImplementation(state.BaseType.GetGenericArguments()[0]);
                var fakeInput = (IDwarfFortressInput) BogusHelpers.Fake(typeOfInput)();
                var dfState = StateHelpers.CreateInitialScreenState(state.Name, fakeInput);
                dfState.Should().NotBeNull($"The DF state {state.Name} should be possible to create given just the corresponding DF input.");
            }
        }

        private IEnumerable<Type> GetUpdaters()
        {
            return coreAssembly.ExportedTypes.Where(t => typeof(IUpdater).IsAssignableFrom(t) && !t.IsAbstract);
        }

        private IEnumerable<Type> GetDwarfFortressStates()
        {
            return GetStates().Where(t =>
                t.BaseType != null &&
                t.BaseType.IsGenericType &&
                t.BaseType.GetGenericTypeDefinition() == typeof(DwarfFortressState<>));
        }

        private IEnumerable<Type> GetStates()
        {
            return coreAssembly.ExportedTypes.Where(t => typeof(IScreenState).IsAssignableFrom(t) && !t.IsAbstract);
        }

        private IEnumerable<Type> GetDwarfFortressInputs()
        {
            return coreAssembly.ExportedTypes.Where(t => typeof(IDwarfFortressInput).IsAssignableFrom(t) && !t.IsAbstract);
        }

        private Type FindImplementation(Type type)
        {
            if (!type.IsAbstract) return type;
            return coreAssembly.ExportedTypes.Single(t => !t.IsAbstract && type.IsAssignableFrom(t));
        }
    }
}
