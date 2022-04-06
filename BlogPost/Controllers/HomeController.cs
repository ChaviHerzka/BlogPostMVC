using BlogPost.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BlogPost.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=BlogPost;Integrated Security=true;";
        public IActionResult Index(int page)
        {
            if (page == 0)
            {
                page = 1;
            }
            BlogPostDB db = new(_connectionString);
            BlogPostsViewModel vm = new BlogPostsViewModel();
            {
                vm.BlogPosts = db.GetBlogPosts(page);
                vm.currentPage = page;
                vm.totalPages = db.GetTotalPages();
            }
            return View(vm);
        }
        public IActionResult ViewBlog(int id)
        {
            string name = Request.Cookies["name"];
            BlogPostDB db = new(_connectionString);
            BlogViewModel vm = new BlogViewModel();
            {
                vm.BlogPost = db.GetBlogPostForId(id);
                vm.Comments = db.GetComments(id);
                vm.Name = name;
            }
            return View(vm);
        }
        public IActionResult Admin()
        {
            return View();
        }

        public IActionResult SubmitPost(Blog post)
        {
            BlogPostDB db = new(_connectionString);
            db.AddPost(post);
            return Redirect("/home/mostrecent");

        }
       
        [HttpPost]
        public IActionResult AddComment(Comment comment)
        {
            Response.Cookies.Append("name", comment.Name);
            BlogPostDB db = new(_connectionString);
            db.AddComment(comment);
            return Redirect($"/home/viewblog?id={comment.BlogPostId}");
        }
        public IActionResult MostRecent()
        {
            BlogPostDB db = new(_connectionString);
            return Redirect($"/home/viewblog?id={db.GetMostRecentPost()}");
        }
    }
}