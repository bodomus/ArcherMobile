using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

using Archer.AMA.BLL.Contracts;
using ArcherMobilApp.Controllers.Base;
using ArcherMobilApp.Models.Swagger;
using Microsoft.AspNetCore.Authorization;
using Archer.AMA.DTO;

namespace ArcherMobilApp.Controllers
{

    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    public class RoomController : EditableWebApiController<RoomDTO, int?, IRoomService>
    {
        private readonly IRoomService _service;
        public RoomController(IRoomService service) : base(service)
        {
            _service = service;
        }

        /// <summary>
        /// Get rooms 
        /// </summary>
        /// <returns>Return rooms</returns>
        [HttpGet("")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(IEnumerable<RoomDTO>), Description = "Return rooms")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(RoomModelExample))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, Description = "Unauthorized")]
        public async Task<ActionResult<IEnumerable<RoomDTO>>> Get()
        {
            return await base.All();
        }

        /// <summary>
        /// Get room 
        /// </summary>
        /// <returns>Return room</returns>
        [HttpGet("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(RoomDTO), Description = "Return room")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(RoomModelExample))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, Description = "Unauthorized")]
        public async Task<ActionResult<RoomDTO>> Get(int id)
        {
            return await base.GetById(id);
        }

        /// <summary>
        /// Update room 
        /// </summary>
        /// <returns>Status code</returns>
        [HttpPut]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Update room")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, Description = "Unauthorized")]
        [Authorize(Roles = "Admins")]
        public async Task<ActionResult> Put([FromBody] RoomDTO model)
        {
            return Ok(await Update(model));
        }

        /// <summary>
        /// Create room 
        /// </summary>
        /// <returns>Status code</returns>
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Create announcement")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, Description = "Unauthorized")]
        [Authorize(Roles = "Admins")]
        public async Task<ActionResult> Post([FromBody] RoomDTO model)
        {
            return Ok(await Create(model));
        }

        /// <summary>
        /// Remove room
        /// </summary>
        /// <returns>Status code</returns>
        [HttpDelete]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Remove room")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, Description = "Unauthorized")]
        [Authorize(Roles = "Admins")]
        public async Task<ActionResult> Delete([FromQuery]int id)
        {
            return Ok(await base.Delete(id));
        }
    }
}