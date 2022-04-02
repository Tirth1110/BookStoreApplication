using BookStore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.Repository
{
    public interface IBookRepository
    {
        Task<int> AddNewBook(BookModel bookModel);
        Task<List<BookModel>> GetAllBookData();
        Task<BookModel> GetBookById(int Id);
        Task<List<BookModel>> GetTopBookAsync(int count);
        List<BookModel> SearchBook(string title, string author);
        string AppName();
    }
}