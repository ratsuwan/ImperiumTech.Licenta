using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;


namespace MagazinWebLicenta.Server.Services.ServiciulAutentificari
{
	public class ServiciulAutentificari : IServiciulAutentificari
	{
		private readonly DataContext context;
		private readonly IConfiguration configuration;
		private readonly IHttpContextAccessor httpContextAccessor;

		public ServiciulAutentificari(DataContext context,
			IConfiguration configuration,
			IHttpContextAccessor httpContextAccessor)
        {
			this.context = context;
			this.configuration = configuration;
			this.httpContextAccessor = httpContextAccessor;
		}

		public int GetUserId() => int.Parse(this.httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

		public string GetUserEmail() => this.httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);

		public async Task<ServiceResponse<string>> Login(string email, string password)
		{
			var response = new ServiceResponse<string>();
			var user = await this.context.Users
				.FirstOrDefaultAsync(x => x.Email.ToLower().Equals(email.ToLower()));
			if (user == null) 
			{
				response.Success = false;
				response.Message = "Utilizatorul nu exista.";
			}
			else if(!VerifyPasswordHash(password, user.PasswordHash, user.PasswordHSalt))
			{
				response.Success = false;
				response.Message = "Parola este gresita. Inceraca din nou!";
			}
			else
			{
				response.Data = CreateToken(user);
			}

			
			return response;
		}

		public async Task<ServiceResponse<int>> Register(User user, string password)
		{
			if(await UserExists(user.Email))
			{
				return new ServiceResponse<int> 
				{ 
					Success = false,
					Message = "Acest cont este deja inregistrat cu mail-ul completat. Inceraca iar!" 
				};
			}

			CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

			user.PasswordHash = passwordHash;
			user.PasswordHSalt = passwordSalt;

			this.context.Users.Add(user);
			await this.context.SaveChangesAsync();

			return new ServiceResponse<int> { Data = user.Id, Message = "Contul a fost inregistrat cu succes!" };
		
		}

		public async Task<bool> UserExists(string email)
		{
			if(await this.context.Users.AnyAsync(user => user.Email.ToLower()
			.Equals(email.ToLower())))
			{
				return true;
			} 
			return false;
		}

		private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
		{
			using(var hmac = new HMACSHA512())
			{
				passwordSalt = hmac.Key;
				passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
			}

		}

		private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
		{
			using (var hmac = new HMACSHA512(passwordSalt))
			{
				var computedHash = 
					hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
				return computedHash.SequenceEqual(passwordHash);
			}
		}

		private string CreateToken(User user)
		{
			List<Claim> claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.Name, user.Email),
				new Claim(ClaimTypes.Role, user.Role)
			};

			var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
				.GetBytes(this.configuration.GetSection("AppSettings:Token").Value));

			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

			var token = new JwtSecurityToken(
				claims: claims,
				expires: DateTime.Now.AddDays(1),
				signingCredentials: creds);

			var jwt = new JwtSecurityTokenHandler().WriteToken(token);

			return jwt;
				
		}

		public async Task<ServiceResponse<bool>> ChangePassword(int userId, string newPassword)
		{
			var user = await this.context.Users.FindAsync(userId);
			if (user == null)
			{
				return new ServiceResponse<bool>
				{
					Success = false,
					Message = "Utilizatorul nu exista."
                };
			}

			CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);
			user.PasswordHash = passwordHash;
			user.PasswordHSalt= passwordSalt;

			await this.context.SaveChangesAsync();

			return new ServiceResponse<bool> { Data = true, Message = "Parola a fost schimbata cu succes."};
		}

		public async Task<User> GetUserByEmail(string email)
		{
			return await this.context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email));
		}
	}
}
