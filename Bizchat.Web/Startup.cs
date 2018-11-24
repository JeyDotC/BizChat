using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bizchat.Core.Repositories;
using Bizchat.Core.Services;
using Bizchat.Core.Verbs;
using Bizchat.Ef;
using Bizchat.Ef.Repositories;
using Bizchat.NServiceBus;
using Bizchat.NServiceBus.Services;
using Bizchat.Web.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

namespace Bizchat.Web
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<BizchatDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("Bizchat.Web")));

            services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddTransient<IChatRoomsRepository, EfChatRoomsRepository>();
            services.AddTransient<IChatUsersRepository, EfChatUsersRepository>();
            services.AddTransient<IChatMessagesRepository, EfChatMessagesRepository>();
            services.AddTransient<IChatMessageReceivedByChatRoomEventsRepository, EfChatMessageReceivedByChatRoomEventsRepository>();
            services.AddTransient<IChatMessageSentEventsRepository, EfChatMessageSentEventsRepository>();

            services.AddTransient<IQueueMessagesService, NServiceBusQueueMessagesService>();
            services.AddTransient<SendMessageVerb>();

            // NServiceBus settings
            var endpointConfiguration = EndPointConfigurationFactory.Create();

            /*var routing = transport.Routing();
            routing.RouteToEndpoint(
                assembly: typeof(MyMessage).Assembly,
                destination: "Samples.ASPNETCore.Endpoint");*/

            var endpoint = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();
            services.AddSingleton<IMessageSession>(endpoint);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
