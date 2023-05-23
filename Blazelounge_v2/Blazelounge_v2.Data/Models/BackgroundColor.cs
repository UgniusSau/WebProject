namespace Blazelounge_v2.Data.Models;

public partial class BackgroundColor
{
    public string? RgbValue { get; set; }

    public long IdBackgroundColor { get; set; }

    public long FkItemidItem { get; set; }

}
