namespace Blazelounge_v2.Data.Models;

public partial class Badge
{
    public string? Url { get; set; }

    public long IdBadge { get; set; }

    public long FkItemidItem { get; set; }

}
