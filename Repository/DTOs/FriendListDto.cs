namespace Repository.DTOs
{
    public class FriendListDto
    {
        public int SenderId { get; set; }
        public string SenderUserName { get; set; }
        public int RecipientId { get; set; }
        public string RecipientUserName { get; set; }
        public DateTime DateSend { get; set; }
        public string? Avatar { get; set; }
    }
}
