using ChatApp.Models.Enum;

namespace ChatApp.Models
{
    public class Message 
    {
        private string id;
        private string? content;
        private string? path;
        private DateTime createdDate;
        private string fromUserId;
        private string inGroupId;
        private FileType? fileType;


        public Message(string? content, string? path, DateTime createdDate, string fromUserId, string inGroupId, FileType? fileType)
        {
            this.id = Guid.NewGuid().ToString("P");
            Content = content;
            Path = path;
            CreatedDate = createdDate;
            FromUserId = fromUserId;
            InGroupId = inGroupId;
            FileType = fileType;
        }

        public string Id 
        { 
            get { return id; } 
        }

        public string? Content 
        { 
            get { return content; } 
            set { content = value; } 
        }

        public string? Path 
        { 
            get { return path; }
            set { path = value; } 
        }

        public DateTime CreatedDate 
        { 
            get { return createdDate; } 
            set { createdDate = value; } 
        }

        public string FromUserId 
        { 
            get { return fromUserId; }
            set { fromUserId = value; } 
        }

        public string InGroupId 
        { 
            get { return inGroupId; } 
            set { inGroupId = value; } 
        }

        public FileType? FileType 
        { 
            get { return fileType; } 
            set { fileType = value; } 
        }
    }
}
