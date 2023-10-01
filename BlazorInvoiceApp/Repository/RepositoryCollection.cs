using AutoMapper;
using BlazorInvoiceApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BlazorInvoiceApp.Repository
{
    public class RepositoryCollection : IRepositoryCollection
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public IInvoiceRepository Invoice { get; private set; }
        public ICustomerRepository Customer { get; private set; }
        public IInvoiceTermsRepository InvoiceTerms { get; private set; }
        public IInvoiceLineItemRepository InvoiceLineItem { get; private set; }


        public RepositoryCollection(IDbContextFactory<ApplicationDbContext> dbFactory, IMapper mapper)
        {
            this.context = dbFactory.CreateDbContext();
            this.mapper = mapper;
            this.Invoice = new InvoiceRepository(context, mapper);
            this.Customer = new CustomerRepository(context, mapper);
            this.InvoiceTerms = new InvoiceTermsRepository(context, mapper);
            this.InvoiceLineItem = new InvoiceLineItemRepository(context, mapper);
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public async Task<int> Save()
        {
            try
            {
                return await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                foreach (EntityEntry item in ex.Entries)
                {
                    if (item.State == EntityState.Modified)
                    {
                        item.CurrentValues.SetValues(item.OriginalValues);
                        item.State = EntityState.Unchanged;
                        throw new RepositoryUpdateException();
                    }
                    else if (item.State == EntityState.Deleted)
                    {
                        item.State = EntityState.Unchanged;
                        throw new RepositoryDeleteException();
                    }
                    else if (item.State == EntityState.Added)
                    {
                        throw new RepositoryAddException();
                    }

                }
            }
            return 0;
        }

    }
}