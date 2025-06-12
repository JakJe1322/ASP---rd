using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ReaderDiary.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Příliš dlouhé")]
        public string Title { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Příliš dlouhé")]
        public string Author { get; set; }

        [DataType(DataType.Date)]
        public DateTime PublishedDate { get; set; }

        public string Description { get; set; }

    }



}
