using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicApp.Data;
using MusicApp.Models;
using MusicApp.Services;

namespace MusicApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicController(ApplicationDbContext context, S3Service s3Service) : ControllerBase
    {
        private readonly ApplicationDbContext _context = context;
        private readonly S3Service _s3Service = s3Service;

        [HttpPost("upload")]
        public async Task<IActionResult> UploadMusic(IFormFile file, string title, string artist, string genre)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");
            // Upload the file to S3
            var key = await _s3Service.UploadFileAsync(file);
            // Save the music metadata to the database
            var music = new Song
            {
                Title = title,
                Artist = artist,
                Genre = genre,
                FileName = file.FileName,
                FileSize = file.Length,
                S3Key = key,
                S3Url = _s3Service.GetFileUrl(key),
                UploadAt = DateTime.UtcNow
            };
            _context.Songs.Add(music);
            await _context.SaveChangesAsync();
            return Ok(new { message = "File uploaded successfully" });
        }
        public async Task<IActionResult> GetAllMusic()
        {
            return Ok(await _context.Songs.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMusic(int id)
        {
            var music = await _context.Songs.FindAsync(id);
            if (music == null)
                return NotFound();
            return Ok(music);
        }
    }
}
