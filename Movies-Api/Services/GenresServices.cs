using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies_Api.Models;

namespace Movies_Api.Services
{
    public class GenresServices : IGenresServices
    {
        private readonly AppDbContext _context;

        public GenresServices(AppDbContext context)
        {
            this._context = context;
        }

        public async Task<Genre> Add(Genre genre)
        {
            await _context.Genres.AddAsync(genre);
            _context.SaveChanges();
            return genre;
        }

        public Genre Delete(Genre genre)
        {
            _context.Genres.Remove(genre);
            _context.SaveChanges();
            return genre;
        }

        public async Task<IEnumerable<Genre>> GetAll()
        {
            return await _context.Genres.OrderBy(g => g.Name).ToListAsync();
        }

        public async Task<Genre> GetById(int id)
        {
            return await _context.Genres.FirstOrDefaultAsync(g => g.Id == id);
        }

        public Task<bool> IsValid(int id)
        {
            return _context.Genres.AnyAsync(g => g.Id == id);
        }

        public Genre Update(Genre genre)
        {
            _context.Update(genre);
            _context.SaveChangesAsync();
            return genre;
        }
    }
}
