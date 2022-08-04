using ChatApp.Models;
using ChatApp.Models.Enum;
using ChatApp.Services;

namespace ChatAppTest
{
    public class UnitTest1
    {
        private UserService userService = new UserService();
        private MessageService messageService = new MessageService();
        private GroupService groupService = new GroupService();


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
            groupService.CreatePublicGroup("test", users);
        }
        [Test]
        public void RegisterUser()
        {
            int length = userService.Count();
            userService.RegisterUser("test5", "1234567890");
            int nextLength = userService.Count();
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
        [TestCase("test", 2)]
        public void CreateGroup(string groupName, int expect)
        {
            List<User> users = new List<User>() {
                userService.GetUser(1),
                userService.GetUser(2),
                userService.GetUser(3)
            };
            groupService.CreatePublicGroup(groupName, users);
            int result = groupService.GetAllGroups().Count;
            Assert.That(result, Is.EqualTo(expect));
        }
        [Test]
        public void JoinGroup()
        {
            int userId = 0;
            string inviteCode = groupService.GetPublicGroupById(0).InviteCode;
            bool result = groupService.JoinPublicGroup(userId, inviteCode);
            Assert.That(result, Is.True);
        }
        [Test]
        public void SendTextMessage()
        {
            bool result = messageService.SendMessage(1, 0, "hello");
            Assert.That(result, Is.True);
        }
        [Test]
        public void SendFileMessage()
        {
            bool result = messageService.SendMessage(1, 0, "hello");
            Assert.That(result, Is.True);
        }


    }

}