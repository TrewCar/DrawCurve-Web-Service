using DrawCurve.Domen.Models;
using Microsoft.EntityFrameworkCore;

namespace DrawCurve.Infrastructure
{
    public class DrawCurveDbContext : DbContext
    {
        public DbSet<User> Users {  get; set; }
        public DbSet<UserLogin> UsersLogin { get; set; }
        public DbSet<RenderInfo> RenderInfo { get; set; }
        public DbSet<VideoInfo> VideoInfo { get; set; }
        public DrawCurveDbContext(DbContextOptions<DrawCurveDbContext> options) : base(options)
        {
            Database.Migrate();
        }
    }
}
