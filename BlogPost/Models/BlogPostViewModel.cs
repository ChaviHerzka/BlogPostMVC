namespace BlogPost.Models
{
    public class BlogPostsViewModel
    {
        public List<Blog> BlogPosts { get; set; }
        public int currentPage;
        public int totalPages;
    }
}
