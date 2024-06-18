using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Repos;

namespace Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddMvc();
            // Add services to the container.
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddMemoryCache();
            builder.Services.AddDbContext<ServerContext>(x =>
                x.UseMySQL(
                    "server=172.20.5.30; port=3307; user=admin; password=password1234_; database=db"
                )
            );
            builder.Services.AddCors(p =>
                p.AddDefaultPolicy(build =>
                {
                    build.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
                })
            );

            builder.Services.AddScoped<IUnauthorize, UnauthorizeService>();
            builder.Services.AddScoped<IAuthorizeUser, AuthorizeUserService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseDirectoryBrowser();
            app.UseCors();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.MapControllers();
            app.Run();
        }
    }
}
