using Microsoft.EntityFrameworkCore;
using Movies_Api.Models;

namespace Movies_Api.Services
{
    public class MoviesService : IMoviesService
    {
        private readonly AppDbContext _context;

        public MoviesService(AppDbContext context)
        {
            this._context = context;
        }

        public async Task<Movie> Add(Movie movie)
        {
            await _context.Movies.AddAsync(movie);
            _context.SaveChanges();
            return movie;
        }

        public Movie Delete(Movie movie)
        {
            _context.Movies.Remove(movie);
            _context.SaveChanges();
            return movie;
        }

        public async Task<IEnumerable<Movie>> GetAll(int genreid = 0)
        {
            return await _context.Movies.Where(m => m.GenreId == genreid || genreid == 0)
                .OrderByDescending(m => m.Rate)
                .Include(m => m.Genre)
                .ToListAsync();
        }

        public async Task<Movie> GetById(int id)
        {
            return await _context.Movies.Include(g => g.Genre).FirstOrDefaultAsync(g => g.Id == id);
        }

        public Movie Update(Movie movie)
        {
            _context.Update(movie);
            _context.SaveChanges();
            return movie;
        }
    }
}
