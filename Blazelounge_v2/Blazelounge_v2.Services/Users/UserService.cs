using Blazelounge_v2.Data.Models;
using Blazelounge_v2.Repositories;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;


namespace Blazelounge_v2.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _UserRepository;


        public UserService(IUserRepository dBUserRepository)
        {
            _UserRepository = dBUserRepository;
        }

        public IQueryable<User> GetUsers()
        {
            return _UserRepository.GetUsers();
        }

        public async Task<User> GetUserByUUID(string uuid)
        {
            return await _UserRepository.GetUserByUUID(uuid);
        }

        public async Task AddUser(RegisterModel registerModel)
        {
            string password = registerModel.Password;
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                Username = registerModel.Username,
                Email = registerModel.Email,
                PasswordHashed = hashedPassword,
                Uuid = Guid.NewGuid().ToString(),
                FirstName = registerModel.First_Name,
                LastName = registerModel.Last_Name
            };

            await _UserRepository.AddUserDB(user);
        }

        public async Task<string?> AuthenticateUser(LoginModel loginModel, string tokenKey)
        {
            
            if(await _UserRepository.AuthenticateUser(loginModel.Username, loginModel.Password))
            {
                string token = CreateToken(loginModel, tokenKey);
                return token;
            }

            return null;
        }

        private string CreateToken(LoginModel loginModel, string tokenKey)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginModel.Username)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(tokenKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public async Task<User> GetUserByName(string username)
        {
            return await _UserRepository.GetUserByName(username);
        }

        public async Task<Profile> GetUserProfile(string id)
        {
            return await _UserRepository.GetUserProfile(id);
        }

        public async Task<bool> ChangePassword(ChangePasswordModel changePasswordModel)
        {
            string hashedNewPassword = BCrypt.Net.BCrypt.HashPassword(changePasswordModel.NewPassword);
            changePasswordModel.NewPassword = hashedNewPassword;

            return await _UserRepository.ChangePassword(changePasswordModel);
        }

		public async Task<string?> ChangeBalance(UniversalGameModel gameModel, string? game)
		{
            string wholeNumber = gameModel.Amount.Split('.')[0];
            
            if(await _UserRepository.ChangeBalance(gameModel.Username, gameModel.Won, double.Parse(wholeNumber), game))
            {
                var user = await GetUserByName(gameModel.Username);
                var profile = await GetUserProfile(user.FkProfileidProfile.ToString());
                return profile.Currency.ToString();
            }

            return null;
		}

		public async Task<bool> CheckBalance(UniversalGameModel gameModel)
		{
			return await _UserRepository.CheckBalance(gameModel.Username, Double.Parse(gameModel.Amount));
		}

        public Task<IQueryable<Item>> GetUserItems(string username)
        {
            return _UserRepository.GetUserItems(username);
        }

        public Task<bool> ChangeUserColor(string username, Item item)
        {
            return _UserRepository.ChangeUserColor(username, item);
        }
    

        public async Task<int> GetSpinValue(string username)
        {
            DateTime? lastSpin = await _UserRepository.GetSpinTime(username);

            List<int> values = new List<int>() { 100, 200, 500, 1000 };

            if (lastSpin == null)
            {
                await UpdateSpin(username);
                return values[Random.Shared.Next(values.Count)];
            }
            else
            {
                if (DateTime.UtcNow - lastSpin >= TimeSpan.FromHours(24))
                {
                    await UpdateSpin(username);
                    return values[Random.Shared.Next(values.Count)];
                }
            }

            return 0;
        }

        public async Task<DateTime?> GetSpinTime(string username)
        {
            DateTime? lastSpin = await _UserRepository.GetSpinTime(username);

            return lastSpin;
        }

        public async Task<bool> UpdateSpin(string username)
        {
            return await _UserRepository.UpdateSpin(username);
        }

        public async Task<IQueryable<GamesStat>> GetUserGamesStats(string username)
        {
            return await _UserRepository.GetUserGamesStats(username);
        }

        public async Task<bool> UpdateUserGamesStats(UniversalGameModel model)
        {
            string wholeNumber = model.Amount.Split('.')[0];
            return await _UserRepository.UpdateUserGamesStats(model.Username, model.GameName, model.Won, double.Parse(wholeNumber));
        }
    }
}
