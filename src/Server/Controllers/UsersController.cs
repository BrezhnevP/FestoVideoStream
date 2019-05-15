using AutoMapper;
using FestoVideoStream.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using FestoVideoStream.Models.Dto;
using FestoVideoStream.Models.Entities;

namespace FestoVideoStream.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// The users controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        /// <summary>
        /// The users service.
        /// </summary>
        private readonly UsersService usersService;

        /// <summary>
        /// The mapper.
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="usersService">
        /// The users service.
        /// </param>
        /// <param name="mapper">
        /// The mapper.
        /// </param>
        public UsersController(UsersService usersService, IMapper mapper)
        {
            this.usersService = usersService;
            this.mapper = mapper;
        }

        /// GET: api/users
        /// <summary>
        /// The get users.
        /// </summary>
        /// <returns>
        /// The <see cref="IActionResult"/>.
        /// </returns>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetUsers()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var users = this.usersService.GetUsers().Select(d => this.mapper.Map<UserDto>(d));

            return Ok(users);
        }

        /// GET: api/Users/5
        /// <summary>
        /// The get user.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = this.mapper.Map<UserDto>(await this.usersService.GetUser(id));

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        /// PATCH: api/Users/5
        /// <summary>
        /// The patch user.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Authorize]
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PatchUser([FromRoute] Guid id, [FromBody] UserDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.Id)
            {
                return BadRequest();
            }

            var result = await this.usersService.UpdateUser(id, this.mapper.Map<User>(user));

            if (result == null)
            {
                BadRequest();
            }

            return Ok(result);
        }

        /// POST: api/Users
        /// <summary>
        /// The post user.
        /// </summary>
        /// <param name="userToCreate">
        /// The user to create.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostUser([FromBody] UserDto userToCreate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdUser = await this.usersService.CreateUser(this.mapper.Map<User>(userToCreate));

            return CreatedAtAction("GetUser", new { createdUser.Id }, createdUser);
        }

        /// DELETE: api/Users/5
        /// <summary>
        /// The delete user.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await usersService.DeleteUser(id);
            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}