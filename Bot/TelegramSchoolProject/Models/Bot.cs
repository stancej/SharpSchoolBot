using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramSchoolProject.Models.Commands;
using TelegramSchoolProject.SchoolDb;
using TelegramSchoolProject.SchoolDb.Models;

namespace TelegramSchoolProject.Models
{
    public static class Bot
    {
        public static SchoolDb.Models.User user { get; set; }

        private static TelegramBotClient client;

        public static scTelegramDB telegramDb { get; private set; }

        public static TelegramBotClient Get()
        {
            if (client != null)
                return client;

            //setDefaultCommandList();

            var token = Environment.GetEnvironmentVariable("token", EnvironmentVariableTarget.Process);
            token = "1457729334:AAFX-fEKpX_poXnBCrv4VUdQ33o0HgQq3DE";
            client = new TelegramBotClient(token);

            //set database. If database is initialised than close and open new one
            if (telegramDb != null)
                telegramDb.Dispose();

            telegramDb = new scTelegramDB();

            return client;
        }

        public static List<Command> GetCommandList(string state)
        {
            List<Command> commandsList = new List<Command>();
            switch (state)
            {
                case "Menu":
                    commandsList.Add(new TestCommand());
                    break;
                case "Test":
                    commandsList.Add(new MenuCommand());
                    break;
                default:
                    commandsList.Add(new StartCommand());
                    break;
            }
            return commandsList;
        }
    }
}
