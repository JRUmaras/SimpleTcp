using System.Threading;
using System.Threading.Tasks;

namespace CodeService.Interfaces
{
    public interface ICodeService
    {
        Task Start(CancellationToken cancellationToken);
    }
}
