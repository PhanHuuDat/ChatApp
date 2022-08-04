namespace ChatApp.Models
{
    public abstract class Group
    {
        private string id;
        private string name;
        private IList<User> memberList;
        private bool isPrivate;

        public Group(string name, IList<User> memberList, bool isPrivate)
        {
            id = Guid.NewGuid().ToString("D");
            Name = name;
            MemberList = memberList;
            IsPrivate = isPrivate;
        }

        public string Id
        {
            get { return id; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public IList<User> MemberList
        {
            get { return memberList; }
            set { memberList = value; }
        }

        public bool IsPrivate
        {
            get { return isPrivate; }
            set { isPrivate = value; }
        }

    }
}
