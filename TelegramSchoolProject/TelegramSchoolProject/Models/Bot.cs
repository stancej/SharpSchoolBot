using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using TelegramSchoolProject.Models.Commands;

namespace TelegramSchoolProject.Models
{
    public static class Bot
    {
        public enum BotStates { Menu = 0, Test = 1 }
        private static BotStates _botState { get; set; }
        public static BotStates CurrentState { get => _botState; set { changeCommandList(value); _botState = value; } }

        private static TelegramBotClient client;

        public static Test test { get; set; }

        private static List<Command> commandsList;
        public static IReadOnlyList<Command> Commands { get => commandsList.AsReadOnly(); }

        public static TelegramBotClient Get()
        {
            if (client != null && commandsList != null)
                return client;

            setDefaultCommandList();

            var token = Environment.GetEnvironmentVariable("token", EnvironmentVariableTarget.Process);
            token = "1457729334:AAFX-fEKpX_poXnBCrv4VUdQ33o0HgQq3DE";
            client = new TelegramBotClient(token);

            return client;
        }

        private static void changeCommandList(BotStates state)
        {
            switch (state)
            {
                case BotStates.Menu:
                    commandsList = new List<Command>();
                    commandsList.Add(new TestCommand());
                    break;
                case BotStates.Test:
                    commandsList = new List<Command>();
                    commandsList.Add(new MenuCommand());
                    break;
                default:
                    setDefaultCommandList();
                    break;
            }
        }

        private static void setDefaultCommandList()
        {
            commandsList = new List<Command>();
            commandsList.Add(new StartCommand());
            //TODO: Add more commands
        }
    }
}
