using Blog_Website_Project.Models.Abstracts;
using Blog_Website_Project.Models.Repositories;
using Core.Repositories;
using Core.Services;
using DataAccess.BlogDataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services.Services;

namespace Blog_Website_Project
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
            services.AddControllersWithViews();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<ITopicRepository, TopicRepository>();
            services.AddScoped<ITopicService, TopicService>();
            services.AddScoped<IUploadPictureRepository, UploadPictureRepository>();


            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            services.AddDbContext<BlogContext>(options => options.UseSqlServer(Configuration.GetConnectionString("LocalConnection")));

            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services.AddMemoryCache();
            services.AddSession();

        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=GeneralHomePage}/{id?}");



            });
        }
    }
}
