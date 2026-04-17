namespace SemiologyAtlas.ViewModels;

public sealed class FilterChip : BaseViewModel
{
    private bool isSelected;

    public FilterChip(string label, string value, bool isSelected = false)
    {
        Label = label;
        Value = value;
        this.isSelected = isSelected;
    }

    public string Label { get; }

    public string Value { get; }

    public bool IsSelected
    {
        get => isSelected;
        set => SetProperty(ref isSelected, value);
    }
}
