namespace Blazelounge_v2.Data.Models;

public partial class Emote
{
    public string? Url { get; set; }

    public long IdEmote { get; set; }

    public long FkItemidItem { get; set; }

}
