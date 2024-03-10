using System.ComponentModel.DataAnnotations;

namespace BusinessObject
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string UserPassword { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Birth Day")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Display(Name = "Display Name")]
        public string KnownAs { get; set; }

        [Required]
        public DateTime CreateAt { get; set; }

        public DateTime? LastActive { get; set; }
        public string? Gender { get; set; }
        public string? Introduction { get; set; }
        public string? Interest { get; set; }
        public string? City { get; set; }

        public List<Photo>? photos { get; set; } = new();
        public List<Friend>? SentByUsers { get; set; }
        public List<Friend>? SentUsers { get; set; }
        public List<Message>? MessagesSent { get; set; }
        public List<Participants>? ParticipatedConversations { get; set; }
    }
}
