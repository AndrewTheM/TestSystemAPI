using AutoMapper;
using System.Linq;
using TestSystem.API.Core.DTO;
using TestSystem.API.Core.Entities;

namespace TestSystem.API.Core.DTO.Mapping
{
    public class DtoMappings : Profile
    {
        public DtoMappings()
        {
            MapTest();
            MapQuestion();
            MapAnswer();
            MapAttempt();
            MapUser();
        }

        protected virtual void MapTest()
        {
            CreateMap<Test, TestDto>()
                .ForMember(dest => dest.Tries, opt => opt.MapFrom(src => src.MaxTries))
                .ForMember(dest => dest.Minutes, opt => opt.MapFrom(src => src.MinutesTimeLimit))
                .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions.ToArray()));

            CreateMap<TestDto, Test>()
                .ForMember(dest => dest.MaxTries, opt => opt.MapFrom(src => src.Tries))
                .ForMember(dest => dest.MinutesTimeLimit, opt => opt.MapFrom(src => src.Minutes))
                .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions));
        }

        protected virtual void MapQuestion()
        {
            CreateMap<Question, QuestionDto>()
                .ForMember(dest => dest.Points, opt => opt.MapFrom(src => src.PointsWorth))
                .ForMember(dest => dest.Multiple, opt => opt.MapFrom(src => src.IsMultipleChoice))
                .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.Answers.ToArray()));

            CreateMap<QuestionDto, Question>()
                .ForMember(dest => dest.PointsWorth, opt => opt.MapFrom(src => src.Points))
                .ForMember(dest => dest.IsMultipleChoice, opt => opt.MapFrom(src => src.Multiple))
                .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Options));
        }

        protected virtual void MapAnswer()
        {
            CreateMap<Answer, AnswerDto>()
                .ForMember(dest => dest.Correct, opt => opt.MapFrom(src => src.IsCorrect));

            CreateMap<AnswerDto, Answer>()
                .ForMember(dest => dest.IsCorrect, opt => opt.MapFrom(src => src.Correct));
        }

        protected virtual void MapAttempt()
        {
            CreateMap<Attempt, AttemptDto>()
                .ForMember(dest => dest.Points, opt => opt.MapFrom(src => src.TotalPoints))
                .ForMember(dest => dest.Max, opt => opt.MapFrom(src => src.MaxPointsThen))
                .ForMember(dest => dest.Completed, opt => opt.MapFrom(src => src.CompletedDateTime));

            CreateMap<AttemptDto, Attempt>()
                .ForMember(dest => dest.TotalPoints, opt => opt.MapFrom(src => src.Points))
                .ForMember(dest => dest.MaxPointsThen, opt => opt.MapFrom(src => src.Max))
                .ForMember(dest => dest.CompletedDateTime, opt => opt.MapFrom(src => src.Completed));
        }
        private void MapUser()
        {
            CreateMap<UserDto, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username));
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName));
        }
    }
}
