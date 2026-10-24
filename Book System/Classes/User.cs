using System.Text.Json.Serialization;

namespace Book_System.Classes
{
    public enum UserRole
    {
        Admin,
        Member
    }

    [JsonDerivedType(typeof(User), typeDiscriminator: "base")]
    [JsonDerivedType(typeof(Admin), typeDiscriminator: "admin")]
    [JsonDerivedType(typeof(Member), typeDiscriminator: "member")]
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserRole Role { get; set; }

        public User() { }

        public User(int id, string name, string email, string pass, UserRole role)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = pass;
            Role = role;
        }
    }
}