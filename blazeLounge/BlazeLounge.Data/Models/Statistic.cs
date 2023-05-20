using System;
using System.Collections.Generic;

namespace BlazeLounge.Data.Models;

public partial class Statistic
{
    public long IdStatistics { get; set; }

    public long FkProfileidProfile { get; set; }

}
