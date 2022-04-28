using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TelegramSchoolProject
{
    public static class StringDictionaryEditing
    {
        
        public static List<string> GetListOfWords(string text)
        {
            var words = text.Split('-').ToList<string>();
            int l = words.Count() - 1;
            words.RemoveAt(l);
            return words;
        }

        public static List<string> RandomiseWords(List<string> words)
        {
            Random rnd = new Random(DateTime.Now.Second);
            int l = words.Count();
            for (int i = 0; i < l * 2; i++)
            {
                int x = rnd.Next() % l; int y = rnd.Next() % l;

                var temp = words[x];
                words[x] = words[y];
                words[y] = temp;
            }
            return words;
        }

        public static string GetStringFromWords(List<string> words)
        {
            string str = "";
            int l = words.Count();
            for (int i = 0; i < l; i++)
            {
                str += words[i] + "-";
            }
            return str;
        }
        
    }
}
