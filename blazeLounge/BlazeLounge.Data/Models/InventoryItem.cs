using System;
using System.Collections.Generic;

namespace BlazeLounge.Data.Models;

public partial class InventoryItem
{
    public long IdInventoryItems { get; set; }

    public long FkInventoryidInventory { get; set; }

    public long FkItemidItem { get; set; }

}
