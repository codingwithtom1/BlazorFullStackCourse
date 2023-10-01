using AutoMapper;
using BlazorInvoiceApp.Data;
using BlazorInvoiceApp.DTOS;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlazorInvoiceApp.Repository
{
    public class InvoiceRepository : GenericOwnedRepository<Invoice, InvoiceDTO>, IInvoiceRepository
    {
        public InvoiceRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper) { }

        public async Task DeleteWithLineItems(ClaimsPrincipal User, string invoiceid)
        {
            string? userid = getMyUserId(User);
            var lineitems = await context.InvoicesLineItems.Where(i => i.InvoiceId == invoiceid && i.UserId == userid).ToListAsync();
            foreach (InvoiceLineItem lineitem in lineitems)
            {
                context.InvoicesLineItems.Remove(lineitem);
            }
            Invoice? invoice = await context.Invoices.Where(i => i.Id == invoiceid && i.UserId == userid).FirstOrDefaultAsync();
            if (invoice != null)
            {
                context.Invoices.Remove(invoice);
            }
        }

        public async Task<List<InvoiceDTO>> GetAllMineFlat(ClaimsPrincipal User)
        {
            string? userid = getMyUserId(User);
            var q = from i in context.Invoices.Where(i => i.UserId == userid).Include(i => i.InvoiceLineItems).Include(i => i.InvoiceTerms).Include(i => i.Customer)
                    select new InvoiceDTO
                    {
                        Id = i.Id,
                        CreateDate = i.CreateDate,
                        InvoiceNumber = i.InvoiceNumber,
                        Description = i.Description,
                        CustomerId = i.CustomerId,
                        CustomerName = i.Customer!.Name,
                        InvoiceTermsId = i.InvoiceTermsId,
                        InvoiceTermsName = i.InvoiceTerms!.Name,
                        Paid = i.Paid,
                        Credit = i.Credit,
                        TaxRate = i.TaxRate,
                        UserId = i.UserId,
                        InvoiceTotal = i.InvoiceLineItems.Sum(a => a.TotalPrice)
                    };


            List<InvoiceDTO>? results = await q.ToListAsync();
            return results;
        }
    }
}
