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
        public JsonRepository()
        {
            Directory.CreateDirectory("Storage");
        }

        public async Task<bool> SaveCodes(IEnumerable<Code> codes)
        {
            await using var createStream = File.Create(Resources.StorageFilePath);

            await JsonSerializer.SerializeAsync(createStream, codes);

            return true;
        }

        public async Task<IEnumerable<Code>> LoadCodes()
        {
            if (!File.Exists(Resources.StorageFilePath)) return new Code[0];

            await using var openStream = File.OpenRead(Resources.StorageFilePath);

            return openStream.Length == 0 
                ? new Code[0] 
                : await JsonSerializer.DeserializeAsync<IEnumerable<Code>>(openStream);
        }
    }
}
