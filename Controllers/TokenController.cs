
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using todolist.Context;
using todolist.Models;

namespace todolist.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly AppDbContext _context;
        public IConfiguration _config;

        public TokenController(IConfiguration config, AppDbContext context)
        {
            this._context = context;
            this._config = config;
        }

        [HttpGet]
        public IActionResult Get()
        {
            User admin = new User();
            admin.UserName = "admin";
            admin.Password = "admin1";
            admin.Email = $"a@a.com";
            
            _context.Users.Add(admin);
            _context.SaveChanges();

            return Ok(new { result = admin});
        }

        [HttpPost]
        public IActionResult Post([FromBody] User userdata)
        {

            try
            {
                if (!string.IsNullOrEmpty(userdata.Email) && !string.IsNullOrEmpty(userdata.Password))
                {
                    var user = _context.Users
                    .FirstOrDefault(x => x.Email.Equals(userdata.Email) && x.Password.Equals(userdata.Password));

                    if (user != null)
                    {
                        var claims = new[]{
                            new Claim(JwtRegisteredClaimNames.Sub, _config["Jwt:Subject"]),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                            new Claim("username",user.UserName),
                            new Claim("email",user.Email),
                            new Claim("userid",user.Id.ToString()),
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                                                            _config["Jwt:Audience"],
                                                            claims,
                                                            expires: DateTime.UtcNow.AddMinutes(60),
                                                            signingCredentials: signIn);

                        return Ok( new{ token = new JwtSecurityTokenHandler().WriteToken(token)});


                    }
                }

                throw new Exception("Is not a valid user!.");
            }
            catch (System.Exception ex)
            {

                return BadRequest(new {message = ex.Message, token = ""});
            }
        }


    }
}