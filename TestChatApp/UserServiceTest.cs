using ChatApp.Data;
using ChatApp.Models;
using ChatApp.Models.Enum;
using ChatApp.Services;

namespace ChatAppTest
{
    public class UserServiceTes
    {
        private static readonly DataStorage dataStorage = DataStorage.GetDataStorage();
        private UserService userService = new UserService(dataStorage);

        [SetUp]
        public void Setup()
        {
            userService.RegisterUser("test1", "123456");
            userService.RegisterUser("test2", "@Abc123");
            userService.RegisterUser("test3", "Hs1Ts1");
            userService.RegisterUser("test4", "123456789");
            List<User> users = new List<User>() {
                userService.GetUser("A1"),
                userService.GetUser("A2")
            };
        }
        [Test]
        [TestCase("test1", "123456", RegisterStatus.RegisterSuccess)]
        [TestCase("test2", "123456", RegisterStatus.RegisterFail)]
        public void TestRegisterUser(string username, string password, RegisterStatus expectation)
        {
            User user = dataStorage.Users.GetFirstOrDefault(user => user.UserName == username);
            Assert.That(userService.RegisterUser(username, password), Is.EqualTo(expectation));
        }

        [Test]
        [TestCase("test1", "123456", LoginStatus.LoginSuccess)]
        [TestCase("test2", "123456", LoginStatus.LoginFail)]
        [TestCase("test5", "123456", LoginStatus.LoginFail)]
        public void TestLogin(string username, string password, LoginStatus result)
        {
            LoginStatus status = userService.LoginByUsername(username, password);
            Assert.That(status, Is.EqualTo(result));
        }
        [Test]
        [TestCase("A1", "B2", 1)]
        [TestCase("B1", "B2", 2)]
        [TestCase("C1", "C2", 2)]
        public void TestFindFriend(string userId, string friendId, int totalFriends)
        {
            userService.AddFriend(userId, friendId);
            int friends = userService.GetUser(userId).FriendList.Count;
            Assert.That(friends, Is.EqualTo(totalFriends));
        }
        [Test]
        public void TestSetAlias()
        {
            User user1 = userService.GetUser("A1");
            User user2 = userService.GetUser("A2");
            Assert.True(userService.SetAlias(user1, user2, "Minh Tan"));

        }
    }

}