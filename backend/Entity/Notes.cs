using System.ComponentModel.DataAnnotations;
// using System.Diagnostics.CodeAnalysis;

namespace Backend.Entity
{
    public class Note
    {
        [Key]
        public int Uid { get; set; }
        [Required]
        public int UserUID { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Content { get; set; } = string.Empty;
        public DateTime? UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
