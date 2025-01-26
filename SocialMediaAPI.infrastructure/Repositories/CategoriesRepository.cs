﻿using SocialMediaAPI.application.Interfaces;
using SocialMediaAPI.domain.entities;
using SocialMediaAPI.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaAPI.infrastructure.Repositories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoriesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Categories> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Categories>> GetAllCategoriesAsync()
        {
            return await _context.Categories
                .ToListAsync();
        }

        public async Task<Categories> AddCategoryAsync(Categories category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category; 
        }

        public async Task<Categories> UpdateCategoryAsync(Categories category)
        {
            // Ensure the entity is tracked
            var existingCategory = await _context.Categories.FindAsync(category.Id);

            if (existingCategory == null)
                return null;

            // Update properties explicitly
            existingCategory.Name = category.Name;

            // Save changes
            await _context.SaveChangesAsync();

            return existingCategory;
        }


        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<Categories> GetCategoryByNameAsync(string categoryName)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Name == categoryName);
        }
    }
}
