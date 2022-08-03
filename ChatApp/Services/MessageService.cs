using ChatApp.Data;
using ChatApp.Models;
using ChatApp.Models.Enum;
using Microsoft.AspNetCore.Http;

namespace ChatApp.Services
{
    public class MessageService
    {
        private readonly DataStorage dataStorage = DataStorage.GetDataStorage();
       


        #region input
        public bool SendMessage(int uid, int groupId, string? content, string? filePath = null, FileType fileType = FileType.Image)
        {
            if (content != null)
            {
                Message message = new Message()
                {
                    Id = GenerateMessageId(),
                    Content = content,
                    CreatedDate = DateTime.Now,
                    Path = filePath,
                    FileType = fileType,
                    FromUserId = uid,
                    InGroupId = groupId,
                };
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
                    DeleteFileIfExist(message.Path, webRootPath);
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

        public List<string>? DisplayAllFile(int groupId)
        {
            List<Message>? messageList = dataStorage.Messages.GetAll(mess => mess.Path != null && mess.InGroupId == groupId).ToList();
            List<string>? filePathList = null;
            if (messageList != null)
            {
                filePathList = messageList.Select(message => message.Path).ToList();
            }
            return filePathList;
        }

        public void DeleteFileIfExist(string imageUrl, string webRootPath)
        {
            var oldImage = Path.Combine(webRootPath, imageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImage))
            {
                System.IO.File.Delete(oldImage);
            }
        }
        public void UploadNewFile(int userId, int groupId, string webRootPath, IFormFileCollection? files)
        {
            string fileName_new = Guid.NewGuid().ToString();
            var uploads = Path.Combine(webRootPath, @"images\menuItems");
            var extension = Path.GetExtension(files[0].FileName);

            using (var fileStream = new FileStream(Path.Combine(uploads, fileName_new + extension), FileMode.Create))
            {
                files[0].CopyTo(fileStream);
            }
            Message message = new()
            {
                Id = GenerateMessageId(),
                Path = @"\images\menuItems\" + fileName_new + extension,
                InGroupId = groupId,
                FromUserId = userId,
                CreatedDate = DateTime.Now,
            };
            dataStorage.Messages.Add(message);
        }
    }
}
