using ChatApp.Data;
using ChatApp.Models;
using ChatApp.Models.Enum;
using System.Text;

namespace ChatApp.Services
{
    public class GroupService
    {
        private readonly DataStorage dataStorage = DataStorage.GetDataStorage();
        private UserService userService = new UserService();

        #region general

        public IList<Group> GetAllGroups()
        {
            var groups = dataStorage.Groups.GetAll()
                                    .OrderBy(group => group.Name)
                                    .ToList();
            return groups;
        }

        //Get all group which the user is the member or admin
        public IList<Group> GetGroupOfUser(User user)
        {
            var groups = dataStorage.Groups.GetAll(group => group.MemberList
                                    .Select(member => member.Id)
                                    .Contains(user.Id))
                                .OrderBy(group => group.Name)
                                .ToList();
            return groups;
        }

        public GroupStatus RemoveUserFromGroup(User user, Group group)
        {
            //check if user is in group
            var index = group.MemberList.IndexOf(user);
            if (index != -1)
            {
                group.MemberList.RemoveAt(index);
                // disband the group if there is no one else
                var count = group.MemberList.Count();
                if (count == 0)
                {
                    dataStorage.Groups.Remove(group);
                }
                else
                {
                    // transfer admin if group is private and the user is admin
                    // check if the group is private first then admin role
                    if (group.IsPrivate)
                    {
                        var privateGroup = (PrivateGroup)group;
                        if (user.Id == privateGroup.Admin.Id)
                        {
                            //transfer the admin candidate - a random user
                            Random random = new Random();
                            var candidate = group.MemberList[random.Next(0, count - 1)];
                            privateGroup.Admin = candidate;
                        }
                    }
                }
                return GroupStatus.UserRemoved;
            }
            else
            {
                return GroupStatus.UserNotBelongToGroup;
            }
        }

        #endregion

        #region public group 

        public GroupStatus CreatePublicGroup(string groupName, List<User> members)
        {
            if (ValidateGroupNameExistance(groupName))
            {
                return GroupStatus.GroupExited;
            } else
            {
                Group group = new PublicGroup()
                {
                    Name = groupName,
                    MemberList = members,
                    IsPrivate = false,
                };
                GenerateUniqueInviteCode((PublicGroup)group);
                dataStorage.Groups.Add(group);
                return GroupStatus.GroupCreated;
            }
        }

        public void GenerateUniqueInviteCode(PublicGroup group)
        {
            while (ValidateUniqueInviteCode(group.InviteCode))
            {
                group.GenerateInviteCode();
            }
        }

        public GroupStatus JoinPublicGroup(User user, string inviteCode)
        {
            var group = GetGroupByCode(inviteCode);
            if (group == null)
            {
                return GroupStatus.GroupNotFound;
            }
            else
            {
                //validate if user is in group
                foreach (var member in group.MemberList)
                {
                    if (member.Id == user.Id)
                    {
                        return GroupStatus.UserBelongToGroup;
                    }
                }
                //add user to the group if all case satisfied
                group.MemberList.Append(user);
                return GroupStatus.UserAdded;
            }
        }

        public GroupStatus AddUserPublic(User user, PublicGroup group)
        {
            //validate if user is in group
            foreach (var member in group.MemberList)
            {
                if (member.Id == user.Id)
                {
                    return GroupStatus.UserBelongToGroup;
                }
            }
            group.MemberList.Append(user);
            return GroupStatus.UserAdded;
        }

        #endregion

        #region private group

        public GroupStatus CreatePrivateGroup(string groupName, User admin, List<User> members)
        {
            if (ValidateGroupNameExistance(groupName))
            {
                return GroupStatus.GroupExited;
            } else
            {
                Group group = new PrivateGroup()
                {
                    Name = groupName,
                    Admin = admin,
                    MemberList = members,
                    IsPrivate = true,
                };
                dataStorage.Groups.Add(group);
                return GroupStatus.GroupCreated;
            }
        }

        public GroupStatus AddUserPrivate(User admin, User user, PrivateGroup group)
        {
            //validate admin permission
            if (group.Admin.Id == admin.Id)
            {
                // validate if user is a member already
                if (group.MemberList.Contains(user))
                {
                    return GroupStatus.UserBelongToGroup;
                }
                else
                {
                    group.MemberList.Append(user);
                    return GroupStatus.UserAdded;
                }
            }
            else
            {
                return GroupStatus.NotPermit;
            }

        }

        #endregion

        #region ultilities

        private bool ValidateGroupNameExistance(string name)
        {
            var group = dataStorage.Groups.GetFirstOrDefault(group => group.Name.Equals(name));
            if (group == null)
            {
                return false;
            } else
            {
                return true;
            }
        }
        
        private PublicGroup? GetGroupByCode(string inviteCode)
        {
            var groups = dataStorage.Groups.GetAll(group => group.IsPrivate == false);
            foreach (var member in groups)
            {
                var group = (PublicGroup)member;
                if (group.InviteCode != null && group.InviteCode.Equals(inviteCode))
                {
                    return group;
                }
            }
            return null;
        }

        private bool ValidateUniqueInviteCode(string inviteCode)
        {
            var exitedGroup = GetGroupByCode(inviteCode);
            return exitedGroup == null ? false : true;

        }

        #endregion
    }
}
