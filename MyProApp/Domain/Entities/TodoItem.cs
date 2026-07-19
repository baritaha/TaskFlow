using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class TodoItem : BaseEntity
    {
        [Required(ErrorMessage ="title is Required")]
        [StringLength(100, MinimumLength =3, ErrorMessage = "title must be between 3 and 100 characters")]
        public string Title { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "description must be less than 500 characters")]
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
    }
}
