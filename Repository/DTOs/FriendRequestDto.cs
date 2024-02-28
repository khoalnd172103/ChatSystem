using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
