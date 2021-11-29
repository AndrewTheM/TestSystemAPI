using System.Collections.Generic;

namespace TestSystem.API.Core.Entities
{
    public class Test : EntityBase
    {
        public string Name { get; set; }

        public string Category { get; set; }

        public int? MaxTries { get; set; }

        public int? MinutesTimeLimit { get; set; }


        public IList<Question> Questions { get; set; }

        public IList<Attempt> Attempts { get; set; }
    }
}
