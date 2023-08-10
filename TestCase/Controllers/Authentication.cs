using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using TestCase.Dto;
using TestCase.Interface;
using TestCase.Models;

namespace TestCase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Authentication : ControllerBase
    {
        private readonly IUser _userRepo;
        private readonly IConfiguration _configuration;

        public Authentication(IUser userRepo, IConfiguration configuration)
        {
            _userRepo = userRepo;
            _configuration = configuration;
        }

        [HttpPost("Register")]
        public IActionResult Register(User user)
        {
            var email = _userRepo.GetAccountByEmail(user.Email);
            var phone = _userRepo.GetAccountByPhoneNumber(user.PhoneNumber);

            if (email != null)
            {
                return BadRequest("Email Already Exist!");

            }
            else if (phone != null)
            {
                return BadRequest("Phone Number Already Exist!");

            }
            else
            {
                string saltPassword = BCrypt.Net.BCrypt.GenerateSalt();
                string hashPassword = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash, saltPassword);

                var registerAccount = new User
                {
                    PasswordHash = hashPassword,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                };

                _userRepo.Register(registerAccount);
                return Ok("Register Berhasil");
            }
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginDto loginDto)
        {
            var checkAccount = _userRepo.GetAccountByEmail(loginDto.Email);

            if (checkAccount == null)
            {
                return BadRequest("Email Not Found!");
            }
            else
            {
                if (!BCrypt.Net.BCrypt.Verify(loginDto.PasswordHash, checkAccount.PasswordHash))
                {
                    return BadRequest("Wrong Password!");
                }
                string token = CreateToken(checkAccount);
                return Ok(token); // dari yang bwah
            }
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.
                GetBytes(_configuration.GetSection("AppSettings:Token").Value!));

            var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(5),
                signingCredentials: credential
                );

            var sendToken = new JwtSecurityTokenHandler().WriteToken(token);
            return sendToken;
        }

        }
    }
