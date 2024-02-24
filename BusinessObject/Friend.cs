using System.ComponentModel.DataAnnotations;

namespace BusinessObject
{
    public class Friend
    {
        [Key]
        public int RequestId { get; set; }
        public User SenderUser { get; set; }
        public int SenderId { get; set; }
        public string SenderUserName { get; set;}

        public User RecipientUser { get; set; }
        public int RecipientId { get; set;}
        public string RecipientUserName { get; set;}
        public DateTime DateSend { get; set; }
        public bool status { get; set; }
    }
}
