using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework1
{
    internal class PasswordCheck
    {
        static void Main1(string[] args)
        {
            string password = "qwerty12";
            bool isPasswordOk = false;
            string secretMessage = "The encryption was here. She's not here right now, but the password is correct.";
            int attemptsNumber = 3;

            Console.WriteLine($"You have {attemptsNumber} attempts to enter correct password.");

            for (int i = 0; i < attemptsNumber; i++)
            {
                Console.Write("Password: ");

                if (password == Console.ReadLine())
                {
                    isPasswordOk = true;
                    break;
                }

            }

            if (isPasswordOk)
            {
                Console.WriteLine(secretMessage);
            }

            else
            {
                Console.WriteLine("You missed.");
            }
            
            Console.ReadKey();
        }
    }
}
