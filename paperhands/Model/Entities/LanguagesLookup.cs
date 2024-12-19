using System;
using System.Collections.Generic;

namespace paperhands.Model.Entities;

public partial class LanguagesLookup
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
