namespace TestSystem.API.Core.DTO
{
    public class AnswerDto : DtoBase
    {
        public string Text { get; set; }

        //public byte[] Image { get; set; }

        public bool Correct { get; set; }

        public double Points { get; set; }


        public int State { get; set; }
    }
}
