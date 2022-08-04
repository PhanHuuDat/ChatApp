using ChatApp.Models;
using ChatApp.Models.Enum;
using ChatApp.Services;

namespace ChatAppTest
{
    public class GroupServiceTest
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
        public void LeaveGroup()
        {
            List<User> users = new List<User>() {
                userService.GetUser(1),
                userService.GetUser(2),
                userService.GetUser(3)
            };
            groupService.CreatePublicGroup("Group1", users);
            Assert.True(groupService.RemoveUserFromGroup(1, 2));
        }
    }

}