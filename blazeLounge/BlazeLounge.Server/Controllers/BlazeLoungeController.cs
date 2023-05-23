using BlazeLounge.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;
using BlazeLounge.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using BlazeLounge.Services.Shop;
using System.Globalization;

namespace BlazeLounge.Server.Controllers
{
    [ApiController]
    public class BlazeLoungeController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IShopService _shopService;
        private readonly IConfiguration _configuration;

        public BlazeLoungeController(IUserService userService, IShopService shopService, IConfiguration configuration)
        {
            _userService = userService;
            _shopService = shopService;
            _configuration = configuration;
        }

        [HttpGet("api/blaze-lounge-users-db")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersDB()
        {
            var result = await _userService.GetUsersDB().ToListAsync();

            if (!result.Any()) return NotFound();
            return Ok(result);
        }

        [HttpGet("api/blaze-lounge/user-UUID/{uuid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> GetUserByUUIDDB(string uuid)
        {
            var user = await _userService.GetUserByUUID(uuid);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet("api/blaze-lounge/user-name/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> GetUserByNameDB(string name)
        {
            var user = await _userService.GetUserByName(name);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet("api/blaze-lounge-profile/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Profile>> GetUserProfileDB(string id)
        {
            var profile = await _userService.GetUserProfile(id);

            if (profile == null)
            {
                return NotFound();
            }

            return Ok(profile);
        }

        


        [HttpPost("api/blaze-lounge-user/register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult> AddUserDB(RegisterModel registerModel)
        {
            try
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

                await _userService.AddUserDB(user);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest( ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to create user: {ex.Message}");
            }

        }


        [HttpPost("api/blaze-lounge-user/login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> AuthenticateUserDB(LoginModel loginModel)
        {
            try
            {
                if (await _userService.AuthenticateUserDB(loginModel.Username, loginModel.Password))
                {
                    string token = CreateToken(loginModel);
                    return Ok(token);
                }
                return BadRequest();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to login user: {ex.Message}");
            }

        }

        [HttpPost("api/blaze-lounge-user/change-pass")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> ChangePasswordDB(ChangePasswordModel changePasswordModel)
        {
            try
            {
                string hashedNewPassword = BCrypt.Net.BCrypt.HashPassword(changePasswordModel.NewPassword);
                changePasswordModel.NewPassword = hashedNewPassword;
                if (await _userService.ChangePasswordDB(changePasswordModel))
                {
                    return Ok(true);
                }
                return BadRequest(false);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to change password: {ex.Message}");
            }

        }

		[HttpPost("api/blaze-lounge-user/change-balance")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<string>> ChangeBalance(UniversalGameModel gameModel, string? game=null)
        {
			try
			{
                string wholeNumber = gameModel.Amount.Split('.')[0];

                if (await _userService.ChangeBalance(gameModel.Username, gameModel.Won, double.Parse(wholeNumber), game))
				{
                    var user = await _userService.GetUserByName(gameModel.Username);
                    var profile = await _userService.GetUserProfile(user.FkProfileidProfile.ToString());
                    return Ok(profile.Currency.ToString());
				}
				return BadRequest("Error occurred while changing balance");
			}
			catch (ArgumentException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return BadRequest($"Failed to change balance: {ex.Message}");
			}
		}

		[HttpPost("api/blaze-lounge-user/check-balance")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<bool>> CheckBalance(UniversalGameModel gameModel)
		{
			try
			{
                bool hasEnough = await _userService.CheckBalance(gameModel.Username, Double.Parse(gameModel.Amount));
                if (hasEnough)
                    return Ok(true);
				return BadRequest(false);
			}
			catch (ArgumentException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception ex)
			{
				return BadRequest($"Failed to check balance: {ex.Message}");
			}
		}

        [HttpGet("api/lucky-wheel/can-spin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> CanSpin(string userName)
        {
            DateTime? lastSpin = await _userService.GetSpinTime(userName);

            if(lastSpin == null)
            {
                // No previous spin
                return Ok(true);
            }
            else
            {
                return Ok(DateTime.UtcNow - lastSpin >= TimeSpan.FromHours(24));
            }

            return BadRequest("Failed to check CanSpin.");
        }

        [HttpGet("api/lucky-wheel/spin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> Spin(string userName)
        {
            DateTime? lastSpin = await _userService.GetSpinTime(userName);
            List<int> values = new List<int>() { 100, 200, 500, 1000 };

            if (lastSpin == null)
            {
                await _userService.UpdateSpin(userName);
                return Ok(values[Random.Shared.Next(values.Count)]);
            }
            else
            {
                if(DateTime.UtcNow - lastSpin >= TimeSpan.FromHours(24))
                {
                    await _userService.UpdateSpin(userName);
                    return Ok(values[Random.Shared.Next(values.Count)]);
                }
            }
            return Ok(0);
        }

        private string CreateToken(LoginModel loginModel)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginModel.Username)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        [HttpGet("api/blaze-lounge-shop/get-user-items/{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Item>>> GetUserItems(string username)
        {
            var result = await _userService.GetUserItems(username);

            if (!result.Any()) return NotFound();
            return Ok(result);
        }

        [HttpPost("api/blaze-lounge-shop/change-user-color/{item}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> ChangeUserColor(string username, Item item)
        {
            var result = await _userService.ChangeUserColor(username, item);

            if (!result) return NotFound(result);
            return Ok(result);
        }

        [HttpGet("api/blaze-lounge/get-user-game-stats/{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<GamesStat>>> GetUserGameStats(string username)
        {
            var result = await _userService.GetUserGamesStats(username);
            if (!result.Any()) return NotFound();
            return Ok(result);
        }

        [HttpPost("api/blaze-lounge/update-user-game-stats")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> UpdateUserGameStats(UniversalGameModel model)
        {
            string wholeNumber = model.Amount.Split('.')[0];

            var result = await _userService.UpdateUserGamesStats(model.Username, model.GameName, model.Won, double.Parse(wholeNumber));
            if (!result) return NotFound();
            return Ok(result);
        }


        //----------------------------------------- SHOP -----------------------------------------//

        [HttpGet("api/blaze-lounge-shop/get-shop-items")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Item>>> GetShopItems()
        {
            var result = await _shopService.GetShopItems().ToListAsync();

            if (!result.Any()) return NotFound();
            return Ok(result);
        }

        [HttpGet("api/blaze-lounge/shop/item/{itemId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Item>> GetItemById(string itemId)
        {
            var item = await _shopService.GetItemById(itemId);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost("api/blaze-lounge/shop/item/{itemId}/buy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> BuyItem(string itemId, string username)
        {
            try
            {
                if (await _shopService.BuyItem(itemId, username))
                    return Ok();
                return BadRequest();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to buy item: {ex.Message}");
            }
        }

    }

}
