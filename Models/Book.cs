using BookSwap.Models.Users;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookSwap.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Введите название книги")]
        [MaxLength(20, ErrorMessage = "Слишком длинное название книги")]
        public string Name { get; set; }
        public string LatinName { get; set; }
        public string PhotoWay { get; set; }
        public string BookPath { get; set; }
        [Required(ErrorMessage = "Введите описание книги")]
        [MinLength(50, ErrorMessage = "Слишком короткое описание книги")]
        [MaxLength(600, ErrorMessage = "Слишком длинное описание книги")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Введите автора книги")]
        public string Author { get; set; }
        public int DownloadCount { get; set; }
        public string UserWhoUploadId { get; set; }
        [ForeignKey("UserWhoUploadId")]
        public User User { get; set; }

        public int BooksGenreId { get; set; }
        public BookGenre BooksGenre { get; set; }

        public List<Comment> Comments { get; set; }
    }
}
