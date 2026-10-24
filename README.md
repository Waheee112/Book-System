# Book System – Library Management (C#)

A C# console application for managing a small library. It supports two roles: **Admins** manage the book catalog and monitor borrowing, while **Members** browse, borrow and return books. All data is stored in JSON files.

## Features

**Admin**
- Add new books to the catalog
- View all books with their availability status
- Edit a book's title, author or category
- View all borrow records (borrow date, due date, returned or not)

**Member**
- View the books that are currently available
- Borrow a book (up to 3 books at a time, due in 14 days)
- Return a borrowed book
- View the books they currently have

**Authentication**
- Register and log in with email and password
- Role-based behavior: the same login returns an `Admin` or a `Member` object

## Tech & Concepts

- C# and .NET (console application)
- Object-Oriented Programming: `Admin` and `Member` inherit from `User`
- JSON persistence with `System.Text.Json`, including polymorphic serialization (`[JsonDerivedType]`) so users are saved and loaded as the correct type
- A generic `JsonService` that loads and saves any `List<T>` to a file
- LINQ for searching and filtering

## Project Structure

```
Book System/
├── Classes/
│   ├── User.cs            # Base class and UserRole enum
│   ├── Admin.cs           # Admin operations
│   ├── Member.cs          # Member operations (borrow / return / view)
│   ├── AuthServices.cs    # Login and registration
│   ├── Book.cs            # Book model
│   ├── Borrow_Record.cs   # Borrow record model
│   └── JsonService.cs     # Generic JSON read/write
├── jsonfiles/
│   ├── Book.json
│   ├── Borrow_Record.json
│   └── User.json
└── Program.cs             # Entry point and menus
```

## Getting Started

1. Clone the repository:
   ```
   git clone https://github.com/<your-username>/Book-System.git
   ```
2. Open `Book System.slnx` in Visual Studio.
3. Press **F5** to run.

## Future Improvements

- Hash passwords instead of storing them as plain text
- Overdue tracking and late fees
- Search books by title, author or category
- Unit tests for the borrow and return logic
- Migrate the storage to a database (EF Core + SQL Server) and expose the logic as an ASP.NET Core Web API
