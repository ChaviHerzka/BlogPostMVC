namespace BlogPost.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int BlogPostId { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; }
    }
}
