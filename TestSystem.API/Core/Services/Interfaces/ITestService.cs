using System.Collections.Generic;
using System.Threading.Tasks;
using TestSystem.API.Core.DTO;

namespace TestSystem.API.Core.Services.Interfaces
{
    public interface ITestService
    {
        Task<IEnumerable<TestDto>> GetAllTests();

        Task<TestDto> GetTestById(int id);

        Task CreateTest(TestDto testDto);

        Task UpdateTest(TestDto testDto);

        Task<bool> RemoveTest(int id);
    }
}
