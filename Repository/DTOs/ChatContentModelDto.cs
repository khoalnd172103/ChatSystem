namespace Repository.DTOs
{
    public class ChatContentModelDto
    {
        public UserDto UserDto { get; set; }

        public List<MessageDto> MessageDtoList { get; set; }
    }
}
