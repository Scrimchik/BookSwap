using System.Collections.Generic;

namespace BookSwap.Models.ViewModels
{
    public class BookListViewModel
    {
        public List<Book> Books { get; set; }
        public string GenreName { get; set; }
        public string GenreLatinName { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
