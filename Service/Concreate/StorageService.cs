using FoodApiService.Service.Abstractions;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Collections.Generic;

namespace FoodApiService.Service.Concreate
{
    public class StorageService : IStorageService
    {
        private readonly ILogger<StorageService> _logger;
        private readonly string _basePath = "wwwroot/foods"; // Dosyaların kaydedileceği ana dizin

        public StorageService(ILogger<StorageService> logger)
        {
            _logger = logger;
        }


        public async Task<string> SaveFileAsync(IFormFile file, string folderName)
        {
            try
            {
                // Hedef dizini oluştur
                var folderPath = Path.Combine(_basePath, folderName);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Dosya yolunu oluştur
                var filePath = Path.Combine(folderPath, file.FileName);

                // Dosyayı kaydet
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Veritabanına kaydedilecek relative dosya yolu
                return Path.Combine(folderName, file.FileName);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Dosya kaydetme hatası: {ex.Message}");
                throw;
            }
        }

        public async Task<List<string>> SaveFileAsync(IFormFileCollection files, string folderName)
        {
            var filePaths = new List<string>();

            try
            {
                // Hedef dizini oluştur
                var folderPath = Path.Combine(_basePath, folderName);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Dosyaların kaydedilmesi için her dosya için işlemi paralel olarak yap
                var tasks = files.Select(file => SaveFileAsync(file, folderName)).ToArray();

                // Tüm dosyaların kaydedilmesini bekle
                var results = await Task.WhenAll(tasks);

                // Dosya yollarını listeye ekle
                filePaths.AddRange(results);

                return filePaths;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Birden fazla dosya kaydetme hatası: {ex.Message}");
                throw;
            }
        }
        public Task<bool> DeleteFileAsync(string fileName, string folderName)
        {
            try
            {
                // Hedef dizini ve dosya yolunu oluştur
                var folderPath = Path.Combine(_basePath, folderName);
                var filePath = Path.Combine(folderPath, fileName);

                // Dosya varsa sil
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return Task.FromResult(true);
                }
                else
                {
                    _logger.LogWarning($"Dosya bulunamadı: {filePath}");
                    return Task.FromResult(false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Dosya silme hatası: {ex.Message}");
                throw;
            }
        }
    }
}
