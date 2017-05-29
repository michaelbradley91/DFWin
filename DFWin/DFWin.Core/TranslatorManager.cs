using System;
using System.Collections.Generic;
using System.Threading;
using DFWin.Core.Inputs;
using DFWin.Core.Services;
using DFWin.Core.Translators;

namespace DFWin.Core
{
    public interface ITranslatorManager
    {
        void TranslateInBackgroundAndUpdateGameInput(Tile[,] tiles);
    }

    public class TranslatorManager : ITranslatorManager
    {
        private readonly IEnumerable<ITranslator> translators;
        private readonly IBackupTranslator backupTranslator;
        private readonly IInputService inputService;

        private readonly object translatorLock = new object();
        private long translationNumber;
        private long lastSubmittedTranslationNumber;

        public TranslatorManager(IEnumerable<ITranslator> translators, IBackupTranslator backupTranslator, IInputService inputService)
        {
            this.translators = translators;
            this.backupTranslator = backupTranslator;
            this.inputService = inputService;
        }

        public void TranslateInBackgroundAndUpdateGameInput(Tile[,] tiles)
        {
            lock (translatorLock)
            {
                translationNumber++;
                var myTranslationNumber = translationNumber;

                ThreadPool.QueueUserWorkItem(t =>
                {
                    try
                    {
                        var dwarfFortressInput = Translate(tiles);
                        lock (translatorLock)
                        {
                            if (lastSubmittedTranslationNumber >= myTranslationNumber) return;

                            lastSubmittedTranslationNumber = myTranslationNumber;
                            inputService.SetDwarfFortressInput(dwarfFortressInput);
                        }
                    }
                    catch (Exception e)
                    {
                        DfWin.Error("Encountered error while translating: " + e);
                    }
                });
            }
        }

        private DwarfFortressInput Translate(Tile[,] tiles)
        {
            try
            {
                foreach (var translator in translators)
                {
                    if (translator.CanTranslate(tiles))
                    {
                        return translator.Translate(tiles);
                    }
                }
                return backupTranslator.Translate(tiles);
            }
            catch (Exception e)
            {
                DfWin.Error("Failed to translate input initially: " + e);
                return backupTranslator.Translate(tiles);
            }
        }
    }
}
