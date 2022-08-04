using ChatApp.Models;
using ChatApp.Models.Enum;
using ChatApp.Services;

namespace ChatAppTest
{
    public class MessageServiceTest
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
        [Test]
        public void DeleteMessage()
        {
            for (int i = 0; i < 2; i++)
            {
                messageService.SendMessage(1, 0, "hello");
            }
            bool result = messageService.DeleteMessage(1, "ABCD");
            Assert.That(result, Is.True);
        }
        [Test]
        public void ShowAllFileInGroup()
        {
            Group group = groupService.GetGroupById(2);
            for (int i = 0; i < 2; i++)
            {
                messageService.UploadNewFile(1, 3, ");
            }
            List<String> fileList = messageService.DisplayAllFile(3);
            Assert.Equals(2, fileList);
        }
        [Test]
        public void showKLatestMessageGroup()
        {
            for (int i = 0; i < 2; i++)
            {
                messageService.SendMessage(1, 0, "Hello");
            }
            List<Message> messageList = messageService.GetTopLatestMessages(1, 10);
            Assert.Equals(messageList, Is.True);
        }
        [Test]
        public void findMessageByKeywordInUser()
        {
            for (int i = 0; i < 3; i++)
            {
                messageService.SendMessage(1, 0, "hello");
            }
            for (int i = 0; i < 3; i++)
            {
                messageService.SendMessage(1, 0, "hello2");
            }
            for (int i = 0; i < 3; i++)
            {
                messageService.SendMessage(1, 2, "hello");
            }
            List<Message> messageList = messageService.FindMessage();
            Assert.Equals(3, messageList);
        }


    }

}