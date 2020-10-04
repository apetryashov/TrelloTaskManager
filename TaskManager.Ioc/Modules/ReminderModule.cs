using AspNetCore.Scheduler.ScheduleTask;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Reminder;

namespace TaskManager.Ioc.Modules
{
    public class ReminderModule : IServiceModule
    {
        private readonly string cronString;

        public ReminderModule(string cronString)
        {
            this.cronString = cronString;
        }

        public void Load(IServiceCollection serviceCollection) => serviceCollection
            .AddSingleton(new CronString(cronString))
            .AddTransient<IUsersProvider, UsersProvider>()
            .AddTransient<IUserRemindResponseGenerator, UserRemindResponseGenerator>()
            .AddSingleton<IScheduledTask, UsersScheduledReminder>()
            .AddScheduler();
    }
}