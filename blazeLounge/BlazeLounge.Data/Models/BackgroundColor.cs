using System;
using System.Collections.Generic;

namespace BlazeLounge.Data.Models;

public partial class BackgroundColor
{
    public string? RgbValue { get; set; }

    public long IdBackgroundColor { get; set; }

    public long FkItemidItem { get; set; }

}
