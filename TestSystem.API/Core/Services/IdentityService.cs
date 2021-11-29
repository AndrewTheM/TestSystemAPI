using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TestSystem.API.Core.DTO;
using TestSystem.API.Core.Entities;
using TestSystem.API.Repositories.Interfaces;
using TestSystem.API.Core.Services.Interfaces;

namespace TestSystem.API.Core.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public IdentityService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _uow = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AuthResponse> RegisterAsync(UserDto newUser)
        {
            var authResponse = new AuthResponse();
            var existingUser = await _uow.UserManager.FindByNameAsync(newUser.Username);

            if (existingUser != null)
            {
                authResponse.Errors = new[] { "User already exists" };
                return authResponse;
            }

            var userToRegister = _mapper.Map<User>(newUser);

            var createdUser = await _uow.UserManager.CreateAsync(userToRegister, newUser.Password);
            if (!createdUser.Succeeded)
            {
                authResponse.Errors = createdUser.Errors.Select(er => er.Description).ToArray();
                return authResponse;
            }

            authResponse.User = await MapUserWithTokenAsync(userToRegister);
            return authResponse;
        }

        public async Task<AuthResponse> LoginAsync(Credentials credentials)
        {
            var authResponse = new AuthResponse();
            var user = await _uow.UserManager.FindByNameAsync(credentials.Username);

            if (user == null)
            {
                authResponse.Errors = new[] { "User does not exist" };
                return authResponse;
            }

            bool passwordCorrect = await _uow.UserManager.CheckPasswordAsync(user, credentials.Password);
            if (!passwordCorrect)
            {
                authResponse.Errors = new[] { "Incorrect password" };
                return authResponse;
            }

            authResponse.User = await MapUserWithTokenAsync(user);
            return authResponse;
        }

        protected async Task<UserDto> MapUserWithTokenAsync(User user)
        {
            var token = await GenerateJwtToken(user);
            var userRoles = await _uow.UserManager.GetRolesAsync(user);

            return new UserDto
            {
                Id = user.Id,
                Username = user.UserName,
                Password = null,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = userRoles.FirstOrDefault() ?? "User",
                Token = token
            };
        }

        protected async Task<string> GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("Secret key for JWT tokens");

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("id", user.Id)
            };

            var userClaims = await _uow.UserManager.GetClaimsAsync(user);
            var userRoles = await _uow.UserManager.GetRolesAsync(user);
            claims.AddRange(userClaims);

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));

                var role = await _uow.RoleManager.FindByNameAsync(userRole);
                if (role == null)
                    continue;

                var roleClaims = await _uow.RoleManager.GetClaimsAsync(role);
                foreach (var roleClaim in roleClaims)
                {
                    if (claims.Contains(roleClaim))
                        continue;
                    claims.Add(roleClaim);
                }
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), 
                                                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task UpdateUserAsync(UserDto userDto)
        {
            var user = await _uow.UserManager.FindByIdAsync(userDto.Id);
            user.UserName = userDto.Username;
            user.Email = userDto.Email;
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            await _uow.UserManager.UpdateAsync(user);
        }

        public async Task<AuthResponse> ChangePasswordAsync(Credentials cred)
        {
            var authResponse = new AuthResponse();
            var user = await _uow.UserManager.FindByNameAsync(cred.Username);
            var token = await _uow.UserManager.GeneratePasswordResetTokenAsync(user);

            authResponse.User = _mapper.Map<UserDto>(user);

            var resetResult = await _uow.UserManager.ResetPasswordAsync(user, token, cred.Password);
            if (!resetResult.Succeeded)
            {
                authResponse.Errors = resetResult.Errors.Select(er => er.Description).ToArray();
                return authResponse;
            }
            return authResponse;
        }
    }
}
