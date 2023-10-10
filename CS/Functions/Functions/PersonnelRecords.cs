using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Functions
{
    internal class PersonnelRecords
    {
        static void Main1(string[] args)
        {
            const string AddCommandString = "add";
            const string ShowCommandString = "showAll";
            const string DeleteCommandString = "delete";
            const string SearchCommandString = "search";
            const string ExitCommandString = "exit";

            string[] fullNames = new string[0];
            string[] posts = new string[0];
            bool isOpen = true;

            while (isOpen)
            {
                Console.WriteLine("\n\nДля работы с программой используйте следующие команды:\n" +
                $"{AddCommandString} - добавляет новую запись в систему\n" +
                $"{ShowCommandString} - вывод всех записей в системе\n" +
                $"{DeleteCommandString} - удаление записи по её номеру\n" +
                $"{SearchCommandString} - поиск по фамилии\n" +
                $"{ExitCommandString} - завершение работы\n");

                switch (Console.ReadLine())
                {
                    case AddCommandString:
                        Console.Write("Введите ФИО сотрудника: ");
                        string enteredFullName = Console.ReadLine();

                        Console.Write("и должность сотрудника: ");
                        string enteredPost = Console.ReadLine();

                        fullNames = AddRecord(fullNames, enteredFullName);
                        posts = AddRecord(posts, enteredPost);
                        break;
                    case ShowCommandString:
                        ShowRecords(fullNames, posts);
                        break;
                    case DeleteCommandString:
                        Console.Write("Введите номер записи для удаления: ");
                        int enteredID = Convert.ToInt32(Console.ReadLine());

                        if (enteredID > 0 && enteredID <= fullNames.Length)
                        {
                            fullNames = DeleteRecord(fullNames, enteredID);
                            posts = DeleteRecord(posts, enteredID);
                        }
                        else
                        {
                            Console.WriteLine("Введён неверный номер записи.");
                        }
                        break;
                    case SearchCommandString:
                        Console.Write("Введите фамилию для поиска: ");
                        string enteredSurname = Console.ReadLine();

                        SearchRecord(fullNames, posts, enteredSurname);
                        break;
                    case ExitCommandString:
                        isOpen = false;
                        break;
                    default:
                        Console.WriteLine("Введена неверная команда, попробуйте ещё раз.");
                        break;
                }
            }
        }
        static string[] AddRecord(string[] baseArray, string reccord)
        {
            string[] newArray = new string[baseArray.Length + 1];

            for (int i = 0; i < baseArray.Length; i++)
            {
                newArray[i] = baseArray[i];
            }

            newArray[newArray.Length - 1] = reccord;

            return newArray;
        }

        static void ShowRecords(string[] fullNames, string[] posts)
        {
            for (int i = 0; i < fullNames.Length; i++)
            {
                Console.WriteLine($"{i + 1} - {fullNames[i]} {posts[i]}");
            }
        }

        static string[] DeleteRecord(string[] baseArray, int id)
        {
            if (baseArray.Length == 0)
            {
                return baseArray;
            }

            string[] newArray = new string[baseArray.Length - 1];
            id--;

            for (int i = 0; i < newArray.Length; i++)
            {
                if (i < id)
                {
                    newArray[i] = baseArray[i];
                }
                else
                {
                    newArray[i] = baseArray[i + 1];
                }
            }

            return newArray;
        }

        static void SearchRecord(string[] fullNames, string[] posts, string surname)
        {
            bool isSearched = false;
            char splitChar = ' ';

            for (int i = 0; i < fullNames.Length; i++)
            {
                string[] fullNameSplited = fullNames[i].Split(splitChar);

                if (fullNameSplited[0] == surname)
                {
                    isSearched = true;

                    Console.WriteLine($"Такой сотрудник имеется в списках.\n" +
                        $"Его ФИО: {fullNames[i]}\n" +
                        $"Его должность: {posts[i]}.\n");
                }
            }

            if(isSearched == false)
            {
                Console.WriteLine("В системе нет сотрудников с такой фамилией =(");
            }
        }
    }
}
