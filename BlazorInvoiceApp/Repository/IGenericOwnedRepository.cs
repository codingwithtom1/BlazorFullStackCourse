using BlazorInvoiceApp.Data;
using BlazorInvoiceApp.DTOS;
using System.Security.Claims;

namespace BlazorInvoiceApp.Repository
{
    public interface IGenericOwnedRepository<TEntity, TDTO>
        where TEntity : class, IEntity, IOwnedEntity
        where TDTO : class, IDTO, IOwnedDTO
    {
        Task<TDTO> GetMineById(ClaimsPrincipal User, string id);
        Task<List<TDTO>> GetAllMine(ClaimsPrincipal User);
        Task<string> AddMine(ClaimsPrincipal User, TDTO dto);
        Task<TDTO> UpdateMine(ClaimsPrincipal User, TDTO dto);
        Task<bool> DeleteMine(ClaimsPrincipal User, string id);

    }
}
