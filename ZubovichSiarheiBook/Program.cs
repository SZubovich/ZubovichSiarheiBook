using System;
using BookStore;
using BookStore.Models;

namespace ZubovichSiarheiBook
{
    class Program
    {
        static void Main(string[] args)
        {
            bool IsApplicationWorks = true;
            int menuPointer = 0;
            string pointer = ">>";

            string[] menuItems = 
            {
                "Add book",
                "Delete book",
                "Show all books",
                "Show all authors",
                "Show all books by author",
                "Exit"
            };

            while (IsApplicationWorks)
            {
                for (int i = 0; i < menuItems.Length; i++)
                {
                    if (i == menuPointer)
                    {
                        Console.Write(pointer);
                    }

                    Console.WriteLine(menuItems[i]);
                }

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        menuPointer = menuPointer == 0 ? menuItems.Length - 1 : menuPointer - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        menuPointer = menuPointer == menuItems.Length - 1 ? 0 : menuPointer + 1;
                        break;
                    case ConsoleKey.Enter:
                        switch (menuPointer)
                        {
                            case 0:
                                Add();
                                break;
                            case 1:
                                Remove();
                                break;
                            case 2:
                                ShowAllBooks();
                                break;
                            case 3:
                                ShowAllAuthors();
                                break;
                            case 4:
                                ShowAllBooksByAuthor();
                                break;
                            case 5:
                                IsApplicationWorks = false;
                                break;
                        }
                        break;
                }

                Console.Clear();
            }
        }

        static void Add()
        {
            Console.WriteLine("Enter author's first name");
            string firstName = Console.ReadLine();
            Console.WriteLine("Enter author's last name");
            string lastName = Console.ReadLine();
            Console.WriteLine("Enter a book title");
            string bookTitle = Console.ReadLine();
            Console.WriteLine("Enter year of publication book");
            if (!int.TryParse(Console.ReadLine(), out int year))
            {
                Console.WriteLine("Year was written with mistake.");
                return;
            }

            try
            {
                BookService.AddBookToStore(new Book() { Title = bookTitle, Year = year },
                    new Author() { FirstName = firstName, LastName = lastName });
                Console.WriteLine("Book created!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something went wrong! {ex.Message}");
            }
            Console.ReadKey();
        }

        static void Remove()
        {
            Console.WriteLine("Enter the book id to delete.");
            BookService.Delete(Convert.ToInt32(Console.ReadLine()));
            Console.WriteLine("Book removed successfuly!");
            Console.ReadKey();
        }

        static void ShowAllAuthors()
        {
            foreach (var author in BookService.ShowAllAuthors())
            {
                Console.WriteLine($"Id: {author.Id} | First name: {author.FirstName} | Last name: {author.LastName}");
            }

            Console.ReadKey();
        }

        static void ShowAllBooks()
        {
            foreach (var book in BookService.ShowAllBooks())
            {
                Console.WriteLine($"Id: {book.Id} | Title: {book.Title} | Year: {book.Year}");
            }

            Console.ReadKey();
        }

        static void ShowAllBooksByAuthor()
        {
            Console.WriteLine("Enter the author ID");
            int authorId = Convert.ToInt32(Console.ReadLine());

            foreach (var book in BookService.ShowAllBooksByAuthor(authorId))
            {
                Console.WriteLine($"Id: {book.Id} | Title: {book.Title} | Year: {book.Year}");
            }

            if (BookService.ShowAllBooksByAuthor(authorId) is null)
            {
                Console.WriteLine("Book with that Author ID wasn't found.");
            }

            Console.ReadKey();
        }
    }
}
