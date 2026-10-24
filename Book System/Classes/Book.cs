using System;
using System.Collections.Generic;
using System.Text;

namespace Book_System.Classes
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;

        public string ISBN { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public bool IsAvailable { get; set; } = true;
        public Book() { }
        public Book(int id,string title,string author,string isbn,string category)
        {
            Id = id; 
            Title = title;
            Author = author;
            ISBN = isbn;
            Category = category;
        }

    }
}
