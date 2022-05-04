using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public class FileTransferContext : IdentityDbContext
    {
        public FileTransferContext(DbContextOptions<FileTransferContext> options) : base(options) { }

        public DbSet<FileTransfer> FileTransfer { get; set; }
        public DbSet<Log> Log { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
