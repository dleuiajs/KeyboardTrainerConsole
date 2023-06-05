using System;
using System.Reflection;

namespace KeyboardTrainerConsole
{
    internal class Achievements
    {
        static public bool[] completedAch = new bool[28]; // 0 - 27

        static public string[] text = {
            // words
            "First step: Enter the word for the first time.", // 0
            "Beginner: Enter 100 different words.", // 1
            "Mega Vocabulary: Enter 1,000 different words.", // 2
            "Word Erudite: Enter 10,000 different words.", // 3
            "Word God: Enter 100,000 different words.", // 4

            // letters in words
            "Long Tongue: Enter a word of 15 letters.", // 5
            "Impressive length: Enter a word of 20 letters.", // 6
            "Record length: Type a word of 25 letters.", // 7

            // words without mistakes
            "Series without error: Enter 25 words in a row without mistakes", // 8
            "Combo Champion: Enter 100 words in a row without mistakes", // 9
            "impenetrable wall: Enter 500 words in a row without mistakes", // 10
            "Super-shooter: Enter 1000 words in a row without mistakes", // 11

            // wpm
            "Words Progress: Reach an average rate of 60 words per minute.", // 12
            "Fast Keyboard: Reach an average rate of 100 words per minute", // 13
            "Speed Master: Reach an average rate of 130 words per minute.", // 14
            "Virtuoso Typing: Reach an average rate of 150 words per minute.", // 15
            "Speed Genius: Reach an average rate of 190 words per minute.", // 16

            // characters
            "Character Array: Enter 1,000 characters in total", // 17
            "Ocean of characters: Enter 10,000 characters in total.", // 18
            "Symbol mogul: Enter 100,000 characters in total", // 19
            "Megabyte Master: Enter 1,000,000 characters in total.", // 20

            // languages
            "Versatile player: Type words in three different languages.", // 21
            "Language Guru: Type words in 5 different languages.", // 22
            "Lord of Linguistics: Type words from 10 different languages.", // 23

            // other
            "Kid: Switch the difficulty to Kid", // 24
            "Best Friends: Play two-person mode with a friend.", // 25 not realeased
            "Tuner: Change the sound frequency to your own.", // 26
            "Scout: View the game description", // 27
        };

        static public int[] need = {
            1, // 0 
            100, // 1 
            1000, // 2
            10000, // 3
            100000, // 4

            15, // 5
            20, // 6
            25, // 7

            25, // 8
            100, // 9
            500, // 10
            1000, // 11

            60, // 12
            100, // 13
            130, // 14
            150, // 15
            190, // 16

            1000, // 17
            10000, // 18
            100000, // 19
            1000000, // 20

            3, // 21
            5, // 22
            10, // 23

            0, // 24
            0, // 25
            0, // 26
            0, // 27
        };

        static public int[] exp = {
            10, // 0
            100, // 1
            1000, // 2
            10000, // 3
            100000, // 4
            
            100, // 5
            250, // 6
            500, // 7

            50, // 8
            250, // 9
            1000, // 10
            5000, // 11

            100, //12
            200, //13
            300, //14
            500, //15
            1000, //16

            200, //17
            2000, //18
            20000, // 19
            200000, // 20

            100, // 21
            250, // 22
            500, // 23

            100, // 24
            100, // 25
            100, // 26
            100, // 27
        };
    }
}
