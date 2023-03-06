using ASP_NET_Video_Games_API.Models;
using Microsoft.EntityFrameworkCore;

namespace ASP_NET_Video_Games_API.Data
{
    public class ApplicationDbContext : DbContext
    {

        // Adding model as a DBSet in order to create database context for VideoGame model
        // VideoGames becomes name of db table
        public DbSet<VideoGame> VideoGames { get; set; }
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {

        }
    }
}
