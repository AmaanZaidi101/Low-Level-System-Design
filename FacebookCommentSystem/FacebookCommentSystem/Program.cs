using System;
using System.Collections.Generic;
using System.Linq;

namespace FacebookCommentSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            User u1 = new User("L");
            Comment c1 = new Comment("First Comment");
            Comment c2 = new Comment("Second Comment");
            Comment c11 = new Comment("First Nested Comment to first post");
            Comment c12 = new Comment("Second Nested Comment to first post");
            Comment c13 = new Comment("Third Nested Comment to first post");
            Comment c22 = new Comment("Second Comment to second post");

            Post p = new Post("This is a post");
            u1.AddCommentToPost(p, c1);
            u1.AddCommentToPost(p, c2);
            u1.ReplyToComment(p, c1.Id, c11);
            u1.ReplyToComment(p, c1.Id, c12);
            u1.ReplyToComment(p, c1.Id, c13);
            u1.ReplyToComment(p, c2.Id, c22);

            foreach (var comment in p.Comments)
            {
                Console.WriteLine(comment.Description);
                foreach (var c in comment.Comments)
                {
                    Console.WriteLine("\t" + c.Description);
                }
            }

            string s = new string('-', 100);
            Console.WriteLine(s);

            string s12 = "2nd nested comment to 1st post";
            string s11 = "1st nested comment to 1st post";

            u1.EditComment(p, c1.Id, c11.Id, s11);
            
            foreach (var comment in p.Comments)
            {
                Console.WriteLine(comment.Description);
                foreach (var c in comment.Comments)
                {
                    Console.WriteLine("\t" + c.Description);
                }
            }

            Console.WriteLine(s);

            u1.DeleteComment(p, c1.Id, c12.Id);
            
            foreach (var comment in p.Comments)
            {
                Console.WriteLine(comment.Description);
                foreach (var c in comment.Comments)
                {
                    Console.WriteLine("\t" + c.Description);
                }
            }

            Console.WriteLine(s);

            u1.DeleteComment(p, c1.Id, c1.Id);
            foreach (var comment in p.Comments)
            {
                Console.WriteLine(comment.Description);
                foreach (var c in comment.Comments)
                {
                    Console.WriteLine("\t" + c.Description);
                }
            }
        }
    }

    class Comment
    {
        private static int id = 1;
        public string Id { get; set; }
        public string ParentId { get; set; } // parent comment
        public string UserId { get; set; } //user who commented
        public string PostId { get; set; } //post the comment was made on
        public string Description { get; set; } //actual comment
        public List<Comment> Comments { get; set; } //child comments
        public Comment(string description)
        {
            Description = description;
            Id = getUniqueCommentId();
            Comments = new List<Comment>();
        }

        internal void AddComment(Comment comment)
        {
            Comments.Add(comment); 
        }
        private string getUniqueCommentId()
        {
            return "c" + id++;
        }
        internal void SetNestedDescription(string commentId, string description)
        {
            var c = Comments.Find(x => x.Id == commentId);
            if (c == null)
                return;
            c.Description = description;
        }
        internal void SetNestedDelete(string commentId)
        {
            var c = Comments.Find(x => x.Id == commentId);
            if (c == null)
                return;

            Comments.Remove(c);
        }
    }

    class Post
    {
        private static int id = 1;
        public string Id { get; set; }
        public string Description { get; set; }
        public List<Comment> Comments { get; set; }

        public Post(string description)
        {
            Id = getUniquePostId();
            Description = description;
            Comments = new List<Comment>();
        }
        private string getUniquePostId()
        {
            return "p" + id++;
        }

        internal void AddComment(Comment comment)
        {
            Comments.Add(comment);
        }

        internal void AddNestedComment(string commentId, Comment comment)
        {
            var c = Comments.Find(x => x.Id == commentId);
            if (c == null)
                return;
            c.AddComment(comment);
        }

        internal void EditComment(string parentId, string commentId, string description)
        {
            var c = Comments.Find(x => x.Id == parentId);
            if (c == null)
                return;
            if (parentId == commentId)
                c.Description = description;
            else
            {
                c.SetNestedDescription(commentId, description);
            }
        }
        internal void DeleteComment(string parentId, string commentId)
        {
            var c = Comments.Find(x => x.Id == parentId);
            if (c == null)
                return;
            if (parentId == commentId)
                Comments.Remove(c);
            else
            {
                c.SetNestedDelete(commentId);
            }
        }
    }

    class User
    {
        private static int id = 1;
        public string Id { get; set; }
        public string Name { get; set; }
        public User(string name)
        {
            Name = name;
            Id = getUniqueUserId();
        }
        private string getUniqueUserId()
        {
            return "u" + id++;
        }
        internal void AddCommentToPost(Post post, Comment comment)
        {
            comment.UserId = this.Id;
            comment.PostId = post.Id;
            comment.ParentId = comment.Id;
            post.AddComment(comment);
        }
        internal void ReplyToComment(Post post,string parentId, Comment comment)
        {
            comment.UserId = this.Id;
            comment.PostId = post.Id;
            comment.ParentId = parentId;
            post.AddNestedComment(parentId, comment);
        }
        internal bool EditComment(Post post, string parentId, string commentId, string description)
        {
            var c = post.Comments.Find(x => x.Id == parentId && x.UserId == this.Id);
            if (c == null)
            { 
                Console.WriteLine("Unable to edit");
                return false;
            }
                
            post.EditComment(parentId, commentId, description);
            return true;
        }
        internal bool DeleteComment(Post post, string parentId, string commentId)
        {
            var c = post.Comments.Find(x => x.Id == parentId && x.UserId == this.Id);
            if (c == null)
            {
                Console.WriteLine("Unable to delete");
                return false; 
            }
            post.DeleteComment(parentId, commentId);
            return true;
        }
    }
}
