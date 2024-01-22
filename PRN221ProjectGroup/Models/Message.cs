namespace PRN221ProjectGroup.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public Conversation Conversation { get; set; }
        public int SenderId { get; set; }
        public string SenderUserName { get; set;}
        public User Sender { get; set; }
        public string Content { get; set; }
        public DateTime DateSend { get; set; } = DateTime.UtcNow;
        public DateTime? DateRead { get; set; }
        public bool SenderDelete {  get; set; }
    }
}
