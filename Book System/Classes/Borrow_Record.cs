using System;

namespace Book_System.Classes
{
    public class BorrowRecord
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int MemberId { get; set; }
        public DateTime BorrowDate { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; } = DateTime.Now.AddDays(14); 
        public DateTime? ReturnDate { get; set; } = null; 
        public bool IsReturned { get; set; } = false;
    }
}