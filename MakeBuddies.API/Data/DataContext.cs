using Microsoft.EntityFrameworkCore;
using MakeBuddies.API.Models;

namespace MakeBuddies.API.Data
{
    public class DataContext : DbContext 
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<UserSetting> UserSettings { get; set; }
        public DbSet<User> Users { get; set; }



    }
}