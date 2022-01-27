using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

using Archer.AMA.BLL.Contracts;
using ArcherMobilApp.Controllers.Base;
using ArcherMobilApp.Models.Swagger;
using Archer.AMA.DTO;
using System;
using Microsoft.AspNetCore.Http;

namespace ArcherMobilApp.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class JobOpportunityController : EditableWebApiController<JobOpportunityDTO, int?, IJobOpportunityService>
    {
        public JobOpportunityController(IJobOpportunityService service) : base(service)
        {
        }

        /// <summary>
        /// Get JobOpportunities 
        /// </summary>
        /// <returns>Return JobOpportunities</returns>
        [HttpGet("")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(IEnumerable<JobOpportunityDTO>), Description = "Return JobOpportunities")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(JobOpportunitiesModelExample))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, Description = "Unauthorized")]
        public async Task<ActionResult<IEnumerable<JobOpportunityDTO>>> Get()
        {
            return await base.All();
        }

        /// <summary>
        /// Get JobOpportunity
        /// </summary>
        /// <returns>Return JobOpportunity by id</returns>
        [HttpGet("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(JobOpportunityDTO), Description = "Return JobOpportunityDTO")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(DocumentModelExample))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, Description = "Unauthorized")]
        [AllowAnonymous]
        public async Task<ActionResult<JobOpportunityDTO>> Get(int id)
        {
            return await base.GetById(id);
        }

        /// <summary>
        /// Update JobOpportunity 
        /// </summary>
        /// <returns>Status code</returns>
        [HttpPut]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(JobOpportunityDTO), Description = "Update JobOpportunity")]
        [SwaggerRequestExample(typeof(DocumentDTO), typeof(JobOpportunityRequestModelExample))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, Description = "Unauthorized")]
        [Authorize(Roles = "Admins")]
        public async Task<ActionResult> Put([FromBody]JobOpportunityDTO model)
        {
            try
            {
                var result = await Update(model);
            }
            catch (Exception ex)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status403Forbidden,
                    Type = "https://example.com/probs/out-of-credit",
                    Title = "You do not have enough credit.",
                    Detail = "Your current balance is 30, but that costs 50.",
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
        /// Create JobOpportunity 
        /// </summary>
        /// <returns>Status code</returns>
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(JobOpportunityDTO), Description = "Create JobOpportunity")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(JobOpportunityRequestModelExample))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, Description = "Unauthorized")]
        [Authorize(Roles = "Admins")]
        public async Task<ActionResult> Post([FromBody]JobOpportunityDTO model)
        {
            return Ok(await Create(model));
        }

        /// <summary>
        /// Remove JobOpportunity
        /// </summary>
        /// <returns>Status code</returns>
        [HttpDelete]
        [SwaggerResponse((int)HttpStatusCode.NoContent, Description = "Remove JobOpportunity")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, Description = "Unauthorized")]
        [Authorize(Roles = "Admins")]
        public async Task<ActionResult<bool>> Delete([FromQuery]int id)
        {
            return await base.Delete(id);
        }
    }
}