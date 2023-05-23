namespace Blazelounge_v2.Data.Models;

public partial class InventoryItem
{
    public long IdInventoryItems { get; set; }

    public long FkInventoryidInventory { get; set; }

    public long FkItemidItem { get; set; }

}
