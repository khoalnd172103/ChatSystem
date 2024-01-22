namespace PRN221ProjectGroup.Models
{
    public class Photo
    {
        public int PhotoId { get; set; }
        public string PhotoUrl { get; set; }
        public bool isMain { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
