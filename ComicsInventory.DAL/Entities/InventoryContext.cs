using System.Data.Entity;

namespace ComicsInventory.DAL.Entities
{
    public class InventoryContext : DbContext
    {
        public InventoryContext()
            : base("Inventory")
        {
        }

        public virtual DbSet<BoxStore> BoxStores { get; set; }
        public virtual DbSet<Grade> Grades { get; set; }
        public virtual DbSet<Issue> Issues { get; set; }
        public virtual DbSet<IssueCondition> IssueConditions { get; set; }
        public virtual DbSet<Publisher> Publishers { get; set; }
        public virtual DbSet<Series> Series { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BoxStore>()
                .Property(e => e.BoxName)
                .IsUnicode(false);

            modelBuilder.Entity<BoxStore>()
                .Property(e => e.QR_Data)
                .IsUnicode(false);

            modelBuilder.Entity<BoxStore>()
                .HasMany(e => e.Issues)
                .WithOptional(e => e.BoxStore)
                .HasForeignKey(e => e.Box_ID);

            modelBuilder.Entity<Grade>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Grade>()
                .HasMany(e => e.IssueConditions)
                .WithOptional(e => e.Grade)
                .HasForeignKey(e => e.Grade_ID);

            modelBuilder.Entity<Issue>()
                .Property(e => e.Number)
                .IsUnicode(false);

            modelBuilder.Entity<Issue>()
                .Property(e => e.publication_date)
                .IsUnicode(false);

            modelBuilder.Entity<Issue>()
                .Property(e => e.page_count)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Issue>()
                .Property(e => e.frequency)
                .IsUnicode(false);

            modelBuilder.Entity<Issue>()
                .Property(e => e.editor)
                .IsUnicode(false);

            modelBuilder.Entity<Issue>()
                .Property(e => e.ISBN)
                .IsUnicode(false);

            modelBuilder.Entity<Issue>()
                .Property(e => e.barcode)
                .IsUnicode(false);

            modelBuilder.Entity<Publisher>()
                .Property(e => e.URL)
                .IsUnicode(false);

            modelBuilder.Entity<Series>()
                .Property(e => e.dimensions)
                .IsUnicode(false);

            modelBuilder.Entity<Series>()
                .Property(e => e.paperStock)
                .IsUnicode(false);
        }
    }
}