using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramSchoolProject.Models.Commands
{
    public class TestCommand : Command
    {
        private bool canExecute = true;

        public override string Name => "/test";

        public override async Task Execute(Message message, TelegramBotClient client)
        {
            if (!canExecute)
            {
                ErrorsMessage.WrongTextMessageExeception(message, client);
                canExecute = true;
                return;
            }

            List<Word> wordsList = new List<Word>() { new Word("Плавать", "Swim"), new Word("Бегать", "Run") };
            Bot.test = new Test(wordsList);
            Bot.CurrentState = Bot.BotStates.Test;

            var answer = $"В данном тесте  слов. Необходимо ввести перевод представленных слов.";
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;

            await client.SendTextMessageAsync(chatId, answer, replyToMessageId: messageId);
            await client.SendTextMessageAsync(chatId, Bot.test.currentWord.ru_Word);
        }

        public override bool Contains(string _message)
        {
            var elements = _message.Split(' ');
            if (elements.Length == 2)
            {
                if (elements[0].Equals(Name, StringComparison.OrdinalIgnoreCase))
                {
                    if(true) //Проверка из датабазы
                        return true;
                }
            }
            else if(elements.Length == 1)
            {
                canExecute = false;
                return true;
            }
            return false;

        }
    }
}
