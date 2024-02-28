namespace Repository.DTOs
{
    public class ConversationDto
    {
        public int ConversationId { get; set; }
        public string ConversationName { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime CreateAt { get; set; }
        public bool isGroup { get; set; }
        public int OtherUserId { get; set; }
        public string OtherUserName { get; set; }
        public string Avatar { get; set; }
    }
}
