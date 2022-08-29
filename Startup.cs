using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using MyWebApp.Models;
using Wkhtmltopdf.NetCore;
using Npgsql;

namespace MyWebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) 
        {
            // string connection = "Server=(localdb)\\mssqllocaldb;Database=MyWebDb;Trusted_Connection=True;";
            //string connection = "Host=localhost;Port=5433;Database=WebDb;Username=postgres;Password=341289000";
            var connectionString = Configuration["PostgreSql:ConnectionString"];
            var dbPassword = Configuration["PostgreSql:DbPassword"];

            var builder = new NpgsqlConnectionStringBuilder(connectionString)
            {
                Password = dbPassword
            };

            services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(builder.ConnectionString));
            //services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
                    options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Account/Denied");//Если допуска нет то выкидывает на соответствующую страницу

                });

            services.AddControllersWithViews();
            services.AddWkhtmltopdf("wkhtmltopdf");

        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}