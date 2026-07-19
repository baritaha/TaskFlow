using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CreateTodoDto
    {
        [Required(ErrorMessage = "title is Required")]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
