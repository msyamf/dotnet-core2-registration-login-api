using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using WebApi.Helpers;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization; 
using WebApi.Services;
using WebApi.Dtos;
using WebApi.Form;
using WebApi.Entities;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using WebApi.Helpers.Validation;

namespace WebApi.UserControllers
{

    
    [Authorize]
    [ApiController]
    [Route("user")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        private DataContext _context;
        internal static string error_message = "";
    

 
        public  AuthorizationFilterContext __;
        public UsersController(
            IUserService userService,
            IMapper mapper,
            IOptions<AppSettings> appSettings,
            DataContext context)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _context = context;
        }
 
        [HttpGet("me"), Authorize]
         public IActionResult Me()
        { 
            try
            {
                ClaimsPrincipal principal = User as ClaimsPrincipal;  
                string UserId=""; 
                if (null != principal)  
                {  
                    foreach (Claim claim in principal.Claims)  
                    {  
                        Console.WriteLine("CLAIM TYPE: " + claim.Type + "; CLAIM VALUE: " + claim.Value );  
                        if (claim.Type == "UserId")
                        {
                            UserId =  claim.Value;
                        }
                    }  
                }  
                var data_user = (
                    from user in _context.Users
                    select new {
                         user.Id ,
                         user.LastName,
                         user.FirstName,
                         user.Username,
                         user.Email,
                         });
                
                //_context.Users.Find(int.Parse(UserId));

                 
                return  Ok(new {UserId = data_user});
            }
            catch (Exception ex)
            {
             return Ok(ex);
            }
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Authenticate([FromBody] LoginForm login)
        {
            try{

                IDictionary<string, string> param_values, rules;
                param_values = new Dictionary<string, string>()
                {
                  {"username", login.username}, 
                  {"pasword", login.pasword}, 
                };

                rules = new Dictionary<string, string>()
                { 
                  {"username", "required"}, 
                  {"pasword", "required"}, 
                };

                string validate_msg = CustomValidation.validate(param_values, rules);
                if (validate_msg != "")
                {
                    error_message = validate_msg;
                    throw new Exception();
                }

                var user = _userService.Authenticate(login.username, login.pasword);

                if (user == null)
                {
                    return BadRequest(new { message = "Username or password is incorrect" });
                }

                var tokenHandler = new JwtSecurityTokenHandler();  
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] 
                    {
                        new Claim(ClaimTypes.Name, user.Id.ToString()),
                        new Claim(ClaimTypes.Email, user.Email.ToString()),
                        new Claim(ClaimTypes.Role, "user"),
                        new Claim("UserId", user.Id.ToString()),

                    }),

                    Expires = DateTime.UtcNow.AddDays(AppSettings.TIME_EXPIRES),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.SecretJWT)), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                // return basic user info (without password) and token to store client side
                return Ok(new {
                    Id = user.Id,
                    Username = user.Username,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Token = tokenString
                });
            }

            catch (Exception e){
                     return Ok(e);
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]UserRegister UserRegister)
        { 

            try 
            {

                IDictionary<string, string> param_values, rules;
                param_values = new Dictionary<string, string>()
                {
                  {"Username", UserRegister.Username}, 
                  {"Password", UserRegister.Password}, 
                  {"Email", UserRegister.Email}, 
                  {"FirstName", UserRegister.FirstName}, 
                  {"LastName", UserRegister.LastName},  
                };

                rules = new Dictionary<string, string>()
                { 
                  {"Username", "required"}, 
                  {"Password", "required"}, 
                  {"Email", "required"}, 
                  {"FirstName", "required"}, 
                  {"LastName", "required"},  
                };

                string validate_msg = CustomValidation.validate(param_values, rules);
                if (validate_msg != "")
                {
                    error_message = validate_msg;
                    throw new Exception();
                }

                var ChechEmail = _context.Users.Where(x => x.Email == UserRegister.Email || x.Username == UserRegister.Username).Count();

                if(ChechEmail > 0)
                {
                    return BadRequest(new { message = "Email atau Username Telah Digunakan" });
                }
                else
                { 
                    byte[] passwordHash, passwordSalt; 
                    using (var hmac = new System.Security.Cryptography.HMACSHA512())
                    {
                        passwordSalt = hmac.Key;
                        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(UserRegister.Password));
                        
                    };
            
                    _context.Users.Add(new User { 
                                    FirstName= UserRegister.FirstName,
                                    LastName= UserRegister.LastName,
                                    Username= UserRegister.Username,
                                    PasswordHash= passwordHash,
                                    PasswordSalt= passwordSalt,
                                    Email = UserRegister.Email,
                                    CreatedDate = DateTime.Now,
                                    UpdatedDate = DateTime.Now
                    });
                    _context.SaveChanges();

                    return Ok(new { message = "Registrasi Berhasil" });
                }
            } 
            catch(AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

      
 
       
        [HttpGet,Authorize]
        public IActionResult GetAll()
        {
            var users =  _userService.GetAll();
            var userDtos = _mapper.Map<IList<UserDto>>(users);
            return Ok(new {
                data = userDtos,
                message = "List all User",
                respon = true,
                meta = new {count = userDtos.Count}                
                });
        }

        [HttpGet("{id}"),Authorize]
        public IActionResult GetById(int id)
        {
            var user =  _userService.GetById(id);
            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }

        [HttpPut("{id}"), Authorize]
        public IActionResult Update(int id, [FromBody]UserDto userDto)
        {
            // map dto to entity and set id
            var user = _mapper.Map<User>(userDto);
            user.Id = id;

            try 
            {
                // save 
                _userService.Update(user, userDto.Password);
                return Ok(new { message = "User Telah Diupdate" });
            } 
            catch(AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}"), Authorize]
        public IActionResult Delete(int id)
        {
            _userService.Delete(id);
            return Ok(new { message = "User Telah Dihapus" });
        }
    }
}
