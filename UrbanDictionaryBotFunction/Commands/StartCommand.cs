using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace UrbanDictionaryBotFunction.Commands
{
    class StartCommand : Command
    {
        public override string Name => @"/start";

        public override async Task Execute(Message message, TelegramBotClient botClient)
        {
            var chatId = message.Chat.Id;
            string text = "Type word or expression you want to get meaning";

            await botClient.SendTextMessageAsync(chatId, text);
        }
    }
}
