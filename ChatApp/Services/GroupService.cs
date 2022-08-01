using ChatApp.Data;
using ChatApp.Models;
using System.Text;

namespace ChatApp.Services
{
    public class GroupService
    {
        private readonly DataStorage dataStorage;
        private UserService userService;
        public GroupService(UserService userService)
        {
            dataStorage = DataStorage.GetDataStorage();
            this.userService = userService;
        }
        //For all boolean methods in this, return if action is success
        #region general
        public List<Group> GetGroupOfUser(User user)
        {
            List<Group> groups = new List<Group>();
            var getAllGroup = dataStorage.Groups.GetAll(group => group.MemberList.Select(m => m.Id)
                                .Contains(user.Id)).ToList();
            return getAllGroup;
        }

        public bool RemoveUserFromGroup(int userId, int groupId)
        {
            //remove the user from the member list
            var group = GetGroupById(groupId);
            var user = userService.GetUser(userId);
            var index = group.MemberList.IndexOf(user);
            if (index != -1)
            {
                group.MemberList.RemoveAt(index);
                // transfer admin if private group and the user is admin
                // check if the group is private first then admin role
                if (group.IsPrivate)
                {
                    var privateGroup = (PrivateGroup)group;
                    if (userId == privateGroup.Admin.Id)
                    {
                        //get the admin candidate - the next earliest user
                        var candidate = group.MemberList[0];
                        privateGroup.Admin = candidate;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region public group 

        public void CreatePublicGroup(string groupName, List<User> members)
        {
            Group group = new PublicGroup()
            {
                Id = GenerateGroupId(),
                Name = groupName,
                MemberList = members,
                IsPrivate = false,
            };
            GenerateInviteCode(group.Id);

            dataStorage.Groups.Add(group);
        }

        public bool JoinPublicGroup(int userId, string inviteCode)
        {
            //validate user existance
            var user = userService.GetUser(userId);
            if (user == null)
            {
                return false;
            }

            //validate invite code
            var group = GetGroupByCode(inviteCode);
            if (group == null)
            {
                return false;
            }

            //validate if user is in group
            foreach (var member in group.MemberList)
            {
                if (member.Id == userId)
                {
                    return false;
                }
            }

            //add user to the group if all case satisfied
            group.MemberList.Append(user);
            return true;

        }

        public bool AddUserPublic(int userId, int groupId)
        {
            //validate group existance
            var group = GetPrivateGroupById(groupId);
            if (group == null)
            {
                return false;
            }

            //validate user exitstance
            var user = userService.GetUser(userId);
            if (user == null)
            {
                return false;
            }

            group.MemberList.Append(user);
            return true;
        }

        #endregion

        #region private group

        public void CreatePrivateGroup(string groupName, User admin, List<User> members)
        {
            Group group = new PrivateGroup()
            {
                Id = GenerateGroupId(),
                Name = groupName,
                Admin = admin,
                MemberList = members,
                IsPrivate = true,
            };
            dataStorage.Groups.Add(group);
        }

        public bool AddUserPrivate(int adminID, int userId, int groupId)
        {
            //validate group existance
            var group = GetPrivateGroupById(groupId);
            if (group == null)
            {
                return false;
            }

            //validate user exitstance
            var user = userService.GetUser(userId);
            if (user == null)
            {
                return false;
            }

            //If all condition are passed, add the user to the group if the admin is truthly an admin
            if (group.Admin.Id == adminID)
            {
                group.MemberList.Append(user);
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region ultilities function

        private Group GetGroupById(int groupId)
        {
            var group = dataStorage.Groups.GetFirstOrDefault(g => g.Id == groupId);
            return group;
        }

        private PublicGroup GetPublicGroupById(int groupId)
        {
            var group = dataStorage.Groups.GetFirstOrDefault(g => g.Id == groupId && !g.IsPrivate);
            return group as PublicGroup;
        }

        private PrivateGroup GetPrivateGroupById(int groupId)
        {
            var group = dataStorage.Groups.GetFirstOrDefault(g => g.Id == groupId && g.IsPrivate);
            return group as PrivateGroup;
        }

        private PublicGroup GetGroupByCode(string inviteCode)
        {
            var groups = dataStorage.Groups.GetAll(g => g.IsPrivate == false);
            foreach (var g in groups)
            {
                var group = g as PublicGroup;
                if (group.InviteCode.Equals(inviteCode))
                {
                    return group;
                }
            }
            return null;
        }

        /// <summary>
        /// Generate a unique group invite code 
        /// Should only be available for public group
        /// </summary>
        private bool GenerateInviteCode(int groupId)
        {
            string generatedInviteCode;

            // validate group existance
            var group = GetPublicGroupById(groupId);
            if (group == null)
            {
                return false;
            }
            else
            {
                //Check if the group has an invite code
                //if not then generate and check unique code again  
                if (String.IsNullOrEmpty(group.InviteCode))
                {
                    do
                    {
                        generatedInviteCode = GenerateRandomString();
                    }
                    while (GetGroupByCode(generatedInviteCode) != null);
                    group.InviteCode = generatedInviteCode;
                }
                return true;
            }
        }

        /// <summary>
        /// Generate a random string with leter value from 65 to 90
        /// Not guarantee to generate unique string
        /// </summary>
        /// <returns>
        /// Return a string with length 7 and random upper or lower case
        /// </returns>
        private string GenerateRandomString()
        {
            int length = 7;
            StringBuilder outBuffer = new StringBuilder();
            Random random = new Random();

            char letter;

            for (int i = 0; i < length; i++)
            {
                double floatNumber = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * floatNumber));
                letter = Convert.ToChar(shift + 65);
                int shouldLowerCase = random.Next(0, 2);
                if (shouldLowerCase == 1)
                {
                    letter = Char.ToLower(letter);
                }
                outBuffer.Append(letter);
            }
            return outBuffer.ToString();
        }

        private int GenerateGroupId()
        {
            int id = 0;
            if (dataStorage.Groups.GetAll().ToArray() != null)
            {
                id = dataStorage.Groups.GetAll().ToArray().Length;
            }
            return id;
        }
        #endregion
    }
}
