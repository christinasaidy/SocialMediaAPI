using SocialMediaAPI.application.Interfaces;
using SocialMediaAPI.domain.entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMediaAPI.application.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ICategoriesRepository _categoriesRepository;

        public CategoriesService(ICategoriesRepository categoriesRepository)
        {
            _categoriesRepository = categoriesRepository;
        }

        public async Task<Categories> GetCategoryByIdAsync(int id)
        {
            return await _categoriesRepository.GetCategoryByIdAsync(id);
        }

        public async Task<IEnumerable<Categories>> GetAllCategoriesAsync()
        {
            return await _categoriesRepository.GetAllCategoriesAsync();
        }

        public async Task<Categories> AddCategoryAsync(Categories category)
        {
            return await _categoriesRepository.AddCategoryAsync(category);
        }

        public async Task<Categories> UpdateCategoryAsync(Categories category)
        {
            return await _categoriesRepository.UpdateCategoryAsync(category);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            await _categoriesRepository.DeleteCategoryAsync(id);
        }

        // Implement the GetCategoryByNameAsync method
        public async Task<Categories> GetCategoryByNameAsync(string categoryName)
        {
            return await _categoriesRepository.GetCategoryByNameAsync(categoryName);
        }
    }
}
