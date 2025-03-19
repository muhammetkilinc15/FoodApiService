namespace FoodApiService.Service.Abstractions
{
    public interface IStorageService
    {
        Task<string> SaveFileAsync(IFormFile file, string folderName);
        Task<List<string>> SaveFileAsync(IFormFileCollection files, string folderName);
        Task<bool> DeleteFileAsync(string fileName, string folderName);
    }
}
