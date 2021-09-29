
using Blog_Website_Project.Models.DTOs;
using Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Blog_Website_Project.Models.Abstracts
{
    public interface IUploadPictureRepository
    {
        User ConvertUserDTO(UserDTO userDTO, string userId);
        string GetPhotoFile(IFormFile file);
    }
}
