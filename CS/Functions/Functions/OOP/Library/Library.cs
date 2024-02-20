using System;
using System.Collections.Generic;

namespace Functions.OOP.Library
{
    internal class LibraryProgram
    {
        static void Main1()
        {
            Library library = new Library();
            library.Run();
        }
    }

    class Library
    {
        private List<Book> _books = new List<Book>();
        private bool _isUserExited = false;

        public void Run()
        {
            while (_isUserExited == false)
            {
                PrintUI();

                int userInput = ReadInt();

                switch (userInput)
                {
                    case (int)UserCommands.AddBook:
                        AddBookUI();
                        break;

                    case (int)UserCommands.RemoveBook:
                        RemoveBookUI();
                        break;

                    case (int)UserCommands.ShowAllBooks:
                        ShowAllBooks();
                        break;

                    case (int)UserCommands.ShowBooksByTitle:
                        ShowBooksByTitleUI();
                        break;

                    case (int)UserCommands.ShowBooksByAuthor:
                        ShowBooksByAuthorUI();
                        break;

                    case (int)UserCommands.ShowBooksByYear:
                        ShowBooksByYearUI();
                        break;

                    case (int)UserCommands.Exit:
                        _isUserExited = true;
                        break;

                    default:
                        Console.WriteLine("Вы ввели неверную команду. Попробуйте снова.");
                        break;
                }

                Console.WriteLine("Нажмите любую клавишу для продолжения.");
                Console.ReadKey();
                Console.Clear();
            }
        }

        private void AddBookUI()
        {
            Book book = ReadBookData();
            _books.Add(book);

            Console.WriteLine("Книга добавлена");
        }

        private void RemoveBookUI()
        {
            Book bookEntered = ReadBookData();
            int removedCount = 0;

            removedCount = _books.RemoveAll(book => book.Equals(bookEntered));

            Console.WriteLine("Всегоу удалено книг: " + removedCount);
        }

        private void ShowAllBooks()
        {
            foreach (Book book in _books)
            {
                book.ShowInfo();
            }
        }

        private void ShowBooksByTitleUI()
        {
            Console.WriteLine("Введите название для поиска по книгам");

            string title = Console.ReadLine();

            foreach (Book book in _books)
            {
                if (book.Title == title)
                {
                    book.ShowInfo();
                }
            }
        }

        private void ShowBooksByAuthorUI()
        {
            Console.WriteLine("Введите автора для поиска по книгам");

            string author = Console.ReadLine();

            foreach (Book book in _books)
            {
                if (book.Author == author)
                {
                    book.ShowInfo();
                }
            }
        }

        private void ShowBooksByYearUI()
        {
            Console.WriteLine("Введите год выпуска для поиска по книгам");

            int yearRealease = ReadInt();

            foreach (Book book in _books)
            {
                if (book.YearRelease == yearRealease)
                {
                    book.ShowInfo();
                }
            }
        }

        private void PrintUI()
        {
            Console.WriteLine("Введите команду:\n" +
                $"{(int)UserCommands.AddBook} - добавить книгу\n" +
                $"{(int)UserCommands.RemoveBook} - удалить книгу\n" +
                $"{(int)UserCommands.ShowAllBooks} - показать все книги\n" +
                $"{(int)UserCommands.ShowBooksByTitle} - показать все книги по названию\n" +
                $"{(int)UserCommands.ShowBooksByAuthor} - показать все книги по автору\n" +
                $"{(int)UserCommands.ShowBooksByYear} - показать все книги по году выпуска\n" +
                $"{(int)UserCommands.Exit} - выйти из программы\n");
        }

        private int ReadInt()
        {
            bool isIntEntered = false;
            int parsedInt = 0;

            while (isIntEntered == false)
            {
                string enteredString;

                Console.WriteLine("Введите число:");
                enteredString = Console.ReadLine();

                isIntEntered = int.TryParse(enteredString, out parsedInt);

                if (isIntEntered)
                {
                    Console.WriteLine($"Введенное число распознано: {parsedInt}");
                }
                else
                {
                    Console.WriteLine("Вы ввели некорректное число. Попробуйте ещё.\n");
                }
            }

            return parsedInt;
        }

        private Book ReadBookData()
        {
            Console.WriteLine("Введите название книги:");
            string title = Console.ReadLine();

            Console.WriteLine("Введите автора книги:");
            string author = Console.ReadLine();

            Console.WriteLine("Введите год выпуска книги:");
            int yearRelease = ReadInt();

            Book book = new Book(title, author, yearRelease);
            book.ShowInfo();

            return book;
        }
    }

    enum UserCommands
    {
        AddBook = 1,
        RemoveBook = 2,
        ShowAllBooks = 3,
        ShowBooksByTitle = 4,
        ShowBooksByAuthor = 5,
        ShowBooksByYear = 6,
        Exit = 7
    }

    class Book
    {
        public Book(string title, string author, int yearRelease)
        {
            Title = title;
            Author = author;
            YearRelease = yearRelease;
        }

        public string Title { get; private set; }
        public string Author { get; private set; }
        public int YearRelease { get; private set; }

        public void ShowInfo()
        {
            Console.WriteLine("Книга: \n" +
                $"Название - {Title} \n" +
                $"Автор - {Author} \n" +
                $"Год выпуска - {YearRelease} \n");
        }

        public bool Equals(Book other)
        {
            if (other == null)
            {
                return false;
            }
            if (other.Title.ToLower() == Title.ToLower() &&
               other.Author.ToLower() == Author.ToLower() &&
               other.YearRelease == YearRelease)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}