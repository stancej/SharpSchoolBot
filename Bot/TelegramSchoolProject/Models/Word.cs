using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramSchoolProject.Models
{
    public class Word
    {
        public string ru_Word { get; set; }
        public string en_Word { get; set; }

        public Word(string _ru, string _eng)
        {
            ru_Word = _ru;
            en_Word = _eng;
        }
    }
}
