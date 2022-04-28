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

                foreach (var command in Bot.Commands)
                {
                    if (command.Contains(message.Text))
                    {
                        await command.Execute(message, client);
                        return;
                    }
                }

                if(Bot.CurrentState == Bot.BotStates.Test)
                {
                    if (message.Text.Equals(Bot.test.currentWord.en_Word, StringComparison.OrdinalIgnoreCase))
                        Bot.test.correctTranslate(message, client);
                    else
                        Bot.test.incorrectTranslate(message, client);
                }
            }
        }

    }
}
