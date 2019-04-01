using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TomasosPizzeriaTest.IdentityData;
using TomasosPizzeriaTest.Models;

namespace TomasosPizzeriaTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSession();
            services.AddDistributedMemoryCache();
            
            services.AddDbContext<TomasosContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer
              (Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            

            services.Configure<IdentityOptions>(options => {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                options.User.AllowedUserNameCharacters ="abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider services)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=StartPage}/{id?}");
            });

            CreateUserRoles(services).Wait();
        }

        private async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            IdentityResult roleResult;
            IdentityResult roleResult2;
            IdentityResult roleResult3;
            //Adding Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck)
            {
                //create the roles and seed them to the database
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin"));
            }
            var roleCheck2 = await RoleManager.RoleExistsAsync("Premium");
            if (!roleCheck2)
            {
                //create the roles and seed them to the database
                roleResult2 = await RoleManager.CreateAsync(new IdentityRole("Premium"));
            }
            var roleCheck3 = await RoleManager.RoleExistsAsync("Regular");
            if (!roleCheck3)
            {
                //create the roles and seed them to the database
                roleResult3 = await RoleManager.CreateAsync(new IdentityRole("Regular"));
            }

            if (await UserManager.FindByNameAsync("Admin") == null)
            {
                var userIdentity = new ApplicationUser
                {
                    UserName = "Admin"                  
                };

                var result = await UserManager.CreateAsync(userIdentity, "Admin123");

                if (result.Succeeded)
                {
                    var resultRole = await UserManager.AddToRoleAsync(userIdentity, "Admin");
                    var reguser = new Kund
                    {
                        Namn = "Admin",
                        Gatuadress = "Admin",
                        Postnr = "Admin",
                        Postort = "Admin",
                        Email = "Admin",
                        Telefon = "Admin",
                        AnvandarNamn = "Admin",
                        Losenord = "Admin123",
                        IdentityId = userIdentity.Id
                    };

                    TomasosContext DB = new TomasosContext();

                    DB.Add(reguser);

                    await DB.SaveChangesAsync();
                }

            }
        }
    }
}
