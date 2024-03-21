namespace Repository.DTOs
{
    public class FriendRequestDto
    {
        public int RequestId { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set;}
        public string AvatarUrl {  get; set; }
        public DateTime DateSend { get; set; }
    }
}
