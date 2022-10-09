using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies_Api.Dtos;
using Movies_Api.Helper;
using Movies_Api.Models;
using Movies_Api.Services;

namespace Movies_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMoviesService _moviesService;
        private readonly IGenresServices _genresServices;
        private readonly IMapper _mapper;
        private new List<string> allowedExtension = new List<string> { ".jpg", ".png" };
        private long maxLengthForImage = 1048576;
        public MoviesController(IMoviesService moviesService, IGenresServices genresServices, IMapper mapper)
        {
            this._moviesService = moviesService;
            this._genresServices = genresServices;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMovieAsync()
        {
            var movies = await _moviesService.GetAll();
            var data = _mapper.Map<IEnumerable<MovieDetailsDto>>(movies);
            return Ok(data);
        }

        [HttpGet("Id")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var genre = await _moviesService.GetById(id);
            if (genre == null)
            {
                return BadRequest("This is InValid Id ");
            }
            var data = _mapper.Map<MovieDetailsDto>(genre);
            return Ok(data);
        }

        [HttpGet("GenreId")]
        public async Task<IActionResult> GetByGenreIdAsync(int id)
        {
            var genre = await _moviesService.GetAll(id);
            if (genre == null)
            {
                return BadRequest("This is InValid Id ");
            }
            var data = _mapper.Map<MovieDetailsDto>(genre);
            return Ok(data);
        }


        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] MovieDto movie)
        {
            if (movie.Poster == null)
            {
                return BadRequest("Poster Is Required");
            }

            if (!allowedExtension.Contains(Path.GetExtension(movie.Poster.FileName).ToLower()))
            {
                return BadRequest("Only .Png and .Jpg Images are Allowed !!");
            }

            if (movie.Poster.Length > maxLengthForImage)
            {
                return BadRequest("Max Length Allowed for Images 1 MB !!");
            }

            var isValidGenre = await _genresServices.IsValid(movie.GenreId);
            if (!isValidGenre)
            {
                return BadRequest("Invalid Genre ID");
            }

            using var dataStream = new MemoryStream();

            await movie.Poster.CopyToAsync(dataStream);

            var mov = _mapper.Map<Movie>(movie);
            mov.Poster=dataStream.ToArray();

            await _moviesService.Add(mov);
            return Ok(mov);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm] MovieDto dto)
        {
            var movie = await _moviesService.GetById(id);
            if (movie == null)
            {
                return NotFound("Movie Not Found");
            }

            var isValidGenre = await _genresServices.IsValid(dto.GenreId);
            if (!isValidGenre)
            {
                return BadRequest("Invalid Genre ID");
            }

            if (dto.Poster != null)
            {
                if (!allowedExtension.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                {
                    return BadRequest("Only .Png and .Jpg Images are Allowed !!");
                }

                if (dto.Poster.Length > maxLengthForImage)
                {
                    return BadRequest("Max Length Allowed for Images 1 MB !!");
                }

                using var dataStream = new MemoryStream();

                await dto.Poster.CopyToAsync(dataStream);

                movie.Poster = dataStream.ToArray();
            }

            movie.Title = dto.Title;
            movie.Year = dto.Year;
            movie.Rate = dto.Rate;
            movie.StoreLine = dto.StoreLine;
            movie.GenreId = dto.GenreId;

            _moviesService.Update(movie);
            return Ok(movie);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var mov = await _moviesService.GetById(id);
            if (mov == null)
            {
                return NotFound("Movie Not Found");
            }
            _moviesService.Delete(mov);
            return Ok("Movie is Deleted");
        }

    }
}
