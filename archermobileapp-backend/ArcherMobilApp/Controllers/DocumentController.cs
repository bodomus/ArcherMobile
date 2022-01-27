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
    [Authorize]
    public class DocumentController : EditableWebApiController<DocumentDTO, int?, IDocumentService>
    {
        private readonly IMapper _mapper;
        private readonly IDocumentService _service;
        public DocumentController(IDocumentService service, IMapper mapper) : base(service)
        {
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Get documents 
        /// </summary>
        /// <returns>Return documents</returns>
        [HttpGet("")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(IEnumerable<DocumentDTO>), Description = "Return documents")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(DocumentModelExample))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, Description = "Unauthorized")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<DocumentDTO>>> Get()
        {
            return await base.All();
        }

        /// <summary>
        /// Get document 
        /// </summary>
        /// <returns>Return document by id</returns>
        [HttpGet("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DocumentDTO), Description = "Return document")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(DocumentModelExample))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, Description = "Unauthorized")]
        [AllowAnonymous]
        public async Task<ActionResult<DocumentDTO>> Get(int id)
        {
            return await base.GetById(id);
        }

        /// <summary>
        /// Update document 
        /// </summary>
        /// <returns>Status code</returns>
        [HttpPut]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DocumentDTO), Description = "Update document")]
        [SwaggerRequestExample(typeof(DocumentDTO), typeof(DocumentRequestModelExample))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, Description = "Unauthorized")]
        [Authorize(Roles = "Admins")]
        public async Task<ActionResult> Put([FromBody]DocumentDTO model)
        {
            try {
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
        /// Create document 
        /// </summary>
        /// <returns>Status code</returns>
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DocumentDTO), Description = "Create document")]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(DocumentModelExample))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, Description = "Unauthorized")]
        [Authorize(Roles = "Admins")]
        public async Task<ActionResult> Post([FromBody]DocumentDTO model)
        {
            return Ok(await Create(model));
        }

        /// <summary>
        /// Remove document
        /// </summary>
        /// <returns>Status code</returns>
        [HttpDelete]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Remove document")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Unexpected error")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, Description = "Unauthorized")]
        [Authorize(Roles = "Admins")]
        public async Task<ActionResult> Delete([FromQuery]int id)
        {
            return Ok(await base.Delete(id));
        }
    }
}