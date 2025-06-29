using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NFC.Domain.Models.Identity;
using NFC.Application.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web;
using NFC.Application.Interfaces.IServices;
namespace NFC_Tag.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly IConfiguration _configuration;
		private readonly UserManager<ApplicationUser> userManager;
		private readonly RoleManager<IdentityRole> roleManager;
		private readonly IEmailSender _emailSender;


        public AccountController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration , IEmailSender emailSender )
		{
			_configuration = configuration;
			this.userManager = userManager;
			this.roleManager = roleManager;
			_emailSender = emailSender;

		}
		[HttpPost("Register")]
		public async Task<IActionResult> Register(RegisterDTO UserDto)
		{
			if (ModelState.IsValid)
			{
				// إنشاء مستخدم جديد
				ApplicationUser user = new ApplicationUser
				{
					UserName = UserDto.UserName,
					Email= UserDto.Email
				};

				// إنشاء المستخدم
				IdentityResult result = await userManager.CreateAsync(user, UserDto.Password);
				if (result.Succeeded)
				{
					// التحقق من وجود الدور (لو مش موجود بننشئه)
					if (!await roleManager.RoleExistsAsync(UserDto.RoleName))
					{
						await roleManager.CreateAsync(new IdentityRole(UserDto.RoleName));
					}

					// إضافة المستخدم للدور المحدد
					await userManager.AddToRoleAsync(user, UserDto.RoleName);

					return Ok($"Account created and assigned to role: {UserDto.RoleName}");
				}

				// إرجاع أول خطأ لو فشل التسجيل
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
						    expires: userDto.RememberMe
							? DateTime.Now.AddDays(7)
                            : DateTime.Now.AddHours(1),
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

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var user = await userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return BadRequest("No user found with this email.");

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = HttpUtility.UrlEncode(token);

            var resetLink = $"https://touchgo-client/reset-password?email={dto.Email}&token={encodedToken}";

            await _emailSender.SendEmailAsync(
              dto.Email,
              "Reset Your Password",
             $"<p>Click the link below to reset your password:</p><a href=\"{resetLink}\">Reset Password</a>"
            );

            return Ok("Password reset link has been sent to your email.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                                       .SelectMany(v => v.Errors)
                                       .Select(e => e.ErrorMessage)
                                       .ToList();

                return BadRequest(new { Errors = errors });
            }

            var user = await userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return BadRequest("No user found with this email.");

            var decodedToken = HttpUtility.UrlDecode(dto.Token);

            var result = await userManager.ResetPasswordAsync(user, decodedToken, dto.NewPassword);

            if (!result.Succeeded)
            {
                var identityErrors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(new { Errors = identityErrors });
            }

            return Ok("Password has been reset successfully.");
        }
    }
}
