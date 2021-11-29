using System;

namespace TestSystem.API.Core.Entities
{
    public class Attempt : EntityBase
    {
        public double TotalPoints { get; set; }

        public double MaxPointsThen { get; set; }

        public DateTime CompletedDateTime { get; set; }


        public int TestId { get; set; }
        public Test Test { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }
}
