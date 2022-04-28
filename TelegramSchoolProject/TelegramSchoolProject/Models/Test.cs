using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Linq;
using TelegramSchoolProject.Models.Commands;

namespace TelegramSchoolProject.Models
{
    public class Test
    {
        private Random rnd;

        private List<Word> initialWords;
        public List<Word> remainingWords { get; set; }
        public Word currentWord;

        //message needs to set seed of random value selector
        public Test(List<Word> words, Message message = null)
        {
            if (message != null)
                rnd = new Random(string.GetHashCode(message.Date.ToString()));
            else
                rnd = new Random();

            initialWords = new List<Word>(words);
            remainingWords = new List<Word>(words);
            currentWord = words[rnd.Next(words.Count)];
        }

        public async void correctTranslate(Message message, TelegramBotClient client)
        {
            if(remainingWords.Count == 1)
            {
                remainingWords = initialWords;
                await client.SendTextMessageAsync(message.Chat.Id, "Тест успешно пройден. Тест начат занаго.");
                await client.SendTextMessageAsync(message.Chat.Id, currentWord.ru_Word);
                return;
            }
            remainingWords.Remove(currentWord);
            currentWord = remainingWords[rnd.Next(remainingWords.Count)];
            await client.SendTextMessageAsync(message.Chat.Id, currentWord.ru_Word);
        }

        public async void incorrectTranslate(Message message, TelegramBotClient client)
        {
            await client.SendTextMessageAsync(message.Chat.Id,"Ответ не верный : " + currentWord.en_Word,replyToMessageId: message.MessageId);

            if(remainingWords.FindAll(x => x.ru_Word == currentWord.ru_Word).Count <= 3)
                remainingWords.Add(currentWord);
            currentWord = remainingWords[rnd.Next(remainingWords.Count)];

            await client.SendTextMessageAsync(message.Chat.Id,currentWord.ru_Word);
        }
    }
}
