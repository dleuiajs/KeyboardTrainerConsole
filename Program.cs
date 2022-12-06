﻿using System;

namespace KeyboardTrainerConsole
{
    internal class Program
    {
        // text
        static string textNeed;
        static string textEntered;

        // generate words
        static Random rnd = new Random();
        static bool dontGenerateNextWord = false;

        // sounds
        static bool enabledSounds = true;
        static int freqwin = 200; // частота выигрыша
        static int freqmiss = 100; // частота мисса
        static int durwin = 250; // дли-сть выигрыша
        static int durmiss = 250; // дли-сть проигрыша

        // statc
        static int level;
        static int exp; // exp += enteredCharacters с одного слова
        static int expNeed = 100; // сколько нужно exp для аппа уровня
        static int money; // деньги будут использоваться для открытия новых режимов
        static int enteredWords;
        static int enteredCharacters;
        static int wins;
        static int misses;

        static void Main() // start
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.InputEncoding = System.Text.Encoding.Unicode;

            ChangeLanguage();
            EnterText();
        }

        static void Commands()
        {
            if (textEntered == "/help")
            {
                dontGenerateNextWord = true;
                Console.WriteLine("Commands:" +
                "\n/help - list of all commands" +
                "\n/language - change the language" +
                "\n/sounds - customize the sounds" +
                "\n/stats - shows your game statistics" +
                "\n/exit - exit the game");
                EnterText();
            }
            else if (textEntered == "/language")
            {
                dontGenerateNextWord = true;
                ChangeLanguage();
                EnterText();
            }
            else if (textEntered == "/sounds")
            {
                dontGenerateNextWord = true;
                Console.WriteLine(
                "To disable sounds write - /disablesounds" +
                "\nTo enable sounds write - /enablesounds" +
                "\nTo change the frequency of the win sound, write - /freqwin" +
                "\nTo change the frequency of the miss sound, write - /freqmiss" +
                "\nTo change the duration of the win sound, write: - /durwin" +
                "\nTo change the duration of the miss sound, write: - /misswin"
                );
                EnterText();
            }
            else if (textEntered == "/disablesounds")
            {
                dontGenerateNextWord = true;
                enabledSounds = false;
                Console.WriteLine("Sounds are now disabled.");
                EnterText();
            }
            else if (textEntered == "/enablesounds")
            {
                dontGenerateNextWord = true;
                enabledSounds = true;
                Console.WriteLine("Sounds are now enabled.");
                EnterText();
            }
            else if (textEntered == "/freqwin")
            {
                Console.WriteLine("Enter frequency in Hz from 37 to 32767 (default 200)");
                dontGenerateNextWord = true;
                int num;
                textEntered = Console.ReadLine();
                bool tryParse = int.TryParse(textEntered, out num);
                if (tryParse)
                {
                    if (num >= 37 && num <= 32767)
                    {
                        freqwin = num;
                        Console.WriteLine("Now the frequency of the win sound is " + freqwin + " Hz.");
                        EnterText();
                    }
                    else
                    {
                        Console.WriteLine("The frequency cannot be less than 37 or more than 32767 hertz.");
                        EnterText();
                    }
                }
                else
                {
                    Console.WriteLine("Enter a number!");
                    EnterText();
                }
                EnterText();
            }
            else if (textEntered == "/freqmiss")
            {
                Console.WriteLine("Enter frequency in Hz from 37 to 32767 (default 100)");
                dontGenerateNextWord = true;
                int num;
                textEntered = Console.ReadLine();
                bool tryParse = int.TryParse(textEntered, out num);
                if (tryParse)
                {
                    if (num >= 37 && num <= 32767)
                    {
                        freqmiss = num;
                        Console.WriteLine("Now the frequency of the win sound is " + freqmiss + " Hz.");
                        EnterText();
                    }
                    else
                    {
                        Console.WriteLine("The frequency cannot be less than 37 or more than 32767 hertz.");
                        EnterText();
                    }
                }
                else
                {
                    Console.WriteLine("Enter a number!");
                    EnterText();
                }
                EnterText();
            }
            else if (textEntered == "/durwin")
            {
                Console.WriteLine("Enter frequency in ms from 1 to 10000 (default 250)");
                dontGenerateNextWord = true;
                int num;
                textEntered = Console.ReadLine();
                bool tryParse = int.TryParse(textEntered, out num);
                if (tryParse)
                {
                    if (num > 0 && num < 10000)
                    {
                        durwin = num;
                        Console.WriteLine("Now the duration of the win sound is " + durwin + " ms.");
                        EnterText();
                    }
                    else
                    {
                        Console.WriteLine("The sound duration cannot be less than 0 or more than 10000 ms.");
                        EnterText();
                    }
                }
                else
                {
                    Console.WriteLine("Enter a number!");
                    EnterText();
                }
                EnterText();
            }
            else if (textEntered == "/durmiss")
            {
                Console.WriteLine("Enter frequency in ms from 1 to 10000 (default 250)");
                dontGenerateNextWord = true;
                int num;
                textEntered = Console.ReadLine();
                bool tryParse = int.TryParse(textEntered, out num);
                if (tryParse)
                {
                    if (num > 0 && num < 10000)
                    {
                        durmiss = num;
                        Console.WriteLine("Now the duration of the win sound is " + durmiss + " ms.");
                        EnterText();
                    }
                    else
                    {
                        Console.WriteLine("The sound duration cannot be less than 0 or more than 10000 ms.");
                        EnterText();
                    }
                }
                else
                {
                    Console.WriteLine("Enter a number!");
                    EnterText();
                }
                EnterText();
            }
            else if (textEntered == "/stats")
            {
                dontGenerateNextWord = true;
                float winRatio = wins / (wins + misses) * 100f;
                Console.WriteLine("Your game statistics:" +
                "\nLevel: " + level +
                "\nExp: " + exp +
                "\nNeed exp for the next level: " + (expNeed - exp) +
                "\nWords entered: " + enteredWords +
                "\nCharacters entered: " + enteredCharacters +
                "\nWins: " + wins +
                "\nMisses: " + misses +
                "\nWin ratio: " + winRatio + "%"); // почему-то показывает 0
                EnterText();
            }

            else if (textEntered == "/exit")
            {
                dontGenerateNextWord = true;
                Console.WriteLine("Goodbye!");
                Environment.Exit(0);
                EnterText();
            }
            else if (textEntered.StartsWith("/"))
            {
                dontGenerateNextWord = true;
                Console.WriteLine("Unknown сommand!");
                EnterText();
            }
        }

