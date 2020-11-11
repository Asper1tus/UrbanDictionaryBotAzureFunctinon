using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace UrbanDictionaryBotFunction.Commands
{
    class GetTopCommand : Command
    {
        public override string Name => @"/getTop";

        public override Task Execute(Message message, TelegramBotClient client)
        {
            throw new NotImplementedException();
        }
    }
}
