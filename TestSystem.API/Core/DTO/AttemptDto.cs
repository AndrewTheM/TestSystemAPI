using System;

namespace TestSystem.API.Core.DTO
{
    public class AttemptDto : DtoBase
    {
        public double Points { get; set; }

        public double Max { get; set; }

        public DateTime Completed { get; set; }

        public int TestId { get; set; }

        public string UserId { get; set; }
    }
}
