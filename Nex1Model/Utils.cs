using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Nexomon1Model
{
    public static class Utils
    {
        public static string UnprotectString(string text)
        {
            if (text.Length >= 15)
            {
                text = text.Remove(4, 3);
            }
            text = text.Remove(text.Length - 3, 3);
            char[] textarray = text.ToCharArray();
            text = Reverse(text);
            text = decode64(text);
            int num = int.Parse(text[0].ToString() ?? "");
            text = text.Remove(0, 1);
            for (int i = 0; i < num; i++)
            {
                text = decode64(text);
                text = Reverse(text);
            }
            num = int.Parse(text[text.Length - 1].ToString() ?? "");
            text = text.Remove(text.Length - 1, 1);
            for (int j = 0; j < num; j++)
            {
                text = decode64(text);
                text = Reverse(text);
            }
            return text;
        }
        public static string ProtectString(string input)
        {
            string text = input;
            int num = Random.Shared.Next(1, 3);
            for (int i = 0; i < num; i++)
            {
                text = Reverse(text);
                text = encode64(text);
            }
            text += num;
            num = Random.Shared.Next(1, 3);
            for (int j = 0; j < num; j++)
            {
                text = Reverse(text);
                text = encode64(text);
            }
            text = num + text;
            text = encode64(text);
            text = Reverse(text);
            text += "!==";
            if (text.Length >= 15)
            {
                text = text.Insert(4, string.Concat(Random.Shared.Next(200, 800)));
            }
            return text;
        }
        public static string Reverse(string text)
        {
            char[] textarray = text.ToCharArray();
            Array.Reverse(textarray);
            return (new string(textarray));
        }
        public static string decode64(string text)
        {
            byte[] bytes = Convert.FromBase64String(text);
            return Encoding.UTF8.GetString(bytes);
        }
        public static string encode64(string input)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(input));
        }

    }
}
