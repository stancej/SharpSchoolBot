using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramSchoolProject.Models.Commands
{
    public class MenuCommand : Command
    {
        public override string Name => "/menu";

        public override async Task Execute(Message message, TelegramBotClient client)
        {
            var answer = "Вы перешли в меню. Если хотите пройти тест введи /test **Название теста**";
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;

            Bot.user.Stage = "Menu";
            await Bot.telegramDb.SaveChangesAsync();

            await client.SendTextMessageAsync(chatId, answer);
        }
    }
}
