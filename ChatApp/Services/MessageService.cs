using ChatApp.Data;
using ChatApp.Models;

namespace ChatApp.Services
{
    public class MessageService
    {
        private readonly DataStorage dataStorage = DataStorage.GetDataStorage();
        private FileService fileService;
        public MessageService(FileService fileService)
        {
            this.fileService = fileService;
        }

        #region input
        public bool SendMessage(int uid, int groupId, string content)
        {
            if (content != null)
            {
                Message message = new Message();
                message.Id = GenerateMessageId();
                message.Content = content;
                message.CreatedDate = DateTime.Now;
                message.FromUserId = uid;
                message.InGroupId = groupId;
                dataStorage.Messages.Add(message);
                return true;
            }
            return false;
        }
        public bool DeleteMessage(int id, string webRootPath)
        {
            Message? message = dataStorage.Messages.GetFirstOrDefault(mess => mess.Id == id);
            if (message != null)
            {
                dataStorage.Messages.Remove(message);
                if (message.Path != null)
                {
                    fileService.DeleteFileIfExist(message.Path, webRootPath);
                }
                return true;
            }
            return false;
        }
        #endregion

        #region output
        public List<int> GetConversations(User user)
        {
            List<int> conversations = new List<int>();
            conversations = dataStorage.Messages.GetAll(u => u.FromUserId == user.Id).Select(m => m.Id).ToList();

            return conversations;
        }

        public List<Message> GetTopLatestMessages(int groupId, int amount)
        {
            List<Message> messagesList;
            messagesList = dataStorage.Messages.GetAll(g => g.Id == groupId)
                            .OrderBy(m => m.CreatedDate)
                            .TakeLast(amount + 1)
                            .Take(amount)
                            .ToList();

            return messagesList;
        }

        public List<Message> GetMessages(int Userid, int groupId, string keyword)
        {
            List<Message> messagesList;
            messagesList = dataStorage.Messages.GetAll(
                            m => m.FromUserId == Userid &&
                            m.InGroupId == groupId &&
                            m.Content.Contains(keyword))
                            .OrderBy(m => m.CreatedDate)
                            .ToList();

            return messagesList;
        }
        #endregion

        #region ultilities
        public int GenerateMessageId()
        {
            int id = 0;
            if (dataStorage.Messages.GetAll().ToArray() != null)
            {
                id = dataStorage.Messages.GetAll().ToArray().Length;
            }
            return id;
        }
        #endregion
    }
}
