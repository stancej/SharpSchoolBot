using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramSchoolProject.Models.Commands
{
    public static class ErrorsMessage
    {
        public static async void WrongTextMessageExeception(Message message, TelegramBotClient client)
        {
            var answer = "Неверный ввод команды. Попробуйте иначе.";
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;

            await client.SendTextMessageAsync(chatId, answer, replyToMessageId: messageId);
        }
    }
}
