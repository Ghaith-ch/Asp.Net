namespace E_commerce.Dto.CategoryDtos
{
    public class GetCategoryDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}
