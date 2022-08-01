namespace ChatApp.Models
{
    public class Alias
    {
        public int Id { get; set; }
        public int AssignorID { get; set; }
        public int AssigneeID { get; set; }
        public string Context { get; set; }

    }
}
