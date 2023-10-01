using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorInvoiceApp.Data
{
    public class InvoiceLineItem : IEntity, IOwnedEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string InvoiceId { get; set; } = String.Empty;
        public Invoice? Invoice { get; set; } = null!;

        public string Description { get; set; } = String.Empty;
        public double UnitPrice { get; set; }
        public double Quantity { get; set; }
        public double TotalPrice { get; private set; }

        public string UserId { get; set; } = null!;
        public IdentityUser? User { get; set; } = null!;

    }
}
