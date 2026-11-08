namespace SocialHub.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<Comment> Comments { get; set; }
    }


}
