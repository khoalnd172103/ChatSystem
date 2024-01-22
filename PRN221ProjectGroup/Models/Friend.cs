namespace PRN221ProjectGroup.Models
{
    public class Friend
    {
        public User SenderUser { get; set; }
        public string RequestId {  get; set; }
        public int SenderId { get; set; }
        public string SenderUserName { get; set;}

        public User RecipientUser { get; set; }
        public int RecipientId { get; set;}
        public string RecipientUserName { get; set;}
        public DateTime DateSend { get; set; }
        public bool status { get; set; }
    }
}
