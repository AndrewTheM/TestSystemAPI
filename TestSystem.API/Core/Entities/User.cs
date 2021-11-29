using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace TestSystem.API.Core.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IList<Attempt> Attempts { get; set; }
    }
}
