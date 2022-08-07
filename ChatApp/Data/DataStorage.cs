using ChatApp.Models;
using ChatApp.Repository;

namespace ChatApp.Data
{
    public class DataStorage
    {
        public Repository<User> Users { get; }
        public Repository<Message> Messages { get; }
        public Repository<Group> Groups { get; }
        public Repository<Alias> Aliases { get; }
        private static DataStorage _dataStorage { get; set; }
        private DataStorage()
        {
            Users = new Repository<User>();
            Messages = new Repository<Message>();
            Groups = new Repository<Group>();
            Aliases = new Repository<Alias>();
            InitData();
        }
        public static DataStorage GetDataStorage()
        {
            if (_dataStorage == null)
            {
                _dataStorage = new DataStorage();
            }

            return _dataStorage;
        }

        private void InitData()
        {
            //create dummy users
            IList<User> testUsers = new List<User>();

            for (int i = 0; i < 6; i++)
            {
                User user = new User("User no." + i, "User@" + i);
                Users.Add(user);
                if (i < 2 || i == 5)
                {
                    testUsers.Add(user);
                }
            }

            //create dummy group

            Group group = new PublicGroup("test public group", testUsers, false);
            Groups.Add(group);
            group = new PrivateGroup(testUsers[1], "test private group", testUsers, true);
            Groups.Add(group);
        }
    }
}
