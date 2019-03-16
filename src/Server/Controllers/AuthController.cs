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
        /// <summary>
        /// The service.
        /// </summary>
        private readonly UsersService service;

        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="service">
        /// The service.
        /// </param>
        /// <param name="mapper">
        /// The mapper.
        /// </param>
        public AuthController(UsersService service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        /// POST: api/auth/login
        /// <summary>
        /// The login.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
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

            var identity = GetIdentity(user.Login, user.Password);
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

        /// <summary>
        /// The get identity.
        /// </summary>
        /// <param name="login">
        /// The login.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <returns>
        /// The <see cref="ClaimsIdentity"/>.
        /// </returns>
        private ClaimsIdentity GetIdentity(string login, string password)
        {
            var user = this.service.GetUser(login, password).Result;
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

            return null;
        }
    }
}