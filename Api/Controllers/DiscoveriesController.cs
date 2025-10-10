// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using PlantAnimalApi.Contracts;
// using PlantAnimalApi.Data;
// using PlantAnimalApi.Models;
// using System.Security.Claims;

// [ApiController]
// [Route("api/[controller]")]
// public class DiscoveriesController : ControllerBase
// {
//     private readonly BioscopeDbContext _db;
//     public DiscoveriesController(BioscopeDbContext db) => _db = db;

//     private Guid? CurrentUserId =>
//         Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub"), out var g) ? g : null;

//     // Public feed
//     [HttpGet("public")]
//     public async Task<IActionResult> PublicFeed(int take = 50, int skip = 0)
//     {
//         var q = _db.Discoveries.AsNoTracking()
//             .Where(d => d.IsPublic)
//             .OrderByDescending(d => d.CreatedAt)
//             .Skip(skip).Take(take)
//             .Select(d => new {
//                 d.Id, d.TopMatchName, d.Confidence, d.CreatedAt,
//                 Author = d.User.DisplayName,
//                 CoverUrl = d.MediaAssets.OrderBy(m => m.CreatedAt).Select(m => m.Url).FirstOrDefault(),
//                 CommentCount = d.Comments.Count(c => !c.IsDeleted)
//             });

//         return Ok(await q.ToListAsync());
//     }

//     // My discoveries (auth required)
//     [Authorize]
//     [HttpGet("mine")]
//     public async Task<IActionResult> Mine(int take = 100, int skip = 0)
//     {
//         var uid = CurrentUserId!.Value;
//         var q = _db.Discoveries.AsNoTracking()
//             .Where(d => d.UserId == uid)
//             .OrderByDescending(d => d.CreatedAt)
//             .Skip(skip).Take(take)
//             .Select(d => new {
//                 d.Id, d.TopMatchName, d.Confidence, d.CreatedAt, d.IsPublic,
//                 CoverUrl = d.MediaAssets.OrderBy(m => m.CreatedAt).Select(m => m.Url).FirstOrDefault(),
//                 CommentCount = d.Comments.Count(c => !c.IsDeleted)
//             });

//         return Ok(await q.ToListAsync());
//     }

//     // Detail (visible if public or owner)
//     [Authorize(AuthenticationSchemes = "Bearer", Policy = null)]
//     [AllowAnonymous] // allow anonymous, but enforce visibility below
//     [HttpGet("{id:guid}")]
//     public async Task<IActionResult> Get(Guid id)
//     {
//         var d = await _db.Discoveries
//             .Include(x => x.User)
//             .Include(x => x.MediaAssets)
//             .FirstOrDefaultAsync(x => x.Id == id);

//         if (d is null) return NotFound();
//         if (!d.IsPublic && d.UserId != CurrentUserId) return Forbid();

//         return Ok(new {
//             d.Id, d.TopMatchName, d.Confidence, d.ApiProvider, d.ApiResult,
//             d.TakenAt, d.Latitude, d.Longitude, d.IsPublic, d.CreatedAt,
//             Author = d.User.DisplayName,
//             Media = d.MediaAssets.OrderBy(m => m.CreatedAt).Select(m => new {
//                 m.Url, m.ThumbUrl, m.ContentType, m.Width, m.Height, m.SizeBytes
//             })
//         });
//     }

//     // Create discovery (owner)
//     [Authorize]
//     [HttpPost]
//     public async Task<IActionResult> Create(CreateDiscoveryRequest req)
//     {
//         var uid = CurrentUserId!.Value;
//         var d = new Discovery
//         {
//             Id = Guid.NewGuid(),
//             UserId = uid,
//             TopMatchName = req.TopMatchName,
//             Confidence = req.Confidence,
//             ApiProvider = string.IsNullOrWhiteSpace(req.ApiProvider) ? "plant.id" : req.ApiProvider,
//             ApiResult = req.ApiResult,
//             TakenAt = req.TakenAt,
//             Latitude = req.Latitude,
//             Longitude = req.Longitude,
//             IsPublic = req.IsPublic,
//             CreatedAt = DateTimeOffset.UtcNow,
//             UpdatedAt = DateTimeOffset.UtcNow
//         };
//         _db.Discoveries.Add(d);

//         if (req.Media is { Count: > 0 })
//         {
//             foreach (var m in req.Media)
//             {
//                 _db.MediaAssets.Add(new MediaAsset
//                 {
//                     Id = Guid.NewGuid(),
//                     DiscoveryId = d.Id,
//                     Url = m.Url,
//                     ThumbUrl = m.ThumbUrl,
//                     ContentType = m.ContentType,
//                     Width = m.Width,
//                     Height = m.Height,
//                     SizeBytes = m.SizeBytes,
//                     CreatedAt = DateTimeOffset.UtcNow
//                 });
//             }
//         }

//         await _db.SaveChangesAsync();
//         return CreatedAtAction(nameof(Get), new { id = d.Id }, new { d.Id });
//     }

//     // Toggle visibility (owner)
//     [Authorize]
//     [HttpPost("{id:guid}/visibility")]
//     public async Task<IActionResult> SetVisibility(Guid id, [FromQuery] bool isPublic = true)
//     {
//         var uid = CurrentUserId!.Value;
//         var d = await _db.Discoveries.FirstOrDefaultAsync(x => x.Id == id && x.UserId == uid);
//         if (d is null) return NotFound();
//         d.IsPublic = isPublic;
//         await _db.SaveChangesAsync();
//         return NoContent();
//     }
// }
