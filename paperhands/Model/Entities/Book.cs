using System;
using System.Collections.Generic;

namespace paperhands.Model.Entities;

public partial class Book
{
    public string Isbn13 { get; set; } = null!;

    public string Title { get; set; } = null!;

    public long? LanguageId { get; set; }

    public decimal? Price { get; set; }

    public DateTime? PublishDate { get; set; }

    public long? GenreId { get; set; }

    public long? PublisherId { get; set; }

    public virtual Genre? Genre { get; set; }

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual LanguagesLookup? Language { get; set; }

    public virtual Publisher? Publisher { get; set; }

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Author> Authors { get; set; } = new List<Author>();
}
