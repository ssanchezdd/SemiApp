namespace SemiologyAtlas.Models;

public sealed class ClinicalPoint
{
    public ClinicalPoint(string title, string detail)
    {
        Title = title;
        Detail = detail;
    }

    public string Title { get; }

    public string Detail { get; }
}
