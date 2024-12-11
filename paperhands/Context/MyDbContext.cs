using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using paperhands.Entities;

namespace paperhands.Context;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<LanguagesLookup> LanguagesLookups { get; set; }

    public virtual DbSet<Publisher> Publishers { get; set; }

    public virtual DbSet<RatingsPerBook> RatingsPerBooks { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    public virtual DbSet<TitlarPerFörfattare> TitlarPerFörfattares { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=bookstore;User Id=sa;Password=Janne2023!; Trusted_Connection=False;TrustServerCertificate=Yes;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Authors__3213E83FCB754E39");

            entity.HasIndex(e => e.Id, "UQ__Authors__3213E83E4E9EC6E5").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Isbn13).HasName("PK__Books__AA00666D13F8F3E4");

            entity.HasIndex(e => e.Isbn13, "UQ__Books__AA00666C0F66D357").IsUnique();

            entity.Property(e => e.Isbn13)
                .HasMaxLength(17)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("isbn13");
            entity.Property(e => e.GenreId).HasColumnName("genre_id");
            entity.Property(e => e.LanguageId).HasColumnName("language_id");
            entity.Property(e => e.Price)
                .HasColumnType("money")
                .HasColumnName("price");
            entity.Property(e => e.PublishDate).HasColumnName("publish_date");
            entity.Property(e => e.PublisherId).HasColumnName("publisher_id");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");

            entity.HasOne(d => d.Genre).WithMany(p => p.Books)
                .HasForeignKey(d => d.GenreId)
                .HasConstraintName("FK__Books__genre_id__5629CD9C");

            entity.HasOne(d => d.Language).WithMany(p => p.Books)
                .HasForeignKey(d => d.LanguageId)
                .HasConstraintName("FK__Books__language___59FA5E80");

            entity.HasOne(d => d.Publisher).WithMany(p => p.Books)
                .HasForeignKey(d => d.PublisherId)
                .HasConstraintName("FK__Books__publisher__5AEE82B9");

            entity.HasMany(d => d.Authors).WithMany(p => p.BookIsbn13s)
                .UsingEntity<Dictionary<string, object>>(
                    "AuthorsBook",
                    r => r.HasOne<Author>().WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Authors_B__autho__5812160E"),
                    l => l.HasOne<Book>().WithMany()
                        .HasForeignKey("BookIsbn13")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__Authors_B__book___59063A47"),
                    j =>
                    {
                        j.HasKey("BookIsbn13", "AuthorId").HasName("PK__Authors___57454CE54D725B7C");
                        j.ToTable("Authors_Books");
                        j.IndexerProperty<string>("BookIsbn13")
                            .HasMaxLength(17)
                            .IsUnicode(false)
                            .IsFixedLength()
                            .HasColumnName("book_isbn13");
                        j.IndexerProperty<long>("AuthorId").HasColumnName("author_id");
                    });
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Genres__3213E83F51AAA7D0");

            entity.HasIndex(e => e.Id, "UQ__Genres__3213E83E17BA97C4").IsUnique();

            entity.HasIndex(e => e.Name, "UQ__Genres__72E12F1B641413BD").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => new { e.StoreId, e.Isbn13 }).HasName("PK__Inventor__6852A56A9C0C58CA");

            entity.ToTable("Inventory");

            entity.Property(e => e.StoreId).HasColumnName("store_id");
            entity.Property(e => e.Isbn13)
                .HasMaxLength(17)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("isbn13");
            entity.Property(e => e.Amount).HasColumnName("amount");

            entity.HasOne(d => d.Isbn13Navigation).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.Isbn13)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inventory__isbn1__5535A963");

            entity.HasOne(d => d.Store).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.StoreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inventory__store__5441852A");
        });

        modelBuilder.Entity<LanguagesLookup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Language__3213E83FDB53E62E");

            entity.ToTable("Languages_Lookup");

            entity.HasIndex(e => e.Id, "UQ__Language__3213E83EF45215C0").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__publishe__3213E83F736709D3");

            entity.ToTable("publisher");

            entity.HasIndex(e => e.Website, "UQ__publishe__2B1892FFB59A05F8").IsUnique();

            entity.HasIndex(e => e.Id, "UQ__publishe__3213E83EABCDF5A8").IsUnique();

            entity.HasIndex(e => e.Name, "UQ__publishe__72E12F1B965B1097").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__publishe__AB6E61641B9D45C2").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Website)
                .HasMaxLength(50)
                .HasColumnName("website");
        });

        modelBuilder.Entity<RatingsPerBook>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("RatingsPerBook");

            entity.Property(e => e.BlogPaperPodcast)
                .HasMaxLength(50)
                .HasColumnName("blog/paper/podcast");
            entity.Property(e => e.BookLanguage)
                .HasMaxLength(50)
                .HasColumnName("book_language");
            entity.Property(e => e.Genre)
                .HasMaxLength(50)
                .HasColumnName("genre");
            entity.Property(e => e.Isbn13)
                .HasMaxLength(17)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("isbn13");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.RatingComment)
                .HasMaxLength(255)
                .HasColumnName("rating_comment");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reviews__3213E83FEFBC36DF");

            entity.HasIndex(e => e.Id, "UQ__Reviews__3213E83E8D609B04").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BookIsbn13)
                .HasMaxLength(17)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("book_isbn13");
            entity.Property(e => e.Comment)
                .HasMaxLength(255)
                .HasColumnName("comment");
            entity.Property(e => e.Critic)
                .HasMaxLength(50)
                .HasColumnName("critic");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.ReviewDate).HasColumnName("review_date");

            entity.HasOne(d => d.BookIsbn13Navigation).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.BookIsbn13)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reviews__book_is__571DF1D5");
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Stores__3213E83F908D4244");

            entity.HasIndex(e => e.Id, "UQ__Stores__3213E83EFCAC666F").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<TitlarPerFörfattare>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("TitlarPerFörfattare");

            entity.Property(e => e.AuthorAge).HasColumnName("Author_Age");
            entity.Property(e => e.AuthorName)
                .HasMaxLength(101)
                .HasColumnName("Author_Name");
            entity.Property(e => e.AuthorTitles).HasColumnName("Author_Titles");
            entity.Property(e => e.InventoryValue)
                .HasColumnType("money")
                .HasColumnName("Inventory_Value");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

