using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string DateOfBirth { get; set; }
        public string ImagePath { get; set; }
    }
}
