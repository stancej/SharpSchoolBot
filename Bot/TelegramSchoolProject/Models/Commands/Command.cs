using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramSchoolProject.Models.Commands
{
    public abstract class Command
    {
        public abstract string Name { get; }
        public abstract Task Execute(Message message, TelegramBotClient client);

        public virtual bool Contains(string _message)
        {
            var elements = _message.Split(' ');
            if (elements.Length > 0)
                return elements[0].Equals(this.Name, StringComparison.OrdinalIgnoreCase);
            return false;
        }
    }
}
