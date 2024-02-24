namespace BusinessObject
{
    public class Participants
    {
        public Conversation Conversation { get; set; }
        public int ConversationId { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public int status { get; set; }
        public bool isAdmin { get; set; }

    }
}
