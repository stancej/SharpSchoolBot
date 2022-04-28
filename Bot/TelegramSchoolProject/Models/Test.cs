using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.Linq;
using TelegramSchoolProject.Models.Commands;

namespace TelegramSchoolProject.Models
{
    public static class Test
    {

        public static async void correctTranslate(Message message, TelegramBotClient client)
        {
            //get list of words
            var w_en = StringDictionaryEditing.GetListOfWords(Bot.user.r_WordsEn);
            var w_ru = StringDictionaryEditing.GetListOfWords(Bot.user.r_WordsRu);

            if (w_en?.Count() > 1)
            {
                w_en.RemoveAt(0);
                w_ru.RemoveAt(0);
                var fw_en = w_en[0];
                var fw_ru = w_ru[0];
                Bot.user.c_WordEn = fw_en;
                Bot.user.c_WordRu = fw_ru;

                var e = StringDictionaryEditing.GetStringFromWords(w_en);
                var r = StringDictionaryEditing.GetStringFromWords(w_ru);

                Bot.user.r_WordsEn = e;
                Bot.user.r_WordsRu = r;

                await Bot.telegramDb.SaveChangesAsync();
                await client.SendTextMessageAsync(message.Chat.Id, fw_ru);
            }
            else
            {
                GoToMenu(message, client);
            }
            
        }

        public static async void incorrectTranslate(Message message, TelegramBotClient client)
        {
            var w_en = StringDictionaryEditing.GetListOfWords(Bot.user.r_WordsEn);
            var w_ru = StringDictionaryEditing.GetListOfWords(Bot.user.r_WordsRu);
            if (w_en?.Count() != 0)
            {
                var fw_en = w_en[0];
                var fw_ru = w_ru[0];

                w_en.RemoveAt(0);
                w_ru.RemoveAt(0);

                w_en.Add(fw_en);
                w_en.Add(fw_en);
                w_en.Add(fw_en);

                w_ru.Add(fw_ru);
                w_ru.Add(fw_ru);
                w_ru.Add(fw_ru);

                w_en = StringDictionaryEditing.RandomiseWords(w_en);
                w_ru = StringDictionaryEditing.RandomiseWords(w_ru);

                var _fw_en = w_en[0];
                var _fw_ru = w_ru[0];
                Bot.user.c_WordEn = _fw_en;
                Bot.user.c_WordRu = _fw_ru;

                var e = StringDictionaryEditing.GetStringFromWords(w_en);
                var r = StringDictionaryEditing.GetStringFromWords(w_ru);

                Bot.user.r_WordsEn = e;
                Bot.user.r_WordsRu = r;
                await Bot.telegramDb.SaveChangesAsync();

                string text = $"Вы ввели неправильный перевод слова. Правильно: {fw_en}";

                await client.SendTextMessageAsync(message.Chat.Id, text);
                await client.SendTextMessageAsync(message.Chat.Id, _fw_ru);

            }
            else
            {
                GoToMenu(message, client);
            }
        }

        public async static void GoToMenu(Message message, TelegramBotClient client)
        {
            MenuCommand m = new MenuCommand();
            await m.Execute(message, client);
        }
        
    }
}
