using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BlazorInvoiceApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<InvoiceTerms> InvoiceTerms { get; set; }
        public DbSet<InvoiceLineItem> InvoicesLineItems { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {


        }

        protected void RemoveFixups(ModelBuilder modelBuilder, Type type)
        {
            foreach (var relationship in modelBuilder.Model.FindEntityType(type)!.GetForeignKeys())
            {
                relationship.DeleteBehavior = DeleteBehavior.ClientNoAction;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // customizations
            RemoveFixups(modelBuilder, typeof(Invoice));
            RemoveFixups(modelBuilder, typeof(InvoiceTerms));
            RemoveFixups(modelBuilder, typeof(Customer));
            RemoveFixups(modelBuilder, typeof(InvoiceLineItem));

            modelBuilder.Entity<Invoice>().Property(u => u.InvoiceNumber).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            modelBuilder.Entity<InvoiceLineItem>()
               .Property(u => u.TotalPrice)
               .HasComputedColumnSql("[UnitPrice] * [Quantity]");

            base.OnModelCreating(modelBuilder);
        }
    }
}