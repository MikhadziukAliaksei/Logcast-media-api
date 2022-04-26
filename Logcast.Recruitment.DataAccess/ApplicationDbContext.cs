using Logcast.Recruitment.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Logcast.Recruitment.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Audio> Audios { get; set; }
        public DbSet<AudioMetadata> AudioMetadatas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Subscription>().HasIndex(p => p.Email).IsUnique();

            modelBuilder.Entity<Audio>()
                .HasOne(am => am.AudioMetadata)
                .WithOne(a => a.Audio)
                .HasForeignKey<AudioMetadata>(am => am.AudioFileId);
        }
    }
}