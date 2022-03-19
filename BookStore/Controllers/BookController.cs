using Microsoft.AspNetCore.Mvc;
using BookStore.Models;
using BookStore.Repository;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;
using Microsoft.AspNetCore.Http;

namespace BookStore.Controllers
{
    //[Route("[controller]/[action]")]
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository = null;
        private readonly ILanguageRepository _languageRepository = null;
        private readonly ICategoryRepository _categoryRepository = null;
        private readonly IWebHostEnvironment _webHostEnvironment = null;
        public BookController(IBookRepository bookRepository,
            ILanguageRepository languageRepository,
            ICategoryRepository categoryRepository,
           IWebHostEnvironment webHostEnvironment)
        {
            _bookRepository = bookRepository;
            _languageRepository = languageRepository;
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        //[Route("book/get-all-book", Name = "getAllBookRoute")]
        [Route("~/get-all-book", Name = "getAllBookRoute")]
        public async Task<ViewResult> GetAllBook()
        {
            var bookFromRepo = await _bookRepository.GetAllBookData();
            return View(bookFromRepo);
        }
        //[Route("book/book-details/{id}", Name = "bookDetailsRoute")]
        //[Route("book-details/{id}", Name = "bookDetailsRoute")]
        [Route("~/book-details/{id:int}", Name = "bookDetailsRoute")]
        public async Task<ViewResult> GetBook(int Id)
        {
            var bookdata = await _bookRepository.GetBookById(Id);
            return View(bookdata);
        }
        public async Task<List<BookModel>> SearchBook(string title, string author)
        {
            //return await _bookRepository.SearchBook(title, author).ToListAsync();
            return null;
        }
        //[Route("book/add-new-book", Name = "addNewBookRoute")]
        public async Task<ViewResult> AddNewBook(bool isSuccess = false, int bookId = 0)
        {
            #region for default selection 
            var selectedLanguage = new BookModel
            {
                //Language = "3"
            };
            #endregion

            #region Fill Dynamic DropDown of Language & Category
            //ViewBag.langaugeDropdown = new SelectList(await _languageRepository.GetAllLanguage(), "Id", "Name");
            //ViewBag.catgeoryDropdown = new SelectList(await _categoryRepository.GetCategories(), "Id", "Name");
            #endregion


            ViewBag.IsSuccess = isSuccess;
            ViewBag.BookId = bookId;
            return View(selectedLanguage);
        }
        [HttpPost]
        public async Task<IActionResult> AddNewBook(BookModel bookModel)
        {
            if (ModelState.IsValid)
            {
                #region Start BookCover Photo Code
                if (bookModel.CoverPhoto != null)
                {
                    string folder = "Books/CoverPhotos/";
                    bookModel.CoverPhotoUrl = await UploadImages(folder, bookModel.CoverPhoto);
                }
                #endregion
                #region Start Gallery Multiples Files (Photos) Add
                if (bookModel.GalleryFiles != null)
                {
                    string folder = "Books/Gallery/";
                    bookModel.Gallery = new List<GalleryModel>();
                    foreach (var item in bookModel.GalleryFiles)
                    {
                        var gallery = new GalleryModel()
                        {
                            Name = item.FileName,
                            URL = await UploadImages(folder, item)
                        };
                        bookModel.Gallery.Add(gallery);
                    }
                }
                #endregion
                #region Start Book PDF Add
                if (bookModel.BookPdf != null)
                {
                    string folder = "Books/BookPdf/";
                    bookModel.BookPdfUrl = await UploadImages(folder, bookModel.BookPdf);
                }
                #endregion

                int id = await _bookRepository.AddNewBook(bookModel);
                if (id > 0)
                {
                    return RedirectToAction(nameof(AddNewBook), new { isSuccess = true, bookid = id });
                }
            }
            #region Fill Dynamic DropDown of Language & Category
            //ViewBag.catgeoryDropdown = new SelectList(await _categoryRepository.GetCategories(), "Id", "Name");
            //ViewBag.langaugeDropdown = new SelectList(await _languageRepository.GetAllLanguage(), "Id", "Name");
            #endregion
            return View();
        }
        private async Task<string> UploadImages(string folderPath, IFormFile file)
        {
            Guid gid = Guid.NewGuid();
            string ext = Path.GetExtension(file.FileName);
            folderPath += gid + ext;
            string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);
            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
            return folderPath;
        }
        //private List<LanguageModel> GetLanguages()
        //{
        //    return new List<LanguageModel>()
        //    {
        //        new LanguageModel(){Id=1,Name="English"},
        //        new LanguageModel(){Id=2,Name="Gujarati"},
        //        new LanguageModel(){Id=3,Name="Hindi"},
        //    };
        //}
    }
}
