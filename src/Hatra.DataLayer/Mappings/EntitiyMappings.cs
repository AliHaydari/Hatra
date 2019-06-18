using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Hatra.Entities;

namespace Hatra.DataLayer.Mappings
{
    public static class EntitiyMappings
    {
        public static void AddCustomEntityMappings(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Menu>(build =>
            {
                build.HasIndex(e => e.ParentId);

                build.Property(p => p.Name).HasMaxLength(450).IsRequired();

                build.HasOne(p => p.ParentMenu)
                    .WithMany(q => q.SubMenus)
                    .HasForeignKey(p => p.ParentId);
                    //.OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Category>(build =>
            {
                build.Property(p => p.Name).HasMaxLength(450).IsRequired();

                build.HasMany(p => p.Pages)
                    .WithOne(p => p.Category)
                    .HasForeignKey(p => p.CategoryId);
            });

            modelBuilder.Entity<PageImage>(build =>
            {
                build.Property(p => p.Name).HasMaxLength(450).IsRequired();
                build.Property(p => p.Name).HasMaxLength(1000).IsRequired();
                build.Property(p => p.Name).HasMaxLength(1000).IsRequired();
            });

            modelBuilder.Entity<Page>(build =>
            {
                build.Property(p => p.Title).HasMaxLength(500).IsRequired();
                build.Property(p => p.BriefDescription).HasMaxLength(500).IsRequired();

                build.HasMany(p => p.Images)
                    .WithOne(p => p.Page)
                    .HasForeignKey(p => p.PageId)
                    .IsRequired();
            });

            modelBuilder.Entity<SlideShow>(build =>
            {
                build.Property(p => p.Title).HasMaxLength(50);
                build.Property(p => p.BriefDescription).HasMaxLength(50);
                build.Property(p => p.Description);
                build.Property(p => p.Image).IsRequired();
                build.Property(p => p.Order).IsRequired();
            });

            modelBuilder.Entity<Folder>(build =>
            {
                build.Property(p => p.Name).HasMaxLength(50).IsRequired();
            });

            modelBuilder.Entity<Picture>(build =>
            {
                build.HasOne(p => p.Folder)
                    .WithMany(p => p.Pictures)
                    .HasForeignKey(p => p.FolderId);
            });

        }
    }
}
