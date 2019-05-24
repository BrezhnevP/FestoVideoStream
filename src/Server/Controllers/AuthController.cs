using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FestoVideoStream.Models.Dto;
using FestoVideoStream.Options;
using FestoVideoStream.Services;
using Microsoft.AspNetCore.Http;

namespace FestoVideoStream.Controllers
{
    /// <summary>
    /// The auth controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly UsersService usersService;

        private readonly IMapper mapper;

        public AuthController(UsersService usersService, IMapper mapper)
        {
            this.usersService = usersService;
            this.mapper = mapper;
        }

        /// POST: api/auth/login
        [HttpPost, Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Login([FromBody]UserDto user)
        {
            if (user == null)
            {
                return BadRequest("Invalid client request");
            }

            var identity = GetIdentity(user);
            if (identity == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = AuthOptions.SigningCredentials
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encodedToken = tokenHandler.WriteToken(token);

            return Ok(new {Token = encodedToken});
        }

        private ClaimsIdentity GetIdentity(UserDto user)
        {
            if (!this.usersService.CheckUser(user.Login, user.Password))
                return null;

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login)
            };
            var claimsIdentity = new ClaimsIdentity(claims, "Token");

            return claimsIdentity;
        }
    }
}