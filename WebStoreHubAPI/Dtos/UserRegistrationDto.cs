namespace WebStoreHubAPI.Dtos
{
    public class UserRegistrationDto
    {
        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }
    }
}
