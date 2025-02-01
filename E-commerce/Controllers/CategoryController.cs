using AutoMapper;
using E_commerce.Dto.CategoryDtos;
using E_commerce.Models;
using E_commerce.Repositories.Interfaces;
using E_commerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryService = categoryService;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCategoryDto>>> GetCategories()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            if (categories == null || !categories.Any())
            {
                return NotFound(new { Message = "No categories found." });
            }

            return Ok(_mapper.Map<IEnumerable<GetCategoryDto>>(categories));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetCategoryDto>> GetCategory(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound(new { Message = $"Category with ID {id} not found." });
            }

            return Ok(_mapper.Map<GetCategoryDto>(category));
        }

        [HttpPost]
        public async Task<ActionResult<GetCategoryDto>> AddCategory([FromForm] CreateCategoryDto createCategoryDto)
        {
            try
            {
                var category = await _categoryService.AddCategoryAsync(createCategoryDto);
                var categoryDto = _mapper.Map<GetCategoryDto>(category);
                return CreatedAtAction(nameof(GetCategory), new { id = category.CategoryId }, categoryDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromForm] UpdateCategoryDto updateCategoryDto)
        {
            try
            {
                var updatedCategory = await _categoryService.UpdateCategoryAsync(id, updateCategoryDto);
                var categoryDto = _mapper.Map<GetCategoryDto>(updatedCategory);
                return Ok(new { Message = "Category updated successfully.", Category = categoryDto });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _categoryRepository.DeleteCategoryAsync(id);
            if (!result)
            {
                return NotFound(new { Message = $"Category with ID {id} not found." });
            }

            return Ok(new { Message = "Category deleted successfully." });
        }
    }
}
