using Blazelounge_v2.Data.Models;

namespace Blazelounge_v2.Services
{
    public interface IUserService
    {
        IQueryable<User> GetUsers();
        Task<User> GetUserByUUID(string uuid);
        Task<User> GetUserByName(string username);
        Task AddUser(RegisterModel registerModel);
        Task<string?> AuthenticateUser(LoginModel loginModel, string tokenKey);
        Task<Profile> GetUserProfile(string id);
        Task<bool> ChangePassword(ChangePasswordModel changePasswordModel);
        Task<string?> ChangeBalance(UniversalGameModel gameModel, string? game);
        Task<bool> CheckBalance(UniversalGameModel gameModel);
        Task<IQueryable<Item>> GetUserItems(string username);
        Task<bool> ChangeUserColor(string username, Item item);
        Task<int> GetSpinValue(string username);
        Task<DateTime?> GetSpinTime(string username);
        Task<bool> UpdateSpin(string username);
        Task<IQueryable<GamesStat>> GetUserGamesStats(string username);
        Task<bool> UpdateUserGamesStats(UniversalGameModel model);

    }
}
