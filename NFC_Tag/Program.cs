using Microsoft.EntityFrameworkCore;
using NFC.Application.Interfaces;
using NFC.Application.Interfaces.IServices;
using NFC.Application.MappingProfiles;
using NFC.Application.Services;
using NFC.Infrastructure.Data;
using NFC.Infrastructure.Data.DataSeeding;
using NFC.Infrastructure.Repository;

namespace NFC_Tag
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString"));
            });

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<INfcTagRepository, NfcTagRepository>();
            builder.Services.AddScoped<INfcTagService, NfcTagService>();
            builder.Services.AddScoped<IChemicalItemService, ChemicalItemService>();
            builder.Services.AddScoped<IDbIntializer, DbIntializer>();

            builder.Services.AddAutoMapper(typeof(ChemicalItemProfile).Assembly);

            var app = builder.Build();

            using(var scope = app.Services.CreateScope())
            {
                var dbinitializer = scope.ServiceProvider.GetRequiredService<IDbIntializer>();
                dbinitializer.IntializerAsync();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
