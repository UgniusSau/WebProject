using System;
using System.Collections.Generic;

namespace BlazeLounge.Data.Models;

public partial class Badge
{
    public string? Url { get; set; }

    public long IdBadge { get; set; }

    public long FkItemidItem { get; set; }

}
