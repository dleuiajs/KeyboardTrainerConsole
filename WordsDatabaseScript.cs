using System;
using System.Reflection;

namespace KeyboardTrainerConsole
{
    internal class WordsDatabaseScript
    {
        public static string language = "none";
        public static string dictionary = "nouns";
        public static string path;

        public static string[] words;
        public static int maxWordsArray;

        static void CheckPath()
        {
            try
            {
                path = (Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\.game\\languages\\" + language + "_" + dictionary + ".txt"); // сделать потом если ошибка то искать на других языках
                words = File.ReadAllLines(path);
                maxWordsArray = words.Length;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unfortunately, this dictionary is not yet available in " + language + ". Try with another language.");
                dictionary = "nouns";
                // CheckPath();
            }
        }

        public static void LanguageSetting()
        {
            CheckPath();
        }

        public static void DictionarySetting()
        {
            CheckPath();
            Console.WriteLine("Selected dictionary - " + "«" + dictionary + "»" + " of the " + language + " language. " + "Number of words: " + maxWordsArray);
        }
    }
}
