namespace TestSystem.API.Core.Entities
{
    public class Answer : EntityBase
    {
        public string Text { get; set; }

        public byte[] Image { get; set; }

        public bool IsCorrect { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }


        public int State { get; set; }
    }
}
