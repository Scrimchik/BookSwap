using System.Linq;

namespace BookSwap.Models.Abstraction
{
    public interface IGenreRepository
    {
        IQueryable<BookGenre> Genres { get; }
    }
}
