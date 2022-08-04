using System.Text;

namespace ChatApp.Models
{
    public class PublicGroup : Group
    {
        private string inviteCode;

        public PublicGroup(string name, IList<User> memberList, bool isPrivate) : base(name, memberList, isPrivate)
        {
        }

        public string InviteCode
        {
            get { return inviteCode; }
        }

        public void GenerateInviteCode()
        {
            inviteCode = GenerateRandomString();
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
    }
}