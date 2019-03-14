using AutoMapper;
using FestoVideoStream.Dto;
using FestoVideoStream.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FestoVideoStream.Options;
using FestoVideoStream.Services;

namespace FestoVideoStream.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly UsersService _service;
        private readonly IMapper _mapper;

        public AuthController(UsersService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // POST: api/auth/login
        [HttpPost, Route("login")]
        public IActionResult Login([FromBody]UserDto user)
        {
            if (user == null)
            {
                return BadRequest("Invalid client request");
            }

            var identity = GetIdentity(user.Login, user.Password);
            if (identity == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            var tokenOptions = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: identity.Claims,
                expires: DateTime.Now.AddMinutes(AuthOptions.LIFETIME),
                signingCredentials: AuthOptions.SigningCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return Ok(new {Token = tokenString});
        }

        private ClaimsIdentity GetIdentity(string login, string password)
        {
            User user = _service.GetUser(login, password).Result;
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login)
                };
                ClaimsIdentity claimsIdentity =
                    new ClaimsIdentity(claims, "Token");
                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }
    }
}