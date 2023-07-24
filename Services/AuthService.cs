using API.Data;
using API.Enums;
using API.Models;
using API.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Services
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _dataContext;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public AuthService(DataContext dataContext, IConfiguration configuration, UserManager<User> userManager, IMapper mapper, RoleManager<Role> roleManager)
        {
            _dataContext = dataContext;
            _configuration = configuration;
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
        }

        public async Task<string> GenerateToken(User user)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["JwtSetting:Key"]);
                var userRoles = await _userManager.GetRolesAsync(user);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName)
                };

                foreach (var claim in userRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, claim));
                }

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<HttpResponse<string>> Login(LoginRequest request)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(request.Username);
                if (user != null && await _userManager.CheckPasswordAsync(user, request.Password))
                {
                    var token = await GenerateToken(user);
                    if (token != null)
                    {
                        return new HttpResponse<string>
                        {
                            IsSuccess = true,
                            Code = Status.Success.GetName(),
                            Message = Success.LOGIN_SUCCESS.GetMessage(),
                            Data =  token 
                        };
                    }
                }

                return new HttpResponse<string>
                {
                    IsSuccess = false,
                    Code = Status.Failed.GetName(),
                    Message = Error.USERNAME_OR_PASSWORD_INVALID.GetMessage(),
                    Data = null
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new HttpResponse<string>
                {
                    IsSuccess = false,
                    Code = Status.Failed.GetName(),
                    Message = Status.InternalServerError.GetMessage(),
                    Data = null
                };
            }
        }

        public async Task<HttpResponse<UserResponse>> Signup(SignupRequest request)
        {
            try
            {
                if (!await _roleManager.RoleExistsAsync("user"))
                {
                    var userRole = new Role { Name = "user" };
                    await _roleManager.CreateAsync(userRole);
                    await _dataContext.SaveChangesAsync();
                }

                var user = _mapper.Map<User>(request);

                if (await _userManager.FindByNameAsync(request.Username) != null)
                {
                    return new HttpResponse<UserResponse>
                    {
                        IsSuccess = false,
                        Code = Status.Failed.GetName(),
                        Message = Error.USERNAME_EXIST.GetMessage(),
                    };
                }

                if (await _userManager.FindByEmailAsync(request.Email) != null)
                {
                    return new HttpResponse<UserResponse>
                    {
                        IsSuccess = false,
                        Code = Status.Failed.GetName(),
                        Message = Error.EMAIL_EXIST.GetMessage(),
                    };
                }

                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "user");
                    await _dataContext.SaveChangesAsync();
                    return new HttpResponse<UserResponse>
                    {
                        IsSuccess = true,
                        Code = Status.Success.GetName(),
                        Message = Success.SIGNUP_SUCCESS.GetMessage(),
                        Data =  _mapper.Map<UserResponse>(user) 
                    }; ;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return new HttpResponse<UserResponse>
            {
                IsSuccess = false,
                Code = Status.Failed.GetName(),
                Message = Status.InternalServerError.GetMessage(),
            };
        }
    }
}
