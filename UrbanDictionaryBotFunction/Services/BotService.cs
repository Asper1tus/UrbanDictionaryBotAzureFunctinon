using System.Collections.Generic;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using UrbanDictionaryBotFunction.Commands;

namespace UrbanDictionaryBotFunction.Services
{
    class BotService
    {

        private string token;
        private readonly TelegramBotClient telegramBotClient;
        private List<Command> commandsList;
        private UrbanDictionaryParserService parserService;

        public BotService()
        {
            token = "";
            telegramBotClient = new TelegramBotClient(token);
            parserService = new UrbanDictionaryParserService();

            commandsList = new List<Command>() {
                new StartCommand(),
                new GetTopCommand() 
            };
        }

        public void Listen(Message message)
        {
            if (commandsList.Any(c => c.Contains(message)))
            {
                CommandHandler(message);
                return;
            }

            TextHandler(message);
        }

        private async void CommandHandler(Message message)
        {
            await commandsList
                .First(c => c.Contains(message))
                .Execute(message, telegramBotClient);
        }

        private async void TextHandler(Message message)
        {
            var chatId = message.Chat.Id;
            var term = parserService.GetTermDefinition(message.Text);

            string text;

            if (term == null)
            {
                text = "Word not found";
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
