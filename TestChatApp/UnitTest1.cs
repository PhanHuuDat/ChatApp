using ChatApp.Models;
using ChatApp.Models.Enum;
using ChatApp.Services;

namespace ChatAppTest
{
    public class UnitTest1
    {
        private UserService userService = new UserService();
        //private MessageService messageService = new MessageService();
        private GroupService groupService = new GroupService();
        //private FileService fileService = new FileService();

        [SetUp]
        public void Setup()
        {
            userService.RegisterUser("test1", "123456");
            userService.RegisterUser("test2", "@Abc123");
            userService.RegisterUser("test3", "Hs1Ts1");
            userService.RegisterUser("test4", "123456789");

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
        [TestCase("test", 1)]
        public void CreateGroup(string groupName, int expect)
        {
            List<User> users = new List<User>() {
                userService.GetUser(1),
                userService.GetUser(2)
            };
            groupService.CreatePublicGroup(groupName, users);
            int result = groupService.GetAllGroups().Count;
            Assert.That(result, Is.EqualTo(expect));
        }

        //[Test]
        //public void FindFriend_Works()
        //{
        //    //Arrange
        //    var name = "Tan";
        //    var user = new List<User>() { new User { FirstName = "Tran", LastName = "Tan" },
        //                new User { FirstName = "user1", LastName = "1" },
        //                new User { FirstName = "user2", LastName = "2" },
        //                new User { FirstName = "user3", LastName = "3" }};
        //    //Act
        //    var findFriend = userService.FindFriend(user, name);
        //    //Assert
        //    Assert.That(findFriend, Is.EqualTo(user.FindAll(friend => friend.UserName == name)));
        //}
        //[Test]
        //public void FindFriend_Null()
        //{
        //    //Arrange
        //    var user = new List<User>() { new User { FirstName = "Tran", LastName = "Tan" },
        //                new User { FirstName = "user1", LastName = "1" },
        //                new User { FirstName = "user2", LastName = "2" },
        //                new User { FirstName = "user3", LastName = "3" }};
        //    //Act
        //    var findFriend = user == null;
        //    //Assert
        //    Assert.That(findFriend, Is.EqualTo(null));
        //}
        //[Test]
        //public void CreateNewUser()
        //{
        //    //Arrange
        //    //Act          
        //    //Assert
        //}
        //[Test]
        //public void LoginByUsername_WrongUsername()
        //{
        //    //Arrange
        //    var usesrname = "minhtan";
        //    var password = "minhtan123";
        //    //Act          
        //    var Login = userService.LoginByUsername(usesrname, password);
        //    //Assert
        //    Assert.That(Login, Is.EqualTo(null));
        //}
        //[Test]
        //public void LoginByUsername_Success()
        //{
        //    //Arrange
        //    var usesrname = "minhtan";
        //    var password = "minhtan123";
        //    //Act          
        //    var Login = userService.LoginByUsername(usesrname, password);
        //    //Assert
        //    Assert.That(Login, Is.EqualTo(user.GetFirstOrDefault(user => user.UserName == username)));
        //}
    }

}