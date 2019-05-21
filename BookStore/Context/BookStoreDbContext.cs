using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Models;
using System.Data.Entity;

namespace BookStore.Context
{
    internal class BookStoreDbContext : DbContext
    {
        public BookStoreDbContext() : base("DbConnection"){ }

        public DbSet<Book> Books { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<AuthorBook> AuthorsBooks { get; set; }
    }
}
