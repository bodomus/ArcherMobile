using Archer.AMA.BLL.Contract.Base;
using Archer.AMA.Core.Pramaneters;
using Archer.AMA.DTO.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ArcherMobilApp.Controllers.Base
{
    /// <summary>
    /// Provides base readonly functionality for API controller
    /// </summary>
    /// <typeparam name="TDTO"></typeparam>
    /// <typeparam name="TID"></typeparam>
    /// <typeparam name="TService"></typeparam>
    public abstract class ReadonlyWebApiController<TDTO, TID, TService> : ControllerBase
        where TService : IRepositoryService<TDTO, TID>
        where TDTO : DataTransferObjectBase<TID>
    {
        protected ReadonlyWebApiController(TService service)
        {
            Service = service;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async virtual Task<ActionResult<IEnumerable<TDTO>>> All()
        {
            return Ok(await Service.AllAsync(HttpContext.User.Identity.Name));
        }


        /// <summary>
        /// Asynchronously returns paginated entities from storage set.
        /// </summary>
        /// <param name="filterParams">Pagination params</param>
        [HttpGet("Paginated")]
        public virtual async Task<ActionResult<PaginatedItems<TDTO, TID>>> AllPaginatedAsync([FromQuery]FilterParams filterParams)
        {
            return Ok(await Service.AllPaginatedAsync(HttpContext.User.Identity.Name, filterParams));
        }

        protected TService Service { get; private set; }

        [ApiExplorerSettings(IgnoreApi = true)]
        
        public async virtual Task<ActionResult<TDTO>> GetById(TID id)
        {
            var result = await Service.GetByIdAsync(id, HttpContext.User.Identity.Name);

            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }
    }
}