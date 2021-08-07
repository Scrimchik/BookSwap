using System.Collections.Generic;

namespace BookSwap.Models
{
    public class BookGenre
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LatinName { get; set; }
        public string PhotoWay { get; set; }

        public List<Book> Books { get; set; }
    }
}
