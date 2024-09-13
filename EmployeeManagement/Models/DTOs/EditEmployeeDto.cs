using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models.DTOs
{
    public class EditEmployeeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string DateOfBirth { get; set; }
        public DateTime DateOfBirth_ { get; set; }
        public string ImagePath { get; set; }
        public IFormFile Photo { get; set; }
    }
}
