using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NFC.Domain.Models.Identity;
using NFC.Application.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace NFC_Tag.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly IConfiguration _configuration;
		private readonly UserManager<ApplicationUser> userManager;
		

		public AccountController(UserManager<ApplicationUser> userManager,IConfiguration configuration)
		{
			_configuration = configuration;
			this.userManager = userManager;
			
		}
		[HttpPost("Register")]
		public async Task<IActionResult> Register(RegisterDTO UserDto)
		{
			if (ModelState.IsValid)
			{
				//user manger who save data so do inject to user manger
				ApplicationUser user = new ApplicationUser();//crat take appuser so we binding Dto in app user
				user.UserName = UserDto.UserName;

				IdentityResult result = await userManager.CreateAsync(user, UserDto.Password);
				if (result.Succeeded)
				{
					return Ok("account add");
				}
				return BadRequest(result.Errors.FirstOrDefault());
			}

			return BadRequest(ModelState);

		}
		[HttpPost("login")]
		public async Task<IActionResult> Login(LoginDTO userDto)
		{
			if (ModelState.IsValid == true)
			{
				ApplicationUser user = await userManager.FindByNameAsync(userDto.UserName);
				if (user != null)
				{
					bool found = await userManager.CheckPasswordAsync(user, userDto.Password);
					if (found)
					{
						//creat Tokien
						//Created claims
						var claims = new List<Claim>();
						claims.Add(new Claim(ClaimTypes.Name, user.UserName));
						claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
						claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
						//add role
						var roles = await userManager.GetRolesAsync(user);
						foreach (var itemrole in roles)
						{
							claims.Add(new Claim(ClaimTypes.Role, itemrole));
						}
						SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:secret"]));
						SigningCredentials signcread = new SigningCredentials(securityKey
							, SecurityAlgorithms.HmacSha256);
						//issur=>url for webApi;;make desigen for tokien (header/paylod/signuture);;audiance=>url for consumar (front)
						JwtSecurityToken Mytokien = new JwtSecurityToken(
				        	issuer: _configuration["jwt:ValidIssuer"],
				            audience: _configuration["jwt:ValidAudiance"],
					        claims: claims,
						    expires: DateTime.Now.AddHours(1),
					        signingCredentials: signcread

							);
						return Ok(new
						{
							Tokien = new JwtSecurityTokenHandler().WriteToken(Mytokien),
							expiration = Mytokien.ValidTo
						});
					}
				}
				return Unauthorized();
			}
			return Unauthorized();
		}
	}
}
