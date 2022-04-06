
namespace BlogPost.Models
{
    public class BlogViewModel
    {
        public Blog BlogPost { get; set; }
        public List<Comment> Comments { get; set; }
        public string Name { get; set; }

    }
}
