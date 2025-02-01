using AutoMapper;
using E_commerce.Dto.CategoryDtos;
using E_commerce.Models;
using E_commerce.Repositories.Interfaces;
using E_commerce.Services.Interfaces;

namespace E_commerce.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<Category> AddCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            if (createCategoryDto == null)
            {
                throw new ArgumentException("Invalid category data");
            }

            var category = _mapper.Map<Category>(createCategoryDto);
            return await _categoryRepository.AddCategoryAsync(category);
        }

        public async Task<Category> UpdateCategoryAsync(int id, UpdateCategoryDto updateCategoryDto)
        {
            if (updateCategoryDto == null)
            {
                throw new ArgumentException("Invalid category data");
            }

            var existingCategory = await _categoryRepository.GetCategoryByIdAsync(id);
            if (existingCategory == null)
            {
                throw new KeyNotFoundException($"Category with ID {id} not found.");
            }

            _mapper.Map(updateCategoryDto, existingCategory);
            return await _categoryRepository.UpdateCategoryAsync(existingCategory);
        }
    }
}
