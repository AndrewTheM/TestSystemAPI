namespace TestSystem.API.Core.DTO
{
    public class QuestionDto : DtoBase
    {
        public string Text { get; set; }

        //public byte[] Image { get; set; }

        public double Points { get; set; }

        public bool Multiple { get; set; }

        public AnswerDto[] Options { get; set; }


        public int State { get; set; }
    }
}
