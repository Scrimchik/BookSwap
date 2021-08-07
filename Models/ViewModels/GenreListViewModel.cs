using System.Collections.Generic;

namespace BookSwap.Models.ViewModels
{
    public class GenreListViewModel
    {
        public PageInfo PageInfo { get; set; }
        public List<BookGenre> BookGenres { get; set; }
    }
}
