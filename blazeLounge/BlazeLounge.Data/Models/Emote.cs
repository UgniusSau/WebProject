using System;
using System.Collections.Generic;

namespace BlazeLounge.Data.Models;

public partial class Emote
{
    public string? Url { get; set; }

    public long IdEmote { get; set; }

    public long FkItemidItem { get; set; }

}
