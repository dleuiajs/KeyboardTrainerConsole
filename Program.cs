using System;
using System.Reflection;

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

        // stats
        static int level;
        static int exp; // exp += enteredCharacters с одного слова
        static int expNeed = 100; // сколько нужно exp для аппа уровня
        // static int money; // деньги будут использоваться для открытия новых режимов
        static int enteredWords;
        static int enteredCharacters;
        static float wins;
        static float misses;

        // accounts
        static string nick = "Player"; // ник игрока
        static bool rememberAccount = false; // запомнить ли этот аккаунт для будущего входа 

        // saves
        public static string[] saveGame;
        public static string[] saveSettings;

        static void Main() // start
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.InputEncoding = System.Text.Encoding.Unicode;

            LoadSettings(); // загружаем настройки из сохранения

            // если аккаунт не запоминался
            if (rememberAccount == false) LoginInAccount(); // входим в аккаунт
            LoadGame(); // загружаем игру
            EnterText();
        }

        static void LoginInAccount()
        {
            string textEntered; // введенный текст пользователя
            bool rembAccEnd = false; // узнали ли мы будет ли запоминаться этот аккаунт в след. раз

            Console.WriteLine("Enter your nickname (minimum 3 ch.)");
            nick = Console.ReadLine();
            Console.WriteLine("Готово! Ваш ник - " + nick);
            Console.WriteLine("Хотите ли вы запомнить данный аккаунт для будущего входа? (в любой момент вы сможете сменить аккаунт введя /account) (Y - Yes, N - No)");
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
                    Console.WriteLine("Неизвестный ответ! Напишите Y (Yes) или N (No)");
                }
            }
            SaveSettings();
            LoadGame(); // загружаем игру
        }

        static void LoadGame()
        {
            try
            {
                saveGame = File.ReadAllLines(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\saves\\" + nick + ".save"); // загружаем сохранения игры
                                                                                                                                              // лоадинг статистики
                level = Convert.ToInt32(saveGame[0]);
                exp = Convert.ToInt32(saveGame[1]);
                enteredWords = Convert.ToInt32(saveGame[2]);
                enteredCharacters = Convert.ToInt32(saveGame[3]);
                wins = Convert.ToInt32(saveGame[4]);
                misses = Convert.ToInt32(saveGame[5]);
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
            }
            catch (Exception e)
            {
                //Console.WriteLine("Exception: " + e.Message);
                Console.WriteLine("Ошибка! Создаем файл сохранения заново.");
                try
                {
                    File.WriteAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\saves\\" + nick + ".save",
                    0 + "\n" + 0 + "\n" + 0 + "\n" + 0 + "\n" + 0 + "\n" + 0 + "\n" + "none" + "\n" + "nouns");
                }
                catch (Exception e2)
                {
                    Console.WriteLine("Exception: " + e2.Message);
                }
                finally
                {
                    Console.WriteLine("Файл сохранения успешно создан!");
                    LoadGame();
                }
            }
            finally
            {
                Console.WriteLine("Игра успешно загружена!");
            }
        }

        static void LoadSettings()
        {
            try
            {
                saveSettings = File.ReadAllLines(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\saves\\" + "settings.cfg"); // загружаем сохранения настроек
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
                //Console.WriteLine("Exception: " + e.Message);
                Console.WriteLine("Ошибка! Создаем файл настроек заново.");
                SaveSettings();
            }
            finally
            {
                Console.WriteLine("Настройки успешно загружены!");
            }
        }

        static void SaveGame()
        {
            try
            {
                //StreamWriter sw = new StreamWriter(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\saves\\" + nick + ".save", true, System.Text.Encoding.UTF8); // открытие файла
                //sw.WriteLine(nick + "\n" + level + "\n" + exp); // записываем данные
                // перезаписываем данные
                File.WriteAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\saves\\" + nick + ".save",
                level + "\n" + exp + "\n" + enteredWords + "\n" + enteredCharacters + "\n" + wins + "\n" + misses + "\n" + WordsDatabaseScript.language + "\n" + WordsDatabaseScript.dictionary);
                //sw.Close(); // закрываем файл
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Игра успешно сохранена!");
            }
        }

        static void SaveSettings()
        {
            try
            {
                // перезаписываем данные
                File.WriteAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\saves\\" + "settings.cfg",
                enabledSounds + "\n" + freqwin + "\n" + freqmiss + "\n" + durwin + "\n" + durmiss + "\n" + nick + "\n" + rememberAccount);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Настройки успешно сохранены!");
            }
        }

        static void Commands()
        {
            if (textEntered.StartsWith("/"))
            {
                if (textEntered == "/help")
                {
                    dontGenerateNextWord = true;
                    Console.WriteLine("Commands:" +
                    "\n/help - list of all commands" +
                    "\n/language - change the language" +
                    "\n/dictionary - change the dictionary" +
                    "\n/sounds - customize the sounds" +
                    "\n/stats - shows your game statistics" +
                    "\n/top - топ игроков" +
                    "\n/account - сменить аккаунт" +
                    "\n/exit - exit the game");
                    EnterText();
                }
                else if (textEntered == "/top")
                {
                    dontGenerateNextWord = true;
                    Console.WriteLine("Топ игроки:\n" +
                    nick + " - " + level + " уровень");
                    EnterText();
                }
                else if (textEntered == "/account")
                {
                    LoginInAccount();
                    EnterText();
                }
                else if (textEntered == "/cheat")
                {
                    dontGenerateNextWord = true;
                    exp += expNeed;
                    LevelCheck();
                    EnterText();
                }
                else if (textEntered == "/language")
                {
                    dontGenerateNextWord = true;
                    ChangeLanguage();
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
                    SaveSettings();
                    EnterText();
                }
                else if (textEntered == "/enablesounds")
                {
                    dontGenerateNextWord = true;
                    enabledSounds = true;
                    Console.WriteLine("Sounds are now enabled.");
                    SaveSettings();
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
                            SaveSettings();
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
                            SaveSettings();
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
                            SaveSettings();
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
                            Console.WriteLine("Now the duration of the miss sound is " + durmiss + " ms.");
                            SaveSettings();
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
                    "\nNickname: " + nick +
                    "\nLevel: " + level +
                    "\nExp: " + exp +
                    "\nNeed exp for the next level: " + (expNeed - exp) +
                    "\nWords entered: " + enteredWords +
                    "\nCharacters entered: " + enteredCharacters +
                    "\nWins: " + wins +
                    "\nMisses: " + misses +
                    "\nWin ratio: " + Math.Round(winRatio, 2) + "%");
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
        }

        static void ChangeLanguage()
        {
            Console.WriteLine("Select a language:" +
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
            else
            {
                Console.WriteLine("Unknown language!");
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
                Console.WriteLine("Select a dictionary:" +
                "\nnouns" + " (" + nounsLvl + " lvl)" +
                "\nletters" + " (" + lettersLvl + " lvl)" +
                "\nsymbols" + " (" + symbolsLvl + " lvl)" +
                "\nadjectives" + " (" + adjectivesLvl + " lvl)" +
                "\nverbs" + " (" + verbsLvl + " lvl)" +
                "\nsurnames" + " (" + surnamesLvl + " lvl)" +
                "\nall" + " (" + allLvl + " lvl)"
                );
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
                    Console.WriteLine("For the dictionary «letters» you need level " + lettersLvl + "!");
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
                    Console.WriteLine("For the dictionary «symbols» you need level " + symbolsLvl + "!");
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
                    Console.WriteLine("For the dictionary «adjectives» you need level " + adjectivesLvl + "!");
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
                    Console.WriteLine("For the dictionary «verbs» you need level " + verbsLvl + "!");
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
                    Console.WriteLine("For the dictionary «surnames» you need level " + surnamesLvl + "!");
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
                    Console.WriteLine("For the dictionary «all» you need level " + allLvl + "!");
                    changeDictionaryWrited = true;
                    ChangeDictionary();
                }
            }
            else
            {
                Console.WriteLine("Unknown dictionary!");
                changeDictionaryWrited = true;
                ChangeDictionary();
            }
            SaveGame();
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
                Console.WriteLine("Level raised! You level is now " + level + "! The next level requires " + (expNeed - exp) + " exp.");
                SaveGame();
            }
        }

        // static bool secWord = false;
        static void TextGenerator()
        {
            int randomNum = rnd.Next(0, WordsDatabaseScript.maxWordsArray);
            textNeed = WordsDatabaseScript.words[randomNum];
            // int randomNum = rnd.Next(0, WordsDatabaseScript.maxWordsArray);
            // nextTextNeed = WordsDatabaseScript.words[randomNum];
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
                SaveGame();
                EnterText();
            }
            else
            {
                misses += 1;
                //enteredCharacters += textEntered.Length;
                dontGenerateNextWord = true;
                Console.WriteLine("Miss! " + "Wins: " + wins + ". Misses: " + misses + ". Characters entered: " + enteredCharacters);
                Beep(freqmiss, durmiss);
                SaveGame();
                EnterText();
            }
        }
    }
}
