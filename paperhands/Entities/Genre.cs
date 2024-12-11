using System;
using System.Collections.Generic;

namespace paperhands.Entities;

public partial class Genre
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
