using ChatApp.Data;
using ChatApp.Models;
using ChatApp.Models.Enum;
using ChatApp.Services;
using Microsoft.AspNetCore.Http;
using static System.Net.Mime.MediaTypeNames;

namespace ChatAppTest
{
    public class MessageServiceTest
    {
        private static readonly DataStorage dataStorage = DataStorage.GetDataStorage();
        private UserService userService = new UserService(dataStorage);
        private MessageService messageService = new MessageService();
        private GroupService groupService = new GroupService(dataStorage);

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
        public void SendTextMessage()
        {
            bool result = messageService.SendMessage("A1", "G1", "hello");
            Assert.That(result, Is.True);
        }
        /*[Test]
      *//*  public void SendFileMessage()
        {
            bool result = messageService.UploadNewFile(1, 0, "ABV/");
            Assert.That(result, Is.True);
        }*/
        [Test]
        public void DeleteMessage()
        {
            for (int i = 0; i < 2; i++)
            {
                messageService.SendMessage("A1", "G1", "hello");
            }
            bool result = messageService.DeleteMessage(1, "ABCD");
            Assert.That(result, Is.True);
        }
      /*  [Test]
        [TestCase("A1","G1","/path",,FileType:Image)]
        public void ShowAllFileInGroup(string userId, string groupId, string webRootPath, IFormFileCollection? files, FileType? fileType)
        {
            User user = dataStorage.Users.GetFirstOrDefault(user => user.UserName.Equals(userId));
            Group group = groupService.GetGroupOfUser(user);
            List<String> fileList = messageService.DisplayAllFile(group);
            Assert.Equals(2, fileList);
        }*/
        [Test]
        public void ShowKLatestMessageGroup()
        {
            for (int i = 0; i < 2; i++)
            {
                messageService.SendMessage("A1", "G1", "hello");
            }
            List<Message> messageList = messageService.GetTopLatestMessages(1, 3);
            Assert.AreEqual(1, messageList.Count);
        }
        [Test]
        [TestCase("A1", "G1", "hello")]
        [TestCase("A1", "G1", "hello")]
        [TestCase("A2", "G2", "hello")]
        public void FindMessageByKeywordInUser(string Userid, string groupId, string keyword)
        {
            for (int i = 0; i < 4; i++)
            {
                messageService.SendMessage("A1", "G1", "hello");
            }
            for (int i = 0; i < 4; i++)
            {
                messageService.SendMessage("A1", "G0", "hello2");
            }
            for (int i = 0; i < 4; i++)
            {
                messageService.SendMessage("A1", "G1", "hello");
            }
            List<Message> messageList = messageService.GetMessages("A1", "G1", "hello");
            Assert.That(messageService.GetMessages(Userid, groupId, keyword),Is.EqualTo(messageList));
        }
        [Test]
        [TestCase("User1")]
        [TestCase("User2")]
        public void ShowAllConversationsInGroup(string userName)
        {
            User user = dataStorage.Users.GetFirstOrDefault(user => user.UserName.Equals(userName));
            for (int i = 0; i < 1; i++)
            {
                messageService.SendMessage("A1", "G1", "hello");
            }
            List<Group> groupList = (List<Group>)groupService.GetGroupOfUser(user);
            Assert.That(messageService.GetConversations(user),Is.True);
        }
        [Test]
        [TestCase("User1")]
        [TestCase("User2")]
        public void ShowAllConversationUser(string userName)
        {
            User user = dataStorage.Users.GetFirstOrDefault(user => user.UserName.Equals(userName));
            Assert.That(messageService.GetConversations(user),Is.True);

        }

    }

}