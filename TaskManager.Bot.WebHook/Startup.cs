using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
            services.AddRazorPages();
            services.AddLogging(x => x.AddConsole());
            ConfigIoc(services)
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

        private IServiceCollection ConfigIoc(IServiceCollection services)
        {
            var botConfiguration = Configuration.GetSection("BotConfiguration").Get<BotConfiguration>();
            var mongoConnectionProperties =
                Configuration.GetSection("MongoConfiguration").Get<MongoConnectionProperties>();
            var croneString = Configuration.GetValue<string>("Cron");

            return services
                .AddScoped<IUpdateService, UpdateService>()
                .AddScoped<IRequestHandler, RequestHandler>()
                .AddModule(new TelegramBotModule(botConfiguration))
                .AddModule(new TrelloModule(botConfiguration.AccessToken, botConfiguration.ReturnUrl))
                .AddModule<CommandModule>()
                .AddModule<CommonModule>()
                .AddModule(new MongoDbAuthorizationStorageModule(mongoConnectionProperties))
                .AddModule(new ReminderModule(croneString));
        }
    }
}