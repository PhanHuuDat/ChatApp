using ChatApp.Models.Enum;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Net.Mail;
using System.Security.Cryptography;

namespace ChatApp.Models
{
    public class User
    {

        #region field
        private string id;
        private string firstName;
        private string lastName;
        private string username;
        private string email;
        private string password;
        private byte[] salt;
        private bool isMale;
        private DateTime dateOfBirth;
        private UserStatus status;
        private IList<User> friendList;
        #endregion
        public User(string username, string password)
        {
            this.id = Guid.NewGuid().ToString("N");
            this.username = username;
            this.salt = GenerateRandomSalt();
            this.password = HashPassword(password, salt);
        }

        #region properties
        public string Id
        {
            get { return id; }
        }
        public string? FirstName
        {
            get
            {
                return this.firstName;
            }
            set
            {
                firstName = value;
            }
        }
        public string? LastName
        {
            get
            {
                return this.lastName;
            }
            set
            {
                lastName = value;
            }
        }
        public string UserName
        {
            get
            {
                return username;
            }
        }
        public string Email
        {
            get
            {
                return this.email;
            }
            set
            {
                if (IsEmailValid(value))
                {
                    this.email = value;
                }
            }
        }
        public string Password { get; }
        public byte[] Salt { get; }
        public bool IsMale { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public UserStatus Status { get; set; } = UserStatus.InActive;
        public IList<User> FriendList { get; set; }
        #endregion
        private byte[] GenerateRandomSalt()
        {
            byte[] salt = new byte[16];
            using (var rngCsp = RandomNumberGenerator.Create())
            {
                rngCsp.GetNonZeroBytes(salt);
            }
            return salt;
        }
        public static string HashPassword(string password, byte[] salt)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));
            return hashed;
        }
        private bool IsEmailValid(string emailaddress)
        {
            bool isValid = MailAddress.TryCreate(emailaddress, out MailAddress? result);
            return isValid;
        }
    }
}