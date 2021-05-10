using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestAPI.Models;

namespace RestAPI.Data
{
    public class SpaceParkDbContext : DbContext
    {
        public SpaceParkDbContext(DbContextOptions<SpaceParkDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Size>(entity =>
                entity.HasCheckConstraint("CK_Only_Range_Between_1_and_4", "[Type] >= 1 AND [Type] < 5"));

            modelBuilder.Entity<Receipt>()
                .HasOne<Size>(r => r.Size)
                .WithMany(s => s.Receipts)
                .HasForeignKey(s => s.SizeId);
        }

        public virtual DbSet<Parking> Parkings { get; set; }
        public virtual DbSet<Receipt> Receipts { get; set; }
        public virtual DbSet<Size> Sizes { get; set; }
        public virtual DbSet<SpacePort> SpacePorts { get; set; }
    }
}
