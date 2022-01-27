using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using AutoMapper;

using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

using Archer.AMA.BLL.Contracts;
using ArcherMobilApp.Controllers.Base;
using ArcherMobilApp.Models.Swagger;
using Archer.AMA.DTO;

namespace ArcherMobilApp.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]

    public class AppoinmentController : EditableWebApiController<AppoinmentDTO, int?, IAppoinmentService>
    {
        private readonly IMapper _mapper;
        private readonly IAppoinmentService _service;
        public AppoinmentController(IAppoinmentService service, IMapper mapper) : base(service)
        {
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Get appoinments 
        /// </summary>
        /// <returns>Return appoinments</returns>
        [HttpGet("")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(IEnumerable<AppoinmentDTO>), Description = "Return appoinments")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AppoinmentsModelExample))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, Description = "Unauthorized")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<AppoinmentDTO>>> Get()
        {
            return await base.All();
        }

        /// <summary>
        /// Get appoinment 
        /// </summary>
        /// <returns>Return appoinment by id</returns>
        [HttpGet("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(AppoinmentDTO), Description = "Return appoinment")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AppoinmentModelExample))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, Description = "Unauthorized")]
        [AllowAnonymous]
        public async Task<ActionResult<AppoinmentDTO>> Get(int id)
        {
            return await base.GetById(id);
        }

        /// <summary>
        /// Update appoinment 
        /// </summary>
        /// <returns>Status code</returns>
        [HttpPut]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(AppoinmentDTO), Description = "Update appoinment")]
        [SwaggerRequestExample(typeof(AppoinmentDTO), typeof(AppoinmentModelExample))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, Description = "Unauthorized")]
        [Authorize(Roles = "Admins")]
        public async Task<ActionResult> Put([FromBody]AppoinmentDTO model)
        {
            try {
                var result = await Update(model);
            }
            catch (Exception ex)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status403Forbidden,
                    Type = "",
                    Title = "Update appoinment.",
                    Detail = "The error occured while update appoinment",
                    Instance = HttpContext.Request.Path
                };
                return new ObjectResult(problemDetails)
                {
                    ContentTypes = { "application/problem+json" },
                    StatusCode = 403,
                };
            }
            return Ok();
        }

        /// <summary>
        /// Create appoinment 
        /// </summary>
        /// <returns>Status code</returns>
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.Created, Type = typeof(AppoinmentDTO), Description = "Create appoinment")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AppoinmentModelExample))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, Description = "Unauthorized")]
        //[Authorize(Roles = "Admins")]
        [AllowAnonymous]
        public async Task<ActionResult> Post([FromBody]AppoinmentDTO model)
        {
            try
            {
                var result = await Create(model);
            }
            catch (Exception ex)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status403Forbidden,
                    Type = "",
                    Title = "Create appoinment.",
                    Detail = "The error occured while create appoinment",
                    Instance = HttpContext.Request.Path
                };
                return new ObjectResult(problemDetails)
                {
                    ContentTypes = { "application/problem+json" },
                    StatusCode = 403,
                };
            }
            return new ObjectResult(model) { StatusCode = StatusCodes.Status201Created };
        }

        /// <summary>
        /// Remove appoinment
        /// </summary>
        /// <returns>Status code</returns>
        [HttpDelete]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Remove appoinment")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, Description = "Unauthorized")]
        [Authorize(Roles = "Admins")]
        public async Task<ActionResult> Delete([FromQuery]int id)
        {
            return Ok(await base.Delete(id));
        }
    }
}