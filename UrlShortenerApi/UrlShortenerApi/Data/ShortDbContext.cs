using Microsoft.EntityFrameworkCore;
using System;
using UrlShortenerApi.Models;

namespace UrlShortenerApi.Data
{
    public class ShortDbContext:DbContext
    {
        public ShortDbContext(DbContextOptions<ShortDbContext> options) : base(options)
        {
        }

        public DbSet<Url> Urls { get; set; } // Add your DbSet properties for each table
    }
}
