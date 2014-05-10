using System;
using System.Data.Entity;

namespace RaindropDemo
{
    public class Article
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }

    public class ArticleDBContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }
    }
}