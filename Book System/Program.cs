using System;
using System.Collections.Generic;
using System.IO;
using Book_System.Classes;

namespace Book_System
{
    internal class Program
    {
        private static string UserPath = Path.Combine("jsonfiles", "User.json");
        private static string BookPath = Path.Combine("jsonfiles", "Book.json");
        private static string RecordPath = Path.Combine("jsonfiles", "Borrow_Record.json");

        private static JsonService _JsonService = new JsonService();

        static void Main(string[] args)
        {
            bool IsRunning = true;
            int choice;
            AuthServices authServices = new AuthServices();

            while (IsRunning)
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine("       WELCOME TO MY BOOK SYSTEM        ");
                Console.WriteLine("========================================");
                Console.WriteLine("[1]- Login");
                Console.WriteLine("[2]- Register");
                Console.WriteLine("[3]- Exit");
                Console.WriteLine("========================================");
                Console.Write("Enter Your Choice: ");

                while (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Enter a valid value...");
                }

                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        Console.WriteLine("========================================");
                        Console.WriteLine("          WELCOME TO LOGIN PAGE         ");
                        Console.WriteLine("========================================");

                        Console.Write("Enter Email: ");
                        string email = Console.ReadLine() ?? "";

                        Console.Write("Enter Password: ");
                        string pass = Console.ReadLine() ?? "";

                        User? currentUser = authServices.Login(email, pass);

                        if (currentUser != null)
                        {
                            Console.WriteLine($"\nLogin Successful! Welcome {currentUser.Name}");
                            Console.WriteLine("Press any key to enter dashboard...");
                            Console.ReadKey();

                            if (currentUser.Role == UserRole.Admin)
                            {
                                ShowAdminMenu((Admin)currentUser);
                            }
                            else if (currentUser.Role == UserRole.Member)
                            {
                                ShowMemberMenu((Member)currentUser);
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nLogin Failed! Invalid email or password.");
                            Console.WriteLine("Press any key to continue...");
                            Console.ReadKey();
                        }
                        break;

                    case 2:
                        Console.Clear();
                        Console.WriteLine("========================================");
                        Console.WriteLine("        WELCOME TO REGISTER PAGE        ");
                        Console.WriteLine("========================================");

                        Console.Write("Enter Email: ");
                        string email1 = Console.ReadLine() ?? "";

                        Console.Write("Enter Password: ");
                        string pass1 = Console.ReadLine() ?? "";

                        Console.Write("Enter Name: ");
                        string name = Console.ReadLine() ?? "";

                        Console.Write("Enter ID: ");
                        int id;
                        while (!int.TryParse(Console.ReadLine(), out id))
                        {
                            Console.WriteLine("Enter a valid integer ID...");
                        }

                        Console.WriteLine("Select Role: [1] Admin  [2] Member");
                        int roleChoice;
                        while (!int.TryParse(Console.ReadLine(), out roleChoice) || (roleChoice != 1 && roleChoice != 2))
                        {
                            Console.WriteLine("Enter a valid choice (1 or 2)...");
                        }
                        UserRole role = (roleChoice == 1) ? UserRole.Admin : UserRole.Member;

                        bool isSuccess = authServices.Register(id, name, email1, pass1, role);
                        if (isSuccess)
                        {
                            Console.WriteLine("\nRegistration successful!");
                        }
                        else
                        {
                            Console.WriteLine("\nRegistration failed! Email or ID already exists.");
                        }

                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;

                    case 3:
                        IsRunning = false;
                        Console.WriteLine("==============================================");
                        Console.WriteLine("   THANK YOU FOR USING BOOK SYSTEM! SEE YOU!");
                        Console.WriteLine("==============================================");
                        break;

                    default:
                        Console.WriteLine("Invalid option! Press any key...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        // --- قائمة المسؤول (Admin Menu) ---
        private static void ShowAdminMenu(Admin admin)
        {
            bool inAdminMenu = true;
            while (inAdminMenu)
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine($"      ADMIN PANEL - Welcome {admin.Name}");
                Console.WriteLine("========================================");
                Console.WriteLine("[1] Add New Book");
                Console.WriteLine("[2] Display All Books");
                Console.WriteLine("[3] Edit Book");
                Console.WriteLine("[4] View Borrow Records");
                Console.WriteLine("[5] Logout");
                Console.WriteLine("========================================");
                Console.Write("Enter Choice: ");

                if (!int.TryParse(Console.ReadLine(), out int choice)) continue;

                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        Console.WriteLine("--- ADD NEW BOOK ---");
                        Console.Write("Enter Book ID: ");
                        int bId = int.Parse(Console.ReadLine() ?? "0");
                        Console.Write("Enter Title: ");
                        string title = Console.ReadLine() ?? "";
                        Console.Write("Enter Author: ");
                        string author = Console.ReadLine() ?? "";
                        Console.Write("Enter ISBN: ");
                        string isbn = Console.ReadLine() ?? "";
                        Console.Write("Enter Category: ");
                        string category = Console.ReadLine() ?? "";

                        Book newBook = new Book
                        {
                            Id = bId,
                            Title = title,
                            Author = author,
                            ISBN = isbn,
                            Category = category,
                            IsAvailable = true
                        };

                        admin.AddBook(newBook);
                        Console.WriteLine("\nBook added successfully!");
                        Console.ReadKey();
                        break;

                    case 2:
                        Console.Clear();
                        Console.WriteLine("--- ALL BOOKS ---");
                        admin.DisplayAllBook();
                        Console.ReadKey();
                        break;

                    case 3:
                        Console.Clear();
                        Console.Write("Enter Book ID to Edit: ");
                        if (int.TryParse(Console.ReadLine(), out int editId))
                        {
                            admin.EditBook(editId);
                        }
                        Console.ReadKey();
                        break;

                    case 4:
                        Console.Clear();
                        admin.ViewBorrowBook();
                        Console.ReadKey();
                        break;

                    case 5:
                        inAdminMenu = false;
                        admin.Logout();
                        break;
                }
            }
        }

        // --- قائمة العضو (Member Menu) ---
        private static void ShowMemberMenu(Member member)
        {
            bool inMemberMenu = true;
            while (inMemberMenu)
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine($"     MEMBER PANEL - Welcome {member.Name}");
                Console.WriteLine("========================================");
                Console.WriteLine("[1] Display Available Books");
                Console.WriteLine("[2] Borrow Book");
                Console.WriteLine("[3] Return Book");
                Console.WriteLine("[4] View My Borrowed Books");
                Console.WriteLine("[5] Logout");
                Console.WriteLine("========================================");
                Console.Write("Enter Choice: ");

                if (!int.TryParse(Console.ReadLine(), out int choice)) continue;

                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        member.DisplayAvailableBooks();
                        Console.ReadKey();
                        break;

                    case 2:
                        Console.Clear();
                        Console.Write("Enter Book ID to Borrow: ");
                        if (int.TryParse(Console.ReadLine(), out int borrowId))
                        {
                            member.BorrowBook(borrowId);
                        }
                        Console.ReadKey();
                        break;

                    case 3:
                        Console.Clear();
                        Console.Write("Enter Book ID to Return: ");
                        if (int.TryParse(Console.ReadLine(), out int returnId))
                        {
                            member.ReturnBook(returnId);
                        }
                        Console.ReadKey();
                        break;

                    case 4:
                        Console.Clear();
                        member.ViewMyBorrowedBooks();
                        Console.ReadKey();
                        break;

                    case 5:
                        inMemberMenu = false;
                        Console.WriteLine($"\nGoodbye, {member.Name}!");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}