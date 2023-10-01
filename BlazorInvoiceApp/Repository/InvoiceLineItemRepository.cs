using AutoMapper;
using BlazorInvoiceApp.Data;
using BlazorInvoiceApp.DTOS;
using System.Security.Claims;

namespace BlazorInvoiceApp.Repository
{
    public class InvoiceLineItemRepository : GenericOwnedRepository<InvoiceLineItem, InvoiceLineItemDTO>, IInvoiceLineItemRepository
    {
        public InvoiceLineItemRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper) { }

        public async Task<List<InvoiceLineItemDTO>> GetAllByInvoiceId(ClaimsPrincipal User, string invoiceid)
        {
            return await GenericQuery(User, l => l.InvoiceId == invoiceid, null!);
        }
    }
}
