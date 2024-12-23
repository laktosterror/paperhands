namespace paperhands.Model.Entities;

public partial class RatingsPerBook
{
    public string Isbn13 { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? BookLanguage { get; set; }

    public string Genre { get; set; } = null!;

    public int Rating { get; set; }

    public string? RatingComment { get; set; }

    public string? BlogPaperPodcast { get; set; }
}
