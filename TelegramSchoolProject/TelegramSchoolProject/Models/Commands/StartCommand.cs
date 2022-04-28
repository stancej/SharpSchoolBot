using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramSchoolProject.Models.Commands
{
    public class StartCommand : Command
    {
        public override string Name => "/start";

        public override async Task Execute(Message message, TelegramBotClient client)
        {
            var answer = "Бот готов к использованию. Можно Приступать!";
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;

            Bot.CurrentState = Bot.BotStates.Menu;

            //Bot.CurrentState = Bot.BotStates.Menu;

            await client.SendTextMessageAsync(chatId, answer, replyToMessageId: messageId);
        }
    }
}
