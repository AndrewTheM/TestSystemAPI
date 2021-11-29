namespace TestSystem.API.Core.DTO
{
    public class TestDto : DtoBase
    {
        public string Name { get; set; }

        public string Category { get; set; }

        public int Tries { get; set; }

        public int Minutes { get; set; }

        public QuestionDto[] Questions { get; set; }
    }
}
