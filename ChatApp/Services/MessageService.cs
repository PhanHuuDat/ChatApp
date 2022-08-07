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
        public bool SendMessage(string uid, string groupId, string? content, string? filePath = null, FileType fileType = FileType.Image)
        {
            if (content != null)
            {
                Message message = new Message(content,filePath,uid,groupId);
                dataStorage.Messages.Add(message);
                return true;
            }
            return false;
        }
        public bool DeleteMessage(int id, string webRootPath)
        {
            Message? message = dataStorage.Messages.GetFirstOrDefault(mess => mess.Id.Equals(id));
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
        public List<string> GetConversations(User user)
        {
            List<string> conversations = new List<string>();
            conversations = dataStorage.Messages.GetAll(u => u.FromUserId.Equals(user.Id)).Select(m => m.Id).ToList();

            return conversations;
        }

        public List<Message> GetTopLatestMessages(int groupId, int amount)
        {
            List<Message> messagesList;
            messagesList = dataStorage.Messages.GetAll(g => g.Id.Equals(groupId))
                            .OrderBy(m => m.CreatedDate)
                            .TakeLast(amount + 1)
                            .Take(amount)
                            .ToList();

            return messagesList;
        }

        public List<Message> GetMessages(string Userid, string groupId, string keyword)
        {
            List<Message> messagesList;
            messagesList = dataStorage.Messages.GetAll(
                            m => m.FromUserId.Equals(Userid) &&
                            m.InGroupId.Equals(groupId) &&
                            m.Content.Contains(keyword))
                            .OrderBy(m => m.CreatedDate)
                            .ToList();

            return messagesList;
        }
        #endregion

        #region file
        public List<string>? DisplayAllFile(string groupId)
        {
            List<Message>? messageList = dataStorage.Messages.GetAll(mess => mess.Path != null && mess.InGroupId.Equals(groupId)).ToList();
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

        public void UploadNewFile(string userId, string groupId, string webRootPath, IFormFileCollection? files, FileType? fileType)
        {
            string fileName_new = Guid.NewGuid().ToString();
            var uploads = Path.Combine(webRootPath, @"images");
            var extension = Path.GetExtension(files[0].FileName);

            using (var fileStream = new FileStream(Path.Combine(uploads, fileName_new + extension), FileMode.Create))
            {
                files[0].CopyTo(fileStream);
            }
            var path = @"\images\" + fileName_new + extension;
            Message message = new Message(string.Empty, path, groupId, userId, fileType);

            dataStorage.Messages.Add(message);
        }
        #endregion
    }
}
