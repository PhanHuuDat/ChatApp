
using ChatApp.Data;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using ChatApp.Models;
using ChatApp.Models.Enum;

namespace ChatApp.Services
{
    public class UserService
    {
        private readonly DataStorage dataStorage = DataStorage.GetDataStorage();

        #region user-crud
        public List<User>? FindFriend(User user, string name)
        {
            List<User>? userList = user.FriendList?.FindAll(friend => friend.UserName == name);
            if (userList == null)
            {
                return null;
            }
            return userList;
        }

        public LoginStatus LoginByUsername(string username, string password)
        {
            User? aUser = dataStorage.Users.GetFirstOrDefault(user => user.UserName == username);

            if (aUser == null)
            {
                return LoginStatus.WrongUsername;
            }
            if (aUser.Password != HashPassword(password, aUser.Salt))
            {
                return LoginStatus.WrongPassword;
            }
            return LoginStatus.LoginSuccess;
        }

        public void RegisterUser(string username, string password)
        {
            int userId = GenerateUserId();
            byte[]? salt = GetRandomSalt();

            User? user = new User()
            {
                Id = userId,
                UserName = username,
                Salt = salt,
                Password = HashPassword(password, salt)
            };
            dataStorage.Users.Add(user);
        }

        public void AddFriend(int userId, int friendId)
        {
            User user = dataStorage.Users.GetFirstOrDefault(user => user.Id == userId);
            User friend = dataStorage.Users.GetFirstOrDefault(user => user.Id == friendId);
            if (!user.FriendList.Contains(friend))
            {
                user.FriendList.Add(friend);
            }           
        }

        #endregion

        #region general
        public int GetAll()
        {
            return dataStorage.Users.GetAll().Count();
        }
        public User GetUser(int userId)
        {
            var user = dataStorage.Users.GetFirstOrDefault(user => user.Id == userId);
            return user;
        }

        public UserStatus ValidateUserExistance(int userId)
        {
            var result = dataStorage.Users.Contain(user => user.Id == userId);
            if (result)
            {
                return UserStatus.UserFound;
            } else
            {
                return UserStatus.UserNotFound;
            }
        }

        public bool SetAlias(User assignor, User assignee, string context)
        {
            //if (assignor != null && Assignee != null)
            //{
            //    Alias alias = new Alias(assignor.Id, assignee.Id, context);
            //    alias.AssignorID = assignor.Id;
            //    alias.AssigneeID = Assignee.Id;
            //    alias.Context = context;
            //    dataStorage.Aliases.Add(alias);
            //    return true;
            //}
            return false;
        }

        #endregion

        #region ultilities
        private int GenerateUserId()
        {
            int id = 0;
            if (dataStorage.Users.GetAll().ToArray() != null)
            {
                id = dataStorage.Users.GetAll().ToArray().Length;
            }          
            return id;
        }

        private string HashPassword(string password, byte[] salt)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));
            return hashed;
        }

        private byte[] GetRandomSalt()
        {
            byte[] salt = new byte[16];
            using (var rngCsp = RandomNumberGenerator.Create())
            {
                rngCsp.GetNonZeroBytes(salt);
            }
            return salt;
        }
        #endregion
    }
}
