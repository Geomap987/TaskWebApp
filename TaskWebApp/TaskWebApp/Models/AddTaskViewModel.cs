using System.ComponentModel.DataAnnotations;
using TaskWebApp.Models.ValidationAttributes;

namespace TaskWebApp.Models
{
    public class AddTaskViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Введите название")]
        [TaskTrackerValidation(ErrorMessage = "Название не должно содержать символы <>&")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Введите описание")]
        [TaskTrackerValidation(ErrorMessage = "Описание не должно содержать символы <>&")]
        public string? Description { get; set; }
        public int Priority { get; set; }
        public List<int>? PriorityOptions { get; set; }

    }
}

