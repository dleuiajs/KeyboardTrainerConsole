using System;
using System.Threading;
using System.Diagnostics;

namespace SchoolTimeTable
{
    internal class Program
    {
        static string textNeed = "loshara";
        static string textEntered;

        static void Main()
        {
            EnterText();
        }

        static void EnterText()
        {
            Console.WriteLine("Enter " + "'" + textNeed + "'");
            textEntered = Console.ReadLine();
            if (textEntered == textNeed)
            {
                Console.WriteLine("Good job!");
                EnterText();
            }
        }
    }
}
