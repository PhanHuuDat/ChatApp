using ChatApp.Models;
using ChatApp.Models.Enum;
using ChatApp.Services;

namespace ChatAppTest
{
    public class UserServiceTes
    {
        private UserService userService = new UserService();

        [SetUp]
        public void Setup()
        {
            userService.RegisterUser("test1", "123456");
            userService.RegisterUser("test2", "@Abc123");
            userService.RegisterUser("test3", "Hs1Ts1");
            userService.RegisterUser("test4", "123456789");
            List<User> users = new List<User>() {
                userService.GetUser(1),
                userService.GetUser(2)
            };
        }
        [Test]
        public void RegisterUser()
        {
            int length = userService.GetAll();
            userService.RegisterUser("test5", "1234567890");
            int nextLength = userService.GetAll();
            Assert.That(length, Is.EqualTo(nextLength - 1));
        }

        [Test]
        [TestCase("test1", "123456", LoginStatus.LoginSuccess)]
        [TestCase("test2", "123456", LoginStatus.WrongPassword)]
        [TestCase("test5", "123456", LoginStatus.WrongUsername)]
        public void Login(string username, string password, LoginStatus result)
        {
            LoginStatus status = userService.LoginByUsername(username, password);
            Assert.That(status, Is.EqualTo(result));
        }
        [Test]
        [TestCase(1, 2, 1)]
        [TestCase(1, 3, 2)]
        [TestCase(1, 2, 2)]
        public void FindFriend(int userId, int friendId, int totalFriends)
        {
            userService.AddFriend(userId, friendId);
            int friends = userService.GetUser(userId).FriendList.Count;
            Assert.That(friends, Is.EqualTo(totalFriends));
        }
        [Test]
        public void SetAlias()
        {
            User user1 = userService.GetUser(1);
            User user2 = userService.GetUser(2);
            Assert.True(userService.SetAlias(user1, user2, "Minh Tan"));

        }
    }

}