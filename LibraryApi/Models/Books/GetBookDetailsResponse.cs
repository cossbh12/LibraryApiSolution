using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Models.Books
{
    public class GetBookDetailsResponse
    {
        public int Id { get; set; }
        [Required][MaxLength(200)]
        public string Title { get; set; }
        [Required][MaxLength(200)]
        public string Author { get; set; }
        [MaxLength(100)]
        public string Genre { get; set; }
    }
}
