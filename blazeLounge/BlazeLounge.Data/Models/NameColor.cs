﻿using System;
using System.Collections.Generic;

namespace BlazeLounge.Data.Models;

public partial class NameColor
{
    public string? RgbValue { get; set; }

    public long? Style { get; set; }

    public long IdNameColor { get; set; }

    public long FkItemidItem { get; set; }

}
