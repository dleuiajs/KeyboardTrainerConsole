using System;
using System.Reflection;

namespace KeyboardTrainerConsole
{
    internal class WordsDatabaseScript
    {
        public static string language = "english";
        public static string exePath;

        public static string[] words;
        public static int maxWordsArray;

        public static void LanguageSetting()
        {
            exePath = (Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + language + ".txt");
            words = File.ReadAllLines(exePath);
            maxWordsArray = words.Length;
            Console.WriteLine("Selected language - " + language + ". Number of words: " + maxWordsArray);
        }
    }
}
