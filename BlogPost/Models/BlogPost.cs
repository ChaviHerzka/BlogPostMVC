namespace BlogPost.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime DatePosted { get; set; }

        public string Text { get; set; }
        public string SubText { get; set; }
    }
}
