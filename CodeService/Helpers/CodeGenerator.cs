using System;
using System.Collections.Generic;
using System.Text;
using CodeService.Enums;
using CodeService.Interfaces;
using CodeService.Models;

namespace CodeService.Helpers
{
    public class CodeGenerator : ICodeGenerator
    {
        #region Settings

        private const string _availableCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        private const int _maxFailedGenerationAttempts = 100_000;

        #endregion

        private readonly Random _rndGenerator = new Random(0);

        /// <summary>
        /// Generates and adds new unique codes to existingCodes.
        /// This operation fails if it cannot generate a unique key in
        /// a certain number of tries.
        /// </summary>
        /// <param name="existingCodes"></param>
        /// <param name="codeLength"></param>
        /// <param name="numberOfCodesToGenerate"></param>
        /// <returns>True if generation succeeded, false - if it failed.</returns>
        public bool GenerateAndAddUniqueCodes(HashSet<Code> existingCodes, int codeLength, int numberOfCodesToGenerate)
        {
            var countOfAttempts = 0;
            var codesGenerated = 0;

            var newCodes = new HashSet<Code>();

            while (codesGenerated < numberOfCodesToGenerate)
            {
                var newCode = GenerateCode(codeLength);

                if (!existingCodes.Contains(newCode) && newCodes.Add(newCode))
                {
                    ++codesGenerated;
                    countOfAttempts = 0;
                }

                countOfAttempts++;
                if (countOfAttempts > _maxFailedGenerationAttempts) break;
            }

            if (codesGenerated != numberOfCodesToGenerate) return false;

            existingCodes.UnionWith(newCodes);
            return true;
        }

        public Code GenerateCode(int length)
        {
            var charSet = new char[length];

            for (var iii = 0; iii < length; ++iii)
            {
                charSet[iii] = _availableCharacters[_rndGenerator.Next(0, _availableCharacters.Length)];
            }

            return new Code(new string(charSet), CodeState.NotUsed);
        }
    }
}
