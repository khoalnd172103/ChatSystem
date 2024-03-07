namespace ChatSystem.Pages.Users
{
    public class UserProfileDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? KnownAs { get; set; }
        public string? Gender { get; set; }
        public string? Introduction { get; set; }
        public string? Interest { get; set; }
        public string? City { get; set; }
        public string? Avatar { get; set; }
    }
}
