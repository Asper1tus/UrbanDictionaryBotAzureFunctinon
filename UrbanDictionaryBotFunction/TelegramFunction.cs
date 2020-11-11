using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using UrbanDictionaryBotFunction.Services;

namespace EchoTelegramBot.AzureFunction
{
    public static class TelegramFunction
    {
        [FunctionName("Telegram")]
        public static async Task<IActionResult> Update(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
            HttpRequest request,
            ILogger logger)
        {
            logger.LogInformation("Invoke telegram update function");

            var body = await request.ReadAsStringAsync();
            var update = JsonConvert.DeserializeObject<Update>(body);
            await HandleUpdate(update);

            return new OkResult();
        }

        private static async Task HandleUpdate(Update update)
        {
            BotService botService = new BotService();

            if (update.Type == UpdateType.Message)
            {
                var message = update.Message;

                await botService.Listen(message);
            }
        }
    }
}