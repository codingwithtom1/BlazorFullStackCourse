using BlazorInvoiceApp.Data;
using BlazorInvoiceApp.DTOS;
using System.Security.Claims;

namespace BlazorInvoiceApp.Repository
{
    public interface IInvoiceRepository : IGenericOwnedRepository<Invoice, InvoiceDTO>
    {
        public Task<List<InvoiceDTO>> GetAllMineFlat(ClaimsPrincipal User);
        public Task DeleteWithLineItems(ClaimsPrincipal User, string invoiceid);

    }
}
