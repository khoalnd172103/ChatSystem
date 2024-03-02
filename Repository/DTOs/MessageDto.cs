namespace Repository.DTOs
{
    public class MessageDto
    {
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public string SenderUserName { get; set; }
        public string Content { get; set; }
        public DateTime DateSend { get; set; }
        public DateTime? DateRead { get; set; }
        public bool SenderDelete { get; set; }
        public string Avatar { get; set; }
        public string DisplaySendTime { get; set; }
    }
}
