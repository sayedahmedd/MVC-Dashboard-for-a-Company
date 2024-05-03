using Demo.BLL;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Data;
using Demo.DAL.Entities;
using Demo.PL.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Demo.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ApplicationDBContext>(Option => Option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            //builder.Services.AddScoped<IDepartmentReposirory, DepartmentRepository>();
            //builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            builder.Services.AddScoped<IUnitOfWork , UnitOfWork>();
            builder.Services.AddAutoMapper(m => m.AddProfile(new MappingProfile() ));

            // Service from Repository at most be Scoped 

            //builder.Services.AddScoped<UserManager<ApplicationUser>>();
            //builder.Services.AddScoped<SignInManager<ApplicationUser>>();
            //builder.Services.AddScoped<RoleManager<IdentityRole>>();

            builder.Services.AddIdentity<ApplicationUser , IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDBContext>()
                .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(config =>
                { config.LoginPath = "/Account/SignIn";
                    config.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                
                });

			//builder.Services.AddAuthentication(" Cookies ")
   //             .AddCookie("Hamda",
   //             config =>
   //             {
   //                 config.LoginPath = "/Account/SignIn" ;
   //                 config.AccessDeniedPath = "/Home/Error";
   //             });

			var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

			app.UseAuthentication();
            app.UseAuthorization();


			app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}