using System;
using System.Reflection;

namespace KeyboardTrainerConsole
{
    internal class WordsDatabaseScript
    {
        public static string language = "english";
        public static string dictionary = "nouns";
        public static string path;

        public static string[] words;
        public static int maxWordsArray;

        static void CheckPath()
        {
            path = (Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\languages\\" + language + "_" + dictionary + ".txt"); // сделать потом если ошибка то искать на других языках
            words = File.ReadAllLines(path);
            maxWordsArray = words.Length;
        }

        public static void LanguageSetting()
        {
            CheckPath();
            Console.WriteLine("Selected language - " + language + ", dictionary - " + "«" + dictionary + "». " + "Number of words: " + maxWordsArray);
        }

        public static void DictionarySetting()
        {
            CheckPath();
            Console.WriteLine("Selected dictionary - " + "«" + dictionary + "»" + " of the " + language + " language. " + "Number of words: " + maxWordsArray);
        }
    }
}
