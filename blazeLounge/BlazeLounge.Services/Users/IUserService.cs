using BlazeLounge.Data.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Linq;
using System.Threading.Tasks;

namespace BlazeLounge.Services
{
    public interface IUserService
    {
        //user ===================================================================
        IQueryable<User> GetUsersDB();
        Task<User> GetUserByUUID(string uuid);
        Task<User> GetUserByName(string username);
        Task AddUserDB(User user);
        Task<bool> AuthenticateUserDB(string username, string password);

        //profile ===============================================================
        Task<Profile> GetUserProfile(string id);
        Task<bool> ChangePasswordDB(ChangePasswordModel changePasswordModel);
        Task<bool> ChangeBalance(string username, bool addBalance, double amount, string? game);
        Task<bool> CheckBalance(string username, double amount);
        Task<IQueryable<Item>> GetUserItems(string username);
        Task<bool> ChangeUserColor(string username, Item item);
        Task<DateTime?> GetSpinTime(string username);
        Task<bool> UpdateSpin(string username);
        Task<IQueryable<GamesStat>> GetUserGamesStats(string username);
        Task<bool> UpdateUserGamesStats(string username, string gameName, bool won, double amount);

    }
}
