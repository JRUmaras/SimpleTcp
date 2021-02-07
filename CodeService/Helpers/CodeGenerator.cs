using System;
using System.Collections.Generic;
using CodeService.Enums;
using CodeService.Interfaces;
using CodeService.Models;

namespace CodeService.Helpers
{
    public class CodeGenerator : ICodeGenerator
    {
        private const string _availableCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        private const int _maxFailedGenerationAttempts = 100_000;

        private readonly Random _rndGenerator = new Random((int)DateTime.Now.Ticks);

        /// <summary>
        /// Generates and adds new unique codes to existingCodes.
        /// This operation does a roll back if it cannot generate enough
        /// unique keys in a certain amount of tries.
        /// </summary>
        /// <param name="existingCodes"></param>
        /// <param name="codeLength"></param>
        /// <param name="numberOfCodesToGenerate"></param>
        /// <returns>True if generation succeeded, false - if it failed.</returns>
        public bool TryAddNewUniqueCodes(ICodesCollection existingCodes, int codeLength, int numberOfCodesToGenerate, out List<Code> newCodesAdded)
        {
            var countOfAttempts = 0;

            newCodesAdded = new List<Code>(numberOfCodesToGenerate);

            while (newCodesAdded.Count < numberOfCodesToGenerate)
            {
                if (countOfAttempts > _maxFailedGenerationAttempts) 
                    break;
                
                var setOfCodes = GenerateCodes(codeLength, numberOfCodesToGenerate - newCodesAdded.Count);

                for (var idx = 0; idx < setOfCodes.Length; ++idx)
                {
                    if (existingCodes.Add(setOfCodes[idx])) newCodesAdded.Add(setOfCodes[idx]);
                }

                countOfAttempts++;
            }

            if (newCodesAdded.Count == numberOfCodesToGenerate) return true;

            existingCodes.RemoveRange(newCodesAdded);
            newCodesAdded = new List<Code>();

            return false;
        }

        public Code GenerateCode(int length)
        {
            var charSet = new char[length];

            for (var idx = 0; idx < length; ++idx)
            {
                charSet[idx] = _availableCharacters[_rndGenerator.Next(0, _availableCharacters.Length)];
            }

            return new Code(new string(charSet), CodeState.NotUsed);
        }

        public Code[] GenerateCodes(int codeLength, int numberOfCodesToGenerate)
        {
            var newCodes = new Code[numberOfCodesToGenerate];

            for (var idx = 0; idx < numberOfCodesToGenerate; ++idx)
            {
                newCodes[idx] = GenerateCode(codeLength);
            }

            return newCodes;
        }
    }
}
