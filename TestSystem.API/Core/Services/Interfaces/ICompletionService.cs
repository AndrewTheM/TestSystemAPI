using System.Collections.Generic;
using System.Threading.Tasks;
using TestSystem.API.Core.DTO;

namespace TestSystem.API.Core.Services.Interfaces
{
    public interface ICompletionService
    {
        Task<IEnumerable<AttemptDto>> GetUserAttemptsAsync(string id);
        Task RecordAttemptAsync(AttemptDto newAttempt);
        Task ClearUserAttemptsAsync(string id);
    }
}
