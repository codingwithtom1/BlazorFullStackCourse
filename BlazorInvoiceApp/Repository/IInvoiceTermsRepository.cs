using BlazorInvoiceApp.Data;
using BlazorInvoiceApp.DTOS;
using System.Security.Claims;

namespace BlazorInvoiceApp.Repository
{
    public interface IInvoiceTermsRepository : IGenericOwnedRepository<InvoiceTerms, InvoiceTermsDTO>
    {
        public Task Seed(ClaimsPrincipal User);
    }
}
