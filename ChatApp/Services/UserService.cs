using ChatApp.Data;
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
            List<User>? userList = user.FriendList?.Where(friend => friend.UserName == name).ToList();
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
                return LoginStatus.LoginFail;
            }
            if (aUser.Password != User.HashPassword(password, aUser.Salt))
            {
                return LoginStatus.LoginFail;
            }
            return LoginStatus.LoginSuccess;
        }

        public RegisterStatus RegisterUser(string username, string password)
        {
            User user;
            User? tempUser = dataStorage.Users.GetFirstOrDefault(user => user.UserName == username);
            if (username != null && password != null)
            {
                if (tempUser == null)
                {
                    user = new User(username, password);
                    dataStorage.Users.Add(user);
                    return RegisterStatus.RegisterSuccess;
                }
            }
            return RegisterStatus.RegisterFail;
        }

        public void AddFriend(string userId, string friendId)
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
        public int Count()
        {
            return dataStorage.Users.GetAll().Count();
        }
        public User GetUser(string userId)
        {
            var user = dataStorage.Users.GetFirstOrDefault(u => u.Id == userId);
            return user;
        }

        public bool SetAlias(User assignor, User assignee, string context)
        {
            if (assignor != null && assignee != null)
            {
                Alias alias = new Alias(assignor.Id, assignee.Id, context);
                dataStorage.Aliases.Add(alias);
                return true;
            }
            return false;
        }

        #endregion

    }
}