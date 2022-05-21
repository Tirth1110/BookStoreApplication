using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Data;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BookStore.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly BookStoreContext _context;
        private readonly IConfiguration _configuration;

        public BookRepository(BookStoreContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<int> AddNewBook(BookModel bookModel)
        {
            var newBook = new Book()
            {
                Id = bookModel.Id,
                Title = bookModel.Title,
                SlugUrl = bookModel.Title.Replace(' ', '-').ToString().ToLower(),
                Author = bookModel.Author,
                LanguageId = bookModel.LanguageId,
                Description = bookModel.Description,
                CategoryId = bookModel.CategoryId,
                CoverPhotoUrl = bookModel.CoverPhotoUrl,
                BookPdfUrl = bookModel.BookPdfUrl,
                TotalPages = bookModel.TotalPages.HasValue ? bookModel.TotalPages.Value : 0,
                CreatedOn = DateTime.Now,
                CreatedBy = 1,
                UpdatedOn = DateTime.Now,
                UpdatedBy = 1
            };
            newBook.bookGallery = new List<BookGallery>();
            foreach (var file in bookModel.Gallery)
            {
                newBook.bookGallery.Add(new BookGallery()
                {
                    Name = file.Name,
                    URL = file.URL,
                });
            }
            await _context.Books.AddAsync(newBook);
            await _context.SaveChangesAsync();

            return newBook.Id;
        }
        public async Task<List<BookModel>> GetAllBookData()
        {
            return await _context.Books.Select(book => new BookModel()
            {
                Author = book.Author,
                CategoryName = book.Category.Name,
                Description = book.Description,
                Id = book.Id,
                LanguageId = book.LanguageId,
                LanguageName = book.Language.Name,
                Title = book.Title,
                SlugUrl = book.SlugUrl,
                TotalPages = book.TotalPages,
                CoverPhotoUrl = book.CoverPhotoUrl
            }).OrderByDescending(b => b.Id).ToListAsync();
        }
        #region Get top (count from view) Book For Home Page 
        public async Task<List<BookModel>> GetTopBookAsync(int count)
        {
            return await _context.Books
                .Select(book => new BookModel()
                {
                    Author = book.Author,
                    CategoryName = book.Category.Name,
                    Description = book.Description,
                    Id = book.Id,
                    LanguageId = book.LanguageId,
                    LanguageName = book.Language.Name,
                    Title = book.Title,
                    SlugUrl = book.SlugUrl,
                    TotalPages = book.TotalPages,
                    CoverPhotoUrl = book.CoverPhotoUrl
                }).Take(count).OrderByDescending(b => b.Id).ToListAsync();
        }
        #endregion
        public async Task<BookModel> GetBookById(int Id)
        {
            return (await _context.Books.Where(x => x.Id == Id)
                 .Select(book => new BookModel()
                 {
                     Id = book.Id,
                     Title = book.Title,
                     SlugUrl = book.SlugUrl,
                     Author = book.Author,
                     Description = book.Description,

                     LanguageId = book.LanguageId,
                     LanguageName = book.Language.Name,

                     CategoryId = book.CategoryId,
                     CategoryName = book.Category.Name,

                     TotalPages = book.TotalPages,

                     CoverPhotoUrl = book.CoverPhotoUrl,
                     BookPdfUrl = book.BookPdfUrl,

                     Gallery = book.bookGallery.Select(g => new GalleryModel()
                     {
                         Id = g.Id,
                         Name = g.Name,
                         URL = g.URL
                     }).ToList()
                 }).FirstOrDefaultAsync());
        }
        public List<BookModel> SearchBook(string title, string author)
        {
            return null;
        }

        public string AppName()
        {
            return _configuration["AppName"];
        }
    }
}
