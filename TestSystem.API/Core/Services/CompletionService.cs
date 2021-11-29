using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestSystem.API.Core.DTO;
using TestSystem.API.Core.Entities;
using TestSystem.API.Repositories.Interfaces;
using TestSystem.API.Core.Services.Interfaces;

namespace TestSystem.API.Core.Services
{
    public class CompletionService : ICompletionService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public CompletionService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _uow = unitOfWork;
        }

        public async Task<IEnumerable<AttemptDto>> GetUserAttemptsAsync(string id)
        {
            var attempts = await _uow.Attempts.GetAll();
            var list = attempts.Where(at => at.UserId == id).ToList();
            var dtoList = _mapper.Map<List<Attempt>, List<AttemptDto>>(list);
            return dtoList;
        }

        public async Task RecordAttemptAsync(AttemptDto newAttempt)
        {
            var attempt = _mapper.Map<Attempt>(newAttempt);
            await _uow.Attempts.Add(attempt);
            await _uow.SaveChangesAsync();
        }

        public async Task ClearUserAttemptsAsync(string id)
        {
            var attempts = await _uow.Attempts.GetAll();
            var userAttempts = attempts.Where(at => at.UserId == id).ToList();

            foreach (var attempt in userAttempts)
            {
                await _uow.Attempts.Remove(attempt);
            }

            await _uow.SaveChangesAsync();
        }
    }
}
