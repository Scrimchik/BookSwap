
using BookSwap.Models.Users;

namespace BookSwap.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string DateAdded { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}
