using System;
using System.Collections.Generic;

namespace Functions
{
    internal class PersonnelRecordsPro
    {
        static void Main(string[] args)
        {
            const string AddDossierCommand = "add";
            const string ShowDossierCommand = "showAll";
            const string DeleteDossierCommand = "delete";
            const string ExitCommand = "exit";

            Dictionary<string, string> dossiers = new Dictionary<string, string>();

            bool isUserExited = false;

            while (isUserExited == false)
            {
                Console.WriteLine("\n\nДля работы с программой используйте следующие команды:\n" +
                $"{AddDossierCommand} - добавляет новую запись в систему\n" +
                $"{ShowDossierCommand} - вывод всех записей в системе\n" +
                $"{DeleteDossierCommand} - удаление записи по её номеру\n" +
                $"{ExitCommand} - завершение работы\n");

                switch (Console.ReadLine())
                {
                    case AddDossierCommand:
                        AddRecord(dossiers);
                        break;

                    case ShowDossierCommand:
                        ShowRecords(dossiers);
                        break;

                    case DeleteDossierCommand:
                        DeleteRecord(dossiers);
                        break;

                    case ExitCommand:
                        isUserExited = true;
                        break;

                    default:
                        Console.WriteLine("Введена неверная команда, попробуйте ещё раз.");
                        break;
                }
            }
        }

        static void AddRecord(Dictionary<string, string> dictionary)
        {
            string fullName;
            string post;

            Console.Write("Ввведите ФИО сотрудника: ");
            fullName = Console.ReadLine();

            Console.Write("Введите должность сотрудника: ");
            post = Console.ReadLine();

            dictionary[fullName] = post;
        }

        static void ShowRecords(Dictionary<string, string> dictionary)
        {
            foreach(KeyValuePair<string, string> keyValuePair in dictionary)
            {
                Console.WriteLine($"{keyValuePair.Key} - {keyValuePair.Value}");
            }
        }

        static void DeleteRecord(Dictionary<string, string> dictionary)
        {
            string fullnameToRemove;

            Console.Write("Введите ФИО сотрудника для удаления: ");
            fullnameToRemove = Console.ReadLine();

            if (dictionary.ContainsKey(fullnameToRemove))
            {
                dictionary.Remove(fullnameToRemove);
            }
            else
            {
                Console.WriteLine("Нет такого сотрудника");
            }
        }
    }
}