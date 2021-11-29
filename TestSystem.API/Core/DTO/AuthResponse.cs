namespace TestSystem.API.Core.DTO
{
    public class AuthResponse
    {
        public UserDto User { get; set; }

        public string[] Errors { get; set; }
    }
}