        static void ChangeLanguage()
        {
            Console.WriteLine("Choose language:" +
            "\nenglish" +
            "\nukrainian" +
            "\nrussian");
            textEntered = Console.ReadLine();
            if (textEntered == "english" || textEntered == "English")
            {
                WordsDatabaseScript.language = "english";
                WordsDatabaseScript.LanguageSetting();
            }
            else if (textEntered == "russian" || textEntered == "Russian")
            {
                WordsDatabaseScript.language = "russian";
                WordsDatabaseScript.LanguageSetting();
            }
            else if (textEntered == "ukrainian" || textEntered == "Ukrainian")
            {
                WordsDatabaseScript.language = "ukrainian";
                WordsDatabaseScript.LanguageSetting();
            }
            else ChangeLanguage();
        }

        static void LevelCheck()
        {
            if (exp >= expNeed)
            {
                level += 1;
                exp -= expNeed;
                expNeed *= 2;
                Console.WriteLine("Level raised! You level is now " + level + "! The next level requires " + (expNeed - exp) + " exp.");
            }
        }

        static void TextGenerator()
        {
            int randomNum = rnd.Next(0, WordsDatabaseScript.maxWordsArray);
            textNeed = WordsDatabaseScript.words[randomNum];
        }

        static void Beep(int frequency, int duration)
        {
            if (enabledSounds) Console.Beep(frequency, duration);
        }

        static void EnterText()
        {
            if (!dontGenerateNextWord)
            {
                TextGenerator();
            }
            else
            {
                dontGenerateNextWord = false;
            }
            Console.WriteLine("Enter " + "«" + textNeed + "»");
            textEntered = Console.ReadLine();
            Commands();
            if (textEntered == textNeed)
            {
                wins += 1;
                enteredWords += 1;
                enteredCharacters += textEntered.Length;
                exp += textEntered.Length;
                LevelCheck();
                Console.WriteLine("Good job! " + "Wins: " + wins + ". Misses: " + misses + ". Characters entered: " + enteredCharacters);
                Beep(freqwin, durwin);
                EnterText();
            }
            else
            {
                misses += 1;
                //enteredCharacters += textEntered.Length;
                dontGenerateNextWord = true;
                Console.WriteLine("Miss! " + "Wins: " + wins + ". Misses: " + misses + ". Characters entered: " + enteredCharacters);
                Beep(freqmiss, durmiss);
                EnterText();
            }
        }
    }
}
