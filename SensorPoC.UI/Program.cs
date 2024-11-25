using Microsoft.Extensions.DependencyInjection;

namespace SensorPoC.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages().AddRazorPagesOptions(options =>
            {
                options.Conventions.AddPageRoute("/Overview", "");
            });

            builder.Configuration["ApiUrl"] = Environment.GetEnvironmentVariable("API_URL")
                                   ?? builder.Configuration["ApiUrl"]
                                   ?? "http://localhost:8081";

            var app = builder.Build();

            app.Use(async (context, next) =>
            {
                context.Items["ApiUrl"] = app.Configuration["ApiUrl"];
                await next.Invoke();
            });

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
