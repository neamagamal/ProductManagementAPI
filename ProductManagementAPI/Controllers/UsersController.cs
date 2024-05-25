using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Product.BL;
using Product.DAL;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProductManagementAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{

    public readonly IConfiguration _Configuration;
    public readonly UserManager<Users> _UserManager;

    public UsersController(UserManager<Users> userManager, IConfiguration configuration)
    {

        _UserManager = userManager;
        _Configuration = configuration;
    }
    [HttpPost]
    [Route("Register")]
    public async Task<ActionResult> Register(RegisterDto registerDto)
    {

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var newUser = new Users()
        {
            UserName = registerDto.fistName + registerDto.lastName,
            firstName = registerDto.fistName,
            lastName = registerDto.fistName,
            Email = registerDto.emailAddress,
        };
        var creationResult = await _UserManager.CreateAsync(newUser, registerDto.password);
        if (!creationResult.Succeeded)
        {
            return BadRequest(creationResult.Errors);
        }
        var Claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, newUser.Id),
                new Claim(ClaimTypes.Role,"Manager"),
            };
        await _UserManager.AddClaimsAsync(newUser, Claims);
        return Ok();
    }
    [HttpPost]
    [Route("Login")]
    public async Task<ActionResult> Login(LoginDTO loginDTO)
    {
        var User = await _UserManager.FindByEmailAsync(loginDTO.emailAddress);
        if (User == null)
        {
            return BadRequest();
        }

        if (await _UserManager.IsLockedOutAsync(User))
        {
            return BadRequest("try again");
        }
        if (!await _UserManager.CheckPasswordAsync(User, loginDTO.password))
        {
            await _UserManager.AccessFailedAsync(User);
            return Unauthorized();
        }
        var UserClaims = await _UserManager.GetClaimsAsync(User);

        //generate Roles
        var roles = await _UserManager.GetRolesAsync(User);
        foreach (var role in roles)
        {
            UserClaims.Add(new Claim(ClaimTypes.Role, role));
        }
        //generate key
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Configuration["JWT : Secret"]));
        // generate Hashresult
        var methodUsedInGeneratingToken = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
        //Genete Token
        var jwt = new JwtSecurityToken(
            claims: UserClaims,
            expires: DateTime.Now.AddMinutes(_Configuration.GetValue<int>("TokenDuration")),
            notBefore: DateTime.Now,
            issuer: _Configuration["JWT : Issuer"],
            audience: _Configuration["JWT : Audience"],
            signingCredentials: methodUsedInGeneratingToken);

        var tokenHandler = new JwtSecurityTokenHandler();
        string tokenString = tokenHandler.WriteToken(jwt);
        return Ok(new
        {
            Token = tokenString,
            Expiry = DateTime.Now.AddMinutes(_Configuration.GetValue<int>("TokenDuration"))
        });
    }



}
