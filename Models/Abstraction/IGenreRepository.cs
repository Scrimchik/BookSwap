using System.Linq;
using System.Threading.Tasks;

namespace BookSwap.Models.Abstraction
{
    public interface IGenreRepository
    {
        IQueryable<BookGenre> Genres { get; }

        Task<BookGenre> GetBookGenreAsync(int id);
        Task<BookGenre> GetBookGenreByNameAsync(string latinName);
        Task AddGenreAsync(BookGenre genre);
        Task UpdateGenreAsync(BookGenre genre);
        Task DeleteGenreAsync(BookGenre genre);
    }
}
