using ChatApp.Models;
using ChatApp.Services;

namespace ChatAppTest
{
    public class UserServiceTests
    {
        private UserService userService;

        [SetUp]
        public void Setup()
        {
            userService = new UserService();
        }
        [Test]
        public void FindFriend_Works()
        {
            //Arrange
            var name = "Tan";
            var user = new List<User>() { new User { FirstName = "Tran", LastName = "Tan" },
                        new User { FirstName = "user1", LastName = "1" },
                        new User { FirstName = "user2", LastName = "2" },
                        new User { FirstName = "user3", LastName = "3" }};
            //Act
            var findFriend = userService.FindFriend(user, name);
            //Assert
            Assert.That(findFriend, Is.EqualTo(user.FindAll(friend => friend.UserName == name)));
        }
        [Test]
        public void FindFriend_NotNull()
        {
            //Arrange
            var user = new List<User>() { new User { FirstName = "Tran", LastName = "Tan" },
                        new User { FirstName = "user1", LastName = "1" },
                        new User { FirstName = "user2", LastName = "2" },
                        new User { FirstName = "user3", LastName = "3" }};
            //Act
            var findFriend = user == null;
            //Assert
            Assert.That(findFriend, Is.EqualTo(null));
        }
        [Test]
        public void CreateNewUser()
        {
            //Arrange
            //Act          
            //Assert
        }
        [Test]
        public void LoginByUsername_WrongUsername()
        {
            //Arrange
            var usesrname = "minhtan";
            var password = "minhtan123";
            //Act          
            var Login = userService.LoginByUsername(usesrname, password);
            //Assert
            Assert.That(Login, Is.EqualTo(null));
        }
        [Test]
        public void LoginByUsername_Success()
        {
            //Arrange
            var usesrname = "minhtan";
            var password = "minhtan123";
            //Act          
            var Login = userService.LoginByUsername(usesrname, password);
            //Assert
            Assert.That(Login, Is.EqualTo(user.GetFirstOrDefault(user => user.UserName == username)));
        }
    }
    public class GroupServiceTests
    {

        [Test]
        public void CreatePublicGroup()
        {
            //Arrange
            PrivateGroup privateGroup = new PrivateGroup();
            //Act          
            //Assert
            Assert.That(CreatePublicGroup, Is.True);
        }
    }
    public class MessageServiceTests
    {
        [Test]
        public void DeleteMessage()
        {
            //Arrange
            //Act          
            //Assert
            Assert.That(DeleteMessage, Is.True);
        }
    }
}