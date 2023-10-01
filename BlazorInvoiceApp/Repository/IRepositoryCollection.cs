namespace BlazorInvoiceApp.Repository
{
    public interface IRepositoryCollection : IDisposable
    {
        IInvoiceRepository Invoice { get; }
        ICustomerRepository Customer { get; }
        IInvoiceTermsRepository InvoiceTerms { get; }
        IInvoiceLineItemRepository InvoiceLineItem { get; }

        Task<int> Save();
    }
}
