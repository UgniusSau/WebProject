using Blazelounge_v2.Data.Models;
using Blazelounge_v2.Repositories;

namespace Blazelounge_v2.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _UserRepository;

        public UserService(IUserRepository dBUserRepository)
        {
            _UserRepository = dBUserRepository;
        }

        public IQueryable<User> GetUsersDB()
        {
            return _UserRepository.GetUsersDB();
        }

        public async Task<User> GetUserByUUID(string uuid)
        {
            return await _UserRepository.GetUserByUUID(uuid);
        }

        public async Task AddUserDB(User user)
        {
            await _UserRepository.AddUserDB(user);
        }

        public async Task<bool> AuthenticateUserDB(string username, string password)
        {
            return await _UserRepository.AuthenticateUserDB(username, password);
        }

        public async Task<User> GetUserByName(string username)
        {
            return await _UserRepository.GetUserByName(username);
        }

        public async Task<Profile> GetUserProfile(string id)
        {
            return await _UserRepository.GetUserProfile(id);
        }

        public async Task<bool> ChangePasswordDB(ChangePasswordModel changePasswordModel)
        {
            return await _UserRepository.ChangePasswordDB(changePasswordModel);
        }

		public async Task<bool> ChangeBalance(string username, bool addBalance, double amount, string? game)
		{
			return await _UserRepository.ChangeBalance(username, addBalance, amount, game);
		}

		public async Task<bool> CheckBalance(string username, double amount)
		{
			return await _UserRepository.CheckBalance(username, amount);
		}
        public Task<IQueryable<Item>> GetUserItems(string username)
        {
            return _UserRepository.GetUserItems(username);
        }

        public Task<bool> ChangeUserColor(string username, Item item)
        {
            return _UserRepository.ChangeUserColor(username, item);
        }
    

        public async Task<DateTime?> GetSpinTime(string username)
        {
            return await _UserRepository.GetSpinTime(username);
        }

        public async Task<bool> UpdateSpin(string username)
        {
            return await _UserRepository.UpdateSpin(username);
        }

        public async Task<IQueryable<GamesStat>> GetUserGamesStats(string username)
        {
            return await _UserRepository.GetUserGamesStats(username);
        }

        public async Task<bool> UpdateUserGamesStats(string username, string gameName, bool won, double amount)
        {
            return await _UserRepository.UpdateUserGamesStats(username, gameName, won, amount);
        }
    }
}
