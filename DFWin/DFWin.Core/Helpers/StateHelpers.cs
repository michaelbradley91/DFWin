using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DFWin.Core.Inputs.DwarfFortress;
using DFWin.Core.Models;
using DFWin.Core.States;

namespace DFWin.Core.Helpers
{
    public static class StateHelpers
    {
        private static readonly IDictionary<string, Type> ScreenStateTypeByName;

        static StateHelpers()
        {
            ScreenStateTypeByName = new Dictionary<string, Type>();
            var allScreens = typeof(IScreen).Assembly.ExportedTypes.Where(t => typeof(IScreenState).IsAssignableFrom(t) && !t.IsAbstract);
            foreach (var screen in allScreens)
            {
                ScreenStateTypeByName[screen.Name] = screen;
            }
        }

        public static IScreenState CreateInitialScreenState(IDwarfFortressInput input)
        {
            var stateName = GetNameOfDwarfFortressState(input.GetType().Name);
            return CreateInitialScreenState(stateName, input);
        }

        public static IScreenState CreateInitialScreenState(string typeName, IDwarfFortressInput input)
        {
            var type = ScreenStateTypeByName[typeName];
            return (IScreenState)Activator.CreateInstance(type, input);
        }

        private static string GetNameOfDwarfFortressState(string inputName)
        {
            return inputName.Substring(0, inputName.Length - "Input".Length) + "State";
        }
    }
}
