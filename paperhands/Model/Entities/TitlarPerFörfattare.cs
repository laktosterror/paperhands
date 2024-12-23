namespace paperhands.Model.Entities;

public class TitlarPerFörfattare
{
    public string AuthorName { get; set; } = null!;

    public int? AuthorAge { get; set; }

    public int? AuthorTitles { get; set; }

    public decimal? InventoryValue { get; set; }
}