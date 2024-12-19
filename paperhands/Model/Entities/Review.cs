using System;
using System.Collections.Generic;

namespace paperhands.Model.Entities;

public partial class Review
{
    public long Id { get; set; }

    public string BookIsbn13 { get; set; } = null!;

    public int Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime? ReviewDate { get; set; }

    public string? Critic { get; set; }

    public virtual Book BookIsbn13Navigation { get; set; } = null!;
}
