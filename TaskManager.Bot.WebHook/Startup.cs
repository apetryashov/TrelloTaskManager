using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TaskManager.Bot.Authorization;
using TaskManager.Bot.WebHook.Services;
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

            services.AddScoped<IUpdateService, UpdateService>();
            services.AddScoped<IRequestHandler, RequestHandler>();

            services
                .AddModule(new TelegramBotModule(botConfiguration))
                .AddModule(new TrelloModule(botConfiguration.AccessToken))
                .AddModule<CommandModule>()
                .AddModule(new MongoDbAuthorizationStorageModule(mongoConnectionProperties))
                .AddControllers()
                .AddNewtonsoftJson();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseRouting();
            app.UseCors();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}