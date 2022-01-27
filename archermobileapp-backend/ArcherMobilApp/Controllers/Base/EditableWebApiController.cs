

using Archer.AMA.BLL.Contract.Base;
using Archer.AMA.DTO.Base;
using Archer.AMA.Entity.Base;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ArcherMobilApp.Controllers.Base
{
    
    public abstract class EditableWebApiController<TDTO, TID, TService> : ReadonlyWebApiController<TDTO, TID, TService>
        where TService : IRepositoryService<TDTO, TID>
        where TDTO : DataTransferObjectBase<TID>
    {
        protected EditableWebApiController(TService service)
            : base(service)
        {
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        protected async virtual Task<ActionResult<TDTO>> Update([FromBody]TDTO dto)
        {

            return Ok(await Service.SaveAsync(dto, HttpContext.User.Identity.Name));
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        protected async virtual Task<ActionResult<TDTO>> Create([FromBody]TDTO dto)
        {
            ResetStateToNew(dto);
            return Created(string.Empty, await Service.SaveAsync(dto, HttpContext.User.Identity.Name));
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        protected async virtual Task<bool> Delete(TID id)
        {
            return await Service.DeleteAsync(id, HttpContext.User.Identity.Name);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        protected virtual void ResetStateToNew(TDTO dto)
        {

        }

        [ApiExplorerSettings(IgnoreApi = true)]
        protected async virtual Task<bool> Test(TID id)
        {
            return await Service.DeleteAsync(id, HttpContext.User.Identity.Name);
        }
    }
}
