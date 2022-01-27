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
    public class AnnouncementController : EditableWebApiController<AnnouncementDTO, int?, IAnnouncementService>
    {
        private readonly IAnnouncementService _service;
        public AnnouncementController(IAnnouncementService service) : base(service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all announcements 
        /// </summary>
        /// <returns>Return announcements</returns>
        [HttpGet("")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(IEnumerable<AnnouncementDTO>), Description = "Return announcements")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AnnouncementsModelExample))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, Description = "Unauthorized")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AnnouncementDTO>>> Get()
        {
            return await base.All();
        }

        /// <summary>
        /// Get announcement 
        /// </summary>
        /// <returns>Return announcement</returns>
        [HttpGet("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(IEnumerable<AnnouncementDTO>), Description = "Return announcement")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AnnouncementsModelExample))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, Description = "Unauthorized")]
        [AllowAnonymous]
        public async Task<ActionResult<AnnouncementDTO>> Get(int id)
        {
            return await base.GetById(id);
        }
         
        /// <summary>
        /// Update announcement 
        /// </summary>
        /// <returns>Status code</returns>
        [HttpPut]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(AnnouncementDTO), Description = "Update announcement")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AnnouncementsModelExample))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, Description = "Unauthorized")]
        [Authorize(Roles = "Admins")]
        public async Task<ActionResult<AnnouncementDTO>> Put([FromBody] AnnouncementDTO model)
        {
            return await Update(model);
        }

        /// <summary>
        /// Create announcement 
        /// </summary>
        /// <returns>Status code</returns>
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(AnnouncementDTO), Description = "Create announcement")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AnnouncementsModelExample))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, Description = "Unauthorized")]
        [Authorize(Roles = "Admins")]
        public async Task<ActionResult> Post([FromBody] AnnouncementDTO model)
        {
            return Ok(await Create(model));
        }

        /// <summary>
        /// Remove announcement
        /// </summary>
        /// <returns>Status code</returns>
        [HttpDelete]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Remove announcement")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, Description = "Unauthorized")]
        [Authorize(Roles = "Admins")]
        public async Task<ActionResult> Delete([FromQuery]int id)
        {
            return Ok(await base.Delete(id));
        }
    }
}