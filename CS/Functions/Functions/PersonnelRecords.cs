using System;

namespace Functions
{
    internal class PersonnelRecords
    {
        static void Main11(string[] args)
        {
            const string AddDossierCommand = "add";
            const string ShowDossierCommand = "showAll";
            const string DeleteDossierCommand = "delete";
            const string SearchDossierCommand = "search";
            const string ExitoDossierCommand = "exit";

            string[] fullNames = new string[0];
            string[] posts = new string[0];
            bool isOpen = true;

            while (isOpen)
            {
                Console.WriteLine("\n\nДля работы с программой используйте следующие команды:\n" +
                $"{AddDossierCommand} - добавляет новую запись в систему\n" +
                $"{ShowDossierCommand} - вывод всех записей в системе\n" +
                $"{DeleteDossierCommand} - удаление записи по её номеру\n" +
                $"{SearchDossierCommand} - поиск по фамилии\n" +
                $"{ExitoDossierCommand} - завершение работы\n");

                switch (Console.ReadLine())
                {
                    case AddDossierCommand:
                        (fullNames, posts) = AddRecord(fullNames, posts);
                        break;

                    case ShowDossierCommand:
                        ShowRecords(fullNames, posts);
                        break;

                    case DeleteDossierCommand:
                        (fullNames, posts) = DeleteRecord(fullNames, posts);
                        break;

                    case SearchDossierCommand:
                        SearchRecord(fullNames, posts);
                        break;

                    case ExitoDossierCommand:
                        isOpen = false;
                        break;

                    default:
                        Console.WriteLine("Введена неверная команда, попробуйте ещё раз.");
                        break;
                }
            }
        }

        static (string[], string[]) AddRecord(string[] fullNames, string[] posts)
        {
            fullNames = AddEllementInArrayToEnd(fullNames, descriptionForUser: "Введите ФИО сотрудника: ");
            posts = AddEllementInArrayToEnd(posts, descriptionForUser: "и должность сотрудника: ");

            return (fullNames, posts);
        }

        static string[] AddEllementInArrayToEnd(string[] baseArray, string descriptionForUser = "Введите новый элемент")
        {
            string[] newArray = new string[baseArray.Length + 1];

            for (int i = 0; i < baseArray.Length; i++)
            {
                newArray[i] = baseArray[i];
            }

            Console.Write(descriptionForUser);
            newArray[newArray.Length - 1] = Console.ReadLine();

            return newArray;
        }

        static void ShowRecords(string[] fullNames, string[] posts)
        {
            for (int i = 0; i < fullNames.Length; i++)
            {
                Console.WriteLine($"{i + 1} - {fullNames[i]} {posts[i]}");
            }
        }

        static (string[], string[]) DeleteRecord(string[] fullNames, string[] posts)
        {
            Console.Write("Введите номер записи для удаления: ");
            int enteredID = Convert.ToInt32(Console.ReadLine());

            if (fullNames.Length == 0)
            {
                Console.WriteLine("Нечего удалять.");
                return (fullNames, posts);
            }
            else if (enteredID <= 0 || enteredID >= fullNames.Length)
            {
                Console.WriteLine("Введён неверный номер записи.");
                return (fullNames, posts);
            }
            else
            {
                fullNames = DeleteElementInArray(fullNames, enteredID);
                posts = DeleteElementInArray(posts, enteredID);
                return (fullNames, posts);
            }
        }

        static void SearchRecord(string[] fullNames, string[] posts)
        {
            bool isSearched = false;
            char splitChar = ' ';

            Console.Write("Введите фамилию для поиска: ");
            string enteredSurname = Console.ReadLine();

            for (int i = 0; i < fullNames.Length; i++)
            {
                string[] fullNameSplited = fullNames[i].Split(splitChar);

                if (fullNameSplited[0] == enteredSurname)
                {
                    isSearched = true;

                    Console.WriteLine($"Такой сотрудник имеется в списках.\n" +
                        $"Его ФИО: {fullNames[i]}\n" +
                        $"Его должность: {posts[i]}.\n");
                }
            }

            if (isSearched == false)
            {
                Console.WriteLine("В системе нет сотрудников с такой фамилией =(");
            }
        }

        static string[] DeleteElementInArray(string[] baseArray, int index)
        {
            string[] newArray = new string[baseArray.Length - 1];
            index--;

            for (int i = 0; i < index; i++)
            {
                newArray[i] = baseArray[i];
            }

            for (int i = index; i < newArray.Length; i++)
            {
                newArray[i] = baseArray[i + 1];
            }

            return newArray;
        }
    }
}