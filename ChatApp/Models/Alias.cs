namespace ChatApp.Models
{
    public class Alias
    {
        private string id;
        private string assignorID;
        private string assigneeID;
        private string context;

        public Alias(string assignorID, string assigneeID, string context)
        {
            id = Guid.NewGuid().ToString("N");
            AssignorID = assignorID;
            AssigneeID = assigneeID;
            Context = context;
        }

        public string Id
        {
            get { return id; }
        }

        public string AssignorID
        {
            get { return assignorID; }
            set { assignorID = value; }
        }

        public string AssigneeID
        {
            get { return assigneeID; }
            set { assigneeID = value; }
        }

        public string Context
        {
            get { return context; }
            set { context = value; }
        }

    }
}
