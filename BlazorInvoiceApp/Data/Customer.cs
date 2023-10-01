using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorInvoiceApp.Data
{
    public class Customer : IEntity, IOwnedEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string UserId { get; set; } = null!;
        public IdentityUser? User { get; set; } = null!;


        public string Name { get; set; } = String.Empty;
    }
}
