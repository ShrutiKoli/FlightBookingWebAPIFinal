
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using UserManagementService.Model;

namespace UserManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User();
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AuthController(IConfiguration configuration, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _configuration = configuration;
            _userManager=userManager;
            _roleManager=roleManager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExist = await _userManager.FindByNameAsync(model.UserName);
            if (userExist != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response {Status="Error",Message="User alredy exists" });
            ApplicationUser user = new ApplicationUser
            {
                 UserName = model.UserName,
                 Email = model.Email,
                 SecurityStamp=Guid.NewGuid().ToString()
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if(!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User Creation Failed" });
            if (!await _roleManager.RoleExistsAsync(UserRole.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRole.Admin));
            if (!await _roleManager.RoleExistsAsync(UserRole.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRole.User));
            if (await _roleManager.RoleExistsAsync(UserRole.User))
            {
                await _userManager.AddToRoleAsync(user, UserRole.User);
            }
            return Ok(new Response { Status = "Success", Message = "User Created Successfullly" });
        }


        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExist = await _userManager.FindByNameAsync(model.UserName);
            if (userExist != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Admin alredy exists" });
            ApplicationUser user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Admin Creation Failed" });
            if (!await _roleManager.RoleExistsAsync(UserRole.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRole.Admin));
            if (!await _roleManager.RoleExistsAsync(UserRole.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRole.User));
            if (await _roleManager.RoleExistsAsync(UserRole.Admin))
            {
                await _userManager.AddToRoleAsync(user,UserRole.Admin);
            }
                return Ok(new Response { Status = "Success", Message = "Admin Created Successfullly" });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user= await _userManager.FindByNameAsync(model.UserName);
            if(user!=null && await _userManager.CheckPasswordAsync(user,model.Password))
            {
                var userRoles=await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name,user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())

                };
                foreach(var userrole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role,userrole));
                }
                var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
                var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
                var token = new JwtSecurityToken(
                    claims: authClaims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: cred);
                return Ok(new
                {
                    token =  new JwtSecurityTokenHandler().WriteToken(token),
                    role = userRoles[0]
            
            });
            }
            return Unauthorized(new Response { Status = "Error", Message = "Invalid Credentials" });  
        }
        
       
        
        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken()
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Created = DateTime.Now,
                Expires = DateTime.Now.AddDays(2)

            };
            return refreshToken;
        }
        private void SetRefreshToken(RefreshToken refreshToken)
        {
            var cookiesOption = new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshToken.Expires
            };
            Response.Cookies.Append("refreshToken", refreshToken.Token, cookiesOption);
            user.RefreshToken = refreshToken.Token;
            user.TokenCreated = refreshToken.Created;
            user.TokenExpires = refreshToken.Expires;
        }
        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            var refreshtoken = Request.Cookies["refreshToken"];
            if (!user.RefreshToken.Equals(refreshtoken))
            {
                return Unauthorized("Invalid refresh token");
            }
            else if (user.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token Expired");
            }
            string token = CreateToken(user);
            var newrefreshToken = GenerateRefreshToken();
            SetRefreshToken(newrefreshToken);

            return Ok(token);

        }
        private bool VerifypasswordHash(string password, byte[] passwordHash, byte[] passwordsalt)
        {
            using (var hmac = new HMACSHA512(passwordsalt))
            {
                var computedhash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedhash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role,user.Role)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;

        }
    }
}
