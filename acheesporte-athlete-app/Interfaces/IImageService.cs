using acheesporte_athlete_app.Dtos.ImageUploadDtos;

namespace acheesporte_athlete_app.Interfaces;

public interface IImageService
{
    Task<ImageUploadResponseDto> UploadProfileImageAsync(FileResult file);
    Task<List<ImageUploadResponseDto>> UploadVenuesImageAsync(List<FileResult> files);

}