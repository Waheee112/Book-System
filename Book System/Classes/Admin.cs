using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Book_System.Classes
{
    public class Admin : User
    {
        private readonly JsonService _js = new JsonService();
        private readonly string _bookPath = Path.Combine("jsonfiles", "Book.json");
        private readonly string _recordPath = Path.Combine("jsonfiles", "Borrow_Record.json");

        public Admin()
        {
            Role = UserRole.Admin;
        }

        public void AddBook(Book bk)
        {
            List<Book> books = _js.LoadFromFile<Book>(_bookPath);
            books.Add(bk);
            // تم تصحيح المسار ليكون داخل مجلد jsonfiles بدلاً من المسار المباشر
            _js.SaveToFile(_bookPath, books);
        }

        public void DisplayAllBook()
        {
            List<Book> books = _js.LoadFromFile<Book>(_bookPath);

            if (books.Count == 0)
            {
                Console.WriteLine("No books found in the library.");
                return;
            }

            foreach (Book book in books)
            {
                string status = book.IsAvailable ? "Available" : "Borrowed";
                // تم ضبط تنسيق الطباعة وإضافة الفواصل والمسافات لسهولة القراءة
                Console.WriteLine($"ID: {book.Id} | Title: {book.Title} | Author: {book.Author} | Category: {book.Category} | ISBN: {book.ISBN} | Status: {status}");
                Console.WriteLine("----------------------------------------------------------------------------------");
            }
        }

        public void EditBook(int id)
        {
            List<Book> books = _js.LoadFromFile<Book>(_bookPath);
            Book? bookToEdit = books.FirstOrDefault(b => b.Id == id);

            if (bookToEdit == null)
            {
                Console.WriteLine("Book not found!");
                return;
            }

            Console.WriteLine($"Editing Book: {bookToEdit.Title}");

            Console.Write("Enter New Title (leave empty to keep current): ");
            string title = Console.ReadLine() ?? "";
            if (!string.IsNullOrWhiteSpace(title)) bookToEdit.Title = title;

            Console.Write("Enter New Author (leave empty to keep current): ");
            string author = Console.ReadLine() ?? "";
            if (!string.IsNullOrWhiteSpace(author)) bookToEdit.Author = author;

            Console.Write("Enter New Category (leave empty to keep current): ");
            string category = Console.ReadLine() ?? "";
            if (!string.IsNullOrWhiteSpace(category)) bookToEdit.Category = category;

            _js.SaveToFile(_bookPath, books);
            Console.WriteLine("Book updated successfully!");
        }

        public void ViewBorrowBook()
        {
            List<BorrowRecord> br = _js.LoadFromFile<BorrowRecord>(_recordPath);

            if (br.Count == 0)
            {
                Console.WriteLine("No borrow records found.");
                return;
            }

            Console.WriteLine("\n--- Borrow Records ---");
            foreach (var record in br)
            {
                string status = record.IsReturned ? "Returned" : "Borrowed";
                Console.WriteLine($"Record ID: {record.Id} | Member ID: {record.MemberId} | Book ID: {record.BookId}");
                Console.WriteLine($"Borrow Date: {record.BorrowDate:yyyy-MM-dd} | Due Date: {record.DueDate:yyyy-MM-dd} | Status: {status}");
                Console.WriteLine("--------------------------------------------------");
            }
        }

        public void Logout()
        {
            Console.WriteLine($"Admin {Name} logged out successfully.");
        }
    }
}