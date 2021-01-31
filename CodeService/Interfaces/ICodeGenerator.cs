﻿using System;
using System.Collections.Generic;
using System.Text;
using CodeService.Models;

namespace CodeService.Interfaces
{
    public interface ICodeGenerator
    {
        bool GenerateAndAddUniqueCodes(HashSet<Code> existingCodes, int codeLength, int numberOfCodesToGenerate);

        Code GenerateCode(int length);
    }
}