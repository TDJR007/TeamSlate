using Microsoft.EntityFrameworkCore;
using TeamSlate.Models;

namespace TeamSlate.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // === DbSets ===
        public DbSet<Resource> Resources { get; set; }
        public DbSet<BillableMaster> BillableMasters { get; set; }
        public DbSet<DesignationMaster> DesignationMasters { get; set; }
        public DbSet<SkillMaster> SkillMasters { get; set; }
        public DbSet<ResourceSkill> ResourceSkills { get; set; }
        public DbSet<WeeklyHour> WeeklyHours { get; set; }

        // === Fluent API Config ===
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ResourceSkill M:M join table composite key
            modelBuilder.Entity<ResourceSkill>()
                .HasKey(rs => new { rs.ResourceId, rs.SkillId });

            modelBuilder.Entity<ResourceSkill>()
                .HasOne(rs => rs.Resource)
                .WithMany(r => r.ResourceSkills)
                .HasForeignKey(rs => rs.ResourceId);

            modelBuilder.Entity<ResourceSkill>()
                .HasOne(rs => rs.Skill)
                .WithMany(s => s.ResourceSkills)
                .HasForeignKey(rs => rs.SkillId)
                .OnDelete(DeleteBehavior.Cascade); // NEW CASCADE DELETE

            // WeeklyHour FK to Resource
            modelBuilder.Entity<WeeklyHour>()
                .HasOne(w => w.Resource)
                .WithMany(r => r.WeeklyHours)
                .HasForeignKey(w => w.ResourceId)
                .OnDelete(DeleteBehavior.Cascade);

            // Resource FK to DesignationMaster
            modelBuilder.Entity<Resource>()
                .HasOne(r => r.Designation)
                .WithMany(d => d.Resources)
                .HasForeignKey(r => r.DesignationId)
                .OnDelete(DeleteBehavior.Restrict);

            // Resource FK to BillableMaster
            modelBuilder.Entity<Resource>()
                .HasOne(r => r.Billable)
                .WithMany(b => b.Resources)
                .HasForeignKey(r => r.BillableId)
                .OnDelete(DeleteBehavior.Restrict);

            // === SEEDING MASTER DATA ===

            modelBuilder.Entity<BillableMaster>().HasData(
                new BillableMaster { Id = 1, Label = "Yes" },
                new BillableMaster { Id = 2, Label = "No" }
            );

            modelBuilder.Entity<DesignationMaster>().HasData(
                new DesignationMaster { Id = 1, Name = "Developer" },
                new DesignationMaster { Id = 2, Name = "Tester" },
                new DesignationMaster { Id = 3, Name = "Tech Lead" }
            );

            modelBuilder.Entity<SkillMaster>().HasData(
                new SkillMaster { Id = 1, Name = "C#" },
                new SkillMaster { Id = 2, Name = "SQL" },
                new SkillMaster { Id = 3, Name = "React" },
                new SkillMaster { Id = 4, Name = "Python" }
            );

            // === SEED RESOURCES ===
            modelBuilder.Entity<Resource>().HasData(
                new Resource { Id = 1, Name = "Khush Shah", DesignationId = 1, BillableId = 1, Availability = "100%" },
                new Resource { Id = 2, Name = "Tom Parks", DesignationId = 2, BillableId = 2, Availability = "50%" },
                new Resource { Id = 3, Name = "Charlie Brown", DesignationId = 1, BillableId = 1, Availability = "75%" },
                new Resource { Id = 4, Name = "Kendall Roy", DesignationId = 3, BillableId = 1, Availability = "100%" },
                new Resource { Id = 5, Name = "Tom Cruise", DesignationId = 3, BillableId = 2, Availability = "40%" },
                new Resource { Id = 6, Name = "Elon Musk", DesignationId = 2, BillableId = 1, Availability = "90%" },
                new Resource { Id = 7, Name = "George Clooney", DesignationId = 1, BillableId = 1, Availability = "60%" }
            );

            // === SEED RESOURCE SKILLS ===
            modelBuilder.Entity<ResourceSkill>().HasData(
                new ResourceSkill { ResourceId = 1, SkillId = 1 }, // Khush - C#
                new ResourceSkill { ResourceId = 1, SkillId = 2 }, // Khush - SQL
                new ResourceSkill { ResourceId = 2, SkillId = 3 }, // Tom P - JavaScript
                new ResourceSkill { ResourceId = 3, SkillId = 1 }, // Charlie - C#
                new ResourceSkill { ResourceId = 4, SkillId = 4 }, // Kendall - Python
                new ResourceSkill { ResourceId = 5, SkillId = 4 }, // Tom - Python
                new ResourceSkill { ResourceId = 6, SkillId = 2 }, // Elon - SQL
                new ResourceSkill { ResourceId = 6, SkillId = 4 }, // Elon - Python
                new ResourceSkill { ResourceId = 7, SkillId = 1 }  // George - C#
            );

            // === SEED WEEKLY HOURS ===
            modelBuilder.Entity<WeeklyHour>().HasData(
                new WeeklyHour { Id = 1, ResourceId = 1, WeekStartDate = new DateTime(2025, 6, 10), Hours = 40 },
                new WeeklyHour { Id = 2, ResourceId = 2, WeekStartDate = new DateTime(2025, 6, 10), Hours = 20 },
                new WeeklyHour { Id = 3, ResourceId = 3, WeekStartDate = new DateTime(2025, 6, 10), Hours = 30 },
                new WeeklyHour { Id = 4, ResourceId = 4, WeekStartDate = new DateTime(2025, 6, 10), Hours = 40 },
                new WeeklyHour { Id = 5, ResourceId = 5, WeekStartDate = new DateTime(2025, 6, 10), Hours = 16 },
                new WeeklyHour { Id = 6, ResourceId = 6, WeekStartDate = new DateTime(2025, 6, 10), Hours = 36 },
                new WeeklyHour { Id = 7, ResourceId = 7, WeekStartDate = new DateTime(2025, 6, 10), Hours = 24 }
            );


        }
    }
}