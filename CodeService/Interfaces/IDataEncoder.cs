using System.IO;
using CodeService.Enums;
using CodeService.Models;

namespace CodeService.Interfaces
{
    public interface IDataEncoder
    {
        GenerationParameters DecodeGenerateRequest(Stream stream);

        byte[] EncodeGenerateResponse(bool value);

        Code DecodeUseCodeRequest(Stream stream);

        byte[] EncodeUseCodeResponse(CodeState value);
    }
}
