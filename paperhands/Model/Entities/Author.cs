using System;
using System.Collections.Generic;

namespace paperhands.Model.Entities;

public partial class Author
{
    public long Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateTime? BirthDate { get; set; }

    public virtual ICollection<Book> BookIsbn13s { get; set; } = new List<Book>();
}
