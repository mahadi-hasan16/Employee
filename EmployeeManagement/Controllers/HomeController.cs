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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
