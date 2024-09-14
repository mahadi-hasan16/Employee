using System.Diagnostics;
using EmployeeManagement.Data;
using EmployeeManagement.Models;
using EmployeeManagement.Models.DTOs;
using EmployeeManagement.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        private readonly IWebHostEnvironment _env;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext dbContext, IWebHostEnvironment env, ILogger<HomeController> logger)
        {
            _dbContext = dbContext;

            _env = env;

            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ViewEmployee()
        {
            var employees = _dbContext.EmployeeInfo.ToList();

            return View(employees);
        }

        [HttpGet]
        public IActionResult FindEmployee(string name, DateTime dateOfBirth, string email)
        {
            var employees = _dbContext.EmployeeInfo.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                employees = employees.Where(e => e.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(email))
            {
                employees = employees.Where(e => e.Email.Contains(email));
            }

            if (dateOfBirth != DateTime.Today)
            {
                employees = employees.Where(e => e.DateOfBirth == dateOfBirth.ToString("dd/MM/yyyy"));
            }
            
            var filteredEmployees = employees.ToList();

            if(filteredEmployees is null)
            {
                return NotFound();
            }
            
            return View(filteredEmployees);
        }


        public IActionResult AddEmployee()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(AddEmployeeDto employee)
        {
            string relativePath = "";
            if (employee.Photo != null && employee.Photo.Length > 0)
            {
                string folder = "Photoes/";
                string extension = Path.GetExtension(employee.Photo.FileName);
                string fileName = Guid.NewGuid().ToString() + "_" + extension;
                string filePath = Path.Combine(folder, fileName);
                string serverFolder = Path.Combine(_env.WebRootPath, filePath);

                using (var image = Image.Load(employee.Photo.OpenReadStream()))
                {
                    image.Mutate(x => x.Resize(50, 50));
                    await image.SaveAsync(serverFolder);
                }

                relativePath = filePath;
            }

            else
            {
                Console.WriteLine("No Image");
            }
            Console.WriteLine(employee.Name);

            var newEmployee = new Employee()
            {
                Name = employee.Name,
                Email = employee.Email,
                Mobile = employee.Mobile,
                DateOfBirth = employee.DateOfBirth.ToString("dd/MM/yyyy"),
                ImagePath = relativePath
            };

            _dbContext.EmployeeInfo.Add(newEmployee);
            _dbContext.SaveChanges();
            return RedirectToAction("ViewEmployee", "Home");
        }

        public IActionResult UpdateEmployee(int id)
        {
            var employee = _dbContext.EmployeeInfo.Find(id);

            if(employee is null)
            {
                return RedirectToAction("Index", "Home");
            }

            var editEmployeeDto = new EditEmployeeDto()
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Mobile = employee.Mobile,
                DateOfBirth = employee.DateOfBirth,
                ImagePath = employee.ImagePath
            };

            ViewData["Id"] = employee.Id;
            ViewData["Name"] = employee.Name;
            ViewData["Email"] = employee.Email;
            ViewData["Mobile"] = employee.Mobile;
            ViewData["DateOfBirth"] = employee.DateOfBirth;
            ViewData["ImagePath"] = employee.ImagePath;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEmployee(int id, EditEmployeeDto editEmployeeDto)
        {
            var employee = await _dbContext.EmployeeInfo.FindAsync(id);

            if (employee is null)
            {
                return RedirectToAction("Index", "Home");
            }

            string relativePath = employee.ImagePath;
            if (editEmployeeDto.Photo != null && editEmployeeDto.Photo.Length > 0)
            {
                if (!string.IsNullOrEmpty(employee.ImagePath))
                {
                    string oldImagePath = Path.Combine(_env.WebRootPath, employee.ImagePath);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                string folder = "Photoes/";
                string extension = Path.GetExtension(editEmployeeDto.Photo.FileName);
                string fileName = Guid.NewGuid().ToString() + "_" + extension;
                string filePath = Path.Combine(folder, fileName);
                string serverFolder = Path.Combine(_env.WebRootPath, filePath);

                using (var image = Image.Load(editEmployeeDto.Photo.OpenReadStream()))
                {
                    image.Mutate(x => x.Resize(50, 50));
                    await image.SaveAsync(serverFolder);
                }

                relativePath = filePath;
            }

            else
            {
                Console.WriteLine("No Image");
            }
            Console.WriteLine(employee.Name);

            string dateOfBirth;
            if(editEmployeeDto.DateOfBirth_ == DateTime.MinValue)
            {
                dateOfBirth = employee.DateOfBirth;
            }
            else
            {
                dateOfBirth = editEmployeeDto.DateOfBirth_.ToString("dd/MM/yyyy");
            }

            employee.Name = editEmployeeDto.Name;
            employee.Email = editEmployeeDto.Email;
            employee.Mobile = editEmployeeDto.Mobile;
            employee.DateOfBirth = dateOfBirth;
            employee.ImagePath = relativePath;

            _dbContext.EmployeeInfo.Update(employee);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("ViewEmployee", "Home");
        }

        public async Task<IActionResult> RemoveEmployee(int id)
        {
            var employee = await _dbContext.EmployeeInfo.FindAsync(id);

            if (employee is null)
            {
                return RedirectToAction("Index", "Home");
            }

            _dbContext.EmployeeInfo.Remove(employee);
            _dbContext.SaveChanges();
            return RedirectToAction("ViewEmployee", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
