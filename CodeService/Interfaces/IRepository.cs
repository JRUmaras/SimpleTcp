using System.Collections.Generic;
using System.Threading.Tasks;
using CodeService.Models;

namespace CodeService.Interfaces
{
    public interface IRepository
    {
        /// <summary>
        /// Saves codes into a repository.
        /// </summary>
        /// <param name="codes"></param>
        /// <returns>Task&lt;bool&gt; indicating whether the save succeeded.</returns>
        Task<bool> SaveCodes(IEnumerable<Code> codes);

        Task<IEnumerable<Code>> LoadCodes();
    }
}
