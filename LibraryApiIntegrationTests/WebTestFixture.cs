using LibraryApi;
using LibraryApi.Domain;
using LibraryApi.Services;
using LibraryApiIntegrationTests.Fakes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace LibraryApiIntegrationTests
{
    public class WebTestFixture : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var systemTimeDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(ISystemTime));
                // todo.
                //check if isystemtime is set up as service
                if (systemTimeDescriptor != null)
                {
                    services.Remove(systemTimeDescriptor);
                    services.AddTransient<ISystemTime, FakeSystemTime>();
                }

                var dbContextOptionsDescriptor = services.SingleOrDefault(
                   d => d.ServiceType == typeof(DbContextOptions<LibraryDataContext>)
                   );

                if (dbContextOptionsDescriptor != null)
                {
                    services.Remove(dbContextOptionsDescriptor);
                    services.AddDbContext<LibraryDataContext>(options =>
                        options.UseInMemoryDatabase(Guid.NewGuid().ToString())
                    );

                }
                //if it is, replace it with folgers
            });
        }
    }
}