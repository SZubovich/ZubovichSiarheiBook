using System;
using System.Collections.Generic;
using System.Linq;
using BookStore.Models;
using BookStore.Context;

namespace BookStore
{
    /// <summary>
    /// Has methods for work with book repository
    /// </summary>
    public static class BookService
    {
        #region Public Methods

        /// <summary>
        /// Adds book into repository.
        /// </summary>
        /// <param name="book">Instance of <see cref="Book"/>.</param>
        /// <param name="authors">Instances of <see cref="Author"/>.</param>
        public static void AddBookToStore(Book book, params Author[] authors)
        {
            Validation(book, authors);
            int bookId = 0;
            List<int> authorsId = new List<int>();

            using (BookStoreDbContext context = new BookStoreDbContext())
            {
                if (context.Books.FirstOrDefault(b => b.Title == book.Title && b.Year == book.Year) is null)
                {
                    context.Books.Add(book);
                    context.SaveChanges();
                }
                bookId = context.Books.First(b => b.Title == book.Title && b.Year == book.Year).Id;

                foreach (var author in authors)
                {
                    if (context.Authors.FirstOrDefault(a => a.FirstName == author.FirstName && 
                        a.LastName == author.LastName) is null)
                    {
                        context.Authors.Add(author);
                        context.SaveChanges();
                    }

                    authorsId.Add(context.Authors.First( a => a.FirstName == author.FirstName &&
                                                              a.LastName == author.LastName).Id);
                }

                foreach (var authorId in authorsId)
                {
                    context.AuthorsBooks.Add(new AuthorBook() { AuthorId = authorId, BookId = bookId });
                }

                context.SaveChanges();
            }
        }

        /// <summary>
        /// Returns filtred sequence of books.
        /// </summary>
        /// <param name="authorId">Id of author that will be used for filtration.</param>
        /// <returns><see cref="IEnumerable{Book}"/> that contains all books which was selected by current authorId.</returns>
        public static IEnumerable<Book> ShowAllBooksByAuthor(int authorId)
        {
            using (BookStoreDbContext context = new BookStoreDbContext())
            {
                return context.AuthorsBooks.Where(ab => ab.AuthorId == authorId)
                    .Join(context.Books, ab => ab.BookId, b => b.Id, (ab, b) => b).ToList();
            }
        }

        /// <summary>
        /// Returns sequence of all authors.
        /// </summary>
        /// <returns><see cref="IEnumerable{Author}"/> that contains all authors.</returns>
        public static IEnumerable<Author> ShowAllAuthors()
        {
            using (BookStoreDbContext context = new BookStoreDbContext())
            {
                return context.Authors.ToList();
            }
        }

        /// <summary>
        /// Returns sequence of all books.
        /// </summary>
        /// <returns><see cref="IEnumerable{Book}"/> that contains all book.</returns>
        public static IEnumerable<Book> ShowAllBooks()
        {
            using (BookStoreDbContext context = new BookStoreDbContext())
            {
                return context.Books.ToList();
            }
        }

        /// <summary>
        /// Deletes specefied element from book repository.
        /// </summary>
        /// <param name="bookId">Id of deleted book.</param>
        public static void Delete(int bookId)
        {
            using (BookStoreDbContext context = new BookStoreDbContext())
            {
                context.AuthorsBooks.Remove(context.AuthorsBooks.FirstOrDefault( ab => ab.BookId == bookId));
                context.Books.Remove(context.Books.FirstOrDefault(b => b.Id == bookId));
                context.SaveChanges();
            }
        }

        #endregion

        private static void Validation(Book book, params Author[] authors)
        {
            if (book is null)
            {
                throw new ArgumentNullException($"{nameof(book)} is null.");
            }
            if (authors is null)
            {
                throw new ArgumentNullException($"{nameof(authors)} is null.");
            }
            if (book.Title is null)
            {
                throw new ArgumentNullException($"{nameof(book.Title)} is null.");
            }

            if (book.Title.Length == 0 || book.Title.Length > 30)
            {
                throw new ArgumentOutOfRangeException($"The {nameof(book.Title)} of the book has wrong length. It must be between 1 and 30 symbols.");
            }

            if (authors.Any(author => author.LastName.Length == 0 || author.LastName.Length > 20))
            {
                throw new ArgumentOutOfRangeException($"The {nameof(authors)} contains author with wrong lastname. It must be between 1 and 20 symbols.");
            }
        }
    }
}
