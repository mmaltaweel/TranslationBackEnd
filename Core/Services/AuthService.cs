using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Authentication;
using Core.DTO.RequestDTO;
using Core.DTO.ResponseDTO;
using Core.Enities.ProjectAggregate;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;


namespace Core.Services;


public class AuthService : IAuthService
{
   private readonly UserManager<User> userManager;
   private readonly RoleManager<IdentityRole> roleManager;
   private readonly IConfiguration _configuration;


   public AuthService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
   {
       this.userManager = userManager;
       this.roleManager = roleManager;
       _configuration = configuration;
   }


   public async Task<LoginResponse> LoginAsync(LoginModel model)
   {
       var user = await userManager.FindByNameAsync(model.Username);
       if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
       {
           var userRoles = await userManager.GetRolesAsync(user);

           var authClaims = new List<Claim>
           {
               new Claim(ClaimTypes.Name, user.UserName),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
               new Claim(ClaimTypes.NameIdentifier, user.Id),
           };

           foreach (var userRole in userRoles)
           {
               authClaims.Add(new Claim(ClaimTypes.Role, userRole));
           }

           var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

           var token = new JwtSecurityToken(
               issuer: _configuration["JWT:ValidIssuer"],
               audience: _configuration["JWT:ValidAudience"],
               expires: DateTime.Now.AddHours(3),
               claims: authClaims,
               signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
           );

           return (new LoginResponse
           {
               token = new JwtSecurityTokenHandler().WriteToken(token),
               expiration = token.ValidTo,
               authRole = userRoles.First(),
               userName = user.UserName,
           });
       }
       return null;
   }

   public async Task<bool> RegisterTranslatorAsync(RegisterModel model)
   {
       var userExists = await userManager.FindByNameAsync(model.Username);
       if (userExists != null) return false;


       User user = new User()
       {
           Email = model.Email,
           SecurityStamp = Guid.NewGuid().ToString(),
           UserName = model.Username,
           FirstName = model.FirstName,
           LastName = model.LastName,
           Role = UserRole.Translator
       };
       var result = await userManager.CreateAsync(user, model.Password);
       if (!result.Succeeded) return false;


       if (!await roleManager.RoleExistsAsync(UserRole.Translator.ToString()))
           await roleManager.CreateAsync(new IdentityRole(UserRole.Translator.ToString()));


       await userManager.AddToRoleAsync(user, UserRole.Translator.ToString());


       return true;
   }


   public async Task<bool> RegisterManagerAsync(RegisterModel model)
   {
       var userExists = await userManager.FindByNameAsync(model.Username);
       if (userExists != null) return false;


       User user = new User()
       {
           Email = model.Email,
           SecurityStamp = Guid.NewGuid().ToString(),
           UserName = model.Username,
           FirstName = model.FirstName,
           LastName = model.LastName,
           Role = UserRole.ProjectManager
       };
       var result = await userManager.CreateAsync(user, model.Password);
       if (!result.Succeeded) return false;


       if (!await roleManager.RoleExistsAsync(UserRole.ProjectManager.ToString()))
           await roleManager.CreateAsync(new IdentityRole(UserRole.ProjectManager.ToString()));


       if (!await roleManager.RoleExistsAsync(UserRole.Translator.ToString()))
           await roleManager.CreateAsync(new IdentityRole(UserRole.Translator.ToString()));


       await userManager.AddToRoleAsync(user, UserRole.ProjectManager.ToString());


       return true;
   }


   public async Task<List<UserRequest>> FetchUsersAsync()
   {
       return await userManager.Users
           .Where(x => x.Role == UserRole.Translator)
           .Select(x => new UserRequest
           {
               UserName = x.UserName,
               Id = x.Id,
               FirstName = x.FirstName
           }).ToListAsync();
   }
}

