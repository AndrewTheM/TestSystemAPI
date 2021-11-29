﻿namespace TestSystem.API.Core.DTO
{
    public class UserDto : Credentials
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public string Token { get; set; }
    }
}
