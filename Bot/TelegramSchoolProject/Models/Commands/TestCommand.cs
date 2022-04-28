using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Linq;
using TelegramSchoolProject.SchoolDb.Models;

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

            #region interact with db
            string wordsEn;
            string wordsRu;
            string f_wordru;
            string f_worden;
            string text = "";
            int wordsCount = GetWordsList(message.Text, out wordsEn, out wordsRu, out f_worden, out f_wordru, out text);

            if (wordsCount == 0)
            {
                ErrorsMessage.WrongTextMessageExeception(message, client);
                canExecute = true;
                return;
            }

            Bot.user.Stage = "Test";
            Bot.user.r_WordsEn = wordsEn;
            Bot.user.r_WordsRu = wordsRu;
            Bot.user.c_WordEn = f_worden;
            Bot.user.c_WordRu = f_wordru;

            await Bot.telegramDb.SaveChangesAsync();
            #endregion

            var answer = $"В данном тесте {wordsCount} слов(а). Необходимо ввести перевод представленных слов.";
            var chatId = message.Chat.Id;
            var messageId = message.MessageId;

            if (!string.IsNullOrEmpty(text))
                await client.SendTextMessageAsync(chatId, text,replyToMessageId: messageId);

            await client.SendTextMessageAsync(chatId, answer, replyToMessageId: messageId);
            await client.SendTextMessageAsync(chatId, f_wordru);
        }

        public override bool Contains(string _message)
        {
            var elements = _message.Split(' ');
            if (elements.Length >= 2)
            {
                if (elements[0].Equals(Name, StringComparison.OrdinalIgnoreCase))
                {
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

        private int GetWordsList(string message, out string wordsEn, out string wordsRu, out string f_worden, out string f_wordru, out string text)
        {
            try
            {
                wordsEn = "";
                wordsRu = "";
                string _wordsEn = "";
                string _wordsRu = "";
                List<string> tests = new List<string>();
                text = "";

                var elements = message.Split(' ');
                for (int i = 1; i < elements.Length; i++)
                {
                    if (elements[i].Length < 1)
                        continue;

                    for (int j = 1; j < elements[i].Length; j++)
                    {
                        tests.Add(elements[i][0].ToString() + elements[i][j].ToString());
                    }
                }

                var dictionaries = new List<Dictionary>();
                foreach (var i in Bot.telegramDb.Dictionaries)
                {
                    foreach (var j in tests)
                    {
                        if (j.Equals(i.Paragraph, StringComparison.OrdinalIgnoreCase))
                        {
                            dictionaries.Add(i);
                            tests.Remove(j);
                            break;
                        }
                    }
                }
                string wrongTests = "";
                foreach (var i in tests)
                {
                    bool c = false;
                    foreach (var j in Bot.telegramDb.Dictionaries)
                    {
                        if (i.Equals(j.Paragraph, StringComparison.OrdinalIgnoreCase))
                        {
                            dictionaries.Add(j);
                            tests.Remove(i);
                            c = true;
                            break;
                        }
                    }
                    if (!c)
                    {
                        wrongTests += i + " ";
                    }
                }
                if (wrongTests.Count() > 0)
                    text = $"Тест {wrongTests} введен некоректно/недобавлен/несуществует";


                if (dictionaries.Count() == 0)
                {
                    wordsEn = null;
                    wordsRu = null;
                    f_worden = null;
                    f_wordru = null;
                    return 0;
                }

                //get list of words
                foreach (var i in dictionaries)
                {
                    _wordsEn += i.WordsEn;
                    _wordsRu += i.WordsRu;
                }

                var en = StringDictionaryEditing.GetListOfWords(_wordsEn);
                var ru = StringDictionaryEditing.GetListOfWords(_wordsRu);
                int l = en.Count();

                en = StringDictionaryEditing.RandomiseWords(en);
                ru = StringDictionaryEditing.RandomiseWords(ru);

                f_wordru = ru[0];
                f_worden = en[0];

                wordsEn = StringDictionaryEditing.GetStringFromWords(en);
                wordsRu = StringDictionaryEditing.GetStringFromWords(ru);

                return l;
            }
            catch(Exception e)
            {
                text = "";
                wordsEn = null;
                wordsRu = null;
                f_worden = null;
                f_wordru = null;
                return 0;
            }
        }
    }
}
