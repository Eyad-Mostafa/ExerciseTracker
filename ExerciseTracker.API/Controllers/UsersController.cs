using ExerciseTracker.Core.Models;
using ExerciseTracker.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ExerciseTracker.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UsersController(JwtOptions jwtOptions, AppDbContext dbContext) : Controller
{
    //private readonly IUsersService _usersService;

    //public UsersController(IUsersService usersService)
    //{
    //    _usersService = usersService;
    //}

    [HttpPost]
    [Route("auth")]
    public ActionResult<string> AuthenticateUser(AuthenticationRequest request)
    {
        var user = dbContext.Set<User>().FirstOrDefault(x => x.Name == request.UserName &&
        x.Password == request.Password);

        if (user == null)
            return Unauthorized();

        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDiscriptor = new SecurityTokenDescriptor
        {
            Issuer = jwtOptions.Issuer,
            Audience = jwtOptions.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
            SecurityAlgorithms.HmacSha256),
            Subject = new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, request.UserName)
            })
        };

        var securityToken = tokenHandler.CreateToken(tokenDiscriptor);
        var accessToken = tokenHandler.WriteToken(securityToken);

        return Ok(accessToken);
    }
}
