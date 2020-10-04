using System.Threading;
using System.Threading.Tasks;
using AspNetCore.Scheduler.ScheduleTask;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using TelegramBot.Core;

namespace TaskManager.Reminder
{
    public class UsersScheduledReminder : IScheduledTask
    {
        private readonly IUserRemindResponseGenerator userRemindResponseGenerator;
        private readonly IUsersProvider usersProvider;
        private readonly ITelegramBotClient client;
        private readonly ILogger<UsersScheduledReminder> logger;

        public UsersScheduledReminder(
            IUsersProvider usersProvider,
            IUserRemindResponseGenerator userRemindResponseGenerator,
            ITelegramBotClient client,
            CronString cron,
            ILogger<UsersScheduledReminder> logger)
        {
            this.usersProvider = usersProvider;
            this.userRemindResponseGenerator = userRemindResponseGenerator;
            this.client = client;
            this.logger = logger;
            Schedule = cron.Value;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Execute user reminder");
            await foreach (var user in usersProvider.GetUsers().WithCancellation(cancellationToken))
            {
                var response = await userRemindResponseGenerator.GetResponse(user);
                await client.SendResponse(user.TelegramId, response);
            }
        }

        public string Schedule { get; }
    }
}