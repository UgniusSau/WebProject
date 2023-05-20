using System;
using System.Collections.Generic;

namespace BlazeLounge.Data.Models;

public partial class Inventory
{
    public long IdInventory { get; set; }

    public long FkProfileidProfile { get; set; }

}
