using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestSystem.API.Core.DTO;
using TestSystem.API.Core.Entities;
using TestSystem.API.Repositories.Interfaces;
using TestSystem.API.Core.Services.Interfaces;

namespace TestSystem.API.Core.Services
{
    public class TestService : ITestService
    {
        private readonly IUnitOfWork _uow;
        private readonly IRepository<Test> _repo;
        private readonly IMapper _mapper;

        public TestService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _uow = unitOfWork;
            _repo = _uow.Tests;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TestDto>> GetAllTests()
        {
            var tests = await _repo.GetAll();
            var expandedTests = tests.Include(t => t.Questions)
                                        .ThenInclude(q => q.Answers)
                                        .ToList();

            var testDtos = _mapper.Map<List<Test>, List<TestDto>>(expandedTests);
            return testDtos;
        }

        public async Task<TestDto> GetTestById(int id)
        {
            var test = await _repo.Get(id);
            if (test == null)
                return null;

            var tests = await _repo.GetAll();
            var expandedTest = tests.Include(t => t.Questions)
                                        .ThenInclude(q => q.Answers)
                                        .Where(t => t.Id == id)
                                        .First();

            var testDto = _mapper.Map<TestDto>(expandedTest);
            return testDto;
        }

        public async Task CreateTest(TestDto testDto)
        {
            var test = _mapper.Map<Test>(testDto);
            await _repo.Add(test);

            foreach (var question in test.Questions)
            {
                question.TestId = test.Id;
                await _uow.Questions.Add(question);

                foreach (var answer in question.Answers)
                {
                    answer.QuestionId = question.Id;
                    await _uow.Answers.Add(answer);
                }
            }

            await _uow.SaveChangesAsync();
        }

        public async Task UpdateTest(TestDto testDto)
        {
            var test = _mapper.Map<Test>(testDto);
            await _uow.ModifyStateAsync(test);

            foreach (var question in test.Questions)
            {
                question.TestId = test.Id;

                if (question.State == -1)
                {
                    if (question.Id > 0)
                    {
                        await _uow.Questions.Remove(question);
                        continue;
                    }
                }
                else if (question.Id == 0)
                {
                    await _uow.Questions.Add(question);
                }
                else if (question.State == 1)
                {
                    await _uow.ModifyStateAsync(question);
                }

                foreach (var answer in question.Answers)
                {
                    answer.QuestionId = question.Id;

                    if (answer.State == -1)
                    {
                        if (question.Id == 0 || answer.Id > 0)
                        {
                            await _uow.Answers.Remove(answer);
                        }
                    }
                    else if (answer.Id == 0)
                    {
                        if (answer.QuestionId > 0)
                        {
                            await _uow.Answers.Add(answer);
                        }
                    }
                    else if (answer.State == 1)
                    {
                        await _uow.ModifyStateAsync(answer);
                    }
                }
            }

            await _uow.SaveChangesAsync();
        }

        public async Task<bool> RemoveTest(int id)
        {
            var test = await _repo.Get(id);
            if (test == null)
                return false;

            await _repo.Remove(test);
            await _uow.SaveChangesAsync();
            return true;
        }
    }
}
