using FoodApiService.Context;
using FoodApiService.DTOs;
using FoodApiService.Entities;
using FoodApiService.Service.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IStorageService _strogeService;
        private readonly ILogger<FoodController> _logger;

        public FoodController(ApplicationDbContext dbContext, IStorageService fileService, ILogger<FoodController> logger)
        {
            this.dbContext = dbContext;
            this._strogeService = fileService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetFoods([FromQuery] GetFoodQuery request, CancellationToken cancellationToken)
        {
            var query = dbContext.Foods.AsQueryable().AsNoTracking();

            if (!string.IsNullOrEmpty(request.Search))
            {
                query = query.Where(f => f.Name.Contains(request.Search));
            }

            if (request.CategoryId.HasValue)
            {
                query = query.Where(f => f.CategoryId == request.CategoryId);
            }

            query = query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize);

            var foods = await query
                        .Include(f => f.FoodImages)
                           .Select(x => new
                           {
                               x.Id,
                               x.Name,
                               x.Price,
                               x.Description,
                               x.CreatedAt,
                               CoverImageUrl = $"{Request.Scheme}://{Request.Host}/foods/{x.FoodImages.FirstOrDefault().ImageUrl}"  // URL oluşturuluyor

                           })
                        .ToListAsync(cancellationToken);

            return Ok(foods);

        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetFood(int id)
        {
            var food = await dbContext.Foods
                .Include(f => f.FoodImages)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Price,
                    x.Description,
                    x.CreatedAt,
                    FoodImages = x.FoodImages.Select(fi => new
                    {
                        fi.Id,
                        // Resim dosyasının URL'sini oluşturun
                        ImageUrl = $"{Request.Scheme}://{Request.Host}/foods/{fi.ImageUrl}"  // URL oluşturuluyor
                    }).ToList()
                })
                .FirstOrDefaultAsync(f => f.Id == id);

            if (food == null)
            {
                return NotFound();
            }

            return Ok(food);
        }


        [HttpPost]
        public async Task<IActionResult> CreateFood([FromForm] CreateFoodDto request)
        {
            Food newFood = new()
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                CategoryId = request.CategoryId
            };

            // Resimlerin kaydedilmesi
            List<string> imagesPaths = await _strogeService.SaveFileAsync(request.Images, Guid.CreateVersion7().ToString());  // food.Id.toString() kullanılıyor

            // FoodImage objesi oluşturulması
            newFood.FoodImages = [.. imagesPaths.Select(imagePath => new FoodImage { ImageUrl = imagePath })];

            // Yeni yemeği veritabanına ekle
            await dbContext.Foods.AddAsync(newFood);
            await dbContext.SaveChangesAsync();

            _logger.LogInformation("Yeni yemek eklendi. Id: {0}", newFood.Id);
            // CreatedAtAction ile yeni yemeği döndür
            return CreatedAtAction(nameof(GetFood), new { id = newFood.Id }, newFood);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateFood([FromBody] UpdateFoodDto request)
        {
            Food newFood = new()
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                CategoryId = request.CategoryId
            };
            dbContext.Foods.Update(newFood);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFood([FromRoute] int id)
        {
            var food = await dbContext.Foods.FindAsync(id);
            if (food == null)
            {
                return NotFound();
            }
            dbContext.Foods.Remove(food);
            await dbContext.SaveChangesAsync();
            return NoContent();
        }

    }
}
