using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class UpdateTodoDto
    {
        public string Title { get; set; }= string.Empty;
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
    }
}
