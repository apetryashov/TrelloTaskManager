using AspNetCore.Scheduler.ScheduleTask;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaskManager.Bot.WebHook.Services;
using TaskManager.Common;
using TaskManager.Ioc;
using TaskManager.Ioc.Modules;

namespace TaskManager.Bot.WebHook
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var botConfiguration = Configuration.GetSection("BotConfiguration").Get<BotConfiguration>();
            var mongoConnectionProperties =
                Configuration.GetSection("MongoConfiguration").Get<MongoConnectionProperties>();

            services.AddRazorPages();

            services.AddScoped<IUpdateService, UpdateService>();
            services.AddScoped<IRequestHandler, RequestHandler>();

            services
                .AddModule(new TelegramBotModule(botConfiguration))
                .AddModule(new TrelloModule(botConfiguration.AccessToken, botConfiguration.ReturnUrl))
                .AddModule<CommandModule>()
                .AddModule<CommonModule>()
                .AddModule(new MongoDbAuthorizationStorageModule(mongoConnectionProperties))
                .AddModule(new ReminderModule("18 * * * *"))
                .AddControllers()
                .AddNewtonsoftJson();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}