namespace ChatApp.Models
{
    public class PrivateGroup : Group
    {
        private User admin;

        public PrivateGroup(User admin, string name, IList<User> memberList, bool isPrivate) : base(name, memberList, isPrivate)
        {
            this.admin = admin;
        }

        public User Admin 
        {
            get { return admin; } 
            set { admin = value; }
        }
    }
}