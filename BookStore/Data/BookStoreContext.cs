using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BookStore.Models;

namespace BookStore.Data
{
    public class BookStoreContext : IdentityDbContext<ApplicationUser>
    {
        public BookStoreContext(DbContextOptions<BookStoreContext> option) : base(option)
        {

        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BookGallery> BookGalleries { get; set; }
    }
}
