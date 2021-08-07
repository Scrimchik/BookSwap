using System.Linq;

namespace BookSwap.Models.Abstraction
{
    public interface IBookRepository
    {
        IQueryable<Book> Books { get; }
    }
}
