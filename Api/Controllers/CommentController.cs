using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantAnimalApi.Contracts;
using PlantAnimalApi.Data;
using PlantAnimalApi.Models;
using System.Security.Claims;


[ApiController]
[Route("api/comments")]
public class CommentsController : ControllerBase
{
    private readonly BioscopeDbContext _db;

    public CommentsController(BioscopeDbContext db)
    {
        _db = db;
    }

 
    [HttpGet("{discoveryId}")]
    public async Task<IActionResult> GetByDiscovery(Guid discoveryId)
    {
        var comments = await _db.Comments
            .Where(c => c.DiscoveryId == discoveryId)
            .Select(c => new 
            {
                c.Id,
                c.DiscoveryId,
                c.Body,
                c.CreatedAt,
                c.UserId
            })
            .ToListAsync();

        return Ok(comments);
    }

    // POST api/comments/create
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CommentCreateDto dto)
    {
        var comment = new Comment
        {
            Id = Guid.NewGuid(),
            DiscoveryId = dto.DiscoveryId,
            UserId = dto.UserId,
            Body = dto.Body,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow,
            IsDeleted = false
        };
        _db.Comments.Add(comment);
        await _db.SaveChangesAsync();
        return Ok(comment);
    }
}
