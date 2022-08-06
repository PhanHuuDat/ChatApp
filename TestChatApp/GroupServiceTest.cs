using ChatApp.Data;
using ChatApp.Models;
using ChatApp.Models.Enum;
using ChatApp.Services;

namespace ChatAppTest
{
    public class GroupServiceTest
    {
        private static readonly DataStorage dataStorage = DataStorage.GetDataStorage();
        private UserService userService = new UserService(dataStorage);
        //private MessageService messageService = new MessageService();
        private GroupService groupService = new GroupService(dataStorage);

        #region general functions group test

        [Test]
        public void TestGetAllGroups()
        {
            Assert.That(groupService.GetAllGroups(), !Is.Empty);
        }

        [Test]
        [TestCase("User no.0", 2)]
        [TestCase("User no.4", 0)]
        public void TestGetGroupOfUser(string userName, int expectation)
        {
            User user = dataStorage.Users.GetFirstOrDefault(user => user.UserName.Equals(userName));
            
            Assert.That(groupService.GetGroupOfUser(user).Count(), Is.EqualTo(expectation));
        }

        [Test]
        [TestCase("User no.4", "test public group", GroupStatus.UserNotBelongToGroup)]
        [TestCase("User no.5", "test public group", GroupStatus.UserRemoved)]
        public void TestLeaveGroup(string userName, string groupName, GroupStatus expectation)
        {
            User user = dataStorage.Users.GetFirstOrDefault(user => user.UserName.Equals(userName));
            Group group = (PublicGroup)dataStorage.Groups
                                .GetFirstOrDefault(group => group.Name.Equals(groupName));

            Assert.That(groupService.RemoveUserFromGroup(user, group), Is.EqualTo(expectation));
        }

        #endregion

        #region public group test

        [Test]
        [TestCase("User no.1", "test public group", GroupStatus.GroupExited)]
        [TestCase("User no.2", "test group", GroupStatus.GroupCreated)]
        public void TestCreatePublicGroup(string userName, string groupName, GroupStatus expectation)
        {
            User user = dataStorage.Users
                    .GetFirstOrDefault(user => user.UserName.Equals(userName));
            
            Assert.That(groupService.CreatePublicGroup(user, groupName), Is.EqualTo(expectation));
        }

        [Test]
        [TestCase("User no.1", "test public group", GroupStatus.UserBelongToGroup)]
        [TestCase("User no.2", "test public group", GroupStatus.UserAdded)]
        public void TestJoinPublicGroup(string userName, string groupName, GroupStatus expectation)
        {
            User user = dataStorage.Users
                    .GetFirstOrDefault(user => user.UserName.Equals(userName));
            PublicGroup publicGroup = (PublicGroup)dataStorage.Groups
                                .GetFirstOrDefault(group => group.Name.Equals(groupName));
            string inviteCode = publicGroup.InviteCode;

            Assert.That(groupService.JoinPublicGroup(user, inviteCode), Is.EqualTo(expectation));
        }

        [Test]
        [TestCase("User no.1", "test public group", GroupStatus.UserBelongToGroup)]
        [TestCase("User no.3", "test public group", GroupStatus.UserAdded)]
        public void TestAddUserPublicGroup(string userName, string groupName, GroupStatus expectation)
        {
            User user = dataStorage.Users.GetFirstOrDefault(user => user.UserName.Equals(userName));
            PublicGroup publicGroup = (PublicGroup)dataStorage.Groups
                                .GetFirstOrDefault(group => group.Name.Equals(groupName));
            
            Assert.That(groupService.AddUserPublic(user, publicGroup), Is.EqualTo(expectation));
        }



        #endregion

        #region private group test

        [Test]
        [TestCase("User no.1", "test private group", GroupStatus.GroupExited)]
        [TestCase("User no.2", "my secret group", GroupStatus.GroupCreated)]
        public void TestCreatePrivateGroup(string userName, string groupName, GroupStatus expectation)
        {
            User user = dataStorage.Users
                    .GetFirstOrDefault(user => user.UserName.Equals(userName));

            Assert.That(groupService.CreatePrivateGroup(user, groupName), Is.EqualTo(expectation));
        }

        [Test]
        [TestCase("User no.1", "User no.0", "test private group", GroupStatus.UserBelongToGroup)]
        [TestCase("User no.1", "User no.2", "test private group", GroupStatus.UserAdded)]
        public void TestAdduserPrivateGroup(string adminUserName, string guestUserName, 
                                            string groupName, GroupStatus expectation)
        {
            User admin = dataStorage.Users
                    .GetFirstOrDefault(user => user.UserName.Equals(adminUserName));
            User guest = dataStorage.Users
                    .GetFirstOrDefault(user => user.UserName.Equals(guestUserName));
            PrivateGroup group = (PrivateGroup)dataStorage.Groups
                    .GetFirstOrDefault(group => group.Name.Equals(groupName));

            Assert.That(groupService.AddUserPrivate(admin, guest, group), Is.EqualTo(expectation));
        }


        #endregion
    }

}