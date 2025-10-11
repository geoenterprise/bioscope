using Microsoft.AspNetCore.Mvc;
using PlantAnimalApi.Services;
using PlantAnimalApi.Models;
using Api.Helpers;
using System.Text.Json; 

namespace PlantAnimalApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganismsController : ControllerBase
    {
        private readonly OrganismService _organismService;

        public OrganismsController(OrganismService organismService)
        {
            _organismService = organismService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Organism>>> GetAll()
        {
            var list = await _organismService.GetAllAsync();
            return Ok(list);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Organism>> Get(int id)
        {
            var org = await _organismService.GetByIdAsync(id);
            if (org == null) return NotFound();
            return Ok(org);
            // return org is null ? NotFound() : Ok(org);
        }
        [HttpPost]
        public async Task<ActionResult<Organism>> Post(Organism org)
        {
            var created = await _organismService.AddAsync(org);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadPhoto([FromForm] IFormFile photo)
        {
            if (photo == null || photo.Length == 0)
                return BadRequest("No photo was sent.");


            var imageUrl = await PhotoHelper.SavePhotoAsync(photo);

            var pathPhoto = Path.Combine(Directory.GetCurrentDirectory(), imageUrl.TrimStart('/'));


            try
            {

                var aiResult = await PhotoHelper.IdentifyPhotoAsync(pathPhoto);


                var aiObject = JsonSerializer.Deserialize<object>(aiResult);


                return Ok(new
                {
                    message = "The photo was received",
                    fileName = photo.FileName,
                    size = photo.Length,
                    url = imageUrl,
                    Result = aiObject
                });

            }
            finally
            {
                if (System.IO.File.Exists(pathPhoto))
                {
                    System.IO.File.Delete(pathPhoto);
                }
                
            }

            
        }

    }
}
