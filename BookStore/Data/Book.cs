using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Data
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int TotalPages { get; set; }
        public int LanguageId { get; set; }
        public Language Language { get; set; }
        public string CoverPhotoUrl { get; set; }
        public string BookPdfUrl { get; set; }
        public DateTime? CreatedOn { get; set; }
        public byte CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public byte UpdatedBy { get; set; }
        public ICollection<BookGallery> bookGallery { get; set; }
    }
}
