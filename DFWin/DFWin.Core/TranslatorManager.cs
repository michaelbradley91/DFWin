using System;
using System.Collections.Generic;
using System.Threading;
using DFWin.Core.Caches;
using DFWin.Core.Inputs;
using DFWin.Core.Models;
using DFWin.Core.Services;
using DFWin.Core.Translators;

namespace DFWin.Core
{
    public interface ITranslatorManager
    {
        void TranslateInBackgroundAndUpdateGameInput(Tiles tiles);
    }

    public class TranslatorManager : ITranslatorManager
    {
        private readonly IEnumerable<ITranslator> translators;
        private readonly IBackupTranslator backupTranslator;
        private readonly IInputService inputService;
        private readonly ITranslatorCache translatorCache;

        private readonly object translatorLock = new object();
        private long translationNumber;
        private long lastSubmittedTranslationNumber;

        public TranslatorManager(IEnumerable<ITranslator> translators, IBackupTranslator backupTranslator, 
            IInputService inputService, ITranslatorCache translatorCache)
        {
            this.translators = translators;
            this.backupTranslator = backupTranslator;
            this.inputService = inputService;
            this.translatorCache = translatorCache;
        }

        public void TranslateInBackgroundAndUpdateGameInput(Tiles tiles)
        {
            lock (translatorLock)
            {
                translationNumber++;
                var inCache = translatorCache.TryGet(tiles, out DwarfFortressInput cachedDwarfFortressInput);
                if (inCache)
                {
                    lastSubmittedTranslationNumber = translationNumber;
                    inputService.SetDwarfFortressInput(cachedDwarfFortressInput);
                    return;
                }

                // Take a copy so that when the task runs, it keeps the same translation number.
                var myTranslationNumber = translationNumber;

                ThreadPool.QueueUserWorkItem(t =>
                {
                    try
                    {
                        var dwarfFortressInput = Translate(tiles);
                        translatorCache.Cache(tiles, dwarfFortressInput);
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

        private DwarfFortressInput Translate(Tiles tiles)
        {
            try
            {
                foreach (var translator in translators)
                {
                    if (translator == backupTranslator) continue;
                    var success = translator.TryTranslate(tiles, out DwarfFortressInput dwarfFortressInput);
                    if (success) return dwarfFortressInput;
                }
                backupTranslator.TryTranslate(tiles, out DwarfFortressInput backupInput);
                return backupInput;
            }
            catch (Exception e)
            {
                DfWin.Error("Failed to translate input initially: " + e);
                backupTranslator.TryTranslate(tiles, out DwarfFortressInput backupInput);
                return backupInput;
            }
        }
    }
}
