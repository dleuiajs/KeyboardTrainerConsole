using System;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using System.Threading;

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
        static int maxSymbols = 999; // максимальное кол-во символов в слове. 999 - default

        // sounds
        static bool enabledSounds = true;
        static int freqwin = 200; // частота выигрыша
        static int freqmiss = 100; // частота мисса
        static int durwin = 250; // дли-сть выигрыша
        static int durmiss = 250; // дли-сть проигрыша

        // stats
        static int level;
        static int exp; // exp += enteredCharacters с одного слова
        static int expNeed = 100; // сколько нужно exp для аппа уровня
        static int enteredWords;
        static int enteredWordsWhttErrors;
        static int enteredCharacters;
        static float wins;
        static float errors;

        // cpm wpm

        static int cpm = 0;
        static int maxCPM = 0;

        static int wpm = 0;
        static int maxWPM = 0;

        static double averageWordLength = 5.0; // Средняя длина слова (в символах) for english

        static int countAverageCPM = 0;
        static int sumAverageCPM = 0;
        static int averageCPM = 0;

        static int averageWPM = 0;

        // accounts
        static string nick = "Player"; // ник игрока
        static bool rememberAccount = false; // запомнить ли этот аккаунт для будущего входа 

        // saves
        //string md = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // path to my documents
        public static string[] saveGame; // массив с данными сохранения игры
        public static string[] saveSettings; // массив с данными сохранения настроек
        public static string[] filesSaves; // пути ко всем файлам сохранениям

        static void Main() // start
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.InputEncoding = System.Text.Encoding.Unicode;

            LoadSettings(); // загружаем настройки из сохранения

            // если аккаунт не запоминался
            if (!rememberAccount)
            {
                LoginInAccount(); // входим в аккаунт
            }
            else
            {
                LoadGame(); // загружаем игру
            }
            Console.WriteLine("Welcome, " + nick + "! For help with commands, type /help");
            EnterText();
        }

        static void LoginInAccount()
        {
            string textEntered; // введенный текст пользователя
            bool rembAccEnd = false; // узнали ли мы будет ли запоминаться этот аккаунт в след. раз

            Console.WriteLine("Enter your nickname");
            nick = Console.ReadLine();
            Console.WriteLine("Done! Your nick is " + nick);
            Console.WriteLine("Do you want to remember this account for future login? (you can change your account at any time by entering /account) (Y - Yes, N - No)");
            while (!rembAccEnd)
            {
                textEntered = Console.ReadLine();
                if (textEntered == "Y" || textEntered == "y" || textEntered == "Yes" || textEntered == "yes")
                {
                    rememberAccount = true;
                    rembAccEnd = true;
                }
                else if (textEntered == "N" || textEntered == "n" || textEntered == "No" || textEntered == "no")
                {
                    rememberAccount = false;
                    rembAccEnd = true;
                }
                else
                {
                    Console.WriteLine("Unknown answer! Write Y (Yes) or N (No)");
                }
            }
            SaveSettings();
            LoadGame(); // загружаем игру
        }

        static void LoadGame()
        {
            try
            {
                saveGame = File.ReadAllLines(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\.game\\saves\\" + nick + ".save"); // загружаем сохранения игры
                // лоадинг статистики
                level = Convert.ToInt32(saveGame[0]);
                exp = Convert.ToInt32(saveGame[1]);
                enteredWords = Convert.ToInt32(saveGame[2]);
                enteredCharacters = Convert.ToInt32(saveGame[3]);
                wins = Convert.ToInt32(saveGame[4]);
                errors = Convert.ToInt32(saveGame[5]);
                WordsDatabaseScript.language = saveGame[6];
                if (WordsDatabaseScript.language == "none")
                {
                    ChangeLanguage();
                }
                else
                {
                    WordsDatabaseScript.LanguageSetting();
                }
                WordsDatabaseScript.dictionary = saveGame[7];
                WordsDatabaseScript.DictionarySetting();
                expNeed = Convert.ToInt32(saveGame[8]);
                maxSymbols = Convert.ToInt32(saveGame[9]);
                enteredWordsWhttErrors = Convert.ToInt32(saveGame[10]);
                maxCPM = Convert.ToInt32(saveGame[11]);
                averageCPM = Convert.ToInt32(saveGame[12]);
                maxWPM = Convert.ToInt32(Math.Floor(maxCPM / averageWordLength));
                averageWPM = Convert.ToInt32(Math.Floor(averageCPM / averageWordLength));
                countAverageCPM = Convert.ToInt32(saveGame[13]);
                sumAverageCPM = Convert.ToInt32(saveGame[14]);
                for (int i = 0; i < Achievements.text.Length; i++)
                {
                    if (saveGame[i + 15] == "0")
                    {
                        Achievements.completedAch[i] = false;
                    }
                    else if (saveGame[i + 15] == "1")
                    {
                        Achievements.completedAch[i] = true;
                    }
                }
            }
            catch (Exception e)
            {
                try
                {
                    File.WriteAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\.game\\saves\\" + nick + ".save",
                    0 + "\n" + // 0
                    0 + "\n" + // 1
                    0 + "\n" + // 2
                    0 + "\n" + // 3
                    0 + "\n" + // 4
                    0 + "\n" + // 5
                    "none" + "\n" +  // 6
                    "nouns" + "\n" +  // 7
                    100 + "\n" +  // 8
                    999 + "\n" +  // 9
                    0 + "\n" + // 10
                    0 + "\n" + // 11
                    0 + "\n" + // 12
                    0 + "\n" + // 13
                    0 + "\n" + // 14
                    0 + "\n" + 0 + "\n" + 0 + "\n" + 0 + "\n" + 0 + "\n" + 0 + "\n" + 0 + "\n" + 0 + "\n" + 0 + "\n" + 0 + "\n" + 0 + "\n" + 0 + "\n" + 0 + "\n" + 0 + "\n" + 0 + "\n" + 0 + "\n" + 0 + "\n" + 0 + "\n" + 0 + "\n" + 0 + "\n" + 0 + "\n" + 0 + "\n" + 0 + "\n" + 0 + "\n" + 0 + "\n" + 0 + "\n" + 0 + "\n" + 0 + "\n" //15 - 43 (achts)
                    ); // !!! после добавления новой переменной в сохранение обязательно добавить и сюда
                }
                catch (Exception e2)
                {
                    Console.WriteLine("Exception: " + e2.Message);
                }
                finally
                {
                    LoadGame();
                }
            }
            finally
            {
            }
        }

        static void LoadSettings()
        {
            try
            {
                saveSettings = File.ReadAllLines(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\.game\\settings.cfg"); // загружаем сохранения настроек
                // лоадинг настроек
                enabledSounds = Convert.ToBoolean(saveSettings[0]);
                freqwin = Convert.ToInt32(saveSettings[1]);
                freqmiss = Convert.ToInt32(saveSettings[2]);
                durwin = Convert.ToInt32(saveSettings[3]);
                durmiss = Convert.ToInt32(saveSettings[4]);
                if (saveSettings[6] == "True")
                {
                    rememberAccount = true;
                    nick = saveSettings[5];
                }
                else if (saveSettings[6] == "False")
                {
                    rememberAccount = false;
                }
            }
            catch (Exception e)
            {
                SaveSettings();
            }
            finally
            {
            }
        }

        static void SaveGame()
        {
            try
            {
                // перезаписываем данные
                File.WriteAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\.game\\saves\\" + nick + ".save",
                level + "\n" + // 0
                exp + "\n" +  // 1
                enteredWords + "\n" +  // 2
                enteredCharacters + "\n" +  // 3
                wins + "\n" +  // 4
                errors + "\n" +  // 5
                WordsDatabaseScript.language + "\n" +  // 6
                WordsDatabaseScript.dictionary + "\n" +  // 7
                expNeed + "\n" +  // 8
                maxSymbols + "\n" + // 9
                enteredWordsWhttErrors + "\n" + // 10
                maxCPM + "\n" + // 11
                averageCPM + "\n" + // 12
                countAverageCPM + "\n" + // 13
                sumAverageCPM + "\n" // 14
                );
                for (int i = 0; i < Achievements.text.Length; i++)
                {
                    if (!Achievements.completedAch[i])
                    {
                        File.AppendAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\.game\\saves\\" + nick + ".save", 0 + "\n");
                    }
                    else
                    {
                        File.AppendAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\.game\\saves\\" + nick + ".save", 1 + "\n");
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
            }
        }

        static void SaveSettings()
        {
            try
            {
                // перезаписываем данные
                File.WriteAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\.game\\settings.cfg",
                enabledSounds + "\n" + freqwin + "\n" + freqmiss + "\n" + durwin + "\n" + durmiss + "\n" + nick + "\n" + rememberAccount);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
            }
        }

        static void OpenFrame()
        {
            Console.WriteLine("==============================");
        }

        static void ConsoleWriteLineColor(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        static void Commands()
        {
            if (textEntered.StartsWith("/"))
            {
                if (textEntered == "/help")
                {
                    OpenFrame();
                    dontGenerateNextWord = true;
                    Console.WriteLine("Commands:" +
                    "\n/help - list of all commands" +
                    "\n/language - change the language" +
                    "\n/dictionary - change the dictionary" +
                    "\n/difficulty - change the difficulty" +
                    "\n/sounds - customize the sounds" +
                    "\n/stats - shows your game statistics" +
                    "\n/achts - shows completed and uncompleted achievements" +
                    "\n/top - top players" +
                    "\n/account - change account" +
                    "\n/about - information about the game" +
                    "\n/exit - exit the game");
                    OpenFrame();
                    EnterText();
                }
                else if (textEntered == "/achts")
                {
                    dontGenerateNextWord = true;
                    OpenFrame();
                    for (int i = 0; i < Achievements.text.Length; i++)
                    {
                        if (Achievements.completedAch[i])
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine((i + 1) + ". " + Achievements.text[i] + " - " + Achievements.exp[i] + " EXP [Completed]");
                            Console.ResetColor();
                        }
                        if (!Achievements.completedAch[i])
                        {
                            //Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine((i + 1) + ". " + Achievements.text[i] + " - " + Achievements.exp[i] + " EXP [Not completed]");
                            //Console.ResetColor();
                        }
                    }
                    OpenFrame();
                    EnterText();
                }
                else if (textEntered == "/difficulty")
                {
                    string difficultyEntered;
                    OpenFrame();

                    ConsoleWriteLineColor("Please note that if the difficulty is not \"Normal\", then the statistics will not be updated.", ConsoleColor.DarkRed);

                    Console.WriteLine("Specify the difficulty:" +
                    "\n1 - Kid (up to 3 characters per word)" +
                    "\n2 - Very easy (up to 5 characters per word)" +
                    "\n3 - Easy (up to 7 characters per word)" +
                    "\n4 - Medium (up to 10 characters per word)" +
                    "\n5 - Normal (unlimited characters per word) - default");
                    OpenFrame();
                    difficultyEntered = Console.ReadLine();
                    if (difficultyEntered == "1")
                    {
                        maxSymbols = 3;
                        ConsoleWriteLineColor("Selected difficulty - Kid", ConsoleColor.Cyan);
                        Achievements.completedAch[24] = true;
                        AchievementsCheck(24);
                    }
                    else if (difficultyEntered == "2")
                    {
                        maxSymbols = 5;
                        ConsoleWriteLineColor("Selected difficulty - Very easy", ConsoleColor.Cyan);
                    }
                    else if (difficultyEntered == "3")
                    {
                        maxSymbols = 7;
                        ConsoleWriteLineColor("Selected difficulty - Easy", ConsoleColor.Cyan);
                    }
                    else if (difficultyEntered == "4")
                    {
                        maxSymbols = 10;
                        ConsoleWriteLineColor("Selected difficulty - Medium", ConsoleColor.Cyan);
                    }
                    else if (difficultyEntered == "5")
                    {
                        maxSymbols = 999;
                        ConsoleWriteLineColor("Selected difficulty - Normal", ConsoleColor.Cyan);
                    }
                    else
                    {
                        ConsoleWriteLineColor("Unknown answer! Enter a number from 1 to 5.", ConsoleColor.Red);
                        dontGenerateNextWord = true;
                    }
                    SaveGame();
                    EnterText();
                }
                else if (textEntered == "/about")
                {
                    dontGenerateNextWord = true;
                    OpenFrame();
                    Console.WriteLine("██╗░░██╗████████╗░█████╗░" +
                                    "\n██║░██╔╝╚══██╔══╝██╔══██╗" +
                                    "\n█████═╝░░░░██║░░░██║░░╚═╝" +
                                    "\n██╔═██╗░░░░██║░░░██║░░██╗" +
                                    "\n██║░╚██╗░░░██║░░░╚█████╔╝" +
                                    "\n╚═╝░░╚═╝░░░╚═╝░░░░╚════╝░");
                    Console.WriteLine("Developer: dleuiajs \nVersion: 1.005");
                    OpenFrame();
                    Achievements.completedAch[27] = true;
                    AchievementsCheck(27);
                    EnterText();
                }
                else if (textEntered == "/top")
                {
                    dontGenerateNextWord = true;
                    filesSaves = Directory.GetFiles(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\.game\\saves\\");
                    OpenFrame();
                    Console.WriteLine("Top players:");
                    string[] arrayNick = new string[0];
                    int[] arrayLevel = new int[0];
                    string top = "";
                    for (int i = 0; i < filesSaves.Length; i++)
                    {
                        string[] save = File.ReadAllLines(filesSaves[i]);
                        string nick = Path.GetFileName(filesSaves[i]).Replace(".save", "");
                        Array.Resize(ref arrayNick, arrayNick.Length + 1);
                        Array.Resize(ref arrayLevel, arrayLevel.Length + 1);
                        arrayNick[i] = nick;
                        arrayLevel[i] = Convert.ToInt32(save[0]);
                    }
                    int temp;
                    string tempString;
                    for (int i = 0; i < arrayLevel.Length - 1; i++)
                    {
                        for (int j = i + 1; j < arrayLevel.Length; j++)
                        {
                            if (arrayLevel[i] < arrayLevel[j])
                            {
                                temp = arrayLevel[i];
                                arrayLevel[i] = arrayLevel[j];
                                arrayLevel[j] = temp;
                                tempString = arrayNick[i];
                                arrayNick[i] = arrayNick[j];
                                arrayNick[j] = tempString;
                            }
                        }
                    }
                    for (int i = 0; i < arrayLevel.Length; i++)
                    {
                        top += arrayNick[i] + " - " + arrayLevel[i] + " lvl" + "\n";
                    }
                    Console.WriteLine(top);
                    OpenFrame();
                    EnterText();
                }
                else if (textEntered == "/account")
                {
                    LoginInAccount();
                    EnterText();
                }
                // else if (textEntered == "/cheat")
                // {
                //     dontGenerateNextWord = true;
                //     exp += expNeed;
                //     LevelCheck();
                //     EnterText();
                // }
                else if (textEntered == "/language")
                {
                    dontGenerateNextWord = true;
                    ChangeLanguage();
                    ConsoleWriteLineColor("Selected language - " + WordsDatabaseScript.language + ", dictionary - " + "«" + WordsDatabaseScript.dictionary + "». " + "Number of words: " + WordsDatabaseScript.maxWordsArray, ConsoleColor.Cyan);
                    EnterText();
                }
                else if (textEntered == "/dictionary")
                {
                    dontGenerateNextWord = true;
                    ChangeDictionary();
                    EnterText();
                }
                else if (textEntered == "/sounds")
                {
                    dontGenerateNextWord = true;
                    OpenFrame();
                    Console.WriteLine(
                    "To disable sounds write - /disablesounds" +
                    "\nTo enable sounds write - /enablesounds" +
                    "\nTo change the frequency of the win sound, write - /freqwin" +
                    "\nTo change the frequency of the miss sound, write - /freqmiss" +
                    "\nTo change the duration of the win sound, write: - /durwin" +
                    "\nTo change the duration of the miss sound, write: - /misswin"
                    );
                    OpenFrame();
                    EnterText();
                }
                else if (textEntered == "/disablesounds")
                {
                    dontGenerateNextWord = true;
                    enabledSounds = false;
                    ConsoleWriteLineColor("Sounds are now disabled.", ConsoleColor.Cyan);
                    SaveSettings();
                    EnterText();
                }
                else if (textEntered == "/enablesounds")
                {
                    dontGenerateNextWord = true;
                    enabledSounds = true;
                    ConsoleWriteLineColor("Sounds are now enabled.", ConsoleColor.Cyan);
                    SaveSettings();
                    EnterText();
                }
                else if (textEntered == "/freqwin")
                {
                    ConsoleWriteLineColor("Enter frequency in Hz from 37 to 32767 (default 200)", ConsoleColor.White);
                    dontGenerateNextWord = true;
                    int num;
                    textEntered = Console.ReadLine();
                    bool tryParse = int.TryParse(textEntered, out num);
                    if (tryParse)
                    {
                        if (num >= 37 && num <= 32767)
                        {
                            freqwin = num;
                            ConsoleWriteLineColor("Now the frequency of the win sound is " + freqwin + " Hz.", ConsoleColor.Cyan);
                            SaveSettings();
                            Achievements.completedAch[26] = true;
                            AchievementsCheck(26);
                            EnterText();
                        }
                        else
                        {
                            ConsoleWriteLineColor("The frequency cannot be less than 37 or more than 32767 hertz.", ConsoleColor.Red);
                            EnterText();
                        }
                    }
                    else
                    {
                        ConsoleWriteLineColor("Enter a number!", ConsoleColor.Red);
                        EnterText();
                    }
                    EnterText();
                }
                else if (textEntered == "/freqmiss")
                {
                    ConsoleWriteLineColor("Enter frequency in Hz from 37 to 32767 (default 100)", ConsoleColor.White);
                    dontGenerateNextWord = true;
                    int num;
                    textEntered = Console.ReadLine();
                    bool tryParse = int.TryParse(textEntered, out num);
                    if (tryParse)
                    {
                        if (num >= 37 && num <= 32767)
                        {
                            freqmiss = num;
                            ConsoleWriteLineColor("Now the frequency of the win sound is " + freqmiss + " Hz.", ConsoleColor.Cyan);
                            SaveSettings();
                            Achievements.completedAch[26] = true;
                            AchievementsCheck(26);
                            EnterText();
                        }
                        else
                        {
                            ConsoleWriteLineColor("The frequency cannot be less than 37 or more than 32767 hertz.", ConsoleColor.Red);
                            EnterText();
                        }
                    }
                    else
                    {
                        ConsoleWriteLineColor("Enter a number!", ConsoleColor.Red);
                        EnterText();
                    }
                    EnterText();
                }
                else if (textEntered == "/durwin")
                {
                    ConsoleWriteLineColor("Enter frequency in ms from 1 to 10000 (default 250)", ConsoleColor.White);
                    dontGenerateNextWord = true;
                    int num;
                    textEntered = Console.ReadLine();
                    bool tryParse = int.TryParse(textEntered, out num);
                    if (tryParse)
                    {
                        if (num > 0 && num < 10000)
                        {
                            durwin = num;
                            ConsoleWriteLineColor("Now the duration of the win sound is " + durwin + " ms.", ConsoleColor.Cyan);
                            SaveSettings();
                            EnterText();
                        }
                        else
                        {
                            ConsoleWriteLineColor("The sound duration cannot be less than 0 or more than 10000 ms.", ConsoleColor.Red);
                            EnterText();
                        }
                    }
                    else
                    {
                        ConsoleWriteLineColor("Enter a number!", ConsoleColor.Red);
                        EnterText();
                    }
                    EnterText();
                }
                else if (textEntered == "/durmiss")
                {
                    ConsoleWriteLineColor("Enter frequency in ms from 1 to 10000 (default 250)", ConsoleColor.White);
                    dontGenerateNextWord = true;
                    int num;
                    textEntered = Console.ReadLine();
                    bool tryParse = int.TryParse(textEntered, out num);
                    if (tryParse)
                    {
                        if (num > 0 && num < 10000)
                        {
                            durmiss = num;
                            ConsoleWriteLineColor("Now the duration of the miss sound is " + durmiss + " ms.", ConsoleColor.Cyan);
                            SaveSettings();
                            EnterText();
                        }
                        else
                        {
                            ConsoleWriteLineColor("The sound duration cannot be less than 0 or more than 10000 ms.", ConsoleColor.Red);
                            EnterText();
                        }
                    }
                    else
                    {
                        ConsoleWriteLineColor("Enter a number!", ConsoleColor.Red);
                        EnterText();
                    }
                    EnterText();
                }
                else if (textEntered == "/stats")
                {
                    dontGenerateNextWord = true;
                    float winRatio = wins / (wins + errors) * 100f;
                    OpenFrame();
                    Console.WriteLine("Your game statistics:" +
                    "\nNickname: " + nick +
                    "\nLevel: " + level +
                    "\nExp: " + exp +
                    "\nNeed exp for the next level: " + (expNeed - exp) +
                    "\nWords entered: " + enteredWords +
                    "\nWords enteres without errors in a row: " + enteredWordsWhttErrors +
                    "\nCharacters entered: " + enteredCharacters +
                    "\nWins: " + wins +
                    "\nErrors: " + errors +
                    "\nWin ratio: " + Math.Round(winRatio, 2) + "%" +
                    "\nMax WPM: " + maxWPM +
                    "\nMax CPM: " + maxCPM +
                    "\nAverage WPM: " + averageWPM +
                    "\nAverage CPM: " + averageCPM
                    );
                    OpenFrame();
                    EnterText();
                }

                else if (textEntered == "/exit")
                {
                    dontGenerateNextWord = true;
                    ConsoleWriteLineColor("Goodbye!", ConsoleColor.Cyan);
                    Environment.Exit(0);
                    EnterText();
                }
                else if (textEntered.StartsWith("/"))
                {
                    dontGenerateNextWord = true;
                    ConsoleWriteLineColor("Unknown сommand!", ConsoleColor.Red);
                    EnterText();
                }
            }
        }

        static void ChangeLanguage()
        {
            OpenFrame();
            Console.WriteLine("Select a language:" +
            "\nenglish" +
            "\nukrainian" +
            "\nrussian");
            OpenFrame();
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
            else
            {
                ConsoleWriteLineColor("Unknown language!", ConsoleColor.Red);
                ChangeLanguage();
            }
            SaveGame();
        }

        static bool changeDictionaryWrited = false;
        const int nounsLvl = 0;
        const int lettersLvl = 2;
        const int symbolsLvl = 3;
        const int adjectivesLvl = 5;
        const int verbsLvl = 10;
        const int surnamesLvl = 15;
        const int allLvl = 20;
        static void ChangeDictionary()
        {
            if (changeDictionaryWrited == false)
            {
                OpenFrame();
                Console.WriteLine("Select a dictionary:" +
                "\nnouns" + " (" + nounsLvl + " lvl)" +
                "\nletters" + " (" + lettersLvl + " lvl)" +
                "\nsymbols" + " (" + symbolsLvl + " lvl)" +
                "\nadjectives" + " (" + adjectivesLvl + " lvl)" +
                "\nverbs" + " (" + verbsLvl + " lvl)" +
                "\nsurnames" + " (" + surnamesLvl + " lvl)" +
                "\nall" + " (" + allLvl + " lvl)"
                );
                OpenFrame();
            }
            else
            {
                changeDictionaryWrited = false;
            }
            textEntered = Console.ReadLine();
            if (textEntered == "nouns" || textEntered == "Nouns")
            {
                WordsDatabaseScript.dictionary = "nouns";
                WordsDatabaseScript.DictionarySetting();
            }
            else if (textEntered == "letters" || textEntered == "Letters")
            {
                if (level >= lettersLvl)
                {
                    WordsDatabaseScript.dictionary = "letters";
                    WordsDatabaseScript.DictionarySetting();
                }
                else
                {
                    ConsoleWriteLineColor("For the dictionary «letters» you need level " + lettersLvl + "!", ConsoleColor.Red);
                    changeDictionaryWrited = true;
                    ChangeDictionary();
                }
            }
            else if (textEntered == "symbols" || textEntered == "Symbols")
            {
                if (level >= symbolsLvl)
                {
                    WordsDatabaseScript.dictionary = "symbols";
                    WordsDatabaseScript.DictionarySetting();
                }
                else
                {
                    ConsoleWriteLineColor("For the dictionary «symbols» you need level " + symbolsLvl + "!", ConsoleColor.Red);
                    changeDictionaryWrited = true;
                    ChangeDictionary();
                }
            }
            else if (textEntered == "adjectives" || textEntered == "Adjectives")
            {
                if (level >= adjectivesLvl)
                {
                    WordsDatabaseScript.dictionary = "adjectives";
                    WordsDatabaseScript.DictionarySetting();
                }
                else
                {
                    ConsoleWriteLineColor("For the dictionary «adjectives» you need level " + adjectivesLvl + "!", ConsoleColor.Red);
                    changeDictionaryWrited = true;
                    ChangeDictionary();
                }
            }
            else if (textEntered == "verbs" || textEntered == "Verbs")
            {
                if (level >= verbsLvl)
                {
                    WordsDatabaseScript.dictionary = "verbs";
                    WordsDatabaseScript.DictionarySetting();
                }
                else
                {
                    ConsoleWriteLineColor("For the dictionary «verbs» you need level " + verbsLvl + "!", ConsoleColor.Red);
                    changeDictionaryWrited = true;
                    ChangeDictionary();
                }
            }
            else if (textEntered == "surnames" || textEntered == "Surnames")
            {
                if (level >= surnamesLvl)
                {
                    WordsDatabaseScript.dictionary = "surnames";
                    WordsDatabaseScript.DictionarySetting();
                }
                else
                {
                    ConsoleWriteLineColor("For the dictionary «surnames» you need level " + surnamesLvl + "!", ConsoleColor.Red);
                    changeDictionaryWrited = true;
                    ChangeDictionary();
                }
            }
            else if (textEntered == "all" || textEntered == "All")
            {
                if (level >= allLvl)
                {
                    WordsDatabaseScript.dictionary = "all";
                    WordsDatabaseScript.DictionarySetting();
                }
                else
                {
                    ConsoleWriteLineColor("For the dictionary «all» you need level " + allLvl + "!", ConsoleColor.Red);
                    changeDictionaryWrited = true;
                    ChangeDictionary();
                }
            }
            else
            {
                ConsoleWriteLineColor("Unknown dictionary!", ConsoleColor.Red);
                changeDictionaryWrited = true;
                ChangeDictionary();
            }
            SaveGame();
        }

        static void Checking()
        {
            LevelCheck();
            AchWordsCheck();
            AchLettersInWordsCheck();
            AchWordsWhttErrorsCheck();
            AchWPMCheck();
            AchCharactersCheck();
            AchLanguagesCheck();
        }

        static void LevelCheck()
        {
            double floatExpNeed;
            if (exp >= expNeed)
            {
                level += 1;
                exp -= expNeed;
                floatExpNeed = Math.Round(expNeed * 1.5f, 0);
                expNeed = Convert.ToInt32(floatExpNeed);
                ConsoleWriteLineColor("Level raised! You level is now " + level + "! The next level requires " + (expNeed - exp) + " exp.", ConsoleColor.Magenta);
                SaveGame();
            }
        }


        static void AchievementsCheck(int achID)
        {
            if (Achievements.completedAch[achID])
            {
                ConsoleWriteLineColor("You have completed the achievement \"" + Achievements.text[achID] + "\"!", ConsoleColor.DarkMagenta);
                exp += Achievements.exp[achID];
                LevelCheck();
            }
        }

        // ----------------- Achievements:

        static int achWordsID = 0;
        static void AchWordsCheck()
        {
            if (achWordsID <= 4)
            {
                if (enteredWords >= Achievements.need[achWordsID] && !Achievements.completedAch[achWordsID])
                {
                    Achievements.completedAch[achWordsID] = true;
                    AchievementsCheck(achWordsID);
                    achWordsID++;
                }
            }
        }

        static int achLettersInWordsID = 5;
        static void AchLettersInWordsCheck()
        {
            if (achLettersInWordsID <= 7)
            {
                if (textEntered.Length >= Achievements.need[achLettersInWordsID] && !Achievements.completedAch[achLettersInWordsID])
                {
                    Achievements.completedAch[achLettersInWordsID] = true;
                    AchievementsCheck(achLettersInWordsID);
                    achLettersInWordsID++;
                }
            }
        }

        static int achWordsWhttErrorsID = 8;
        static void AchWordsWhttErrorsCheck()
        {
            if (achWordsWhttErrorsID <= 11)
            {
                if (enteredWordsWhttErrors >= Achievements.need[achWordsWhttErrorsID] && !Achievements.completedAch[achWordsWhttErrorsID])
                {
                    Achievements.completedAch[achWordsWhttErrorsID] = true;
                    AchievementsCheck(achWordsWhttErrorsID);
                    achWordsWhttErrorsID++;
                }
            }
        }

        static int achWPMID = 12;
        static void AchWPMCheck()
        {
            if (achWPMID <= 16)
            {
                if (averageWPM >= Achievements.need[achWPMID] && !Achievements.completedAch[achWPMID])
                {
                    Achievements.completedAch[achWPMID] = true;
                    AchievementsCheck(achWPMID);
                    achWPMID++;
                }
            }
        }

        static int achCharactersID = 17;
        static void AchCharactersCheck()
        {
            if (achCharactersID <= 20)
            {
                if (enteredCharacters >= Achievements.need[achCharactersID] && !Achievements.completedAch[achCharactersID])
                {
                    Achievements.completedAch[achCharactersID] = true;
                    AchievementsCheck(achCharactersID);
                    achCharactersID++;
                }
            }
        }

        static int languagesUsed = 0;
        static int achLanguagesID = 21;
        static void AchLanguagesCheck() // сделать когда добавлю побольше языков
        {
            if (achLanguagesID <= 23)
            {
                if (languagesUsed >= Achievements.need[achLanguagesID] && !Achievements.completedAch[achLanguagesID])
                {
                    Achievements.completedAch[achLanguagesID] = true;
                    AchievementsCheck(achLanguagesID);
                    achLanguagesID++;
                }
            }
        }

        // __________________

        static void TextGenerator()
        {
            int randomNum = rnd.Next(0, WordsDatabaseScript.maxWordsArray);
            if (WordsDatabaseScript.words[randomNum].Length <= maxSymbols)
            {
                textNeed = WordsDatabaseScript.words[randomNum];
            }
            else
            {
                TextGenerator();
            }
        }

        static void Beep(int frequency, int duration)
        {
            Thread beepThread = new Thread(() => BeepThread(frequency, duration));
            beepThread.Start();
        }

        static void BeepThread(int frequency, int duration)
        {
            if (enabledSounds) Console.Beep(frequency, duration);
        }

        static void EnterText()
        {
            System.Text.Encoding tempOutputEncoding = Console.OutputEncoding;
            System.Text.Encoding tempInputEncoding = Console.InputEncoding;
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            bool stopTimer = false;
            Stopwatch stopwatch = new Stopwatch();
            Thread timerThread = new Thread(() => TimerThread(stopwatch, ref stopTimer));

            if (!dontGenerateNextWord)
            {
                TextGenerator();
            }
            else
            {
                dontGenerateNextWord = false;
            }
            ConsoleWriteLineColor("Enter " + "«" + textNeed + "»", ConsoleColor.Yellow);
            ConsoleKeyInfo keyInfo = Console.ReadKey();

            timerThread.Start();

            Console.OutputEncoding = tempOutputEncoding;
            Console.InputEncoding = tempInputEncoding;

            string textEnteredBefore = Console.ReadLine();

            textEntered = keyInfo.KeyChar.ToString() + textEnteredBefore;

            stopTimer = true; // Устанавливаем флаг, чтобы остановить поток
            timerThread.Join(); // Дожидаемся завершения потока


            Commands();
            if (textEntered == textNeed)
            {
                double elapsedSeconds = stopwatch.ElapsedMilliseconds / 1000.0;
                int cpm = Convert.ToInt32(Math.Floor(textEntered.Length / elapsedSeconds * 60.0));
                int wpm = Convert.ToInt32(Math.Floor(cpm / averageWordLength));
                if (maxSymbols == 999)
                {
                    wins += 1;
                    enteredWords += 1;
                    enteredWordsWhttErrors += 1;
                    enteredCharacters += textEntered.Length;
                    exp += textEntered.Length;

                    if (cpm > maxCPM)
                    {
                        maxCPM = cpm;
                        maxWPM = wpm;
                    }
                    countAverageCPM++;
                    sumAverageCPM += cpm;
                    averageCPM = sumAverageCPM / countAverageCPM;
                    averageWPM = Convert.ToInt32(Math.Floor(averageCPM / averageWordLength));

                    Checking();
                }
                ConsoleWriteLineColor("Good job! " + "Wins: " + wins + ". Errors: " + errors + ". Characters entered: " + enteredCharacters + ". WPM: " + wpm, ConsoleColor.Green);
                Beep(freqwin, durwin);
            }
            else
            {
                if (maxSymbols == 999)
                {
                    enteredWordsWhttErrors = 0;
                    errors += 1;
                }
                dontGenerateNextWord = true;
                ConsoleWriteLineColor("Miss! " + "Wins: " + wins + ". Errors: " + errors + ". Characters entered: " + enteredCharacters, ConsoleColor.Red);
                Beep(freqmiss, durmiss);
            }
            SaveGame();
            EnterText();
        }


        static void TimerThread(Stopwatch stopwatch, ref bool stopTimer)
        {
            stopwatch.Start();

            while (!stopTimer)
            {
                TimeSpan elapsed = stopwatch.Elapsed;
            }
        }
    }
}
