

using MyGuitarShop.Common.DTOs;
using MyGuitarShop.Common.Interfaces;
using MyGuitarShop.Data.Ado.Entities;
using MyGuitarShop.Data.Ado.Factories;
using MyGuitarShop.Data.Ado.Repository;
using System.Diagnostics;

namespace MyGuitarShop.api
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);

                AddServices(builder);

                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                if (builder.Environment.IsDevelopment())
                {
                    builder.Services.AddEndpointsApiExplorer();
                    builder.Services.AddSwaggerGen();
                }

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                ConfigureAppLocation(app);
                await app.RunAsync();
            }
            catch (Exception ex)
            {

                if (Debugger.IsAttached) Debugger.Break();

                Console.WriteLine(ex.Message);
            }
        }

        private static void ConfigureAppLocation(WebApplication app)
        {
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
        }

        private static void AddServices(WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("MyGuitarShop")
                ?? throw new InvalidOperationException("MyGuitarShop connection string not found.");

            builder.Services.AddSingleton(new SqlConnectionFactory(connectionString));

            builder.Services.AddScoped<IRepository<ProductDTO>, ProductRepo>();

            builder.Services.AddScoped<IRepository<AddressDTO>, AddressRepo>();

            builder.Services.AddScoped<IRepository<AdminDTO>, AdminRepo>();

            builder.Services.AddScoped<IRepository<CategoryDTO>, CategoryRepo>();

            // Add services to the container.
            builder.Services.AddControllers();
        }
    }
}
