using Microsoft.EntityFrameworkCore;

namespace LearnVocabulary.Models
{
    public class LearnContext : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-3GFPOJ9\\SQLEXPRESS;initial Catalog=LearnVocabularyDb; integrated Security=true; TrustServerCertificate=true;");
        }

        public DbSet<Word> Words { get; set; }
        public DbSet<PhoneticInfo> Phonetics { get; set; }
        public DbSet<MeaningInfo> Meanings { get; set; }
        public DbSet<DefinitionInfo> Definitions { get; set; }
        public DbSet<LicanseInfo> LicanseInfos { get; set; }
        public DbSet<UnknownWord> UnknownWords { get; set; }
        public DbSet<WordsSentence> WordsSentences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PhoneticInfo>()
            .HasOne(p => p.Word)
            .WithMany(w => w.Phonetics)
            .HasForeignKey(p => p.WordId);

            modelBuilder.Entity<MeaningInfo>()
                .HasOne(m => m.Word)
                .WithMany(w => w.Meanings)
                .HasForeignKey(m => m.WordId);

            modelBuilder.Entity<DefinitionInfo>()
                .HasOne(d => d.Meaning)
                .WithMany(m => m.Definitions)
                .HasForeignKey(d => d.MeaningId);

            modelBuilder.Entity<WordsSentence>()
                .HasOne(d=>d.UnknownWord)
                .WithMany(m=>m.WordsSentences)
                .HasForeignKey(d => d.UnknownWordId);

            modelBuilder.Entity<PhoneticInfo>().Property(e => e.Id).UseIdentityColumn();
            modelBuilder.Entity<MeaningInfo>().Property(e => e.Id).UseIdentityColumn();
            modelBuilder.Entity<DefinitionInfo>().Property(e => e.Id).UseIdentityColumn();
            modelBuilder.Entity<LicanseInfo>().Property(e => e.Id).UseIdentityColumn();


        }
    }
}
