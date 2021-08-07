using System.Collections.Generic;

namespace BookSwap.Models.ViewModels
{
    public class AddBookViewModel
    {
        public Book Book { get; set; }
        public List<string> BookGenres { get; set; }
    }
}
