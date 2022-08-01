using ChatApp.Models.Enum;

namespace ChatApp.Models
{
    public class User
    {
        
        public int Id { get; set; } = 0;

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public byte[]? Salt { get; set; } = null;

        public bool IsMale { get; set; } = true;
        public DateTime? DateOfBirth { get; set; }
        public UserStatus Status { get; set; } = UserStatus.InActive;
        public List<User> FriendList { get; set; } = new List<User>();
    }
}
