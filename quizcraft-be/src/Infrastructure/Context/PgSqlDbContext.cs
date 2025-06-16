using Microsoft.EntityFrameworkCore;
using src.Domain.Models;

namespace src.Infrastructure.Context
{
    public class PgSqlDbContext : DbContext
    {
        DbSet<User> Users { get; set; }

        public PgSqlDbContext(DbContextOptions options) : base(options) { }
    }
}
