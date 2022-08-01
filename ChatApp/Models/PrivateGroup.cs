namespace ChatApp.Models
{
    public class PrivateGroup : Group
    {
        public User Admin { get; set; }
    }
}