using LibraryApi.DTOs;
using LibraryApi.Models;
using LibraryApi.Services;
using LibraryApi.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(ITokenService tokenService, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IConfiguration configuration, ILogger<AuthController> logger)
        {
            _tokenService = tokenService;
            _roleManager = roleManager;
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName!);
            if (user is not null && await _userManager.CheckPasswordAsync(user, model.Password!))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.UserName!),
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim("id",user.UserName!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = _tokenService.GenerateAccessToken(authClaims, _configuration);

                var refreshToken = _tokenService.GenerateRefreshToken();

                _ = int.TryParse(_configuration["Jwt:RefreshTokenValidityInMinutes"],
                                              out int refreshTokenValidityInMinutes);

                user.RefreshTokenExpiryTime =
                        DateTime.Now.AddMinutes(refreshTokenValidityInMinutes);

                user.RefreshToken = refreshToken;

                await _userManager.UpdateAsync(user);

                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken,
                    Expiration = token.ValidTo
                });
            }

            return NotFound("Unregistered user");
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var emailExists = await _userManager.FindByEmailAsync(model.Email!);
            if(emailExists != null)
            {
                _logger.LogInformation(1, "User email exists!");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response { Status = "Error", Message = "User email exists!" });
            }

            if (!PasswordValidation.ValidatePassword(model.Password))
            {
                _logger.LogInformation(2, "The password must contain uppercase letters, lowercase letters, numbers and special characters!");
                return BadRequest("The password must contain uppercase letters, lowercase letters, numbers and special characters");
            };
   
            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password!);

            if (!result.Succeeded)
            {
                _logger.LogInformation(3, "User creation Failed!");
                return StatusCode(StatusCodes.Status500InternalServerError,
                        new Response { Status = "Error", Message = "User creation Failed" });
            }

            _logger.LogInformation(4, "User created successfully!");
            return Ok(new Response { Status = "Success", Message = "User created successfully" });
        }

        [HttpPost]
        [Route("refresh-token")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            if (tokenModel == null)
                
            {
                _logger.LogInformation(1,"Invalid client request!");
                return BadRequest("Invalid client request");
            }

            string? accessToken = tokenModel.AccessToken
                        ?? throw new ArgumentNullException(nameof(tokenModel));

            string? refreshToken = tokenModel.RefreshToken
                        ?? throw new ArgumentNullException(nameof(tokenModel));

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken!, _configuration);

            if (principal == null)
            {
                _logger.LogInformation(2, "Invalid access token/refresh token!");
                return BadRequest("Invalid access token/refresh token");
            }

            string username = principal.Identity.Name;

            var user = await _userManager.FindByNameAsync(username!);

            if (user == null || user.RefreshToken != refreshToken
                             || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                _logger.LogInformation(3, "Invalid access token/refresh token");
                return BadRequest("Invalid access token/refresh token");
            }

            var newAccessToken = _tokenService.GenerateAccessToken(
                                                principal.Claims.ToList(), _configuration);

            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;

            await _userManager.UpdateAsync(user);

            _logger.LogInformation(4, "Token refreshed");

            return new ObjectResult(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                refreshToken = newRefreshToken
            });
        }

        [Authorize]
        [HttpPost]
        [Route("revoke/{username}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Revoke(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                _logger.LogInformation(1, "Invalid username");
                return BadRequest("Invalid username");
            }

            user.RefreshToken = null;

            await _userManager.UpdateAsync(user);

            _logger.LogInformation(2, "Revoked");
            return NoContent();
        }

        [HttpPost]
        [Route("CreateRole")]
        [Authorize(Policy = "SuperAdminOnly")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExists)
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));

                if (roleResult.Succeeded)
                {
                    _logger.LogInformation(1, "Roles Added");
                    return StatusCode(StatusCodes.Status200OK,
                        new Response { Status = "Success", Message = $"Role {roleName} added successfully" });
                }
                else
                {
                    _logger.LogInformation(2, "Error");
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new Response { Status = "Error", Message = $"Issue adding the new {roleName} role" });
                }
            }

            _logger.LogInformation(3, "Role already exist");
            return StatusCode(StatusCodes.Status400BadRequest,
                new Response { Status = "Error", Message = "Role already exist" });
        }

        [HttpPost]
        [Route("AddUserToRole")]
        [Authorize(Policy = "SuperAdminOnly")]

        public async Task<IActionResult> AddUserToRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _userManager.AddToRoleAsync(user, roleName);

                if (result.Succeeded)
                {
                    _logger.LogInformation(1, $"User added to the {roleName} role");
                    return StatusCode(StatusCodes.Status200OK,
                        new Response { Status = "Success", Message = $"User added to the {roleName} role" });
                }
                else
                {
                    _logger.LogInformation(2, $"Error: Unable to add user to the {roleName} role");
                    return StatusCode(StatusCodes.Status400BadRequest, new Response
                    {
                        Status = "Error",
                        Message =
                        $"Error: Unable to add user to the {roleName} role"
                    });
                }
            }

            _logger.LogInformation(3, $"Unable to find user");
            return BadRequest(new { error = "Unable to find user" });
        }

        [HttpDelete("delete/user/{email}")]
        [Authorize(Policy = "SuperAdminOnly")]

        public async Task<IActionResult> DeleteUser(string email)
        {

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogError(1,"User not found");
                return NotFound(new { message = "User not found" });
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                _logger.LogError(2,"Error deleting user");
                return StatusCode(500, new
                {
                    message = "Error deleting user",
                    errors = result.Errors.Select(e => e.Description)
                });
            }

            _logger.LogInformation(3,"User deleted");
            return Ok(new { message = "User deleted" });
        }

        [HttpDelete]
        [Route("DeleteRole")]
        [Authorize(Policy = "SuperAdminOnly")]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                _logger.LogInformation(1,"Role not found");
                return NotFound(new { message = "Role not found" });
            }
                
            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded) {
                _logger.LogInformation(2, "Role deleted");
                return Ok($"Role '{roleName}' deleted");
            }
            else
                return BadRequest(result.Errors);
        }
    }
}

