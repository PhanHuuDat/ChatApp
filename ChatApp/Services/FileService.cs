using ChatApp.Data;
using ChatApp.Models;

namespace ChatApp.Services
{
    public class FileService
    {
        private readonly DataStorage dataStorage = DataStorage.GetDataStorage();
        private MessageService messageService;
        public FileService(MessageService messageService)
        {
            this.messageService = messageService;
        }
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
                Id = messageService.GenerateMessageId(),
                Path = @"\images\menuItems\" + fileName_new + extension,
                InGroupId = groupId,
                FromUserId = userId,
                CreatedDate = DateTime.Now,
            };
            dataStorage.Messages.Add(message);
        }

    }
}