using System.ComponentModel.DataAnnotations;


namespace Backend.Entity
{
    public class User
    {
        [Key]
        public int Uid { get; set; }
        public Guid UUid {get; set;} =  Guid.NewGuid();
        [Required]
        public string Role { get; set; } = "user";
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;

        public DateTime? LastLogin { get; set; }
        public DateTime CreatedDate { get; set; }  = DateTime.UtcNow;
    }
}
