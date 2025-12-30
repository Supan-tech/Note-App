using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class CreateNoteDto
    {
        [Required(ErrorMessage = "A title is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters")]
        public string Title { get; set; } = string.Empty;

        [MaxLength(5000, ErrorMessage = "Content cannot exceed 5000 characters")]
        public string Content { get; set; } = string.Empty;

    }
}