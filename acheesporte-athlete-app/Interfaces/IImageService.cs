using acheesporte_athlete_app.Dtos.ImageDtos;

namespace acheesporte_athlete_app.Interfaces;

public interface IImageService
{
    Task<ImageUploadResponseDto> UploadProfileImageAsync(FileResult file);
}
