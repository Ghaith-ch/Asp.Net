using System.ComponentModel.DataAnnotations;

namespace Job_Application_Management_System.Dto
{
    public class UpdateApplicationStatusDto
    {
        [Required(ErrorMessage = "Status is required.")]
        [StringLength(50, ErrorMessage = "Status cannot exceed 50 characters.")]
        public string Status { get; set; } = string.Empty;
    }
}
