using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Functions.Collections
{
    internal class Dictionary
    {
        static void Main(string[] args)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            
            dictionary.Add("mouse", "small animal");
            dictionary.Add("dog", "medium animal");
            dictionary.Add("elephant", "big animal");

            bool isUserExited = false;

            const string ExitCommand = "exit";
            string dictionaryNotContainsAnswer = "Dictionary don`t contains such word.";

            while(isUserExited == false) 
            {
                PrintUI(ExitCommand);

                string enteredWord = Console.ReadLine();

                if(enteredWord == ExitCommand)
                {
                    isUserExited = true;
                    break;
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
            //Console.SetCursorPosition(0, 0);
            Console.Write("\nEnter a word to get its translation\n" +
                         $"Enter \"{exitCommand}\" to exit programm.\n\n");
        }
    }
}
