using Movies_Api.Models;

namespace Movies_Api.Services
{
    public interface IGenresServices
    {
        Task<IEnumerable<Genre>> GetAll();
        Task<Genre> GetById(int id);
        Task<Genre> Add(Genre genre);
        Genre Update(Genre genre);
        Genre Delete(Genre genre);
        Task<bool> IsValid(int id); 
    }
}
