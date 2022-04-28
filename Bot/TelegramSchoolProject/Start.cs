using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramSchoolProject.Models;
using System.Linq;

namespace TelegramSchoolProject
{
    public static class TelegramHttpStart
    {

        [FunctionName("Telegram")]
        public static async Task<IActionResult> Update(
                [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]
            HttpRequest request,
                ILogger logger)
        {
            logger.LogInformation("Invoke telegram update function");

            var body = await request.ReadAsStringAsync();
            var update = JsonConvert.DeserializeObject<Update>(body);
            await HandleUpdate(update);

            return new OkResult();
        }

        private static async Task HandleUpdate(Update update)
        {
            if (update.Type == UpdateType.Message)
            {
                var client = Bot.Get();
                var message = update.Message;

                #region interact with db 
                //getting user what send message
                bool f = false;
                foreach(var u in Bot.telegramDb.Users)
                {
                    if (u.ChatId == message.Chat.Id)
                    {
                        Bot.user = u;
                        f = true;
                        break;
                    }
                }

                if (!f)
                {
                    //adding new user
                    TelegramSchoolProject.SchoolDb.Models.User u = new TelegramSchoolProject.SchoolDb.Models.User
                    {
                        Name = message.From.FirstName + " " + message.From.LastName,
                        ChatId = message.Chat.Id,
                        Stage = "Start"
                    };
                    Bot.telegramDb.Users.Add(u);
                    Bot.user = u;
                    await Bot.telegramDb.SaveChangesAsync();
                }
                #endregion
                
                foreach (var command in Bot.GetCommandList(Bot.user?.Stage))
                {
                    if (command.Contains(message.Text))
                    {
                        await command.Execute(message, client);
                        return;
                    }
                }

                if (Bot.user?.Stage == "Test" && !string.IsNullOrWhiteSpace(Bot.user.c_WordEn))
                {
                    if (message.Text.Equals(Bot.user.c_WordEn, StringComparison.OrdinalIgnoreCase))
                        Test.correctTranslate(message, client);
                    else
                        Test.incorrectTranslate(message, client);
                }
            }
        }

    }
}
