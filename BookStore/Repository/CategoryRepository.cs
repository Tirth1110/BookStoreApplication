using BookStore.Data;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BookStoreContext _context;
        public CategoryRepository(BookStoreContext context)
        {
            _context = context;
        }
        public async Task<List<CategoryModel>> GetCategories()
        {
            var category = await _context.Categories.Select(x => new CategoryModel()
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();

            return category;
        }
    }
}
