using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CodeService.Interfaces;
using CodeService.Models;
using CodeService.Properties;
using System.Text.Json;

namespace CodeService.Repository
{
    public class JsonRepository : IRepository
    {
        private readonly string _fullStorageFilePath = Resources.RelativeStorageFilePath + Resources.StorageFilename;

        public async Task<bool> SaveCodes(IEnumerable<Code> codes)
        {
            Directory.CreateDirectory(Resources.RelativeStorageFilePath);
            await using var createStream = File.Create(_fullStorageFilePath);

            await JsonSerializer.SerializeAsync(createStream, codes);

            return true;
        }

        public async Task<IEnumerable<Code>> LoadCodes()
        {
            if (!File.Exists(_fullStorageFilePath)) return new Code[0];

            await using var openStream = File.OpenRead(_fullStorageFilePath);

            return openStream.Length == 0 
                ? new Code[0] 
                : await JsonSerializer.DeserializeAsync<IEnumerable<Code>>(openStream);
        }
    }
}
