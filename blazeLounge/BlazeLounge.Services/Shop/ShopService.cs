using BlazeLounge.Data.Models;
using BlazeLounge.Repositories.Shop;
using BlazeLounge.Repositories.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazeLounge.Services.Shop
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
