using System.Data.SqlClient;
namespace BlogPost.Models
{
    public class BlogPostDB
    {
        private readonly string _connectionString;
        public BlogPostDB(string connectionString)
        {
            _connectionString = connectionString;
        }


        public Blog GetBlogPostForId(int id)
        {
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"select * from BlogPost where Id = @Id";
            cmd.Parameters.AddWithValue("@Id", id);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return NewBlogPost(reader);
            }
            return null;
        }
        public void AddPost(Blog blogPost)
        {
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"insert into BlogPost (Title, DatePosted, Text)
                               values(@title, @dateposted, @text)";
            cmd.Parameters.AddWithValue("@title", blogPost.Title);
            cmd.Parameters.AddWithValue("@dateposted", DateTime.Now);
            cmd.Parameters.AddWithValue("@text", blogPost.Text);
            conn.Open();
            cmd.ExecuteNonQuery();

        }
        public void AddComment(Comment comment) 
        {
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"insert into Comments(Name, BlogPostId, Date, Text)
                                values(@name, @blogpostid, @date, @text)";
            cmd.Parameters.AddWithValue("@name", comment.Name);
            cmd.Parameters.AddWithValue("@blogpostid", comment.BlogPostId);
            cmd.Parameters.AddWithValue("@date", DateTime.Now);
            cmd.Parameters.AddWithValue("@text", comment.Text);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
        public List<Comment> GetComments(int id)
        {
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"select * from comments where BlogPostId = @id";
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Comment> comments = new();
            while (reader.Read())
            {
                comments.Add(NewComment(reader));
            }
            return comments;


        }
        public List<Blog> GetBlogPosts(int page)
        {
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"select * from BlogPost order by Id desc OFFSET @page ROWS FETCH NEXT 2 ROWS ONLY ";
            cmd.Parameters.AddWithValue("@page", (page - 1) * 2);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Blog> blogposts = new();
            while (reader.Read())
            {
                blogposts.Add(NewBlogPost(reader));
            }
            return blogposts;

        }
        public int GetMostRecentPost()
        {
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"select top 1 * from BlogPost order by Id desc";
            conn.Open();
            return (int)cmd.ExecuteScalar();

        }
        public int GetTotalPages() 
        {
            using SqlConnection conn = new(_connectionString);
            using SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "Select count(*) from BlogPost";
            conn.Open();
            int blogs = (int)cmd.ExecuteScalar();
            int pages = blogs / 2;
            if (blogs % 2 != 0) 
            {
                pages++;
            };
            return pages;

        }
        private Blog NewBlogPost(SqlDataReader reader)
        {
            string text = (string)reader["Text"];
            string subText;
            if (text.Length > 200)
            {
                subText = text.Substring(0, 200);
                subText += "...";
            }
            else
            {
                subText = text;
            }
            return new Blog
            {
                Id = (int)reader["Id"],
                Title = (string)reader["Title"],
                DatePosted = (DateTime)reader["DatePosted"],
                Text = text,
                SubText = subText


            };

        }
        private Comment NewComment(SqlDataReader reader)
        {
            return new Comment
            {
                Id = (int)reader["Id"],
                Name = (string)reader["Name"],
                BlogPostId = (int)reader["Id"],
                Date = (DateTime)reader["Date"],
                Text = (string)reader["Text"]
            };
        }

    }
}

