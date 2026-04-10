using Microsoft.AspNetCore.Mvc;
using ULife.MongoApi.DTOs.Comments;
using ULife.MongoApi.DTOs.Common;
using ULife.MongoApi.Services;

namespace ULife.MongoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly CommentService _service;

    public CommentsController(CommentService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] CommentQueryParams query)
        => Ok(await _service.GetAllAsync(query));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var comment = await _service.GetByIdAsync(id);
        return comment is null ? NotFound() : Ok(comment);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCommentDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return created is null
            ? BadRequest("PostId o ParentCommentId inválido, o el post no existe.")
            : CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateCommentDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}