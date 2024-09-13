using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models.DTOs
{
    public class AddEmployeeDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        public IFormFile Photo { get; set; }
    }
}
