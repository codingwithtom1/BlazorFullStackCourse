using BlazorInvoiceApp.Data;
using BlazorInvoiceApp.DTOS;
using System.Security.Claims;

namespace BlazorInvoiceApp.Repository
{
    public interface IInvoiceLineItemRepository : IGenericOwnedRepository<InvoiceLineItem, InvoiceLineItemDTO>
    {
        public Task<List<InvoiceLineItemDTO>> GetAllByInvoiceId(ClaimsPrincipal User, string invoiceid);
    }
}
