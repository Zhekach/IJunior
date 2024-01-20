using System;
using System.Collections.Generic;

namespace Functions.Collections
{
    internal class Dictionary
    {
        static void Main1(string[] args)
        {
            const string ExitCommand = "exit";

            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            
            dictionary.Add("mouse", "small animal");
            dictionary.Add("dog", "medium animal");
            dictionary.Add("elephant", "big animal");

            bool isUserExited = false;

            string dictionaryNotContainsAnswer = "Dictionary don`t contains such word.";

            while(isUserExited == false) 
            {
                PrintUI(ExitCommand);

                string enteredWord = Console.ReadLine();

                if(enteredWord == ExitCommand)
                {
                    isUserExited = true;
                    continue;
                }

                if(dictionary.ContainsKey(enteredWord))
                {
                    Console.WriteLine(enteredWord + "-" + dictionary[enteredWord]);
                }
                else 
                {
                    Console.WriteLine(dictionaryNotContainsAnswer);
                }
            }
        }

        private static void PrintUI(string exitCommand)
        {
            Console.Write("\nEnter a word to get its translation\n" +
                         $"Enter \"{exitCommand}\" to exit programm.\n\n");
        }
    }
}
