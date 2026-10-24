using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Book_System.Classes
{
    public class AuthServices
    {
        private readonly JsonService _service = new JsonService();
        private static readonly string UsersFilePath = Path.Combine("jsonfiles", "User.json");

        public User? Login(string email, string password)
        {
            List<User> users = _service.LoadFromFile<User>(UsersFilePath);

            return users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)
                                          && u.Password == password);
        }

        public bool Register(int id, string name, string email, string password, UserRole role)
        {
            List<User> users = _service.LoadFromFile<User>(UsersFilePath);

            if (users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) || u.Id == id))
            {
                return false;
            }

            User newUser;
            if (role == UserRole.Admin)
            {
                newUser = new Admin { Id = id, Name = name, Email = email, Password = password, Role = role };
            }
            else
            {
                newUser = new Member { Id = id, Name = name, Email = email, Password = password, Role = role };
            }

            users.Add(newUser);
            _service.SaveToFile(UsersFilePath, users);
            return true;
        }
    }
}