using System;
using E_commerce.Dto.CategoryDtos;
using E_commerce.Models;

namespace E_commerce.Services.Interfaces;

public interface ICategoryService
{
    Task<Category> AddCategoryAsync(CreateCategoryDto createCategoryDto);
    Task<Category> UpdateCategoryAsync(int id, UpdateCategoryDto updateCategoryDto);
}
