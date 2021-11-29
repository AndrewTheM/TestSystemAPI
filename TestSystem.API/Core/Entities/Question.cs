using System.Collections.Generic;

namespace TestSystem.API.Core.Entities
{
    public class Question : EntityBase
    {
        public string Text { get; set; }

        public byte[] Image { get; set; }

        public double PointsWorth { get; set; }

        public bool IsMultipleChoice { get; set; }

        public int TestId { get; set; }
        public Test Test { get; set; }

        public IList<Answer> Answers { get; set; }


        public int State { get; set; }
    }
}
