using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies_Api.Dtos;
using Movies_Api.Models;
using Movies_Api.Services;

namespace Movies_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenresServices _genresServices;

        public GenresController(IGenresServices genresServices)
        {
            this._genresServices = genresServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var Genres = await _genresServices.GetAll();
            return Ok(Genres);
        }

        [HttpPost]
        public async Task<IActionResult> AddGenreAsync(GenreDto dto)
        {
            var genre = new Genre { Name = dto.Name };

            await _genresServices.Add(genre);
            return Ok(genre);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] GenreDto dto)
        {
            var genre = await _genresServices.GetById(id);

            if (genre == null)
            {
                return NotFound($"No Genre Is Found With ID {id}");
            }
            genre.Name = dto.Name;

            _genresServices.Update(genre);
            return Ok(genre);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var genre = await _genresServices.GetById(id);

            if (genre == null)
            {
                return NotFound($"No Genre Is Found With ID {id}");
            }

            _genresServices.Delete(genre);
            return Ok("Genre Deleted Successfull");
        }
    }
}
