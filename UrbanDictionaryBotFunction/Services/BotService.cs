using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using UrbanDictionaryBotFunction.Commands;

namespace UrbanDictionaryBotFunction.Services
{
    class BotService
    {
        private readonly string token;
        private readonly TelegramBotClient telegramBotClient;
        private readonly List<Command> commandsList;
        private readonly UrbanDictionaryParserService parserService;

        public BotService()
        {
            token = Environment.GetEnvironmentVariable("token", EnvironmentVariableTarget.Process);
            telegramBotClient = new TelegramBotClient(token);
            parserService = new UrbanDictionaryParserService();

            commandsList = new List<Command>() 
            {
                new StartCommand()
            };
        }

        public async Task StartListeningAsync(Update update)
        {
            if (update.Type == UpdateType.Message)
            {
                var message = update.Message;
                if (commandsList.Any(c => c.Contains(message)))
                {
                    await CommandHandlerAsync(message);
                    return;
                }

                await TextHandler(message);
            }
        }

        private async Task CommandHandlerAsync(Message message)
        {
            await commandsList
                .First(c => c.Contains(message))
                .Execute(message, telegramBotClient);
        }

        private async Task TextHandler(Message message)
        {
            var chatId = message.Chat.Id;
            var term = parserService.GetTermDefinition(message.Text);

            string text;

            if (term == null)
            {
                text = "Meaning not found";
            }
            else
            {
                text = $"*{term.Word}* by [{term.Author}]({term.AuthorUrl}) \n\n" 
                    + $"*Meaning:*\n {term.Meaning} \n\n"
                    + $"*Exapmle:*\n {term.Example}\n\n" 
                    + $"[Source]({term.SourceUrl})";
            }

            await telegramBotClient.SendTextMessageAsync(chatId, text, parseMode: ParseMode.Markdown, disableWebPagePreview: true);
        }
    }
}
