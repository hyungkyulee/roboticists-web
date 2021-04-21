using System;
using RoboticistsApis.Models.Wrapper;

namespace RoboticistsApis.Models.Domain
{
    public class BlogPost
    {
        public PostId BlogId { get; }
        public string Category { get; }
        public string Title { get; }
        public string Content { get; }
        public DateTime DateTimestamp { get; }

        public BlogPost(PostId blogBlogId, 
            string category, 
            string title, 
            string content,
            DateTime dateTimestamp)
        {
            BlogId = blogBlogId;
            Category = category;
            Title = title;
            Content = content;
            DateTimestamp = dateTimestamp;
        }

        public static (BlogPost post, ArgumentException error) Create(PostId blogId, 
            string category, 
            string title, 
            string content)
        {
            var errorMessage = string.Empty;
            if (blogId.Value == Guid.Empty) errorMessage += $"{nameof(blogId)}";
            if (string.IsNullOrEmpty(category)) errorMessage += $"{nameof(category)}";
            if (string.IsNullOrEmpty(title)) errorMessage += $"{nameof(category)}";
            if (string.IsNullOrEmpty(content)) errorMessage += $"{nameof(category)}";

            if (!string.IsNullOrEmpty(errorMessage))
            {
                return (null, new ArgumentException($"Key data: ${errorMessage}, cannot be empty."));
            }
                
            return (new BlogPost(blogId, category, title, content, DateTime.Now), null);
        }
    }
}