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
       /* [Test]
        public void SendFileMessage()
        {
            bool result = messageService.UploadNewFile(1, 0, "ABV/");
            Assert.That(result, Is.True);
        }*/
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
       /* [Test]
        public void ShowAllFileInGroup()
        {
            Group group = groupService.GetGroupById(2);
            for (int i = 0; i < 2; i++)
            {
                messageService.UploadNewFile(1, 2, "Hello");
            }
            List<String> fileList = messageService.DisplayAllFile(2);
            Assert.Equals(2, fileList);
        }*/
        [Test]
        public void ShowKLatestMessageGroup()
        {
            for (int i = 0; i < 2; i++)
            {
                messageService.SendMessage(1, 0, "Hello");
            }
            List<Message> messageList = messageService.GetTopLatestMessages(1, 3);
            Assert.AreEqual(1,messageList.Count);
        }
        [Test]
        public void FindMessageByKeywordInUser()
        {
            for (int i = 0; i < 4; i++)
            {
                messageService.SendMessage(1, 0, "hello");
            }
            for (int i = 0; i < 4; i++)
            {
                messageService.SendMessage(1, 0, "hello2");
            }
            for (int i = 0; i < 4; i++)
            {
                messageService.SendMessage(1, 2, "hello");
            }
            List<Message> messageList = messageService.GetMessages(1, 0, "hello2");
            Assert.AreEqual(4, messageList.Count);
        }
        [Test]
        public void ShowAllConversationsInGroup()
        {
            User user1 = userService.GetUser(1);
            for (int i = 0; i < 1; i++)
            {
                messageService.SendMessage(1, 0, "Hello");
            }
            List<Group> groupList = groupService.GetGroupOfUser(user1);
            Assert.AreEqual(1, groupList.Count);
        }
        [Test]
        public void ShowAllConversationUser()
        {
            User user2 = userService.GetUser(2);
            List<int> conversation = messageService.GetConversations(user2);
            Assert.AreEqual(0, conversation.Count);

        }

    }

}