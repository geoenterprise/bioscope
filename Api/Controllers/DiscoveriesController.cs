using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantAnimalApi.Contracts;
using PlantAnimalApi.Data;
using PlantAnimalApi.Models;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class DiscoveriesController : ControllerBase
{
    private readonly BioscopeDbContext _db;
    public DiscoveriesController(BioscopeDbContext db) => _db = db;

    // private Guid? CurrentUserId =>
    //     Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub"), out var g) ? g : null;


    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var discovery = await _db.Discoveries.FindAsync(id);
        if (discovery == null)
            return NotFound();

        return Ok(discovery);
    }
    // Public feed
    // [HttpGet("public")]
    // public async Task<IActionResult> PublicFeed(int take = 50, int skip = 0)
    // {
    //     var q = _db.Discoveries.AsNoTracking()
    //         .Where(d => d.IsPublic)
    //         .OrderByDescending(d => d.CreatedAt)
    //         .Skip(skip).Take(take)
    //         .Select(d => new {
    //             d.Id, d.TopMatchName, d.Confidence, d.CreatedAt,
    //             Author = d.User.DisplayName,
    //             CoverUrl = d.MediaAssets.OrderBy(m => m.CreatedAt).Select(m => m.Url).FirstOrDefault(),
    //             CommentCount = d.Comments.Count(c => !c.IsDeleted)
    //         });

    //     return Ok(await q.ToListAsync());
    // }

    // My discoveries (auth required)
    // [Authorize]
    // [HttpGet("mine")]
    // public async Task<IActionResult> Mine(int take = 100, int skip = 0)
    // {
    //     var uid = CurrentUserId!.Value;
    //     var q = _db.Discoveries.AsNoTracking()
    //         .Where(d => d.UserId == uid)
    //         .OrderByDescending(d => d.CreatedAt)
    //         .Skip(skip).Take(take)
    //         .Select(d => new {
    //             d.Id, d.TopMatchName, d.Confidence, d.CreatedAt, d.IsPublic,
    //             CoverUrl = d.MediaAssets.OrderBy(m => m.CreatedAt).Select(m => m.Url).FirstOrDefault(),
    //             CommentCount = d.Comments.Count(c => !c.IsDeleted)
    //         });

    //     return Ok(await q.ToListAsync());
    // }

    // Detail (visible if public or owner)
    // [Authorize(AuthenticationSchemes = "Bearer", Policy = null)]
    // [AllowAnonymous] // allow anonymous, but enforce visibility below
    // [HttpGet("{id:guid}")]
    // public async Task<IActionResult> Get(Guid id)
    // {
    //     var d = await _db.Discoveries
    //         .Include(x => x.User)
    //         .Include(x => x.MediaAssets)
    //         .FirstOrDefaultAsync(x => x.Id == id);

    //     if (d is null) return NotFound();
    //     if (!d.IsPublic && d.UserId != CurrentUserId) return Forbid();

    //     return Ok(new {
    //         d.Id, d.TopMatchName, d.Confidence, d.ApiProvider, d.ApiResult,
    //         d.TakenAt, d.Latitude, d.Longitude, d.IsPublic, d.CreatedAt,
    //         Author = d.User.DisplayName,
    //         Media = d.MediaAssets.OrderBy(m => m.CreatedAt).Select(m => new {
    //             m.Url, m.ThumbUrl, m.ContentType, m.Width, m.Height, m.SizeBytes
    //         })
    //     });
    // }

    // [Authorize]
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUserId(Guid userId, int take = 100, int skip = 0)
    {
        var q = _db.Discoveries.AsNoTracking()
        .Where(d => d.UserId == userId)
        .OrderByDescending(d => d.DiscoveryId)
        .Skip(skip).Take(take)
        .Select(d => new
        {
            d.DiscoveryId,
            d.UserId,
            d.CommonName,
            d.ScientificName,
            d.WikiDescription,
            d.Confidence,
            d.AssetUrl
        });

        var discoveries = await q.ToListAsync();

        if (!discoveries.Any())
            return NotFound(new { message = "No discoveries found for this user." });

        return Ok(discoveries);
    }



    // [Authorize]
    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateDiscoveryRequest req)
    {
        // var uid = CurrentUserId!.Value; 

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == "test@example.com");

        if (user == null)
        {
            return NotFound("User not found");
        }


        var discovery = new Discovery
        {
            DiscoveryId = Guid.NewGuid(),
            UserId = user.Id,
            WikiDescription = req.WikiDescription,
            Confidence = req.Confidence ?? 0,
            AssetUrl = req.AssetUrl,
            CommonName = req.CommonName,
            ScientificName = req.ScientificName
        };

        _db.Discoveries.Add(discovery);


        //saving the user comments in comments table
        if (!string.IsNullOrWhiteSpace(req.Comment))
        {
            var comment = new Comment
            {
                DiscoveryId = discovery.DiscoveryId,
                UserId = user.Id,
                Body = req.Comment,
                CreatedAt = DateTimeOffset.UtcNow
            };

            _db.Comments.Add(comment);
        }

        await _db.SaveChangesAsync();


        var discoveryDto = new
        {
            discovery.DiscoveryId,
            discovery.CommonName,
            discovery.ScientificName,
            discovery.WikiDescription,
            discovery.Confidence,
            discovery.AssetUrl,
            User = new
            {
                discovery.User.Id,
                discovery.User.DisplayName,
                discovery.User.Email
            }
        };

        return Ok(new { message = "Discovery saved", discovery = discoveryDto });
        // return CreatedAtAction(nameof(Get), new { id = discovery.DiscoveryId }, new { discovery.DiscoveryId });
    }


    [HttpPatch("updateDescription/{id}")]
    public async Task<IActionResult> UpdateDescription(Guid id, [FromBody] UpdateDescriptionDto dtoUpdate)
    {

        var discovery = await _db.Discoveries.FindAsync(id);
        if (discovery == null)
            return NotFound("Discovery no found.");


        discovery.WikiDescription = dtoUpdate.WikiDescription;


        await _db.SaveChangesAsync();

        return Ok(discovery);
    }
    
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteDiscovery(Guid id)
    {
        var discovery = await _db.Discoveries.FindAsync(id);
        if (discovery == null) 
            return NotFound();

        _db.Discoveries.Remove(discovery);
        await _db.SaveChangesAsync();

        return NoContent(); 
    }



    // Toggle visibility (owner)
    // [Authorize]
    // [HttpPost("{id:guid}/visibility")]
    // public async Task<IActionResult> SetVisibility(Guid id, [FromQuery] bool isPublic = true)
    // {
    //     var uid = CurrentUserId!.Value;
    //     var d = await _db.Discoveries.FirstOrDefaultAsync(x => x.Id == id && x.UserId == uid);
    //     if (d is null) return NotFound();
    //     d.IsPublic = isPublic;
    //     await _db.SaveChangesAsync();
    //     return NoContent();
    // }
}
