namespace Blazelounge_v2.Data.Models;

public partial class Item
{
    public string Name { get; set; } = null!;

    public double Price { get; set; }

    public string? Description { get; set; }

    public long Type { get; set; }

    public long IdItem { get; set; }

    public long? FkShopidShop { get; set; }

}
