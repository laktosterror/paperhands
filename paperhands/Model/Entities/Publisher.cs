using System;
using System.Collections.Generic;

namespace paperhands.Model.Entities;

public partial class Publisher
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Website { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
