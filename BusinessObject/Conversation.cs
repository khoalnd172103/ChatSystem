namespace BusinessObject
{
    public class Conversation
    {
        public int ConversationId { get; set; }
        public string ConversationName { get; set; }
        public User user { get; set; }
        public int UserId { get; set; }
        public DateTime CreateAt {  get; set; }
        public bool isGroup { get; set; }
        public List<Message> MessagesReceived { get; set; }
        public List<Participants> Participants {  get; set; }

    }
}
