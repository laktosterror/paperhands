namespace paperhands.Model.Entities;

public class LanguagesLookup
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}