using Blazelounge_v2.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Blazelounge_v2.Repositories
{
    public class ShopRepository : IShopRepository
    {

        private readonly DbContext2 _context;

        public ShopRepository(DbContext2 context)
        {
            _context = context;
        }

        public IQueryable<Item> GetShopItems()
        {
            return _context.Items;
        }

        public async Task<Item> GetItemById(string id)
        {
            return await _context.Items.SingleOrDefaultAsync(i => i.IdItem.ToString() == id);
        }

        public async Task<bool> BuyItem(string id, string username)
        {
            var item = await _context.Items.SingleOrDefaultAsync(i => i.IdItem.ToString() == id);
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);

            if (user != null && item != null)
            {
                var profile = await _context.Profiles.SingleOrDefaultAsync(p => p.IdProfile == user.FkProfileidProfile);

                if (profile != null)
                {
                    var inventory = await _context.Inventories.SingleOrDefaultAsync(i => i.FkProfileidProfile == profile.IdProfile);

                    if (inventory == null)
                    {
                        inventory = new Inventory
                        {
                            FkProfileidProfile = profile.IdProfile,
                        };

                        await _context.Inventories.AddAsync(inventory);
                        await _context.SaveChangesAsync();
                    }


                    var invItem = await _context.InventoryItems.SingleOrDefaultAsync(it => it.FkInventoryidInventory == inventory.IdInventory && it.FkItemidItem == item.IdItem);

                    if (invItem == null)
                    {
                        invItem = new InventoryItem
                        {
                            FkInventoryidInventory = inventory.IdInventory,
                            FkItemidItem = item.IdItem
                        };

                        await _context.InventoryItems.AddAsync(invItem);
                        profile.Currency -= item.Price;
                        await _context.SaveChangesAsync();
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
