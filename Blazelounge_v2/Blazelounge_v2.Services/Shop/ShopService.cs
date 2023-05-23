using Blazelounge_v2.Data.Models;
using Blazelounge_v2.Repositories;

namespace Blazelounge_v2.Services
{
    public class ShopService : IShopService
    {
        private readonly IShopRepository _ShopRepository;

        public ShopService(IShopRepository shopRepository)
        {
            _ShopRepository = shopRepository;
        }

        public IQueryable<Item> GetShopItems()
        {
            return _ShopRepository.GetShopItems();
        }
        public async Task<Item> GetItemById(string id)
        {
            return await _ShopRepository.GetItemById(id);
        }

        public async Task<bool> BuyItem(string id, string username)
        {
            return await _ShopRepository.BuyItem(id, username);
        }
    }
}
