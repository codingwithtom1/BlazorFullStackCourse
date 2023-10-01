using AutoMapper;
using BlazorInvoiceApp.Data;
using BlazorInvoiceApp.Pages.Components;
using BlazorInvoiceApp.Repository;
using Bunit;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Radzen;
using System.Security.Claims;

namespace BlazorInvoiceAppTests
{
    public class SetupCustomersComponentTests : TestContext
    {
        public SetupCustomersComponentTests()
        {
            SetupEnvironment();
        }
        private void SetupEnvironment()
        {
            Claim[] claims = {
                    new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "53de24b2-187c-4a67-adac-210534db81f3")
                };

            var authContext = this.AddTestAuthorization();
            authContext.SetAuthorized("tom@test.com");
            authContext.SetClaims(claims);
            JSInterop.SetupVoid("Radzen.preventArrows", _ => true);
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            Services.AddSingleton(mapper);
            Services.AddScoped<DialogService>();
            var mockDbFactory = new Mock<IDbContextFactory<ApplicationDbContext>>();
            mockDbFactory.Setup(f => f.CreateDbContext())
            .Returns(() => {
                var context = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=testdb;Trusted_Connection=True;MultipleActiveResultSets=true")
                .Options);
                InitData(context);
                return context;

            }
            );
            Services.AddSingleton(mockDbFactory.Object);
            Services.AddScoped<IRepositoryCollection, RepositoryCollection>();

        }

        [Fact]
        public void SetupCustomersComponentAddTest()
        {


            var cut = RenderComponent<CustomerSetupComponent>();

            cut.WaitForAssertion(() => cut.Find("span[title='Test Customer 1']"));
            cut.Find("span[title='Test Customer 2']");
            cut.Find("span[title='Test Customer 3']");

            // All initial customers rendered correctly
            // Try to add a customer

            var buttonElement = cut.Find("button");
            buttonElement.Click();
          
            var buttons = cut.FindAll("button");
            var input = cut.Find("input");
            input.Change("Test Customer 4");

            buttons[1].Click();
            cut.WaitForAssertion(() => cut.Find("span[title='Test Customer 4']"));
            cut.Find("span[title='Test Customer 1']");
            cut.Find("span[title='Test Customer 2']");
            cut.Find("span[title='Test Customer 3']");
            



        }


        [Fact]
        public void SetupCustomersComponentModifyTest()
        {


            var cut = RenderComponent<CustomerSetupComponent>();

            cut.WaitForAssertion(() => cut.Find("span[title='Test Customer 1']"));
            cut.Find("span[title='Test Customer 2']");
            cut.Find("span[title='Test Customer 3']");

            // All initial customers rendered correctly
            // Try to add a customer


            var buttons = cut.FindAll("button");
            buttons[1].Click();
            cut.WaitForElement("input");


            var input = cut.Find("input");
            input.Change("Test Customer 5");
            buttons = cut.FindAll("button");


            buttons[1].Click();
            cut.WaitForAssertion(() => cut.Find("span[title='Test Customer 5']"));
            cut.Find("span[title='Test Customer 2']");
            cut.Find("span[title='Test Customer 3']");


        }

        [Fact]
        public void SetupCustomersComponentDeleteTest()
        {
            //SetupEnvironment();

            var cut = RenderComponent<CustomerSetupComponent>();

            cut.WaitForAssertion(() => cut.Find("span[title='Test Customer 1']"));
            cut.Find("span[title='Test Customer 2']");
            cut.Find("span[title='Test Customer 3']");

            var buttons = cut.FindAll("button");
            buttons[2].Click();

            cut.WaitForAssertion(() => Assert.DoesNotContain("Test Customer 1", cut.Markup));
            cut.Find("span[title='Test Customer 2']");
            cut.Find("span[title='Test Customer 3']");
        }

        private void InitData(ApplicationDbContext context)
        {

            if (context != null)
            {
                context.Database.EnsureDeleted();
                context.Database.Migrate();

                context.SaveChanges();
                IdentityUser user = new IdentityUser
                {
                    Id = "53de24b2-187c-4a67-adac-210534db81f3",
                    UserName = "tom@test.com",
                    NormalizedUserName = "TOM@TEST.COM",
                    Email = "tom@test.com",
                    NormalizedEmail = "TOM@TEST.COM",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAIAAYagAAAAEDGgJMx0qDS+ecWG2+aeD5Rzz1KTKcx362EdaFzU79vYX9/J5RjRhO0MfBpJpfnGNw==",
                    SecurityStamp = "SEQ6T4L46XMQVNGZJTYK3YIHDHTC5N6R",
                    ConcurrencyStamp = "3b7e9372-cbea-40ee-97aa-67a9f91d5cc8",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                };
                context.Users.Add(user);

                Customer customer1 = new Customer
                {
                    Name = "Test Customer 1",
                    Id = "74739f26-264b-4f7a-96ee-e29dde9f6601",
                    UserId = "53de24b2-187c-4a67-adac-210534db81f3"

                };
                Customer customer2 = new Customer
                {
                    Name = "Test Customer 2",
                    Id = "74739f26-264b-4f7a-96ee-e29dde9f6602",
                    UserId = "53de24b2-187c-4a67-adac-210534db81f3"

                };
                Customer customer3 = new Customer
                {
                    Name = "Test Customer 3",
                    Id = "74739f26-264b-4f7a-96ee-e29dde9f6603",
                    UserId = "53de24b2-187c-4a67-adac-210534db81f3"

                };
                context.Customers.Add(customer1);
                context.Customers.Add(customer2);
                context.Customers.Add(customer3);


                context.SaveChanges();
            }
        }
    }
}