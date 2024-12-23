using SocialMediaAPI.domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaAPI.application.Interfaces
{
    public interface ICategoriesService
    {
        Task<Categories> GetCategoryByIdAsync(int id);
        Task<IEnumerable<Categories>> GetAllCategoriesAsync();
        Task<Categories> AddCategoryAsync(Categories category);
        Task<Categories> UpdateCategoryAsync(Categories category);
        Task DeleteCategoryAsync(int id);
    }
}
