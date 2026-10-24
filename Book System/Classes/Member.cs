using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Book_System.Classes
{
    public class Member : User
    {
        private readonly JsonService _js = new JsonService();
        private readonly string _bookPath = Path.Combine("jsonfiles", "Book.json");
        private readonly string _recordPath = Path.Combine("jsonfiles", "Borrow_Record.json");

        public const int MaxAllowedBook = 3;

        public Member()
        {
            Role = UserRole.Member;
        }

        // Books this member currently has = unreturned records that belong to this member.
        // Reading from the saved records means the data survives logout / restart.
        private List<int> GetBorrowedBookIds()
        {
            List<BorrowRecord> records = _js.LoadFromFile<BorrowRecord>(_recordPath);
            return records
                .Where(r => r.MemberId == this.Id && !r.IsReturned)
                .Select(r => r.BookId)
                .ToList();
        }

        // 1. Display only the books that can be borrowed
        public void DisplayAvailableBooks()
        {
            List<Book> books = _js.LoadFromFile<Book>(_bookPath);
            var availableBooks = books.Where(b => b.IsAvailable).ToList();

            if (availableBooks.Count == 0)
            {
                Console.WriteLine("No available books at the moment.");
                return;
            }

            Console.WriteLine("\n--- Available Books ---");
            foreach (var book in availableBooks)
            {
                Console.WriteLine($"ID: {book.Id} | Title: {book.Title} | Author: {book.Author} | Category: {book.Category}");
                Console.WriteLine("--------------------------------------------------");
            }
        }

        // 2. Borrow a book
        public void BorrowBook(int bookId)
        {
            // Check the maximum limit using the saved records
            if (GetBorrowedBookIds().Count >= MaxAllowedBook)
            {
                Console.WriteLine($"\nCannot borrow. You have reached the maximum allowed books limit ({MaxAllowedBook}).");
                return;
            }

            List<Book> books = _js.LoadFromFile<Book>(_bookPath);
            Book? bookToBorrow = books.FirstOrDefault(b => b.Id == bookId && b.IsAvailable);

            if (bookToBorrow == null)
            {
                Console.WriteLine("\nBook is either not available or does not exist!");
                return;
            }

            // 1. Mark the book as borrowed
            bookToBorrow.IsAvailable = false;
            _js.SaveToFile(_bookPath, books);

            // 2. Add a new borrow record in Borrow_Record.json
            List<BorrowRecord> records = _js.LoadFromFile<BorrowRecord>(_recordPath);
            int newRecordId = records.Count > 0 ? records.Max(r => r.Id) + 1 : 1;

            BorrowRecord newRecord = new BorrowRecord
            {
                Id = newRecordId,
                BookId = bookId,
                MemberId = this.Id,
                BorrowDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(14),
                IsReturned = false
            };

            records.Add(newRecord);
            _js.SaveToFile(_recordPath, records);

            Console.WriteLine($"\nBook '{bookToBorrow.Title}' borrowed successfully!");
        }

        // 3. Return a book
        public void ReturnBook(int bookId)
        {
            // Find the active borrow record of this member for this book
            List<BorrowRecord> records = _js.LoadFromFile<BorrowRecord>(_recordPath);
            BorrowRecord? recordToUpdate = records.FirstOrDefault(r =>
                r.BookId == bookId && r.MemberId == this.Id && !r.IsReturned);

            if (recordToUpdate == null)
            {
                Console.WriteLine("\nYou haven't borrowed this book.");
                return;
            }

            // 1. Update the borrow record
            recordToUpdate.IsReturned = true;
            recordToUpdate.ReturnDate = DateTime.Now;
            _js.SaveToFile(_recordPath, records);

            // 2. Make the book available again
            List<Book> books = _js.LoadFromFile<Book>(_bookPath);
            Book? bookToReturn = books.FirstOrDefault(b => b.Id == bookId);

            if (bookToReturn != null)
            {
                bookToReturn.IsAvailable = true;
                _js.SaveToFile(_bookPath, books);
            }

            Console.WriteLine("\nBook returned successfully!");
        }

        // 4. Display the books this member currently has
        public void ViewMyBorrowedBooks()
        {
            List<int> borrowedIds = GetBorrowedBookIds();

            if (borrowedIds.Count == 0)
            {
                Console.WriteLine("\nYou have no borrowed books.");
                return;
            }

            List<Book> books = _js.LoadFromFile<Book>(_bookPath);
            var myBooks = books.Where(b => borrowedIds.Contains(b.Id)).ToList();

            Console.WriteLine("\n--- My Borrowed Books ---");
            foreach (var book in myBooks)
            {
                Console.WriteLine($"ID: {book.Id} | Title: {book.Title} | Author: {book.Author}");
                Console.WriteLine("--------------------------------------------------");
            }
        }
    }
}