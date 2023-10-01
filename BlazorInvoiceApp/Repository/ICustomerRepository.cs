using BlazorInvoiceApp.Data;
using BlazorInvoiceApp.DTOS;
using System.Security.Claims;

namespace BlazorInvoiceApp.Repository
{
    public interface ICustomerRepository : IGenericOwnedRepository<Customer,CustomerDTO>
    {
        public Task Seed(ClaimsPrincipal User);

    }
}
