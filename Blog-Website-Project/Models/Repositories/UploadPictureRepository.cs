using Blog_Website_Project.Models.Abstracts;
using Blog_Website_Project.Models.DTOs;
using Core.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Blog_Website_Project.Models.Repositories
{
    public class UploadPictureRepository : IUploadPictureRepository
    {
        private IWebHostEnvironment Environment;

        public UploadPictureRepository(IWebHostEnvironment _environment)
        {
            this.Environment = _environment;
        }
        public string GetPhotoFile(IFormFile file)
        {
            if (file == null)
            {
                return @"\Images\user-icon.png";
            }
            string wwwPath = this.Environment.WebRootPath;
            string contentPath = this.Environment.ContentRootPath;
            string path = Path.Combine(Environment.WebRootPath, "Images");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fileName = Path.GetFileName(file.FileName);
            FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create);
            file.CopyTo(stream);

            return GetPhotoPathFormat(stream.Name);
        }

        private string GetPhotoPathFormat(string path)
        {
            string[] str = path.Split(@"\");
            string newPath = @"\" + str[str.Length - 2] + @"\" + str[str.Length - 1];
            return newPath;
        }
        public User ConvertUserDTO(UserDTO userDTO, string userId)
        {
            User user = new User()
            {
                UserID = Guid.Parse(userId),
                UserName = userDTO.UserName,
                FullName = userDTO.FullName,
                Description = userDTO.Description,
                Picture = GetPhotoFile(userDTO.Picture),
                Url = userDTO.Url,
                Email = userDTO.Email
            };
            return user;
        }
    }
}
