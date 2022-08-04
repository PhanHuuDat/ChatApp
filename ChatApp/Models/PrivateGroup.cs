namespace ChatApp.Models
{
    public class PrivateGroup : Group
    {
        private User admin;

        public PrivateGroup(string name, IList<User> memberList, bool isPrivate) : base(name, memberList, isPrivate)
        {
        }

        public User Admin 
        {
            get { return admin; } 
            set { admin = value; }
        }
    }
}