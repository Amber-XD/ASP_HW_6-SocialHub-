namespace SocialHub.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }

        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
