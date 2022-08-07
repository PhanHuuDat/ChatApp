using ChatApp.Models.Enum;

namespace ChatApp.Models
{
    public class Message
    {
        private string id;
        private string? content;
        private string? path;
        private DateTime createDate;
        private string fromUserId;
        private string inGroupId;
        private FileType? fileType;

        public Message(string? content, string? path, string fromUserId, string inGroupId, FileType? fileType = null)
        {
            this.id = Guid.NewGuid().ToString("N");
            this.content = content;
            this.path = path;
            this.fromUserId = fromUserId;
            this.fileType = fileType;
            this.inGroupId = inGroupId;
            this.fileType = fileType;
            createDate = DateTime.Now;
        }

        public int Id { get; }
        public string? Content
        {
            get
            {
                return content != null ? content : string.Empty;
            }
            set
            {
                if (value != null)
                {
                    content = value;
                }
            }
        }
        public string? Path
        {
            get
            {
                return path != null ? path : string.Empty;
            }
            set
            {
                if (value != null)
                {
                    path = value;
                }
            }
        }
        public DateTime CreatedDate { get; }
        public string FromUserId
        {
            get
            {
                return fromUserId != null ? fromUserId : string.Empty;
            }
            set
            {
                if (value != null)
                {
                    fromUserId = value;
                }
            }
        }
        public string InGroupId
        {
            get
            {
                return inGroupId != null ? inGroupId : string.Empty;
            }
            set
            {
                if (value != null)
                {
                    inGroupId = value;
                }
            }
        }
        public FileType? FileType
        {
            get
            {
                return fileType;
            }
            set
            {
                if (value != null)
                {
                    fileType = value;
                }
            }
        }
    }
}
