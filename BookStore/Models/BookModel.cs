using BookStore.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class BookModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Title")]
        [StringLength(100, MinimumLength = 5)]
        //[MyCustomeValidation(ErrorMessage ="This is a Custom Validation")]
        //if ctor created then pass "Azure" 
        //[MyCustomValidation("Azure")]
        public string Title { get; set; }
        public string SlugUrl { get; set; }
        [Required]
        public string Author { get; set; }
        [StringLength(500, MinimumLength = 5)]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Category Name")]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        [Required(ErrorMessage = "Please Enter No of Pages")]
        [Display(Name = "Total Pages of Book")]
        public int? TotalPages { get; set; }
        [Required]
        [Display(Name = "Language Name")]
        public int LanguageId { get; set; }
        public string LanguageName { get; set; }
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Enter Email")]
        [EmailAddress]
        public string Email { get; set; }
        //Single File Upload
        [Display(Name = "Choose Display Photo")]
        [Required]
        public IFormFile CoverPhoto { get; set; }
        public string CoverPhotoUrl { get; set; }
        //Multiple File Upload
        [Display(Name = "Choose Multiple (Files) Photos")]
        [Required]
        //List<IFormFile> & IEnumerable<IFormFile> & IFormFileCollection All are same for multiple upload img
        //public List<IFormFile> GalleryFiles { get; set; }
        //public IEnumerable<IFormFile> GalleryFiles { get; set; }
        public IFormFileCollection GalleryFiles { get; set; }
        public List<GalleryModel> Gallery { get; set; }
        //PDF File
        [Display(Name = "Choose Pdf File")]
        [Required]
        public IFormFile BookPdf { get; set; }
        public string BookPdfUrl { get; set; }
    }
}
